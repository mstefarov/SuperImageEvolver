using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;


namespace SuperImageEvolver {
    partial class Canvas : UserControl {
        public Canvas() {
            InitializeComponent();
            DoubleBuffered = true;
            Click += delegate( object sender, EventArgs e ) {
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
            };
        }

        public void Init( TaskState _state ) {
            state = _state;
        }
        TaskState state;

        public bool Wireframe { get; set; }
        public bool ShowLastChange { get; set; }
        const string PlaceholderText = "best match";

        protected override void OnPaint( PaintEventArgs e ) {
            Graphics g = e.Graphics;
            g.Clear( Color.White );
            DNA tempDNA = state.BestMatch;
            if( tempDNA == null ) {
                SizeF align = g.MeasureString( PlaceholderText, Font );
                g.DrawString( PlaceholderText, Font, Brushes.Black, Width / 2 - align.Width / 2, Height / 2 - align.Height / 2 );
            }else{
                g.SmoothingMode = SmoothingMode.HighQuality;
                for( int i = 0; i < tempDNA.Shapes.Length; i++ ) {
                    g.FillPolygon( new SolidBrush( tempDNA.Shapes[i].Color ), tempDNA.Shapes[i].Points, FillMode.Winding );
                    if( Wireframe ) {
                        g.DrawPolygon( Pens.Black, tempDNA.Shapes[i].Points );
                    }
                }
                if( ShowLastChange ) {
                    for( int i = 0; i < tempDNA.Shapes.Length; i++ ) {
                        if( tempDNA.Shapes[i].Changed ) {
                            g.DrawPolygon( Pens.White, tempDNA.Shapes[i].Points );
                            g.DrawPolygon( Pens.Black, tempDNA.Shapes[i].PreviousState.Points );
                        }
                    }
                }
            }
            base.OnPaint( e );
        }
    }
}