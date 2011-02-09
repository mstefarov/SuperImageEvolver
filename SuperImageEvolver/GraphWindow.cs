using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SuperImageEvolver {
    public partial class GraphWindow : UserControl {

        public PointF[] Points { get; private set; }
        Font font = new Font( FontFamily.GenericSansSerif, 15, FontStyle.Bold );

        public float LogSteepness = 100;


        public GraphWindow() {
            InitializeComponent();
            this.DoubleBuffered = true;
        }


        protected override void OnPaint( PaintEventArgs e ) {
            if( MainForm.state != null && MainForm.state.BestMatch != null && Points != null && Points.Length > 1 ) {
                Graphics g = e.Graphics;
                g.Clear( Color.White );
                for( int i = 1; i < 10; i++ ) {
                    float level = (float)(Math.Log( i / 10f * LogSteepness + 1 ) / Math.Log( LogSteepness + 1 )) * Height;
                    g.DrawLine( Pens.Silver, 0, level, Width, level );
                }
                g.DrawLines( new Pen(Color.Black,2), Points );
                g.DrawString( (1 - MainForm.state.BestMatch.Divergence).ToString( "0.0000%" ), font, Brushes.Black, PointF.Empty );
            }
            base.OnPaint( e );
        }


        public void SetData( PointF[] input, bool LogXAxis, bool LogYAxis, bool NormalizeYStart, bool NormalizeYEnd, bool NormalizeXStart, bool NormalizeXEnd ) {
            if( input.Length < 2 ) return;
            PointF[] output = new PointF[input.Length];

            float minX = float.MaxValue,
                  maxX = float.MinValue,
                  minY = float.MaxValue,
                  maxY = float.MinValue;


            for( int i = 0; i < input.Length; i++ ) {
                minX = Math.Min( minX, input[i].X );
                maxX = Math.Max( maxX, input[i].X );
                minY = Math.Min( minY, input[i].Y );
                maxY = Math.Max( maxY, input[i].Y );
            }

            if( !NormalizeXStart ) minX = 0;
            if( !NormalizeYStart ) minY = 0;
            if( !NormalizeXEnd ) maxX = 1;
            if( !NormalizeYEnd ) maxY = 1;

            float multiplierX = 1 / (maxX - minX);
            float constantX = -minX / (maxX - minX);
            float multiplierY = 1 / (maxY - minY);
            float constantY = -minY / (maxY - minY);

            double logScaleMultiplier = 1 / Math.Log( LogSteepness + 1 );

            for( int i = 0; i < input.Length; i++ ) {
                output[i].X = input[i].X * multiplierX + constantX; // normalize
                if( LogXAxis ) {
                    output[i].X = (float)(Math.Log( output[i].X * LogSteepness + 1 ) * logScaleMultiplier); // scale
                }
                output[i].X = (float)((Width - 1) * output[i].X);


                output[i].Y = input[i].Y * multiplierY + constantY; // normalize
                if( LogYAxis ) {
                    output[i].Y = (float)(Math.Log( output[i].Y * LogSteepness + 1 ) * logScaleMultiplier); // scale
                }
                output[i].Y = (float)((Height - 1) * output[i].Y);
            }

            Points = output;
        }
    }
}
