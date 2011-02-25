using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;


namespace SuperImageEvolver {
    partial class Canvas : UserControl {
        public Canvas() {
            InitializeComponent();
            DoubleBuffered = true;

            MouseClick += delegate( object sender, MouseEventArgs e ) {
                switch( e.Button ) {
                    case MouseButtons.Right:
                        if( !Wireframe && !ShowLastChange ) {
                            Wireframe = true;
                        } else if( Wireframe && !ShowLastChange ) {
                            ShowLastChange = true;
                        } else if( Wireframe && ShowLastChange ) {
                            Wireframe = false;
                        } else {
                            ShowLastChange = false;
                        }
                        Invalidate();
                        break;

                    case MouseButtons.Left:
                        state.ClickLocation = this.PointToClient( MousePosition );
                        break;

                    case MouseButtons.Middle:
                        state.ClickLocation = default( Point );
                        break;
                }
            };
        }

        public void Init( TaskState _state ) {
            state = _state;
        }
        TaskState state;

        public bool Wireframe { get; set; }
        public bool ShowLastChange { get; set; }
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
                DNA tempDNA = state.BestMatch;

                g.SmoothingMode = (state.Evaluator.Smooth ? SmoothingMode.HighQuality : SmoothingMode.HighSpeed);

                for( int i = 0; i < tempDNA.Shapes.Length; i++ ) {
                    g.FillPolygon( new SolidBrush( tempDNA.Shapes[i].Color ), tempDNA.Shapes[i].Points, FillMode.Winding );
                    if( Wireframe ) {
                        g.DrawPolygon( Pens.Black, tempDNA.Shapes[i].Points );
                    }
                }
                if( ShowLastChange ) {
                    for( int i = 0; i < tempDNA.Shapes.Length; i++ ) {
                        if( tempDNA.Shapes[i].PreviousState != null ) {
                            g.DrawPolygon( LastChangePen, tempDNA.Shapes[i].Points );
                            g.DrawPolygon( Pens.Black, tempDNA.Shapes[i].PreviousState.Points );
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