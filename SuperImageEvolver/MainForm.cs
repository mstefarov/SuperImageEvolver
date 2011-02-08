using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.IO.Compression;


namespace SuperImageEvolver {
    public partial class MainForm : Form {
        public static TaskState state = new TaskState();
        bool stopped = true;

        const int threadCount = 2;
        Thread[] threads = new Thread[threadCount];

        Bitmap clonedOriginal;


        public MainForm( string[] args ) {
            InitializeComponent();

            ModuleManager.LoadFactories( System.Reflection.Assembly.GetExecutingAssembly() );

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

            picBestMatch.Width = state.ImageWidth;
            picBestMatch.Height = state.ImageHeight;
            picBestMatch.Init( state );
            picBestMatch.Invalidate();

            picDiff.Width = state.ImageWidth;
            picDiff.Height = state.ImageHeight;
            picDiff.Init( state );
            picDiff.Invalidate();
        }


        Dictionary<MutationType, int> mutationCounts = new Dictionary<MutationType, int>();
        Dictionary<MutationType, double> mutationImprovements = new Dictionary<MutationType, double>();


        void Run() {
            Random rand = new Random();
            Bitmap testCanvas = new Bitmap( state.ImageWidth, state.ImageHeight );

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
                            mutationCounts[mutation.LastMutation]++;
                            mutationImprovements[mutation.LastMutation] += improvement;

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

                Thread.Sleep( 750 );
            }
        }

        void UpdateTick() {
            try {
                int mutationDelta = state.MutationCounter - LastMutationtCounter;
                LastMutationtCounter = state.MutationCounter;
                double timeDelta = (DateTime.UtcNow - lastUpdate).TotalSeconds;
                lastUpdate = DateTime.UtcNow;

                tMutationStats.Text = String.Format(
@"Fitness: {0:0.0000}%
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
                   state.MutationCounter - state.LastImprovementMutationCount);
                StringBuilder sb = new StringBuilder( Environment.NewLine );
                foreach( MutationType type in Enum.GetValues( typeof( MutationType ) ) ) {
                    double rate = 0;
                    if( mutationCounts[type] != 0 ) {
                        rate = mutationImprovements[type] / (double)mutationCounts[type];
                    }
                    sb.AppendFormat( "{0} - {1} * {2:0.0000} ({3:0.0000})", type, mutationCounts[type], rate * 100, mutationImprovements[type] * 100 );
                    sb.Append( Environment.NewLine );
                }
                tMutationStats.Text += sb.ToString();
                graphWindow1.Invalidate();

            } catch( ObjectDisposedException ) { }
        }


        Thread updateThread;
        private void bStartStop_Click( object sender, EventArgs e ) {
            bStartStop.Enabled = false;
            if( stopped ) {
                Start(true);
            } else {
                Stop();
            }
            bStartStop.Enabled = true;
        }


        void Start(bool reset) {
            cInitializer.Enabled = false;
            nPolygons.Enabled = false;
            nVertices.Enabled = false;
            if( reset ) {
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

            foreach( MutationType type in Enum.GetValues( typeof( MutationType ) ) ) {
                mutationCounts[type] = 0;
                mutationImprovements[type] = 0;
            }

            state.SetEvaluator( state.Evaluator );

            stopped = false;
            for( int i = 0; i < threads.Length; i++ ) {
                threads[i] = new Thread( Run );
                threads[i].Start();
            }
            updateThread = new Thread( UpdateStatus );
            updateThread.Start();
            bStartStop.Text = "Stop";
        }


        void Stop() {
            stopped = true;
            for( int i = 0; i < threads.Length; i++ ) {
                if( threads[i] != null ) threads[i].Join();
            }
            Application.DoEvents();
            if( updateThread != null ) updateThread.Join();
            bStartStop.Text = "Start";
            cInitializer.Enabled = true;
            nPolygons.Enabled = true;
            nVertices.Enabled = true;
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
                    state.Mutator = new TranslateMutator() { PreserveAspectRatio = true }; break;
                case 6:
                    state.Mutator = new TranslateMutator() { PreserveAspectRatio = false }; break;
            }
        }


