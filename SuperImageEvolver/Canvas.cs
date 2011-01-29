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
                Wireframe = !Wireframe;
                Invalidate();
            };
        }

        public DNA DNA;
        public bool Wireframe { get; set; }

        protected override void OnPaint( PaintEventArgs e ) {
            Graphics g = e.Graphics;
            g.Clear( Color.White );
            DNA tempDNA = DNA;
            if( tempDNA != null ) {
                g.SmoothingMode = SmoothingMode.HighQuality;
                for( int i = 0; i < tempDNA.Shapes.Length; i++ ) {
                    if( Wireframe ) {
                        g.DrawPolygon( new Pen( tempDNA.Shapes[i].Color ), tempDNA.Shapes[i].Points );
                    } else {
                        g.FillPolygon( new SolidBrush( tempDNA.Shapes[i].Color ), tempDNA.Shapes[i].Points, FillMode.Winding );
                    }
                }
            }
            base.OnPaint( e );
        }
    }
}