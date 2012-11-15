using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace SuperImageEvolver {
    public sealed class Shape : ICloneable {
        public Shape() {}


        public Shape( Shape other ) {
            Color = other.Color;
            Points = (PointF[])other.Points.Clone();
        }


        public Color Color;
        public PointF[] Points;
        public Shape PreviousState;


        public Shape( NBTag tag ) {
            Color = tag["Color"].GetColor();
            var pointsTag = (NBTList)tag["Points"];
            Points = new PointF[pointsTag.Tags.Length];
            for( int i = 0; i < Points.Length; i++ ) {
                Points[i] = pointsTag[i].GetPointF();
            }
        }


        public NBTag SerializeNBT() {
            NBTCompound tag = new NBTCompound( "Shape" );
            tag.Append( "Color", Color );
            NBTList points = new NBTList( "Points", NBTType.PointF, Points.Length );
            for( int p = 0; p < Points.Length; p++ ) {
                points[p] = new NBTag( NBTType.PointF, null, Points[p], points );
            }
            tag.Append( points );
            return tag;
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
            el.Add( new XAttribute( "opacity", Color.A / 255f ) );
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


        public object Clone() {
            return new Shape( this );
        }
    }
}