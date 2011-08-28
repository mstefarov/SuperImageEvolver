using System;
using System.Drawing;
using System.Windows.Forms;

namespace SuperImageEvolver {
    public sealed partial class GraphWindow : UserControl {
        PointF[] Points { get; set; }
        readonly Font font = new Font( FontFamily.GenericSansSerif, 15, FontStyle.Bold );
        const float LogSteepness = 100;


        public GraphWindow() {
            InitializeComponent();
            DoubleBuffered = true;
        }


        readonly Pen pen = new Pen( Color.Black, 2 );

        protected override void OnPaint( PaintEventArgs e ) {
            Graphics g = e.Graphics;
            g.Clear( Color.White );
            for( int i = 1; i < 10; i++ ) {
                float level = (float)(Math.Log( i / 10f * LogSteepness + 1 ) / Math.Log( LogSteepness + 1 )) * Height;
                g.DrawLine( Pens.Silver, 0, level, Width, level );
            }
            if( Points != null && Points.Length > 1 ) {
                try {
                    g.DrawLines( pen, Points );
                } catch( Exception ex ) {
                    Console.WriteLine( ex );
                }
            }
            if( MainForm.State != null && MainForm.State.BestMatch != null ) {
                g.DrawString( (1 - MainForm.State.BestMatch.Divergence).ToString( "0.0000%" ), font, Brushes.Black, PointF.Empty );
            }
            base.OnPaint( e );
        }


        public void SetData( PointF[] input, bool logXAxis, bool logYAxis, bool normalizeYStart, bool normalizeYEnd, bool normalizeXStart, bool normalizeXEnd ) {
            if( input.Length < 2 ) {
                Points = null;
            }
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

            if( !normalizeXStart ) minX = 0;
            if( !normalizeYStart ) minY = 0;
            if( !normalizeXEnd ) maxX = 1;
            if( !normalizeYEnd ) maxY = 1;

            float multiplierX = 1 / (maxX - minX);
            float constantX = -minX / (maxX - minX);
            float multiplierY = 1 / (maxY - minY);
            float constantY = -minY / (maxY - minY);

            double logScaleMultiplier = 1 / Math.Log( LogSteepness + 1 );

            for( int i = 0; i < input.Length; i++ ) {
                output[i].X = input[i].X * multiplierX + constantX; // normalize
                if( logXAxis ) {
                    output[i].X = (float)(Math.Log( output[i].X * LogSteepness + 1 ) * logScaleMultiplier); // scale
                }
                output[i].X = (Width - 1) * output[i].X;


                output[i].Y = input[i].Y * multiplierY + constantY; // normalize
                if( logYAxis ) {
                    output[i].Y = (float)(Math.Log( output[i].Y * LogSteepness + 1 ) * logScaleMultiplier); // scale
                }
                output[i].Y = (Height - 1) * output[i].Y;
            }

            Points = output;
        }
    }
}