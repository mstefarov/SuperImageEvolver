using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;


namespace SuperImageEvolver {
    public partial class MainForm : Form {
        static MainForm instance;
        public static TaskState state = new TaskState();
        bool stopped = true;

        Thread[] threads = new Thread[Environment.ProcessorCount];

        Bitmap clonedOriginal;


        public MainForm( string[] args ) {
            instance = this;
            InitializeComponent();

            ModuleManager.LoadFactories( Assembly.GetExecutingAssembly() );

            Shown += delegate( object sender, EventArgs eventArgs ) {
                Bitmap image;
                if( args.Length == 1 ) {
                    image = (Bitmap)Bitmap.FromFile( args[0] );
                } else if( File.Exists( "original.png" ) ) {
                    image = (Bitmap)Bitmap.FromFile( "original.png" );
                } else {
                    OpenFileDialog fd = new OpenFileDialog();
                    fd.Filter = "Images|*.jpg;*.png;*.bmp;*.gif;*.tiff;*.tga";
                    if( fd.ShowDialog() == DialogResult.OK ) {
                        image = (Bitmap)Bitmap.FromFile( fd.FileName );
                    } else {
                        Application.Exit();
                        return;
                    }
                }

                state = new TaskState();
                SetImage( image );
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
                cInitializer.SelectedIndex = 1;
                cMutator.SelectedIndex = 1;
                cEvaluator.SelectedIndex = 0;
            };

            FormClosing += delegate( object sender, FormClosingEventArgs e ) {
                stopped = true;
            };
        }


        void SetImage( Bitmap image ) {
            state.Image = image;

            clonedOriginal = (Bitmap)state.Image.Clone();
            state.ImageData = clonedOriginal.LockBits( new Rectangle( Point.Empty, state.Image.Size ),
                                                       ImageLockMode.ReadOnly,
                                                       PixelFormat.Format32bppArgb );
            state.ImageWidth = state.Image.Width;
            state.ImageHeight = state.Image.Height;

            picOriginal.Width = state.ImageWidth;
            picOriginal.Height = state.ImageHeight;
            picOriginal.Image = state.Image;

            picBestMatch.State = state;
            picBestMatch.Invalidate();

            picDiff.Width = state.ImageWidth;
            picDiff.Height = state.ImageHeight;
            picDiff.Init( state );
            picDiff.Invalidate();
        }


        void Run() {
            Random rand = new Random();
            Bitmap testCanvas = new Bitmap( state.ImageWidth,state.ImageHeight );

            while( !stopped ) {
                Interlocked.Increment( ref state.MutationCounter );
                DNA mutation = state.Mutator.Mutate( rand, state.BestMatch, state );
                mutation.Divergence = state.Evaluator.CalculateDivergence( testCanvas, mutation, state, state.BestMatch.Divergence );

                double improvement = state.BestMatch.Divergence - mutation.Divergence;
                if( improvement > 0 ) {
                    lock( state.ImprovementLock ) {
                        mutation.Divergence = state.Evaluator.CalculateDivergence( testCanvas, mutation, state, state.BestMatch.Divergence );
                        improvement = state.BestMatch.Divergence - mutation.Divergence;
                        if( improvement > 0 ) {
                            state.ImprovementCounter++;
                            state.MutationCounts[mutation.LastMutation]++;
                            state.MutationImprovements[mutation.LastMutation] += improvement;

                            state.MutationLog.Add( new Mutation( state.BestMatch, mutation ) );
                            state.BestMatch = mutation;
                            state.LastImprovementTime = DateTime.UtcNow;
                            state.LastImprovementMutationCount = state.MutationCounter;
                            picBestMatch.Invalidate();
                            picDiff.Invalidate();
                            PointF[] points = new PointF[state.MutationLog.Count];
                            for( int i = 0; i < points.Length; i++ ) {
                                points[i].X = (float)state.MutationLog[i].Timestamp.Subtract( state.TaskStart ).TotalSeconds;
                                points[i].Y = (float)state.MutationLog[i].NewDNA.Divergence;
                            }
                            graphWindow1.SetData( points, true, true, false, false, true, true );
                        }
                    }
                }
            }
        }

