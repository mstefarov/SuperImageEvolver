using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace SuperImageEvolver {
    sealed unsafe partial class DiffCanvas : UserControl {
        public DiffCanvas() {
            InitializeComponent();
            DoubleBuffered = true;
        }


        public void Init( TaskState _state ) {
            state = _state;
            canvasImage = new Bitmap( state.ImageWidth, state.ImageHeight );
            Zoom = zoom;
        }


        #region Properties

        public bool Invert {
            get { return invert; }
            set {
                invert = value;
                Invalidate();
            }
        }

        bool invert;


        public bool ShowColor {
            get { return showColor; }
            set {
                showColor = value;
                Invalidate();
            }
        }

        bool showColor = true;


        public bool Exaggerate {
            get { return exaggerate; }
            set {
                exaggerate = value;
                Invalidate();
            }
        }

        bool exaggerate = true;


        public float Zoom {
            get { return zoom; }
            set {
                zoom = value;
                if( state != null ) {
                    Size = new Size {
                        Width = (int)Math.Ceiling( state.ImageWidth*Zoom ),
                        Height = (int)Math.Ceiling( state.ImageHeight*Zoom )
                    };
                }
                Invalidate();
            }
        }

        float zoom = 1;


        public bool ShowLastChange {
            get { return showLastChange; }
            set {
                showLastChange = value;
                Invalidate();
            }
        }

        bool showLastChange;



        public TaskState State {
            get { return state; }
            set {
                state = value;
                if( state != null ) {
                    Size = new Size {
                        Width = (int)Math.Ceiling( state.ImageWidth*Zoom ),
                        Height = (int)Math.Ceiling( state.ImageHeight*Zoom )
                    };
                }
                Invalidate();
            }
        }

        TaskState state;

        #endregion


        const string PlaceholderText = "differences";
        Bitmap canvasImage;
        
        protected override void OnPaint( PaintEventArgs e ) {
            Graphics g2 = e.Graphics;
            if( state != null && state.CurrentMatch != null ) {
                e.Graphics.ScaleTransform( zoom, zoom );
                DNA tempDNA = state.CurrentMatch;
                state.Evaluator.DrawDivergence(canvasImage, tempDNA, state, Invert, Exaggerate);

                //using( Graphics g = Graphics.FromImage( canvasImage ) ) {
                //    g.Clear( state.ProjectOptions.Matte );

                //    g.SmoothingMode = (state.Evaluator.Smooth ? SmoothingMode.HighQuality : SmoothingMode.HighSpeed);
                //    for( int i = 0; i < tempDNA.Shapes.Length; i++ ) {
                //        g.FillPolygon( new SolidBrush( tempDNA.Shapes[i].Color ),
                //                       tempDNA.Shapes[i].Points,
                //                       FillMode.Alternate );
                //    }
                //}

                //BitmapData testData = canvasImage.LockBits( new Rectangle( Point.Empty, canvasImage.Size ),
                //                                            ImageLockMode.ReadOnly,
                //                                            PixelFormat.Format32bppArgb );
                //for( int i = 0; i < canvasImage.Height; i++ ) {
                //    byte* originalPointer = (byte*)state.WorkingImageData.Scan0 + state.WorkingImageData.Stride*i;
                //    byte* testPointer = (byte*)testData.Scan0 + testData.Stride*i;
                //    for( int j = 0; j < state.ImageWidth; j++ ) {
                //        if( !showColor ) {
                //            byte val;
                //            int originalLumi =
                //                (Math.Min( Math.Min( originalPointer[2], originalPointer[1] ), *originalPointer ) +
                //                 Math.Max( Math.Max( originalPointer[2], originalPointer[1] ), *originalPointer ))/2;
                //            int testLumi = (Math.Min( Math.Min( testPointer[2], testPointer[1] ), *testPointer ) +
                //                            Math.Max( Math.Max( testPointer[2], testPointer[1] ), *testPointer ))/2;

                //            if( exaggerate ) {
                //                double exaggeratedVal = 127 -
                //                                        Math.Sign( originalLumi - testLumi )*
                //                                        Math.Sqrt( Math.Abs( originalLumi - testLumi )/255d )*255d;
                //                val = (byte)Math.Max( 0, Math.Min( 255, exaggeratedVal ) );
                //            } else {
                //                val = (byte)Math.Max( 0, Math.Min( 255, 127 - (originalLumi - testLumi) ) );
                //            }

                //            if( invert ) val = (byte)(255 - val);
                //            testPointer[2] = val;
                //            testPointer[1] = val;
                //            *testPointer = val;

                //        } else if( invert ) {
                //            if( exaggerate ) {
                //                testPointer[2] =
                //                    (byte)
                //                    (255 -
                //                     (int)(255*Math.Sqrt( Math.Abs( originalPointer[2] - testPointer[2] )/255d )));
                //                testPointer[1] =
                //                    (byte)
                //                    (255 -
                //                     (int)(255*Math.Sqrt( Math.Abs( originalPointer[1] - testPointer[1] )/255d )));
                //                *testPointer =
                //                    (byte)
                //                    (255 -
                //                     (int)(255*Math.Sqrt( Math.Abs( *originalPointer - *testPointer )/255d )));
                //            } else {
                //                testPointer[2] = (byte)(255 - Math.Abs( originalPointer[2] - testPointer[2] ));
                //                testPointer[1] = (byte)(255 - Math.Abs( originalPointer[1] - testPointer[1] ));
                //                *testPointer = (byte)(255 - Math.Abs( *originalPointer - *testPointer ));
                //            }

                //        } else {
                //            if( exaggerate ) {
                //                testPointer[2] =
                //                    (byte)(255*Math.Sqrt( Math.Abs( originalPointer[2] - testPointer[2] )/255d ));
                //                testPointer[1] =
                //                    (byte)(255*Math.Sqrt( Math.Abs( originalPointer[1] - testPointer[1] )/255d ));
                //                *testPointer =
                //                    (byte)(255*Math.Sqrt( Math.Abs( *originalPointer - *testPointer )/255d ));
                //            } else {
                //                testPointer[2] = (byte)Math.Abs( originalPointer[2] - testPointer[2] );
                //                testPointer[1] = (byte)Math.Abs( originalPointer[1] - testPointer[1] );
                //                *testPointer = (byte)Math.Abs( *originalPointer - *testPointer );
                //            }
                //        }
                //        originalPointer += 4;
                //        testPointer += 4;
                //    }
                //}
                //canvasImage.UnlockBits( testData );
                if( zoom == 2 || zoom == 1 ) {
                    g2.InterpolationMode = InterpolationMode.NearestNeighbor;
                }
                g2.DrawImageUnscaled( canvasImage, 0, 0 );

                if( showLastChange ) {
                    Pen lastChangePen1 = new Pen( state.ProjectOptions.LastChangeColor1, 2/zoom );
                    Pen lastChangePen2 = new Pen( state.ProjectOptions.LastChangeColor2, 1/zoom );
                    g2.SmoothingMode = (state.Evaluator.Smooth ? SmoothingMode.HighQuality : SmoothingMode.HighSpeed);
                    for( int i = 0; i < tempDNA.Shapes.Length; i++ ) {
                        if( tempDNA.Shapes[i].PreviousState != null ) {
                            g2.DrawPolygon( lastChangePen1, tempDNA.Shapes[i].Points );
                            g2.DrawPolygon( lastChangePen2, tempDNA.Shapes[i].PreviousState.Points );
                        }
                    }
                }

            } else {
                SizeF align = g2.MeasureString( PlaceholderText, Font );
                g2.DrawString( PlaceholderText,
                               Font,
                               Brushes.White,
                               Width/2f - align.Width/2,
                               Height/2f - align.Height/2 );
            }
            base.OnPaint( e );
        }
    }
}
