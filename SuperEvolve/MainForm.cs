using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;


namespace SuperImageEvolver {
    public partial class MainForm : Form {
        TaskState state = new TaskState();
        bool stopped = true;

        const int threadCount = 2;
        Thread[] threads = new Thread[threadCount];

        Bitmap clonedOriginal;

        public MainForm( string[] args ) {
            InitializeComponent();

            Shown += delegate( object sender, EventArgs eventArgs ) {
                if( args.Length == 1 ) {
                    state.Image = (Bitmap)Bitmap.FromFile( args[0] );
                } else if( File.Exists( "original.png" ) ) {
                    state.Image = (Bitmap)Bitmap.FromFile( "original.png" );
                } else {
                    OpenFileDialog fd = new OpenFileDialog();
                    fd.Filter = "Images|*.jpg;*.png;*.bmp;*.gif;*.tiff;*.tga";
                    if( fd.ShowDialog() == DialogResult.OK ) {
                        state.Image = (Bitmap)Bitmap.FromFile( fd.FileName );
                    } else {
                        Application.Exit();
                        return;
                    }
                }

                clonedOriginal = (Bitmap)state.Image.Clone();
                state.ImageData = clonedOriginal.LockBits( new Rectangle( Point.Empty, state.Image.Size ),
                                                                ImageLockMode.ReadOnly,
                                                                PixelFormat.Format32bppArgb );
                state.ImageWidth = state.Image.Width;
                state.ImageHeight = state.Image.Height;

                pictureBox1.Width = state.ImageWidth;
                pictureBox1.Height = state.ImageHeight;

                canvas1.Width = state.ImageWidth;
                canvas1.Height = state.ImageHeight;
                pictureBox1.Image = state.Image;

                diffCanvas1.Width = state.ImageWidth;
                diffCanvas1.Height = state.ImageHeight;
                diffCanvas1.Init( state );

                comboBox1.SelectedIndex = 1;
                comboBox2.SelectedIndex = 1;
                comboBox3.SelectedIndex = 0;
            };

            FormClosing += delegate( object sender, FormClosingEventArgs e ) {
                stopped = true;
            };
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
                            state.BestMatch = mutation;
                            canvas1.DNA = state.BestMatch;
                            canvas1.Invalidate();
                            diffCanvas1.Invalidate();
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

                textBox1.Text = String.Format(
@"Fitness: {0:0.0000}%
Improvements: {1} ({2:0.00}/s)
Mutations: {3} ({4:0}/s)
Elapsed: {5}",
                   100 - state.BestMatch.Divergence * 100,
                   state.ImprovementCounter,
                   state.ImprovementCounter / DateTime.UtcNow.Subtract( state.TaskStart ).TotalSeconds,
                   state.MutationCounter,
                   mutationDelta / timeDelta,
                   DateTime.UtcNow.Subtract( state.TaskStart ) );
                StringBuilder sb = new StringBuilder();
                foreach( MutationType type in Enum.GetValues( typeof( MutationType ) ) ) {
                    double rate = 0;
                    if( mutationCounts[type] != 0 ) {
                        rate = mutationImprovements[type] / (double)mutationCounts[type];
                    }
                    sb.AppendFormat( "{0} - {1} * {2:0.0000} ({3:0.0000})", type, mutationCounts[type], rate * 100, mutationImprovements[type] * 100 );
                    sb.Append( Environment.NewLine );
                }
                mutationStats.Text = sb.ToString();

            } catch( ObjectDisposedException ) { }
        }


        Thread updateThread;
        private void button1_Click( object sender, EventArgs e ) {
            button1.Enabled = false;
            if( stopped ) {
                Start();
            } else {
                Stop();
            }
            button1.Enabled = true;
        }

        void Start() {
            comboBox1.Enabled = false;
            numericUpDown1.Enabled = false;
            numericUpDown2.Enabled = false;
            state.TaskStart = DateTime.UtcNow;
            state.Shapes = (int)numericUpDown1.Value;
            state.Vertices = (int)numericUpDown2.Value;
            state.ImprovementCounter = 0;
            LastMutationtCounter = 0;
            state.MutationCounter = 0;

            foreach( MutationType type in Enum.GetValues( typeof( MutationType ) ) ) {
                mutationCounts[type] = 0;
                mutationImprovements[type] = 0;
            }

            state.BestMatch = state.Initializer.Initialize( new Random(), state );
            state.SetEvaluator( state.Evaluator );

            stopped = false;
            for( int i = 0; i < threads.Length; i++ ) {
                threads[i] = new Thread( Run );
                threads[i].Start();
            }
            updateThread = new Thread( UpdateStatus );
            updateThread.Start();
            button1.Text = "Stop";
        }

        void Stop() {
            stopped = true;
            for( int i = 0; i < threads.Length; i++ ) {
                if( threads[i] != null ) threads[i].Join();
            }
            Application.DoEvents();
            if( updateThread != null ) updateThread.Join();
            button1.Text = "Start";
            comboBox1.Enabled = true;
            numericUpDown1.Enabled = true;
            numericUpDown2.Enabled = true;
        }


        private void comboBox1_SelectedIndexChanged( object sender, EventArgs e ) {
            switch( comboBox1.SelectedIndex ) {
                case 0:
                    state.Initializer = new SolidColorInitializer( Color.Black ); break;
                case 1:
                    state.Initializer = new SegmentedInitializer( Color.Black ); break;
            }
        }

        private void comboBox2_SelectedIndexChanged( object sender, EventArgs e ) {
            switch( comboBox2.SelectedIndex ) {
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
            }
        }

        private void comboBox3_SelectedIndexChanged( object sender, EventArgs e ) {
            switch( comboBox3.SelectedIndex ) {
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