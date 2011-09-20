using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.ComponentModel;


namespace SuperImageEvolver {
    sealed unsafe partial class DiffCanvas : UserControl {
        public DiffCanvas() {
            InitializeComponent();
            DoubleBuffered = true;
            Invert = false;
            ShowColor = true;
            Exaggerate = true;
        }


        public void Init( TaskState _state ) {
            state = _state;
            canvasImage = new Bitmap( state.ImageWidth, state.ImageHeight );
        }


        #region Properties

        bool invert;
        [DefaultValue( false )]
        public bool Invert {
            get { return invert; }
            set { invert = value; Invalidate(); }
        }


        bool showColor = true;
        [DefaultValue( true )]
        public bool ShowColor {
            get { return showColor; }
            set { showColor = value; Invalidate(); }
        }


        bool exaggerate = true;
        [DefaultValue( true )]
        public bool Exaggerate {
            get { return exaggerate; }
            set { exaggerate = value; Invalidate(); }
        }


        float zoom = 1;
        [DefaultValue( 1 )]
        public float Zoom {
            get { return zoom; }
            set {
                zoom = value;
                if( state != null ) {
                    Size = new Size {
                        Width = (int)Math.Ceiling( state.ImageWidth * Zoom ),
                        Height = (int)Math.Ceiling( state.ImageHeight * Zoom )
                    };
                }
                Invalidate();
            }
        }


        bool showLastChange;
        [DefaultValue( false )]
        public bool ShowLastChange {
            get { return showLastChange; }
            set { showLastChange = value; Invalidate(); }
        }



        TaskState state;
        public TaskState State {
            get { return state; }
            set {
                state = value;
                if( state != null ) {
                    Size = new Size {
                        Width = (int)Math.Ceiling( state.ImageWidth * Zoom ),
                        Height = (int)Math.Ceiling( state.ImageHeight * Zoom )
                    };
                }
                Invalidate();
            }
        }

        #endregion


        Bitmap canvasImage;

        const string PlaceholderText = "differences";

        static readonly Pen LastChangePen = new Pen( Color.White, 2 ) {
            EndCap = LineCap.Round
        };

