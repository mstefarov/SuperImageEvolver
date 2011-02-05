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
            Inverse = true;

            Click += delegate( object sender, EventArgs e ) {
                if( !Inverse && !Monochrome ) {
                    Inverse = true;
                } else if( Inverse && !Monochrome ) {
                    Monochrome = true;
                } else if( Inverse && Monochrome ) {
                    Inverse = false;
                } else {
                    Monochrome = false;
                }
                Invalidate();
            };
        }


        public void Init( TaskState _state ) {
            state = _state;
            canvasImage = new Bitmap( state.ImageWidth, state.ImageHeight );
        }


        public bool Inverse { get; set; }
        public bool Monochrome { get; set; }
        TaskState state;
        Bitmap canvasImage;


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
                        if( Monochrome ) {

                            /*int originalLuma = (299 * originalPointer[2] + 587 * originalPointer[1] + 114 * *originalPointer) / 1000;
                            int testLuma = (299 * testPointer[2] + 587 * testPointer[1] + 114 * *testPointer) / 1000;
                            int deltaLuma = (299 * Math.Abs( originalPointer[2] - testPointer[2] ) +
                                            587 * Math.Abs( originalPointer[1] - testPointer[1] ) +
                                            114 * Math.Abs( *originalPointer - *testPointer )) / 1000; // 0-511

                            int deltaU = Math.Abs( (originalPointer[2] - originalLuma) - (testPointer[2] - testLuma) ); // 0-255
                            int deltaV = Math.Abs( (*originalPointer - originalLuma) - (*testPointer - testLuma) ); // 0-255

                            byte val = (byte)(deltaLuma/3);// + (deltaU + deltaV)/2);*/


                            int originalLumi = (Math.Min( Math.Min( originalPointer[2], originalPointer[1] ), *originalPointer ) +
                                          Math.Max( Math.Max( originalPointer[2], originalPointer[1] ), *originalPointer )) / 2;
                            int testLumi = (Math.Min( Math.Min( testPointer[2], testPointer[1] ), *testPointer ) +
                                          Math.Max( Math.Max( testPointer[2], testPointer[1] ), *testPointer )) / 2;

                            byte val = (byte)Math.Abs( originalLumi - testLumi );


                            int oringinalChroma = (Math.Max( Math.Max( originalPointer[2], originalPointer[1] ), *originalPointer ) -
                                                   Math.Min( Math.Min( originalPointer[2], originalPointer[1] ), *originalPointer ));
                            int testChroma = (Math.Max( Math.Max( testPointer[2], testPointer[1] ), *testPointer ) -
                                              Math.Min( Math.Min( testPointer[2], testPointer[1] ), *testPointer ));

                            if( val != 0 ) {
                                val = (byte)(255 * Math.Sqrt( (val / 2 + Math.Abs( oringinalChroma - testChroma ) * val / (255*2)) / 255d ));
                            }

                            val = (byte)Math.Max( 0, Math.Min( 255, 127 + (originalLumi - testLumi)*2 ) );

                            /*
                            byte val = (byte)(255 * Math.Sqrt( Math.Abs( (originalPointer[2] + originalPointer[1] + *originalPointer) -
                                                                         (testPointer[2] + testPointer[1] + *testPointer) )/767d ));*/
                            if( Inverse ) val = (byte)(255 - val);
                            testPointer[2] = val;
                            testPointer[1] = val;
                            *testPointer = val;

                        } else if( Inverse ) {
                            testPointer[2] = (byte)(255 - (int)(255 * Math.Sqrt( Math.Abs( originalPointer[2] - testPointer[2] ) / 255d )));
                            testPointer[1] = (byte)(255 - (int)(255 * Math.Sqrt( Math.Abs( originalPointer[1] - testPointer[1] ) / 255d )));
                            *testPointer = (byte)(255 - (int)(255 * Math.Sqrt( Math.Abs( *originalPointer - *testPointer ) / 255d )));
                        } else {
                            testPointer[2] = (byte)(255 * Math.Sqrt( Math.Abs( originalPointer[2] - testPointer[2] ) / 255d ));
                            testPointer[1] = (byte)(255 * Math.Sqrt( Math.Abs( originalPointer[1] - testPointer[1] ) / 255d ));
                            *testPointer = (byte)(255 * Math.Sqrt( Math.Abs( *originalPointer - *testPointer ) / 255d ));
                        }
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