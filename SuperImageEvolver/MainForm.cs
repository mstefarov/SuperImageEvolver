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
    public sealed partial class MainForm : Form {
        public static TaskState State = new TaskState();
        bool stopped = true;

        readonly Thread[] threads = new Thread[Environment.ProcessorCount];

        Bitmap clonedOriginal;


        public MainForm( string[] args ) {
            InitializeComponent();

            ModuleManager.LoadFactories( Assembly.GetExecutingAssembly() );

            Shown += delegate {
                Bitmap image;
                if( args.Length == 1 ) {
                    image = (Bitmap)Image.FromFile( args[0] );
                } else if( File.Exists( "original.png" ) ) {
                    image = (Bitmap)Image.FromFile( "original.png" );
                } else {
                    OpenFileDialog fd = new OpenFileDialog {
                        Filter = "Images|*.jpg;*.png;*.bmp;*.gif;*.tiff;*.tga"
                    };
                    if( fd.ShowDialog() == DialogResult.OK ) {
                        image = (Bitmap)Image.FromFile( fd.FileName );
                    } else {
                        Application.Exit();
                        return;
                    }
                }

                State = new TaskState();
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
                cEvaluator.SelectedIndex = 2;
            };

            FormClosing += delegate {
                stopped = true;
            };
        }


        void SetImage( Bitmap image ) {
            State.OriginalImage = image;

            clonedOriginal = (Bitmap)State.OriginalImage.Clone();
            State.OriginalImageData = clonedOriginal.LockBits( new Rectangle( Point.Empty, State.OriginalImage.Size ),
                                                       ImageLockMode.ReadOnly,
                                                       PixelFormat.Format32bppArgb );
            State.ImageWidth = State.OriginalImage.Width;
            State.ImageHeight = State.OriginalImage.Height;

            picOriginal.Width = State.ImageWidth;
            picOriginal.Height = State.ImageHeight;
            picOriginal.Image = State.OriginalImage;

            picBestMatch.State = State;
            picBestMatch.Invalidate();

            picDiff.Width = State.ImageWidth;
            picDiff.Height = State.ImageHeight;
            picDiff.Init( State );
            picDiff.Invalidate();
        }


        void Run() {
            Random rand = new Random();
            Bitmap testCanvas = new Bitmap( State.ImageWidth,State.ImageHeight );

            while( !stopped ) {
                Interlocked.Increment( ref State.MutationCounter );
                DNA mutation = State.Mutator.Mutate( rand, State.BestMatch, State );
                mutation.Divergence = State.Evaluator.CalculateDivergence( testCanvas, mutation, State, State.BestMatch.Divergence );

                double improvement = State.BestMatch.Divergence - mutation.Divergence;
                if( improvement > 0 ) {
                    lock( State.ImprovementLock ) {
                        mutation.Divergence = State.Evaluator.CalculateDivergence( testCanvas, mutation, State, State.BestMatch.Divergence );
                        improvement = State.BestMatch.Divergence - mutation.Divergence;
                        if( improvement > 0 ) {
                            State.ImprovementCounter++;
                            State.MutationCounts[mutation.LastMutation]++;
                            State.MutationImprovements[mutation.LastMutation] += improvement;

                            State.MutationLog.Add( new Mutation( State.BestMatch, mutation ) );
                            State.BestMatch = mutation;
                            State.LastImprovementTime = DateTime.UtcNow;
                            State.LastImprovementMutationCount = State.MutationCounter;
                            picBestMatch.Invalidate();
                            picDiff.Invalidate();
                            PointF[] points = new PointF[State.MutationLog.Count];
                            for( int i = 0; i < points.Length; i++ ) {
                                points[i].X = (float)State.MutationLog[i].Timestamp.Subtract( State.TaskStart ).TotalSeconds;
                                points[i].Y = (float)State.MutationLog[i].NewDNA.Divergence;
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
                int mutationDelta = State.MutationCounter - LastMutationtCounter;
                LastMutationtCounter = State.MutationCounter;
                double timeDelta = (DateTime.UtcNow - lastUpdate).TotalSeconds;
                lastUpdate = DateTime.UtcNow;

                tMutationStats.Text = String.Format(
@"Fitness: {0:0.00000}%
Improvements: {1} ({2:0.00}/s)
Mutations: {3} ({4:0}/s)
Elapsed: {5}
SinceImproved: {7} / {6}",
                   100 - State.BestMatch.Divergence * 100,
                   State.ImprovementCounter,
                   State.ImprovementCounter / DateTime.UtcNow.Subtract( State.TaskStart ).TotalSeconds,
                   State.MutationCounter,
                   mutationDelta / timeDelta,
                   DateTime.UtcNow.Subtract( State.TaskStart ).ToCompactString(),
                   DateTime.UtcNow.Subtract( State.LastImprovementTime ).ToCompactString(),
                   State.MutationCounter - State.LastImprovementMutationCount );
                StringBuilder sb = new StringBuilder( Environment.NewLine );
                sb.Append( Environment.NewLine );
                foreach( MutationType type in Enum.GetValues( typeof( MutationType ) ) ) {
                    double rate = 0;
                    if( State.MutationCounts[type] != 0 ) {
                        rate = State.MutationImprovements[type] / State.MutationCounts[type];
                    }
                    sb.AppendFormat( "{0} - {1}*{2:0.0000} ({3:0.0000})",
                                     type,
                                     State.MutationCounts[type],
                                     rate * 100,
                                     State.MutationImprovements[type] * 100 );
                    sb.Append( Environment.NewLine );
                }
                tMutationStats.Text += sb.ToString();
                graphWindow1.Invalidate();

            } catch( ObjectDisposedException ) { }
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
            State.MutationLog.Clear();
            LastMutationtCounter = 0;
            State.MutationCounter = 0;
            State.BestMatch = State.Initializer.Initialize( new Random(), State );
        }

        void Start( bool reset ) {
            cInitializer.Enabled = false;
            nPolygons.Enabled = false;
            nVertices.Enabled = false;
            if( reset ) {
                Reset();
            } else {
                LastMutationtCounter = State.MutationCounter;
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


        private void cInitializer_SelectedIndexChanged( object sender, EventArgs e ) {
            switch( cInitializer.SelectedIndex ) {
                case 0:
                    State.Initializer = new SolidColorInitializer( Color.Black ); break;
                case 1:
                    State.Initializer = new SegmentedInitializer( Color.Black ); break;
                case 2:
                    State.Initializer = new RadialInitializer( Color.Black ); break;
            }
        }


        private void cMutator_SelectedIndexChanged( object sender, EventArgs e ) {
            switch( cMutator.SelectedIndex ) {
                case 0:
                    State.Mutator = new HarderMutator(); break;
                case 1:
                    State.Mutator = new HardMutator(); break;
                case 2:
                    State.Mutator = new MediumMutator(); break;
                case 3:
                    State.Mutator = new SoftMutator( 8, 12 ); break;
                case 4:
                    State.Mutator = new SoftMutator( 1, 2 ); break;
                case 5:
                    State.Mutator = new TranslateMutator {
                        PreserveAspectRatio = true
                    }; break;
                case 6:
                    State.Mutator = new TranslateMutator(); break;
                case 7:
                    State.Mutator = new TranslateMutator {
                        PreserveAspectRatio = true,
                        EnableRotation = true
                    }; break;
                case 8:
                    State.Mutator = new TranslateMutator {
                        EnableRotation = true
                    }; break;
                case 9:
                    State.Mutator = new SoftTranslateMutator {
                        PreserveAspectRatio = true
                    }; break;
                case 10:
                    State.Mutator = new SoftTranslateMutator(); break;
                case 11:
                    State.Mutator = new SoftTranslateMutator {
                        PreserveAspectRatio = true,
                        EnableRotation = true
                    }; break;
                case 12:
                    State.Mutator = new SoftTranslateMutator {
                        EnableRotation = true
                    }; break;
                case 13:
                    State.Mutator = new HardishMutator {
                        MaxColorDelta = 16,
                        MaxPosDelta = 64,
                        MaxOverlap = 6
                    }; break;
            }
        }


        private void cEvaluator_SelectedIndexChanged( object sender, EventArgs e ) {
            switch( cEvaluator.SelectedIndex ) {
                case 0:
                    State.SetEvaluator( new SloppyRGBEvaluator() ); break;
                case 1:
                    State.SetEvaluator( new RGBEvaluator( false ) ); break;
                case 2:
                    State.SetEvaluator( new RGBEvaluator( true ) ); break;
                case 3:
                    State.SetEvaluator( new LumaEvaluator( false ) ); break;
                case 4:
                    State.SetEvaluator( new LumaEvaluator( true ) ); break;
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


        readonly SaveFileDialog exportImageDialog = new SaveFileDialog {
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


        readonly SaveFileDialog exportSVGDialog = new SaveFileDialog {
            Filter = "SVG Image|*.svg",
            Title = "Saving best match image (SVG)..."
        };

        private void bExportVectors_Click( object sender, EventArgs e ) {
            if( exportSVGDialog.ShowDialog() == DialogResult.OK ) {
                State.SerializeSVG().Save( exportSVGDialog.FileName );
            }
        }


        private void bNewProject_Click( object sender, EventArgs e ) {
            if( !stopped ) Stop();
            OpenFileDialog fd = new OpenFileDialog {
                Filter = "Images|*.jpg;*.png;*.bmp;*.gif;*.tiff;*.tga",
                Title = "Creating Project from an Image"
            };
            if( fd.ShowDialog() == DialogResult.OK ) {
                Bitmap image = (Bitmap)Image.FromFile( fd.FileName );
                State = new TaskState();
                SetImage( image );
            } else {
                return;
            }
        }


        readonly SaveFileDialog saveTaskDialog = new SaveFileDialog {
            Filter = "SIE - SuperImageEvolver task|*.sie",
            Title = "Save Project As..."
        };

        private void bSaveProjectAs_Click( object sender, EventArgs e ) {
            if( saveTaskDialog.ShowDialog() == DialogResult.OK ) {
                State.ProjectFileName = saveTaskDialog.FileName;
                using( FileStream fs = File.Create( saveTaskDialog.FileName ) ) {
                    State.Serialize( fs );
                }
                Text = "SuperImageEvolver | " + Path.GetFileName( State.ProjectFileName ) + " | saved " + DateTime.Now;
            }
        }

        private void bSaveProject_Click( object sender, EventArgs e ) {
            if( State.ProjectFileName != null ) {
                using( FileStream fs = File.Create( State.ProjectFileName ) ) {
                    State.Serialize( fs );
                }
                Text = "SuperImageEvolver | " + Path.GetFileName( State.ProjectFileName ) + " | saved " + DateTime.Now;
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
                    State = new TaskState( fs );
                }
                State.ProjectFileName = fd.FileName;
                SetImage( State.OriginalImage );
                Start( false );
            }
        }


        private void bStart_Click( object sender, EventArgs e ) {
            if( stopped ) {
                Start( State.BestMatch == null );
            }
        }

        private void bRestart_Click( object sender, EventArgs e ) {
            if( stopped ) {
                Reset();
                UpdateTick();
                picDiff.Invalidate();
                picBestMatch.Invalidate();
            } else {
                Stop();
                Start( true );
            }
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
            ModuleSettingsDisplay md = new ModuleSettingsDisplay( State.Initializer );
            if( md.ShowDialog() == DialogResult.OK ) {
                State.Initializer = (IInitializer)md.Module;
            }
        }

        private void bEditMutatorSettings_Click( object sender, EventArgs e ) {
            ModuleSettingsDisplay md = new ModuleSettingsDisplay( State.Mutator );
            if( md.ShowDialog() == DialogResult.OK ) {
                State.Mutator = (IMutator)md.Module;
            }
        }

        private void bEditEvaluatorSettings_Click( object sender, EventArgs e ) {
            ModuleSettingsDisplay md = new ModuleSettingsDisplay( State.Evaluator );
            if( md.ShowDialog() == DialogResult.OK ) {
                State.SetEvaluator( (IEvaluator)md.Module );
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
            if( State == null || State.BestMatch == null ) return;
            List<string> parts = new List<string> {
                State.Vertices.ToString(),
                State.Shapes.ToString()
            };
            foreach( Shape shape in State.BestMatch.Shapes ) {
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
                        int r = Int32.Parse(parts[offset]);
                        int g = Int32.Parse(parts[offset+1]);
                        int b = Int32.Parse(parts[offset+2]);
                        int a = (int)(float.Parse(parts[offset+3])*255);
                        shape.Color = Color.FromArgb( a, r, g, b );
                        offset += 4;
                        for( int v = 0; v < State.Vertices; v++ ) {
                            float X = float.Parse( parts[offset] );
                            float Y = float.Parse( parts[offset+1] );
                            shape.Points[v] = new PointF( X, Y );
                            offset += 2;
                        }
                        importedDNA.Shapes[s] = shape;
                    }
                    State.BestMatch = importedDNA;
                    State.SetEvaluator( State.Evaluator );
                    picBestMatch.Invalidate();
                    picDiff.Invalidate();

                } catch( FormatException ex ) {
                    MessageBox.Show( "Could not import DNA!" + Environment.NewLine + ex );
                }
            }
        }

        private void cmDiffShowLastChange_Click( object sender, EventArgs e ) {
            picDiff.ShowLastChange = cmDiffShowLastChange.Checked;
        }
    }
}