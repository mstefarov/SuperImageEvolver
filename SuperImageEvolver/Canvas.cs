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

        public DNA DNA;
        public bool Wireframe { get; set; }
        public bool ShowLastChange { get; set; }

        protected override void OnPaint( PaintEventArgs e ) {
            Graphics g = e.Graphics;
            g.Clear( Color.White );
            DNA tempDNA = DNA;
            if( tempDNA != null ) {
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