        private void cEvaluator_SelectedIndexChanged( object sender, EventArgs e ) {
            switch( cEvaluator.SelectedIndex ) {
                case 0:
                    state.SetEvaluator( new RGBEvaluator( false ) ); break;
                case 1:
                    state.SetEvaluator( new RGBEvaluator( true ) ); break;
                case 2:
                    state.SetEvaluator( new LumaEvaluator( false ) ); break;
                case 3:
                    state.SetEvaluator( new LumaEvaluator( true ) ); break;
            }
        }


        #region Menu

        private void menuExit_Click( object sender, EventArgs e ) {
            Application.Exit();
        }

        private void menuListModules_Click( object sender, EventArgs e ) {
            StringBuilder sb = new StringBuilder();
            IModuleFactory[] factories = ModuleManager.ListAllModules().OrderBy(fac=>fac.Function).ToArray();
            foreach( IModuleFactory factory in factories ) {
                sb.AppendFormat( "{0} {1}", factory.Function, factory.ID );
                sb.AppendLine();
            }
            MessageBox.Show( sb.ToString(), "List of loaded modules" );
        }


        private void menuOriginalImage_Click( object sender, EventArgs e ) {
            picOriginal.Visible = menuOriginalImage.Checked;
        }

        private void menuBestMatchImage_Click( object sender, EventArgs e ) {
            picBestMatch.Visible = menuBestMatchImage.Checked;
        }

        private void menuDifferenceImage_Click( object sender, EventArgs e ) {
            picDiff.Visible = menuDifferenceImage.Checked;
        }

        private void menuStatistics_Click( object sender, EventArgs e ) {
            tMutationStats.Visible = menuStatistics.Checked;
        }

        SaveFileDialog exportImageDialog = new SaveFileDialog {
            Filter = "PNG Image|*.png|TIFF Image|*.tif;*.tiff|BMP Bitmap Image|*.bmp|JPEG Image|*.jpg;*.jpeg",
            Title = "Saving best match image (raster)..."
        };

        private void menuExportImage_Click( object sender, EventArgs e ) {
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
        private void menuExportSVG_Click( object sender, EventArgs e ) {
            if( exportSVGDialog.ShowDialog() == DialogResult.OK ) {
                state.SerializeSVG().Save( exportSVGDialog.FileName );
            }
        }

        private void menuNewTask_Click( object sender, EventArgs e ) {
            if( !stopped ) Stop();
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "Images|*.jpg;*.png;*.bmp;*.gif;*.tiff;*.tga";
            if( fd.ShowDialog() == DialogResult.OK ) {
                Bitmap image = (Bitmap)Bitmap.FromFile( fd.FileName );
                state = new TaskState();
                SetImage( image );
            } else {
                return;
            }
        }

        #endregion

        SaveFileDialog saveTaskDialog = new SaveFileDialog {
            Filter = "SIE - SuperImageEvolver task|*.sie",
            Title = "Saving task..."
        };
        private void menuSaveTask_Click( object sender, EventArgs e ) {
            if( saveTaskDialog.ShowDialog() == DialogResult.OK ) {
                using( FileStream fs = File.Create( saveTaskDialog.FileName ) ) {
                    state.Serialize( fs );
                }
            }
        }

        private void menuOpenTask_Click( object sender, EventArgs e ) {
            if( !stopped ) Stop();
            OpenFileDialog fd = new OpenFileDialog {
                Filter = "SIE - SuperImageEvolver task|*.sie",
                Title = "Opening task..."
            };
            if( fd.ShowDialog() == DialogResult.OK ) {
                using( FileStream fs = File.OpenRead( fd.FileName ) ) {
                    state = new TaskState( fs );
                }
                SetImage( state.Image );
                Start( false );
            }
        }
    }

    public enum MutationType {
        ReplaceShape,
        ReplaceColor,
        ReplacePoint,
        ReplacePoints,
        AdjustColor,
        AdjustPoint,
        AdjustPoints,
        SwapShapes
    }
}