        protected override void OnPaint( PaintEventArgs e ) {
            Graphics g2 = e.Graphics;
            if( state != null && state.BestMatch != null ) {
                e.Graphics.ScaleTransform( zoom, zoom );
                DNA tempDNA = state.BestMatch;
                using( Graphics g = Graphics.FromImage( canvasImage ) ) {
                    g.Clear( state.ProjectOptions.Matte );

                    g.SmoothingMode = (state.Evaluator.Smooth ? SmoothingMode.HighQuality : SmoothingMode.HighSpeed);
                    for( int i = 0; i < tempDNA.Shapes.Length; i++ ) {
                        g.FillPolygon( new SolidBrush( tempDNA.Shapes[i].Color ), tempDNA.Shapes[i].Points, FillMode.Alternate );
                    }
                }

                BitmapData testData = canvasImage.LockBits( new Rectangle( Point.Empty, canvasImage.Size ),
                                                            ImageLockMode.ReadOnly,
                                                            PixelFormat.Format32bppArgb );
                for( int i = 0; i < canvasImage.Height; i++ ) {
                    byte* originalPointer = (byte*)state.WorkingImageData.Scan0 + state.WorkingImageData.Stride * i;
                    byte* testPointer = (byte*)testData.Scan0 + testData.Stride * i;
                    for( int j = 0; j < state.ImageWidth; j++ ) {
                        if( !showColor ) {
                            byte val;
                            /*int originalLuma = (299 * originalPointer[2] + 587 * originalPointer[1] + 114 * *originalPointer) / 1000;
                            int testLuma = (299 * testPointer[2] + 587 * testPointer[1] + 114 * *testPointer) / 1000;
                            int deltaLuma = (299 * Math.Abs( originalPointer[2] - testPointer[2] ) +
                                            587 * Math.Abs( originalPointer[1] - testPointer[1] ) +
                                            114 * Math.Abs( *originalPointer - *testPointer )) / 1000; // 0-511

                            int deltaU = Math.Abs( (originalPointer[2] - originalLuma) - (testPointer[2] - testLuma) ); // 0-255
                            int deltaV = Math.Abs( (*originalPointer - originalLuma) - (*testPointer - testLuma) ); // 0-255

                            byte val = (byte)(deltaLuma/3);// + (deltaU + deltaV)/2);

                            */
                            int originalLumi = (Math.Min( Math.Min( originalPointer[2], originalPointer[1] ), *originalPointer ) +
                                                Math.Max( Math.Max( originalPointer[2], originalPointer[1] ), *originalPointer )) / 2;
                            int testLumi = (Math.Min( Math.Min( testPointer[2], testPointer[1] ), *testPointer ) +
                                            Math.Max( Math.Max( testPointer[2], testPointer[1] ), *testPointer )) / 2;

                            /*
                            val = (byte)Math.Abs( originalLumi - testLumi );
                            
                            int oringinalChroma = (Math.Max( Math.Max( originalPointer[2], originalPointer[1] ), *originalPointer ) -
                                                   Math.Min( Math.Min( originalPointer[2], originalPointer[1] ), *originalPointer ));
                            int testChroma = (Math.Max( Math.Max( testPointer[2], testPointer[1] ), *testPointer ) -
                                              Math.Min( Math.Min( testPointer[2], testPointer[1] ), *testPointer ));

                            if( val != 0 ) {
                                val = (byte)(255 * Math.Sqrt( (val / 2 + Math.Abs( oringinalChroma - testChroma ) * val / (255 * 2)) / 255d ));
                            }
                            */

                            if( exaggerate ) {
                                val = (byte)Math.Max( 0, Math.Min( 255, 127 - Math.Sign( originalLumi - testLumi ) * Math.Sqrt( Math.Abs( originalLumi - testLumi ) / 255d ) * 255d ) );
                            } else {
                                val = (byte)Math.Max( 0, Math.Min( 255, 127 - (originalLumi - testLumi) ) );
                            }

                            if( invert ) val = (byte)(255 - val);
                            testPointer[2] = val;
                            testPointer[1] = val;
                            *testPointer = val;

                        } else if( invert ) {
                            if( exaggerate ) {
                                testPointer[2] = (byte)(255 - (int)(255 * Math.Sqrt( Math.Abs( originalPointer[2] - testPointer[2] ) / 255d )));
                                testPointer[1] = (byte)(255 - (int)(255 * Math.Sqrt( Math.Abs( originalPointer[1] - testPointer[1] ) / 255d )));
                                *testPointer = (byte)(255 - (int)(255 * Math.Sqrt( Math.Abs( *originalPointer - *testPointer ) / 255d )));
                            } else {
                                testPointer[2] = (byte)(255 - Math.Abs( originalPointer[2] - testPointer[2] ));
                                testPointer[1] = (byte)(255 - Math.Abs( originalPointer[1] - testPointer[1] ));
                                *testPointer = (byte)(255 - Math.Abs( *originalPointer - *testPointer ));
                            }

                        } else {
                            if( exaggerate ) {
                                testPointer[2] = (byte)(255 * Math.Sqrt( Math.Abs( originalPointer[2] - testPointer[2] ) / 255d ));
                                testPointer[1] = (byte)(255 * Math.Sqrt( Math.Abs( originalPointer[1] - testPointer[1] ) / 255d ));
                                *testPointer = (byte)(255 * Math.Sqrt( Math.Abs( *originalPointer - *testPointer ) / 255d ));
                            } else {
                                testPointer[2] = (byte)Math.Abs( originalPointer[2] - testPointer[2] );
                                testPointer[1] = (byte)Math.Abs( originalPointer[1] - testPointer[1] );
                                *testPointer = (byte)Math.Abs( *originalPointer - *testPointer );
                            }
                        }
                        originalPointer += 4;
                        testPointer += 4;
                    }
                }
                canvasImage.UnlockBits( testData );
                if( zoom == 2 || zoom == 1 ) {
                    g2.InterpolationMode = InterpolationMode.NearestNeighbor;
                }
                g2.DrawImageUnscaled( canvasImage, 0, 0 );

                if( showLastChange ) {
                    g2.SmoothingMode = (state.Evaluator.Smooth ? SmoothingMode.HighQuality : SmoothingMode.HighSpeed);
                    for( int i = 0; i < tempDNA.Shapes.Length; i++ ) {
                        if( tempDNA.Shapes[i].PreviousState != null ) {
                            g2.DrawPolygon( LastChangePen, tempDNA.Shapes[i].Points );
                            g2.DrawPolygon( new Pen( Brushes.Black, 1 / zoom ), tempDNA.Shapes[i].PreviousState.Points );
                        }
                    }
                }

            } else {
                SizeF align = g2.MeasureString( PlaceholderText, Font );
                g2.DrawString( PlaceholderText, Font, Brushes.White, Width / 2 - align.Width / 2, Height / 2 - align.Height / 2 );
            }
            base.OnPaint( e );
        }
    }
}