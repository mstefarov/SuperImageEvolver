using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SuperImageEvolver {
    public sealed partial class MainForm : Form {
        public static TaskState State = new TaskState();
        bool stopped = true;

        readonly Thread[] threads = new Thread[1];


        public MainForm( string[] args ) {
            InitializeComponent();

            ModuleManager.LoadFactories( Assembly.GetExecutingAssembly() );

            Shown += delegate {
                /*
                cInitializer.Items.Clear();
                foreach( var preset in ModuleManager.GetPresets( ModuleFunction.Initializer ) ) {
                    cInitializer.Items.Add( preset.Value.Name );
                }

                cMutator.Items.Clear();
                foreach( var preset in ModuleManager.GetPresets( ModuleFunction.Mutator ) ) {
                    cMutator.Items.Add( preset.Value.Name );
                }

                cEvaluator.Items.Clear();
                foreach( var preset in ModuleManager.GetPresets( ModuleFunction.Evaluator ) ) {
                    cEvaluator.Items.Add( preset.Value.Name );
                }
                */

                if( args.Length == 1 ) {
                    if( args[0].EndsWith( ".sie" ) ) {
                        OpenProject( args[0] );
                    } else {
                        CreateProject( args[0] );
                    }
                } else if( args.Length > 1 ) {
                    MessageBox.Show( "Drag either a project file (.sie) or an image to open it." );
                } else {
                    Reset();
                    ResetUI();
                    tMutationStats.Text = "No project loaded";
                }
            };

            FormClosing += delegate {
                stopped = true;
            };
        }


        float OriginalZoom {
            get { return originalZoom; }
            set {
                originalZoom = value;
                if( picOriginal.Image != null ) {
                    picOriginal.Width = (int)Math.Round( picOriginal.Image.Width * originalZoom );
                    picOriginal.Height = (int)Math.Round( picOriginal.Image.Height * originalZoom );
                }
            }
        }

        float originalZoom = 1;


        void SetImage( Bitmap image ) {
            State.OriginalImage = image;

            if( State.WorkingImageCopy != null ) {
                State.WorkingImageCopy.Dispose();
            }

            State.WorkingImageCopy = new Bitmap( image.Width, image.Height );
            using( Graphics g = Graphics.FromImage( State.WorkingImageCopy ) ) {
                g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.Clear( State.ProjectOptions.Matte );
                g.DrawImageUnscaled( image, Point.Empty );
            }

            if( State.WorkingImageData != null ) {
                State.WorkingImageCopyClone.UnlockBits( State.WorkingImageData );
                State.WorkingImageCopyClone.Dispose();
            }
            State.WorkingImageCopyClone = (Bitmap)State.WorkingImageCopy.Clone();
            State.WorkingImageData =
                State.WorkingImageCopyClone.LockBits( new Rectangle( Point.Empty, State.OriginalImage.Size ),
                                                      ImageLockMode.ReadOnly,
                                                      PixelFormat.Format32bppArgb );
            State.ImageWidth = State.OriginalImage.Width;
            State.ImageHeight = State.OriginalImage.Height;

            picOriginal.Image = State.WorkingImageCopy;
            OriginalZoom = OriginalZoom; // force resize

            picBestMatch.State = State;
            picBestMatch.Invalidate();

            picDiff.Init( State ); // force resize
            picDiff.Invalidate();
        }


        readonly object autosaveLock = new object();
        DateTime autosaveNext;
        readonly TimeSpan autosaveInterval = TimeSpan.FromSeconds( 30 );


        void AutoSave() {
            if( State.ProjectFileName != null && DateTime.UtcNow > autosaveNext ) {
                lock( autosaveLock ) {
                    if( DateTime.UtcNow > autosaveNext ) {
                        State.SerializeNBT().WriteTag( State.ProjectFileName + ".autosave.sie" );
                        autosaveNext = DateTime.UtcNow.Add( autosaveInterval );
                        BeginInvoke( (Action)delegate {
                            Text = Path.GetFileName( State.ProjectFileName ) + " | SuperImageEvolver | autosaved " +
                                   DateTime.Now;
                        } );
                    }
                }
            }
        }


        void Run() {
            Random rand = new Random();
            Bitmap testCanvas = new Bitmap( State.ImageWidth, State.ImageHeight );

            while( !stopped ) {
                Interlocked.Increment( ref State.MutationCounter );
                DNA mutation = State.Mutator.Mutate( rand, State.BestMatch, State );

                bool takeRisk = (rand.NextDouble() < State.ProjectOptions.RiskRate * State.BestMatch.Divergence);
                double riskMargin = -(State.BestMatch.Divergence * State.BestMatch.Divergence * State.BestMatch.Divergence) *
                                    State.ProjectOptions.RiskMargin;
                if( !takeRisk ) riskMargin = 0;

                mutation.Divergence = State.Evaluator.CalculateDivergence( testCanvas,
                                                                           mutation,
                                                                           State,
                                                                           State.BestMatch.Divergence - riskMargin );

                if( Math.Abs( mutation.Divergence - 1 ) < float.Epsilon ) continue;

                double improvement = State.BestMatch.Divergence - mutation.Divergence;

                if( improvement > 0 || takeRisk && (improvement > riskMargin) ) {
                    lock( State.ImprovementLock ) {
                        riskMargin = -(State.BestMatch.Divergence*State.BestMatch.Divergence)*
                                     State.ProjectOptions.RiskMargin;
                        if( !takeRisk ) riskMargin = 0;
                        mutation.Divergence = State.Evaluator.CalculateDivergence( testCanvas,
                                                                                   mutation,
                                                                                   State,
                                                                                   1 );
                        improvement = State.BestMatch.Divergence - mutation.Divergence;

                        if( improvement > 0 || takeRisk && (improvement > riskMargin) ) {
                            State.HasChangedSinceSave = true;
                            State.ImprovementCounter++;
                            if( improvement <= 0 ) {
                                State.RiskyMoveCounter++;
                            } else {
                                State.MutationCounts[mutation.LastMutation]++;
                                State.MutationImprovements[mutation.LastMutation] += improvement;
                            }

                            State.MutationDataLog.Add( new PointF {
                                X = (float)DateTime.UtcNow.Subtract( State.TaskStart ).TotalSeconds,
                                Y = (float)mutation.Divergence
                            } );
                            State.BestMatch = mutation;
                            State.LastImprovementTime = DateTime.UtcNow;
                            State.LastImprovementMutationCount = State.MutationCounter;
                            picBestMatch.Invalidate();
                            picDiff.Invalidate();
                            graphWindow1.SetData( State.MutationDataLog, true, true, false, false, true, true );
                        }
                    }
                    AutoSave();
                }
            }
        }


        int lastMutationtCounter;
        DateTime lastUpdate;


        void UpdateStatus() {
            while( !stopped ) {
                try {
                    Invoke( (Action)UpdateTick );
                } catch( ObjectDisposedException ) {}

                Thread.Sleep( State.ProjectOptions.RefreshRate );
            }
        }


        void UpdateTick() {
            try {
                int mutationDelta = State.MutationCounter - lastMutationtCounter;
                lastMutationtCounter = State.MutationCounter;
                double timeDelta = ( DateTime.UtcNow - lastUpdate ).TotalSeconds;
                lastUpdate = DateTime.UtcNow;

                tMutationStats.Text = String.Format(
@"Fitness: {0:0.00000}%
Improvements: {1} ({2:0.00}/s)
Mutations: {3} ({4:0}/s)
Elapsed: {5}
SinceImproved: {7} / {6}",
                    State.BestMatch == null ? 0 : 100 - State.BestMatch.Divergence * 100,
                    State.ImprovementCounter,
                    State.ImprovementCounter / DateTime.UtcNow.Subtract( State.TaskStart ).TotalSeconds,
                    State.MutationCounter,
                    mutationDelta / timeDelta,
                    DateTime.UtcNow.Subtract( State.TaskStart ).ToCompactString(),
                    DateTime.UtcNow.Subtract( State.LastImprovementTime ).ToCompactString(),
                    State.MutationCounter - State.LastImprovementMutationCount );
                StringBuilder sb = new StringBuilder( Environment.NewLine );
                sb.AppendLine();
                double totalImprovements = State.MutationImprovements.Values.Sum();
                foreach( MutationType type in Enum.GetValues( typeof( MutationType ) ) ) {
                    double rate = 0;
                    if( State.MutationCounts[type] != 0 ) {
                        rate = State.MutationImprovements[type] / State.MutationCounts[type];
                    }
                    sb.AppendFormat( "{0} - {1}*{2:0.0000} ({3:0.0}%)",
                                     type,
                                     State.MutationCounts[type],
                                     rate * 100,
                                     ( State.MutationImprovements[type] / totalImprovements ) * 100 );
                    sb.AppendLine();
                }

                if( State.BestMatch != null ) {
                    sb.AppendFormat( "Risk: margin {0:0.0000}, rate {1:0.0}%, taken {2} times",
                                     (State.BestMatch.Divergence * State.BestMatch.Divergence * State.BestMatch.Divergence) *
                                     State.ProjectOptions.RiskMargin*100,
                                     State.BestMatch.Divergence*State.ProjectOptions.RiskRate * 100,
                                     State.RiskyMoveCounter );
                }

                tMutationStats.Text += sb.ToString();
                graphWindow1.Invalidate();

            } catch( ObjectDisposedException ) {}
        }


        Thread updateThread;


        void Reset() {
            foreach( MutationType type in Enum.GetValues( typeof( MutationType ) ) ) {
                State.MutationCounts[type] = 0;
                State.MutationImprovements[type] = 0;
            }
            //cInitializer_SelectedIndexChanged( cInitializer, EventArgs.Empty );
            //cMutator_SelectedIndexChanged( cMutator, EventArgs.Empty );
            //cEvaluator_SelectedIndexChanged( cEvaluator, EventArgs.Empty );
            State.TaskStart = DateTime.UtcNow;
            State.Shapes = (int)nPolygons.Value;
            State.Vertices = (int)nVertices.Value;
            State.ImprovementCounter = 0;
            State.MutationDataLog.Clear();
            lastMutationtCounter = 0;
            State.MutationCounter = 0;
            State.RiskyMoveCounter = 0;
            State.LastImprovementMutationCount = 0;
            State.BestMatch = State.Initializer.Initialize( new Random(), State );
            State.HasChangedSinceSave = true;
        }


        void ResetUI() {
            cInitializer.SelectedIndex = 1;
            cMutator.SelectedIndex = 1;
            cEvaluator.SelectedIndex = 2;

            if( cmBestMatchWireframe.Checked ) cmBestMatchWireframe.PerformClick();
            if( cmBestMatchShowLastChange.Checked ) cmBestMatchShowLastChange.PerformClick();

            if( cmDiffInvert.Checked ) cmDiffInvert.PerformClick();
            if( !cmDiffShowColor.Checked ) cmDiffShowColor.PerformClick();
            if( !cmDiffExaggerate.Checked ) cmDiffExaggerate.PerformClick();
            if( cmDiffShowLastChange.Checked ) cmDiffShowLastChange.PerformClick();

            if( cmOriginalZoomSync.Checked ) cmOriginalZoomSync.PerformClick();
            cmOriginalZoom100.PerformClick();
            cmBestMatchZoom100.PerformClick();
            cmDiffZoom100.PerformClick();
        }


        void Start( bool reset ) {
            cInitializer.Enabled = false;
            nPolygons.Enabled = false;
            nVertices.Enabled = false;
            if( reset ) {
                Reset();
            } else {
                lastMutationtCounter = State.MutationCounter;
            }

            State.SetEvaluator( State.Evaluator );

            stopped = false;
            for( int i = 0; i < threads.Length; i++ ) {
                threads[i] = new Thread( Run );
                threads[i].Start();
            }
            updateThread = new Thread( UpdateStatus );
            updateThread.Start();

            bStart.Enabled = false;
            bRestart.Enabled = true;
            bStop.Enabled = true;
        }


        void Stop() {
            stopped = true;
            for( int i = 0; i < threads.Length; i++ ) {
                if( threads[i] != null ) threads[i].Join();
            }
            Application.DoEvents();
            if( updateThread != null ) updateThread.Join();
            cInitializer.Enabled = true;
            nPolygons.Enabled = true;
            nVertices.Enabled = true;

            bStart.Enabled = true;
            bRestart.Enabled = true;
            bStop.Enabled = false;
        }


        void cInitializer_SelectedIndexChanged( object sender, EventArgs e ) {
            State.HasChangedSinceSave = true;
            switch( cInitializer.SelectedIndex ) {
                case 0:
                    State.Initializer = new SolidColorInitializer( Color.Black );
                    break;
                case 1:
                    State.Initializer = new SegmentedInitializer( Color.Black );
                    break;
                case 2:
                    State.Initializer = new RadialInitializer( Color.Black );
                    break;
            }
        }


        void cMutator_SelectedIndexChanged( object sender, EventArgs e ) {
            State.HasChangedSinceSave = true;
            switch( cMutator.SelectedIndex ) {
                case 0:
                    State.Mutator = new HarderMutator();
                    break;
                case 1:
                    State.Mutator = new HardMutator();
                    break;
                case 2:
                    State.Mutator = new MediumMutator();
                    break;
                case 3:
                    State.Mutator = new SoftMutator( 8, 12 );
                    break;
                case 4:
                    State.Mutator = new SoftMutator( 1, 2 );
                    break;
                case 5:
                    State.Mutator = new TranslateMutator {
                        PreserveAspectRatio = true
                    };
                    break;
                case 6:
                    State.Mutator = new TranslateMutator();
                    break;
                case 7:
                    State.Mutator = new TranslateMutator {
                        PreserveAspectRatio = true,
                        EnableRotation = true
                    };
                    break;
                case 8:
                    State.Mutator = new TranslateMutator {
                        EnableRotation = true
                    };
                    break;
                case 9:
                    State.Mutator = new SoftTranslateMutator {
                        PreserveAspectRatio = true
                    };
                    break;
                case 10:
                    State.Mutator = new SoftTranslateMutator();
                    break;
                case 11:
                    State.Mutator = new SoftTranslateMutator {
                        PreserveAspectRatio = true,
                        EnableRotation = true
                    };
                    break;
                case 12:
                    State.Mutator = new SoftTranslateMutator {
                        EnableRotation = true
                    };
                    break;
                case 13:
                    State.Mutator = new HardishMutator {
                        MaxColorDelta = 16,
                        MaxPosDelta = 64
                    };
                    break;
            }
        }


        void cEvaluator_SelectedIndexChanged( object sender, EventArgs e ) {
            State.HasChangedSinceSave = true;
            switch( cEvaluator.SelectedIndex ) {
                case 0:
                    State.SetEvaluator( new SloppyRGBEvaluator() );
                    break;
                case 1:
                    State.SetEvaluator( new RGBEvaluator( false ) );
                    break;
                case 2:
                    State.SetEvaluator( new RGBEvaluator( true ) );
                    break;
                case 3:
                    State.SetEvaluator( new LumaEvaluator( false ) );
                    break;
                case 4:
                    State.SetEvaluator( new LumaEvaluator( true ) );
                    break;
            }
            picBestMatch.Invalidate();
            graphWindow1.Invalidate();
        }


        #region Menus

        void bHelpListModules_Click( object sender, EventArgs e ) {
            StringBuilder sb = new StringBuilder();
            IModuleFactory[] factories = ModuleManager.ListAllModules().OrderBy( fac => fac.Function ).ToArray();
            foreach( IModuleFactory factory in factories ) {
                sb.AppendFormat( "{0} {1}", factory.Function, factory.ID );
                sb.AppendLine();
            }
            MessageBox.Show( sb.ToString(), "List of loaded modules" );
        }


        void bViewOriginalImage_Click( object sender, EventArgs e ) {
            picOriginal.Visible = bViewOriginalImage.Checked;
        }


        void bViewBestMatchImage_Click( object sender, EventArgs e ) {
            picBestMatch.Visible = bViewBestMatchImage.Checked;
        }


        void bViewDifferenceImage_Click( object sender, EventArgs e ) {
            picDiff.Visible = bViewDifferenceImage.Checked;
        }


        void bViewStatistics_Click( object sender, EventArgs e ) {
            pStatistics.Visible = bViewStatistics.Checked;
        }


        readonly SaveFileDialog exportImageDialog = new SaveFileDialog {
            Filter = "PNG Image|*.png|TIFF Image|*.tif;*.tiff|BMP Bitmap Image|*.bmp|JPEG Image|*.jpg;*.jpeg",
            Title = "Saving best match image (raster)..."
        };


        void bExportImage_Click( object sender, EventArgs e ) {
            if( exportImageDialog.ShowDialog() == DialogResult.OK ) {
                Bitmap exportBitmap = new Bitmap( picBestMatch.Width, picBestMatch.Height );
                picBestMatch.DrawToBitmap( exportBitmap, new Rectangle( Point.Empty, picBestMatch.Size ) );
                exportBitmap.Save( exportImageDialog.FileName );
            }
        }


        readonly SaveFileDialog exportSvgDialog = new SaveFileDialog {
            Filter = "SVG Image|*.svg",
            Title = "Saving best match image (SVG)..."
        };


        void bExportVectors_Click( object sender, EventArgs e ) {
            if( exportSvgDialog.ShowDialog() == DialogResult.OK ) {
                State.SerializeSVG().Save( exportSvgDialog.FileName );
            }
        }


        void bNewProject_Click( object sender, EventArgs e ) {
            if( !stopped ) Stop();
            OpenFileDialog fd = new OpenFileDialog {
                Filter = "Images|*.jpg;*.png;*.bmp;*.gif;*.tiff;*.tga",
                Title = "Creating Project from an Image"
            };
            if( fd.ShowDialog() == DialogResult.OK ) {
                CreateProject( fd.FileName );
            }
        }


        void bSaveProjectAs_Click( object sender, EventArgs e ) {
            SaveProjectAs();
        }


        void bSaveProject_Click( object sender, EventArgs e ) {
            SaveProject();
        }


        void bOpenProject_Click( object sender, EventArgs e ) {
            OpenFileDialog fd = new OpenFileDialog {
                Filter = "SuperImageEvolver project|*.sie",
                Title = "Open Existing Project"
            };
            if( fd.ShowDialog() == DialogResult.OK ) {
                OpenProject( fd.FileName );
            }
        }


        void bStart_Click( object sender, EventArgs e ) {
            if( stopped ) {
                Start( State.BestMatch == null );
            }
        }


        void bRestart_Click( object sender, EventArgs e ) {
            if( stopped ) {
                Reset();
                State.SetEvaluator( State.Evaluator );
                UpdateTick();
                picDiff.Invalidate();
                picBestMatch.Invalidate();
            } else {
                Stop();
                Start( true );
            }
        }


        void bStop_Click( object sender, EventArgs e ) {
            if( !stopped ) {
                Stop();
            }
        }


        void bEditInitializerSetting_Click( object sender, EventArgs e ) {
            var md = new ModuleSettingsDisplay<IInitializer>( State.Initializer );
            if( md.ShowDialog() == DialogResult.OK ) {
                State.Initializer = md.Module;
                State.HasChangedSinceSave = true;
            }
        }


        void bEditMutatorSettings_Click( object sender, EventArgs e ) {
            var md = new ModuleSettingsDisplay<IMutator>( State.Mutator );
            if( md.ShowDialog() == DialogResult.OK ) {
                State.Mutator = md.Module;
                State.HasChangedSinceSave = true;
            }
        }


        void bEditEvaluatorSettings_Click( object sender, EventArgs e ) {
            var md = new ModuleSettingsDisplay<IEvaluator>( State.Evaluator );
            if( md.ShowDialog() == DialogResult.OK ) {
                State.SetEvaluator( md.Module );
                graphWindow1.Invalidate();
                State.HasChangedSinceSave = true;
            }
        }


        void showWireframeToolStripMenuItem_CheckedChanged( object sender, EventArgs e ) {
            picBestMatch.Wireframe = cmBestMatchWireframe.Checked;
        }


        void showLastChangeToolStripMenuItem_CheckedChanged( object sender, EventArgs e ) {
            picBestMatch.ShowLastChange = cmBestMatchShowLastChange.Checked;
        }


        #region Zoom

        void cmOriginalZoom_DropDownItemClicked( object sender, ToolStripItemClickedEventArgs e ) {
            if( e.ClickedItem is ToolStripSeparator ) return;
            if( e.ClickedItem == cmOriginalZoomSync ) {
                cmOriginalZoomSync.Checked = !cmOriginalZoomSync.Checked;
                cmBestMatchZoomSync.Checked = cmOriginalZoom.Checked;
                cmDiffZoomSync.Checked = cmOriginalZoom.Checked;
            } else {
                ApplyOriginalZoom( e.ClickedItem );
            }
            if( cmOriginalZoomSync.Checked ) {
                if( cmOriginalZoomSync.Checked ) {
                    if( cmOriginalZoom50.Checked ) {
                        ApplyBestMatchZoom( cmBestMatchZoom50 );
                        ApplyDiffZoom( cmDiffZoom50 );
                    } else if( cmOriginalZoom75.Checked ) {
                        ApplyBestMatchZoom( cmBestMatchZoom75 );
                        ApplyDiffZoom( cmDiffZoom75 );
                    } else if( cmOriginalZoom100.Checked ) {
                        ApplyBestMatchZoom( cmBestMatchZoom100 );
                        ApplyDiffZoom( cmDiffZoom100 );
                    } else if( cmOriginalZoom125.Checked ) {
                        ApplyBestMatchZoom( cmBestMatchZoom125 );
                        ApplyDiffZoom( cmDiffZoom125 );
                    } else if( cmOriginalZoom150.Checked ) {
                        ApplyBestMatchZoom( cmBestMatchZoom150 );
                        ApplyDiffZoom( cmDiffZoom150 );
                    } else if( cmOriginalZoom200.Checked ) {
                        ApplyBestMatchZoom( cmBestMatchZoom200 );
                        ApplyDiffZoom( cmDiffZoom200 );
                    }
                }
            }
        }


        void ApplyOriginalZoom( ToolStripItem item ) {
            cmOriginalZoom50.Checked = false;
            cmOriginalZoom75.Checked = false;
            cmOriginalZoom100.Checked = false;
            cmOriginalZoom125.Checked = false;
            cmOriginalZoom150.Checked = false;
            cmOriginalZoom200.Checked = false;
            ((ToolStripMenuItem)item).Checked = true;
            OriginalZoom = float.Parse( item.Tag.ToString() );
        }


        void cmBestMatchZoom_DropDownItemClicked( object sender, ToolStripItemClickedEventArgs e ) {
            if( e.ClickedItem is ToolStripSeparator ) return;
            if( e.ClickedItem == cmBestMatchZoomSync ) {
                cmBestMatchZoomSync.Checked = !cmBestMatchZoomSync.Checked;
                cmDiffZoomSync.Checked = cmBestMatchZoomSync.Checked;
                cmOriginalZoomSync.Checked = cmBestMatchZoomSync.Checked;
            } else {
                ApplyBestMatchZoom( e.ClickedItem );
            }
            if( cmBestMatchZoomSync.Checked ) {
                if( cmBestMatchZoomSync.Checked ) {
                    if( cmBestMatchZoom50.Checked ) {
                        ApplyDiffZoom( cmDiffZoom50 );
                        ApplyOriginalZoom( cmOriginalZoom50 );
                    } else if( cmBestMatchZoom75.Checked ) {
                        ApplyDiffZoom( cmDiffZoom75 );
                        ApplyOriginalZoom( cmOriginalZoom75 );
                    } else if( cmBestMatchZoom100.Checked ) {
                        ApplyDiffZoom( cmDiffZoom100 );
                        ApplyOriginalZoom( cmOriginalZoom100 );
                    } else if( cmBestMatchZoom125.Checked ) {
                        ApplyDiffZoom( cmDiffZoom125 );
                        ApplyOriginalZoom( cmOriginalZoom125 );
                    } else if( cmBestMatchZoom150.Checked ) {
                        ApplyDiffZoom( cmDiffZoom150 );
                        ApplyOriginalZoom( cmOriginalZoom150 );
                    } else if( cmBestMatchZoom200.Checked ) {
                        ApplyDiffZoom( cmDiffZoom200 );
                        ApplyOriginalZoom( cmOriginalZoom200 );
                    }
                }
            }
        }


        void ApplyBestMatchZoom( ToolStripItem item ) {
            cmBestMatchZoom50.Checked = false;
            cmBestMatchZoom75.Checked = false;
            cmBestMatchZoom100.Checked = false;
            cmBestMatchZoom125.Checked = false;
            cmBestMatchZoom150.Checked = false;
            cmBestMatchZoom200.Checked = false;
            ((ToolStripMenuItem)item).Checked = true;
            picBestMatch.Zoom = float.Parse( item.Tag.ToString() );
        }


        void cmDiffZoom_DropDownItemClicked( object sender, ToolStripItemClickedEventArgs e ) {
            if( e.ClickedItem is ToolStripSeparator ) return;
            if( e.ClickedItem == cmDiffZoomSync ) {
                cmDiffZoomSync.Checked = !cmDiffZoomSync.Checked;
                cmBestMatchZoomSync.Checked = cmDiffZoomSync.Checked;
                cmOriginalZoomSync.Checked = cmDiffZoomSync.Checked;
            } else {
                ApplyDiffZoom( e.ClickedItem );
            }
            if( cmDiffZoomSync.Checked ) {
                if( cmDiffZoomSync.Checked ) {
                    if( cmDiffZoom50.Checked ) {
                        ApplyBestMatchZoom( cmBestMatchZoom50 );
                        ApplyOriginalZoom( cmOriginalZoom50 );
                    } else if( cmDiffZoom75.Checked ) {
                        ApplyBestMatchZoom( cmBestMatchZoom75 );
                        ApplyOriginalZoom( cmOriginalZoom75 );
                    } else if( cmDiffZoom100.Checked ) {
                        ApplyBestMatchZoom( cmBestMatchZoom100 );
                        ApplyOriginalZoom( cmOriginalZoom100 );
                    } else if( cmDiffZoom125.Checked ) {
                        ApplyBestMatchZoom( cmBestMatchZoom125 );
                        ApplyOriginalZoom( cmOriginalZoom125 );
                    } else if( cmDiffZoom150.Checked ) {
                        ApplyBestMatchZoom( cmBestMatchZoom150 );
                        ApplyOriginalZoom( cmOriginalZoom150 );
                    } else if( cmDiffZoom200.Checked ) {
                        ApplyBestMatchZoom( cmBestMatchZoom200 );
                        ApplyOriginalZoom( cmOriginalZoom200 );
                    }
                }
            }
        }


        void ApplyDiffZoom( ToolStripItem item ) {
            cmDiffZoom50.Checked = false;
            cmDiffZoom75.Checked = false;
            cmDiffZoom100.Checked = false;
            cmDiffZoom125.Checked = false;
            cmDiffZoom150.Checked = false;
            cmDiffZoom200.Checked = false;
            ( (ToolStripMenuItem)item ).Checked = true;
            picDiff.Zoom = float.Parse( item.Tag.ToString() );
        }

        #endregion


        void cmDiffExaggerate_CheckedChanged( object sender, EventArgs e ) {
            picDiff.Exaggerate = cmDiffExaggerate.Checked;
        }


        void cmDiffInvert_CheckedChanged( object sender, EventArgs e ) {
            picDiff.Invert = cmDiffInvert.Checked;
        }


        void cmDiffShowColor_CheckedChanged( object sender, EventArgs e ) {
            picDiff.ShowColor = cmDiffShowColor.Checked;
        }


        void bExportDNA_Click( object sender, EventArgs e ) {
            if( State == null || State.BestMatch == null ) return;
            List<string> parts = new List<string> {
                State.Vertices.ToString(),
                State.Shapes.ToString()
            };
            foreach( Shape shape in State.BestMatch.Shapes ) {
                parts.Add( shape.Color.R.ToString() );
                parts.Add( shape.Color.G.ToString() );
                parts.Add( shape.Color.B.ToString() );
                parts.Add( ( shape.Color.A / 255f ).ToString() );
                foreach( PointF point in shape.Points ) {
                    parts.Add( ( (int)Math.Round( point.X ) ).ToString() );
                    parts.Add( ( (int)Math.Round( point.Y ) ).ToString() );
                }
            }
            Clipboard.SetText( String.Join( " ", parts.ToArray() ) );
            MessageBox.Show( "DNA Copied to clipboard." );
        }


        void bImportDNA_Click( object sender, EventArgs e ) {
            DNAImportWindow win = new DNAImportWindow();
            if( win.ShowDialog() == DialogResult.OK ) {
                Reset();
                try {
                    string[] parts = win.DNA.Split( ' ' );
                    Stop();
                    State.Vertices = Int32.Parse( parts[0] );
                    State.Shapes = Int32.Parse( parts[1] );
                    nVertices.Value = State.Vertices;
                    nPolygons.Value = State.Shapes;
                    DNA importedDNA = new DNA {
                        Shapes = new Shape[State.Shapes]
                    };
                    int offset = 2;
                    for( int s = 0; s < State.Shapes; s++ ) {
                        Shape shape = new Shape {
                            Points = new PointF[State.Vertices]
                        };
                        int r = Int32.Parse( parts[offset] );
                        int g = Int32.Parse( parts[offset + 1] );
                        int b = Int32.Parse( parts[offset + 2] );
                        int a = (int)( float.Parse( parts[offset + 3] ) * 255 );
                        shape.Color = Color.FromArgb( a, r, g, b );
                        offset += 4;
                        for( int v = 0; v < State.Vertices; v++ ) {
                            float X = float.Parse( parts[offset] );
                            float Y = float.Parse( parts[offset + 1] );
                            shape.Points[v] = new PointF( X, Y );
                            offset += 2;
                        }
                        importedDNA.Shapes[s] = shape;
                    }
                    State.BestMatch = importedDNA;
                    State.SetEvaluator( State.Evaluator );
                    UpdateTick();
                    picBestMatch.Invalidate();
                    picDiff.Invalidate();

                } catch( FormatException ex ) {
                    MessageBox.Show( "Could not import DNA!" + Environment.NewLine + ex );
                }
            }
        }


        void cmDiffShowLastChange_Click( object sender, EventArgs e ) {
            picDiff.ShowLastChange = cmDiffShowLastChange.Checked;
        }


        void bProjectOptions_Click( object sender, EventArgs e ) {
            if( State == null ) return;
            var md = new ModuleSettingsDisplay<ProjectOptions>( State.ProjectOptions );
            if( md.ShowDialog() != DialogResult.OK ) {
                return;
            }
            State.HasChangedSinceSave = true;
            bool oldStopped = stopped;
            if( !oldStopped ) Stop();
            State.ProjectOptions = md.Module;
            BackColor = State.ProjectOptions.BackColor;
            if( BackColor.R*0.2126 + BackColor.G*0.7152 + BackColor.B*0.0722 > 128 ) {
                panel1.ForeColor = Color.Black;
            } else {
                panel1.ForeColor = Color.White;
            }
            if( State.OriginalImage != null ) {
                SetImage( State.OriginalImage );
            }
            graphWindow1.Invalidate();
            if( !oldStopped ) Start( false );
        }

        #endregion


        void CreateProject( string fileName ) {
            Bitmap image = (Bitmap)Image.FromFile( fileName );
            State = new TaskState();
            SetImage( image );
            Reset();
            ResetUI();
            State.SetEvaluator( State.Evaluator );
            UpdateTick();
            picDiff.Invalidate();
            picBestMatch.Invalidate();

            bStart.Enabled = true;
            bRestart.Enabled = true;
            bSaveProject.Enabled = true;
            bSaveProjectAs.Enabled = true;
            menuExport.Enabled = true;
        }


        void OpenProject( string filename ) {
            if( !stopped ) Stop();
            NBTag taskData = NBTag.ReadFile( filename );
            State = new TaskState( taskData );
            BackColor = State.ProjectOptions.BackColor;
            if( filename.EndsWith( ".autosave.sie" ) ) {
                State.ProjectFileName = filename.Substring( 0, filename.Length - 13 );
            } else {
                State.ProjectFileName = filename;
            }
            Text = Path.GetFileName( State.ProjectFileName ) + " | SuperImageEvolver";
            nVertices.Value = State.Vertices;
            nPolygons.Value = State.Shapes;
            SetImage( State.OriginalImage );

            if( taskData.Contains( "Presentation" ) ) {
                NBTag presentationTag = taskData["Presentation"];
                picOriginal.Visible = presentationTag.GetBool( "OriginalVisible", picOriginal.Visible );
                OriginalZoom = presentationTag.GetFloat( "OriginalZoom", OriginalZoom );
                picBestMatch.Visible = presentationTag.GetBool( "BestMatchVisible", picBestMatch.Visible );
                picBestMatch.Zoom = presentationTag.GetFloat( "BestMatchZoom", picBestMatch.Zoom );
                picBestMatch.Wireframe = presentationTag.GetBool( "BestMatchWireframe", picBestMatch.Wireframe );
                picBestMatch.ShowLastChange = presentationTag.GetBool( "BestMatchShowLastChange", picBestMatch.ShowLastChange );
                picDiff.Visible = presentationTag.GetBool( "DiffVisible", picDiff.Visible );
                picDiff.Invert = presentationTag.GetBool( "DiffInvert", picDiff.Invert );
                picDiff.Exaggerate = presentationTag.GetBool( "DiffExaggerate", picDiff.Exaggerate );
                picDiff.ShowColor = presentationTag.GetBool( "DiffShowColor", picDiff.ShowColor );
                picDiff.Zoom = presentationTag.GetFloat( "DiffZoom", picDiff.Zoom );
                picDiff.ShowLastChange = presentationTag.GetBool( "DiffShowLastChange", picDiff.ShowLastChange );
            }
            State.SetEvaluator( State.Evaluator );
            UpdateTick();
            picDiff.Invalidate();
            picBestMatch.Invalidate();
            State.HasChangedSinceSave = false;

            bStart.Enabled = true;
            bRestart.Enabled = true;
            bSaveProject.Enabled = true;
            bSaveProjectAs.Enabled = true;
            menuExport.Enabled = true;
        }


        void SaveProject() {
            if( State.ProjectFileName != null ) {
                NBTag presentationTag = new NBTCompound( "Presentation" );
                presentationTag.Append( "OriginalVisible", picOriginal.Visible );
                presentationTag.Append( "OriginalZoom", OriginalZoom );
                presentationTag.Append( "BestMatchVisible", picBestMatch.Visible );
                presentationTag.Append( "BestMatchZoom", picBestMatch.Zoom );
                presentationTag.Append( "BestMatchWireframe", picBestMatch.Wireframe );
                presentationTag.Append( "BestMatchShowLastChange", picBestMatch.ShowLastChange );
                presentationTag.Append( "DiffVisible", picDiff.Visible );
                presentationTag.Append( "DiffInvert", picDiff.Invert );
                presentationTag.Append( "DiffExaggerate", picDiff.Exaggerate );
                presentationTag.Append( "DiffShowColor", picDiff.ShowColor );
                presentationTag.Append( "DiffZoom", picDiff.Zoom );
                presentationTag.Append( "DiffShowLastChange", picDiff.ShowLastChange );
                presentationTag.Append( "SyncZoom", cmOriginalZoomSync.Checked );

                NBTag tag = State.SerializeNBT();
                tag.Append( presentationTag );
                tag.WriteTag( State.ProjectFileName );
                Text = Path.GetFileName( State.ProjectFileName ) + " | SuperImageEvolver | saved " + DateTime.Now;
            } else {
                SaveProjectAs();
            }
        }


        void SaveProjectAs() {
            SaveFileDialog saveTaskDialog = new SaveFileDialog {
                Filter = "SuperImageEvolver project|*.sie",
                Title = "Save Project As..."
            };
            if( saveTaskDialog.ShowDialog() == DialogResult.OK ) {
                State.ProjectFileName = saveTaskDialog.FileName;
                SaveProject();
            }
        }


        void MainForm_FormClosing( object sender, FormClosingEventArgs e ) {
            if( State != null && State.OriginalImage != null && State.HasChangedSinceSave ) {
                DialogResult result = MessageBox.Show( "Save changes to before exiting?",
                                                       "Exiting SuperImageEvolver",
                                                       MessageBoxButtons.YesNoCancel,
                                                       MessageBoxIcon.Warning,
                                                       MessageBoxDefaultButton.Button3 );
                switch( result ) {
                    case DialogResult.Yes:
                        SaveProject();
                        break;
                    case DialogResult.No:
                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                }
            }
        }
    }
}