        int LastMutationtCounter;
        DateTime lastUpdate;

        void UpdateStatus() {
            while( !stopped ) {
                try {
                    Invoke( (Action)UpdateTick );
                } catch( ObjectDisposedException ) { }

                Thread.Sleep( 500 );
            }
        }

        void UpdateTick() {
            try {
                int mutationDelta = state.MutationCounter - LastMutationtCounter;
                LastMutationtCounter = state.MutationCounter;
                double timeDelta = (DateTime.UtcNow - lastUpdate).TotalSeconds;
                lastUpdate = DateTime.UtcNow;

                tMutationStats.Text = String.Format(
@"Fitness: {0:0.00000}%
Improvements: {1} ({2:0.00}/s)
Mutations: {3} ({4:0}/s)
Elapsed: {5}
SinceImproved: {7} / {6}",
                   100 - state.BestMatch.Divergence * 100,
                   state.ImprovementCounter,
                   state.ImprovementCounter / DateTime.UtcNow.Subtract( state.TaskStart ).TotalSeconds,
                   state.MutationCounter,
                   mutationDelta / timeDelta,
                   DateTime.UtcNow.Subtract( state.TaskStart ).ToCompactString(),
                   DateTime.UtcNow.Subtract( state.LastImprovementTime ).ToCompactString(),
                   state.MutationCounter - state.LastImprovementMutationCount );
                StringBuilder sb = new StringBuilder( Environment.NewLine );
                sb.Append( Environment.NewLine );
                foreach( MutationType type in Enum.GetValues( typeof( MutationType ) ) ) {
                    double rate = 0;
                    if( state.MutationCounts[type] != 0 ) {
                        rate = state.MutationImprovements[type] / (double)state.MutationCounts[type];
                    }
                    sb.AppendFormat( "{0} - {1}*{2:0.0000} ({3:0.0000})",
                                     type,
                                     state.MutationCounts[type],
                                     rate * 100,
                                     state.MutationImprovements[type] * 100 );
                    sb.Append( Environment.NewLine );
                }
                tMutationStats.Text += sb.ToString();
                graphWindow1.Invalidate();

            } catch( ObjectDisposedException ) { }
        }


        Thread updateThread;

