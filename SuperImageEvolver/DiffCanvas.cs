using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.ComponentModel;


namespace SuperImageEvolver {
    unsafe partial class DiffCanvas : UserControl {
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

        bool _invert = false;
        [DefaultValue( false )]
        public bool Invert {
            get { return _invert; }
            set { _invert = value; Invalidate(); }
        }


        bool _showColor = true;
        [DefaultValue( true )]
        public bool ShowColor {
            get { return _showColor; }
            set { _showColor = value; Invalidate(); }
        }


        bool _exaggerate = true;
        [DefaultValue( true )]
        public bool Exaggerate {
            get { return _exaggerate; }
            set { _exaggerate = value; Invalidate(); }
        }


        float _zoom = 1;
        [DefaultValue( 1 )]
        public float Zoom {
            get { return _zoom; }
            set {
                _zoom = value;
                if( state != null ) {
                    Size = new Size {
                        Width = (int)Math.Ceiling( state.ImageWidth * Zoom ),
                        Height = (int)Math.Ceiling( state.ImageHeight * Zoom )
                    };
                }
                Invalidate();
            }
        }


        bool _showLastChange;
        [DefaultValue( false )]
        public bool ShowLastChange {
            get { return _showLastChange; }
            set { _showLastChange = value; Invalidate(); }
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
                e.Graphics.ScaleTransform( _zoom, _zoom );
                DNA tempDNA = state.BestMatch;
                using( Graphics g = Graphics.FromImage( canvasImage ) ) {
                    g.Clear( Color.White );

                    g.SmoothingMode = (state.Evaluator.Smooth ? SmoothingMode.HighQuality : SmoothingMode.HighSpeed);
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
                        if( !_showColor ) {
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

                            val = (byte)Math.Abs( originalLumi - testLumi );

                            /*
                            int oringinalChroma = (Math.Max( Math.Max( originalPointer[2], originalPointer[1] ), *originalPointer ) -
                                                   Math.Min( Math.Min( originalPointer[2], originalPointer[1] ), *originalPointer ));
                            int testChroma = (Math.Max( Math.Max( testPointer[2], testPointer[1] ), *testPointer ) -
                                              Math.Min( Math.Min( testPointer[2], testPointer[1] ), *testPointer ));

                            if( val != 0 ) {
                                val = (byte)(255 * Math.Sqrt( (val / 2 + Math.Abs( oringinalChroma - testChroma ) * val / (255 * 2)) / 255d ));
                            }
                            */
                            if( _exaggerate ) {
                                val = (byte)Math.Max( 0, Math.Min( 255, 127 - Math.Sign( originalLumi - testLumi ) * Math.Sqrt( Math.Abs( originalLumi - testLumi ) / 255d ) * 255d ) );
                            } else {
                                val = (byte)Math.Max( 0, Math.Min( 255, 127 - (originalLumi - testLumi) ) );
                            }

                            if( _invert ) val = (byte)(255 - val);
                            testPointer[2] = val;
                            testPointer[1] = val;
                            *testPointer = val;

                        } else if( _invert ) {
                            if( _exaggerate ) {
                                testPointer[2] = (byte)(255 - (int)(255 * Math.Sqrt( Math.Abs( originalPointer[2] - testPointer[2] ) / 255d )));
                                testPointer[1] = (byte)(255 - (int)(255 * Math.Sqrt( Math.Abs( originalPointer[1] - testPointer[1] ) / 255d )));
                                *testPointer = (byte)(255 - (int)(255 * Math.Sqrt( Math.Abs( *originalPointer - *testPointer ) / 255d )));
                            } else {
                                testPointer[2] = (byte)(255 - Math.Abs( originalPointer[2] - testPointer[2] ));
                                testPointer[1] = (byte)(255 - Math.Abs( originalPointer[1] - testPointer[1] ));
                                *testPointer = (byte)(255 - Math.Abs( *originalPointer - *testPointer ));
                            }

                        } else {
                            if( _exaggerate ) {
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
                if( _zoom == 2 || _zoom == 1 ) {
                    g2.InterpolationMode = InterpolationMode.NearestNeighbor;
                }
                g2.DrawImageUnscaled( canvasImage, 0, 0 );

                if( _showLastChange ) {
                    g2.SmoothingMode = (state.Evaluator.Smooth ? SmoothingMode.HighQuality : SmoothingMode.HighSpeed);
                    for( int i = 0; i < tempDNA.Shapes.Length; i++ ) {
                        if( tempDNA.Shapes[i].PreviousState != null ) {
                            g2.DrawPolygon( LastChangePen, tempDNA.Shapes[i].Points );
                            g2.DrawPolygon( new Pen( Brushes.Black, 1 / _zoom ), tempDNA.Shapes[i].PreviousState.Points );
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