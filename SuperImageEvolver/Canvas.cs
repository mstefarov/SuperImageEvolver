using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SuperImageEvolver {
    sealed partial class Canvas : UserControl {
        public Canvas() {
            InitializeComponent();
            DoubleBuffered = true;
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

        bool wireframe;

        [DefaultValue( false )]
        public bool Wireframe {
            get { return wireframe; }
            set {
                wireframe = value;
                Invalidate();
            }
        }


        bool showLastChange;

        [DefaultValue( false )]
        public bool ShowLastChange {
            get { return showLastChange; }
            set {
                showLastChange = value;
                Invalidate();
            }
        }


        const string PlaceholderText = "best match";




        protected override void OnPaint( PaintEventArgs e ) {
            Graphics g = e.Graphics;
            if( state != null && state.BestMatch != null ) {
                g.Clear( state.ProjectOptions.Matte );
                e.Graphics.ScaleTransform( zoom, zoom );
                DNA tempDNA = state.BestMatch;
                g.SmoothingMode = ( state.Evaluator.Smooth ? SmoothingMode.HighQuality : SmoothingMode.HighSpeed );

                Pen wireframePen = new Pen( state.ProjectOptions.WireframeColor, 1 / zoom );

                for( int i = 0; i < tempDNA.Shapes.Length; i++ ) {
                    g.FillPolygon( new SolidBrush( tempDNA.Shapes[i].Color ), tempDNA.Shapes[i].Points, FillMode.Winding );
                    if( Wireframe ) {
                        g.DrawPolygon( wireframePen, tempDNA.Shapes[i].Points );
                    }
                }
                if( showLastChange ) {
                    Pen lastChangePen1 = new Pen( state.ProjectOptions.LastChangeColor1, 2 / zoom );
                    Pen lastChangePen2 = new Pen( state.ProjectOptions.LastChangeColor2, 1 / zoom );
                    for( int i = 0; i < tempDNA.Shapes.Length; i++ ) {
                        if( tempDNA.Shapes[i].PreviousState != null ) {
                            g.DrawPolygon( lastChangePen1, tempDNA.Shapes[i].Points );
                            g.DrawPolygon( lastChangePen2, tempDNA.Shapes[i].PreviousState.Points );
                        }
                    }
                }
            } else {
                g.Clear( Color.White );
                SizeF align = g.MeasureString( PlaceholderText, Font );
                g.DrawString( PlaceholderText, Font, Brushes.Black,
                              Width / 2f - align.Width / 2,
                              Height / 2f - align.Height / 2 );
            }
            base.OnPaint( e );
        }
    }
}