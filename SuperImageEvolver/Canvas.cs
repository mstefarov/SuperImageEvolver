using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;


namespace SuperImageEvolver {
    partial class Canvas : UserControl {
        public Canvas() {
            InitializeComponent();
            DoubleBuffered = true;

            MouseClick += delegate( object sender, MouseEventArgs e ) {
                switch( e.Button ) {
                    case MouseButtons.Left:
                        state.ClickLocation = this.PointToClient( MousePosition );
                        break;

                    case MouseButtons.Middle:
                        state.ClickLocation = default( Point );
                        break;
                }
            };
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

        bool _wireframe;
        [DefaultValue( false )]
        public bool Wireframe {
            get { return _wireframe; }
            set { _wireframe = value; Invalidate(); }
        }


        bool _showLastChange;
        [DefaultValue( false )]
        public bool ShowLastChange {
            get { return _showLastChange; }
            set { _showLastChange = value; Invalidate(); }
        }


        const string PlaceholderText = "best match";


        static readonly Pen LastChangePen = new Pen( Color.White, 2 ) {
            EndCap = LineCap.Round
        };
        static readonly Pen HighlightPen = new Pen( Color.Red, 2 ) {
            EndCap = LineCap.Round
        };


        protected override void OnPaint( PaintEventArgs e ) {
            Graphics g = e.Graphics;
            g.Clear( Color.White );
            if( state != null && state.BestMatch != null ) {
                e.Graphics.ScaleTransform( _zoom, _zoom );
                LastChangePen.Width = 2 / _zoom;
                DNA tempDNA = state.BestMatch;
                g.SmoothingMode = (state.Evaluator.Smooth ? SmoothingMode.HighQuality : SmoothingMode.HighSpeed);

                for( int i = 0; i < tempDNA.Shapes.Length; i++ ) {
                    g.FillPolygon( new SolidBrush( tempDNA.Shapes[i].Color ), tempDNA.Shapes[i].Points, FillMode.Winding );
                    if( Wireframe ) {
                        g.DrawPolygon( new Pen( Brushes.Black, 1 / _zoom ), tempDNA.Shapes[i].Points );
                    }
                }
                if( _showLastChange ) {
                    for( int i = 0; i < tempDNA.Shapes.Length; i++ ) {
                        if( tempDNA.Shapes[i].PreviousState != null ) {
                            g.DrawPolygon( LastChangePen, tempDNA.Shapes[i].Points );
                            g.DrawPolygon( new Pen( Brushes.Black, 1 / _zoom ), tempDNA.Shapes[i].PreviousState.Points );
                        }

                        if( tempDNA.Shapes[i].Highlight ) {
                            g.DrawPolygon( HighlightPen, tempDNA.Shapes[i].Points );
                            g.DrawPolygon( Pens.White, tempDNA.Shapes[i].Points );
                        }
                    }
                }
            } else {
                SizeF align = g.MeasureString( PlaceholderText, Font );
                g.DrawString( PlaceholderText, Font, Brushes.Black, Width / 2 - align.Width / 2, Height / 2 - align.Height / 2 );
            }
            base.OnPaint( e );
        }
    }
}