        void Start( bool reset ) {
            cInitializer.Enabled = false;
            nPolygons.Enabled = false;
            nVertices.Enabled = false;
            if( reset ) {
                foreach( MutationType type in Enum.GetValues( typeof( MutationType ) ) ) {
                    state.MutationCounts[type] = 0;
                    state.MutationImprovements[type] = 0;
                }
                //cInitializer_SelectedIndexChanged( cInitializer, EventArgs.Empty );
                //cMutator_SelectedIndexChanged( cMutator, EventArgs.Empty );
                //cEvaluator_SelectedIndexChanged( cEvaluator, EventArgs.Empty );
                state.TaskStart = DateTime.UtcNow;
                state.Shapes = (int)nPolygons.Value;
                state.Vertices = (int)nVertices.Value;
                state.ImprovementCounter = 0;
                state.MutationLog.Clear();
                LastMutationtCounter = 0;
                state.MutationCounter = 0;
                state.BestMatch = state.Initializer.Initialize( new Random(), state );
            } else {
                LastMutationtCounter = state.MutationCounter;
            }

            state.SetEvaluator( state.Evaluator );

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


        private void cInitializer_SelectedIndexChanged( object sender, EventArgs e ) {
            switch( cInitializer.SelectedIndex ) {
                case 0:
                    state.Initializer = new SolidColorInitializer( Color.Black ); break;
                case 1:
                    state.Initializer = new SegmentedInitializer( Color.Black ); break;
                case 2:
                    state.Initializer = new RadialInitializer( Color.Black ); break;
            }
        }


        private void cMutator_SelectedIndexChanged( object sender, EventArgs e ) {
            switch( cMutator.SelectedIndex ) {
                case 0:
                    state.Mutator = new HarderMutator(); break;
                case 1:
                    state.Mutator = new HardMutator(); break;
                case 2:
                    state.Mutator = new MediumMutator(); break;
                case 3:
                    state.Mutator = new SoftMutator( 10 ); break;
                case 4:
                    state.Mutator = new SoftMutator( 2 ); break;
                case 5:
                    state.Mutator = new TranslateMutator {
                        PreserveAspectRatio = true
                    }; break;
                case 6:
                    state.Mutator = new TranslateMutator(); break;
                case 7:
                    state.Mutator = new TranslateMutator {
                        PreserveAspectRatio = true,
                        EnableRotation = true
                    }; break;
                case 8:
                    state.Mutator = new TranslateMutator {
                        EnableRotation = true
                    }; break;
                case 9:
                    state.Mutator = new SoftTranslateMutator {
                        PreserveAspectRatio = true
                    }; break;
                case 10:
                    state.Mutator = new SoftTranslateMutator(); break;
                case 11:
                    state.Mutator = new SoftTranslateMutator {
                        PreserveAspectRatio = true,
                        EnableRotation = true
                    }; break;
                case 12:
                    state.Mutator = new SoftTranslateMutator {
                        EnableRotation = true
                    }; break;
                case 13:
                    state.Mutator = new HardishMutator {
                        MaxColorDelta = 64,
                        MaxPosDelta = 64,
                        MaxOverlap = 6
                    }; break;
            }
        }


        private void cEvaluator_SelectedIndexChanged( object sender, EventArgs e ) {
            switch( cEvaluator.SelectedIndex ) {
                case 0:
                    state.SetEvaluator( new SloppyRGBEvaluator() ); break;
                case 1:
                    state.SetEvaluator( new RGBEvaluator( false ) ); break;
                case 2:
                    state.SetEvaluator( new RGBEvaluator( true ) ); break;
                case 3:
                    state.SetEvaluator( new LumaEvaluator( false ) ); break;
                case 4:
                    state.SetEvaluator( new LumaEvaluator( true ) ); break;
            }
            picBestMatch.Invalidate();
        }


        #region Menu


        private void bHelpListModules_Click( object sender, EventArgs e ) {
            StringBuilder sb = new StringBuilder();
            IModuleFactory[] factories = ModuleManager.ListAllModules().OrderBy( fac => fac.Function ).ToArray();
            foreach( IModuleFactory factory in factories ) {
                sb.AppendFormat( "{0} {1}", factory.Function, factory.ID );
                sb.AppendLine();
            }
            MessageBox.Show( sb.ToString(), "List of loaded modules" );
        }


        private void bViewOriginalImage_Click( object sender, EventArgs e ) {
            picOriginal.Visible = bViewOriginalImage.Checked;
        }

        private void bViewBestMatchImage_Click( object sender, EventArgs e ) {
            picBestMatch.Visible = bViewBestMatchImage.Checked;
        }

        private void bViewDifferenceImage_Click( object sender, EventArgs e ) {
            picDiff.Visible = bViewDifferenceImage.Checked;
        }

        private void bViewStatistics_Click( object sender, EventArgs e ) {
            pStatistics.Visible = bViewStatistics.Checked;
        }



        SaveFileDialog exportImageDialog = new SaveFileDialog {
            Filter = "PNG Image|*.png|TIFF Image|*.tif;*.tiff|BMP Bitmap Image|*.bmp|JPEG Image|*.jpg;*.jpeg",
            Title = "Saving best match image (raster)..."
        };

        private void bExportImage_Click( object sender, EventArgs e ) {
            if( exportImageDialog.ShowDialog() == DialogResult.OK ) {
                Bitmap exportBitmap = new Bitmap( picBestMatch.Width, picBestMatch.Height );
                picBestMatch.DrawToBitmap( exportBitmap, new Rectangle( Point.Empty, picBestMatch.Size ) );
                exportBitmap.Save( exportImageDialog.FileName );
            }
        }

        SaveFileDialog exportSVGDialog = new SaveFileDialog {
            Filter = "SVG Image|*.svg",
            Title = "Saving best match image (SVG)..."
        };

        private void bExportVectors_Click( object sender, EventArgs e ) {
            if( exportSVGDialog.ShowDialog() == DialogResult.OK ) {
                state.SerializeSVG().Save( exportSVGDialog.FileName );
            }
        }


        private void bNewProject_Click( object sender, EventArgs e ) {
            if( !stopped ) Stop();
            OpenFileDialog fd = new OpenFileDialog {
                Filter = "Images|*.jpg;*.png;*.bmp;*.gif;*.tiff;*.tga",
                Title = "Creating Project from an Image"
            };
            if( fd.ShowDialog() == DialogResult.OK ) {
                Bitmap image = (Bitmap)Bitmap.FromFile( fd.FileName );
                state = new TaskState();
                SetImage( image );
            } else {
                return;
            }
        }

        SaveFileDialog saveTaskDialog = new SaveFileDialog {
            Filter = "SIE - SuperImageEvolver task|*.sie",
            Title = "Save Project As..."
        };

        private void bSaveProjectAs_Click( object sender, EventArgs e ) {
            if( saveTaskDialog.ShowDialog() == DialogResult.OK ) {
                state.ProjectFileName = saveTaskDialog.FileName;
                using( FileStream fs = File.Create( saveTaskDialog.FileName ) ) {
                    state.Serialize( fs );
                }
                Text = "SuperImageEvolver | " + Path.GetFileName( state.ProjectFileName ) + " | saved " + DateTime.Now;
            }
        }

        private void bSaveProject_Click( object sender, EventArgs e ) {
            if( state.ProjectFileName != null ) {
                using( FileStream fs = File.Create( state.ProjectFileName ) ) {
                    state.Serialize( fs );
                }
                Text = "SuperImageEvolver | " + Path.GetFileName( state.ProjectFileName ) + " | saved " + DateTime.Now;
            } else {
                bSaveProjectAs_Click( sender, e );
            }
        }


        private void bOpenProject_Click( object sender, EventArgs e ) {
            if( !stopped ) Stop();
            OpenFileDialog fd = new OpenFileDialog {
                Filter = "SIE - SuperImageEvolver task|*.sie",
                Title = "Open Existing Project"
            };
            if( fd.ShowDialog() == DialogResult.OK ) {
                using( FileStream fs = File.OpenRead( fd.FileName ) ) {
                    state = new TaskState( fs );
                }
                state.ProjectFileName = fd.FileName;
                SetImage( state.Image );
                Start( false );
            }
        }


        private void bStart_Click( object sender, EventArgs e ) {
            if( stopped ) {
                Start( state.BestMatch == null );
            }
        }

        private void bRestart_Click( object sender, EventArgs e ) {
            if( stopped ) {
                Stop();
            }
            Start( true );
        }

        private void bStop_Click( object sender, EventArgs e ) {
            if( !stopped ) {
                Stop();
            }
        }

        #endregion

        private void bCopyStats_Click( object sender, EventArgs e ) {
            Clipboard.SetText( tMutationStats.Text );
        }

        private void bEditInitializerSetting_Click( object sender, EventArgs e ) {
            ModuleSettingsDisplay md = new ModuleSettingsDisplay( state.Initializer );
            if( md.ShowDialog() == DialogResult.OK ) {
                state.Initializer = (IInitializer)md.Module;
            }
        }

        private void bEditMutatorSettings_Click( object sender, EventArgs e ) {
            ModuleSettingsDisplay md = new ModuleSettingsDisplay( state.Mutator );
            if( md.ShowDialog() == DialogResult.OK ) {
                state.Mutator = (IMutator)md.Module;
            }
        }

        private void bEditEvaluatorSettings_Click( object sender, EventArgs e ) {
            ModuleSettingsDisplay md = new ModuleSettingsDisplay( state.Evaluator );
            if( md.ShowDialog() == DialogResult.OK ) {
                state.SetEvaluator( (IEvaluator)md.Module );
            }
        }

        private void showWireframeToolStripMenuItem_CheckedChanged( object sender, EventArgs e ) {
            picBestMatch.Wireframe = cmBestMatchWireframe.Checked;
        }

        private void showLastChangeToolStripMenuItem_CheckedChanged( object sender, EventArgs e ) {
            picBestMatch.ShowLastChange = cmBestMatchShowLastChange.Checked;
        }

        private void zoomToolStripMenuItem_DropDownItemClicked( object sender, ToolStripItemClickedEventArgs e ) {
            foreach( ToolStripMenuItem item in cmBestMatchZoom.DropDownItems ) {
                item.Checked = false;
            }
            picBestMatch.Zoom = float.Parse(e.ClickedItem.Tag.ToString());
        }

        private void cmDiffZoom_DropDownItemClicked( object sender, ToolStripItemClickedEventArgs e ) {
            foreach( ToolStripMenuItem item in cmDiffZoom.DropDownItems ) {
                item.Checked = false;
            }
            picDiff.Zoom = float.Parse( e.ClickedItem.Tag.ToString() );
        }

        private void cmDiffExaggerate_CheckedChanged( object sender, EventArgs e ) {
            picDiff.Exaggerate = cmDiffExaggerate.Checked;
        }

        private void cmDiffInvert_CheckedChanged( object sender, EventArgs e ) {
            picDiff.Invert = cmDiffInvert.Checked;
        }

        private void cmDiffShowColor_CheckedChanged( object sender, EventArgs e ) {
            picDiff.ShowColor = cmDiffShowColor.Checked;
        }

        private void bExportDNA_Click( object sender, EventArgs e ) {
            if( state == null || state.BestMatch == null ) return;
            List<string> parts = new List<string>();
            parts.Add( state.Vertices.ToString() );
            parts.Add( state.Shapes.ToString() );
            foreach( Shape shape in state.BestMatch.Shapes ) {
                parts.Add( shape.Color.R.ToString() );
                parts.Add( shape.Color.G.ToString() );
                parts.Add( shape.Color.B.ToString() );
                parts.Add( (shape.Color.A / 255f).ToString() );
                foreach( PointF point in shape.Points ) {
                    parts.Add( ((int)Math.Round( point.X )).ToString() );
                    parts.Add( ((int)Math.Round( point.Y )).ToString() );
                }
            }
            Clipboard.SetText( String.Join( " ", parts.ToArray() ) );
            MessageBox.Show( "DNA Copied to clipboard." );
        }

        private void bImportDNA_Click( object sender, EventArgs e ) {
            DNAImportWindow win = new DNAImportWindow();
            if( win.ShowDialog() == DialogResult.OK ) {
                try {
                    string[] parts = win.DNA.Split( ' ' );
                    Stop();
                    state.Vertices = Int32.Parse( parts[0] );
                    state.Shapes = Int32.Parse( parts[1] );
                    DNA importedDNA = new DNA();
                    importedDNA.Shapes = new Shape[state.Shapes];
                    int offset = 2;
                    for( int s = 0; s < state.Shapes; s++ ) {
                        Shape shape = new Shape();
                        shape.Points = new PointF[state.Vertices];
                        int R = Int32.Parse(parts[offset]);
                        int G = Int32.Parse(parts[offset+1]);
                        int B = Int32.Parse(parts[offset+2]);
                        int A = (int)(float.Parse(parts[offset+3])*255);
                        shape.Color = Color.FromArgb( A, R, G, B );
                        offset += 4;
                        for( int v = 0; v < state.Vertices; v++ ) {
                            float X = float.Parse( parts[offset] );
                            float Y = float.Parse( parts[offset+1] );
                            shape.Points[v] = new PointF( X, Y );
                            offset += 2;
                        }
                        importedDNA.Shapes[s] = shape;
                    }
                    state.BestMatch = importedDNA;
                    state.SetEvaluator( state.Evaluator );
                    picBestMatch.Invalidate();
                    picDiff.Invalidate();

                } catch( FormatException ex ) {
                    MessageBox.Show( "Could not import DNA!" + Environment.NewLine + ex );
                }
            }
        }
    }
}