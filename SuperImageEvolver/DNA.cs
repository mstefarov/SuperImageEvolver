using System;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Text;


namespace SuperImageEvolver {
    public class DNA : ICloneable {
        public DNA() { }

        public DNA( DNA other ) {
            Shapes = new Shape[other.Shapes.Length];
            for( int i = 0; i < Shapes.Length; i++ ) {
                Shapes[i] = new Shape( other.Shapes[i] );
            }
            Divergence = other.Divergence;
        }

        public object Clone() {
            return new DNA( this );
        }
        public MutationType LastMutation;

        public Shape[] Shapes;
        public double Divergence;

        public class Shape : ICloneable {
            public Shape() { }
            public Shape( Shape other ) {
                Color = other.Color;
                Points = (PointF[])other.Points.Clone();
            }

            public Color Color;
            public PointF[] Points;
            public Shape PreviousState;

            public object Clone() {
                return new Shape( this );
            }

            public void Serialize( Stream stream ) {
                BinaryWriter writer = new BinaryWriter( stream );
                writer.Write( Color.ToArgb() );
                for( int p = 0; p < Points.Length; p++ ) {
                    writer.Write( Points[p].X );
                    writer.Write( Points[p].Y );
                }
            }

            public Shape( Stream stream, int vertices ) {
                BinaryReader reader = new BinaryReader( stream );
                Color = Color.FromArgb( reader.ReadInt32() );
                Points = new PointF[vertices];
                for( int p = 0; p < Points.Length; p++ ) {
                    Points[p].X = reader.ReadSingle();
                    Points[p].Y = reader.ReadSingle();
                }
            }

            public XElement SerializeSVG( XNamespace xmlns ) {
                XElement el = new XElement( xmlns + "polygon" );
                StringBuilder sb = new StringBuilder();
                foreach( PointF point in Points ) {
                    sb.AppendFormat( "{0} {1} ", point.X, point.Y );
                }
                el.Add( new XAttribute( "points", sb.ToString() ) );
                el.Add( new XAttribute( "fill", String.Format( "rgb({0},{1},{2})", Color.R, Color.G, Color.B ) ) );
                el.Add( new XAttribute( "opacity", Color.A/255f ) );
                return el;
            }

            public RectangleF GetBoundaries() {
                RectangleF rect = new RectangleF( int.MaxValue, int.MaxValue, 0, 0 );
                for( int i = 0; i < Points.Length; i++ ) {
                    rect.X = Math.Min( rect.X, Points[i].X );
                    rect.Y = Math.Min( rect.Y, Points[i].Y );
                    rect.Width = Math.Max( rect.Width, Points[i].X );
                    rect.Height = Math.Max( rect.Height, Points[i].Y );
                }
                rect.Width -= rect.X;
                rect.Height -= rect.Y;
                return rect;
            }
        }

        public void Serialize( Stream stream ) {
            for( int i = 0; i < Shapes.Length; i++ ) {
                Shapes[i].Serialize( stream );
            }
        }

        public DNA( Stream stream, int shapes, int vertices ) {
            Shapes = new Shape[shapes];
            for( int i = 0; i < Shapes.Length; i++ ) {
                Shapes[i] = new Shape( stream, vertices );
            }
        }
    }


    public class Mutation {
        public Mutation( DNA _previousDNA, DNA _newDNA ) {
            PreviousDNA = _previousDNA;
            NewDNA = _newDNA;
            Timestamp = DateTime.UtcNow;
        }

        public DNA PreviousDNA;
        public DNA NewDNA;

        public DateTime Timestamp;

        public double DivergenceDelta {
            get {
                return PreviousDNA.Divergence - NewDNA.Divergence;
            }
        }
    }
}