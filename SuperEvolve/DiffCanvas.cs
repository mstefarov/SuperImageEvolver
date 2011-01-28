using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;


namespace SuperImageEvolver {
    unsafe partial class DiffCanvas : UserControl {
        public DiffCanvas() {
            InitializeComponent();
            DoubleBuffered = true;
        }

        public void Init( TaskState _state ) {
            state = _state;
            canvasImage = new Bitmap( state.ImageWidth, state.ImageHeight );
        }

        TaskState state;
        Bitmap canvasImage;
        public bool lumaMode = true;

        protected override void OnPaint( PaintEventArgs e ) {
            Graphics g2 = e.Graphics;
            if( state != null && state.BestMatch != null ) {
                DNA tempDNA = state.BestMatch;
                using( Graphics g = Graphics.FromImage( canvasImage ) ) {
                    g.Clear( Color.White );
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    for( int i = 0; i < tempDNA.Shapes.Length; i++ ) {
                        g.FillPolygon( new SolidBrush( tempDNA.Shapes[i].Color ), tempDNA.Shapes[i].Points, FillMode.Alternate );
                    }
                }

                byte* originalPointer, testPointer;

                BitmapData testData = canvasImage.LockBits( new Rectangle( Point.Empty, canvasImage.Size ),
                                                          ImageLockMode.ReadOnly,
                                                          PixelFormat.Format32bppArgb );
                for( int i = 0; i < canvasImage.Height; i++ ) {
                    originalPointer = (byte*)state.ImageData.Scan0 + state.ImageData.Stride * i;
                    testPointer = (byte*)testData.Scan0 + testData.Stride * i;
                    for( int j = 0; j < state.ImageWidth; j++ ) {

                        testPointer[2] = (byte)(255 - (int)(255 * Math.Sqrt( Math.Abs( originalPointer[2] - testPointer[2] ) / 255d )));
                        testPointer[1] = (byte)(255 - (int)(255 * Math.Sqrt( Math.Abs( originalPointer[1] - testPointer[1] ) / 255d )));
                        *testPointer = (byte)(255 - (int)(255 * Math.Sqrt( Math.Abs( *originalPointer - *testPointer ) / 255d )));
                        originalPointer += 4;
                        testPointer += 4;
                    }
                }
                canvasImage.UnlockBits( testData );
                g2.DrawImageUnscaled( canvasImage, 0, 0 );
            }
            base.OnPaint( e );
        }
    }
}