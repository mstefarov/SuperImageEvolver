using System;
using System.Drawing;
using System.Text;
using System.Xml.Linq;

namespace SuperImageEvolver {
    public sealed class Shape : ICloneable {
        public Shape() {}


        public Shape( Shape other ) {
            Color = other.Color;
            Points = (PointF[])other.Points.Clone();
            OutlineColor = other.OutlineColor;
        }


        public Color Color;
        public PointF[] Points;
        public Shape PreviousState;
        public Color OutlineColor = Color.Transparent;


        public Shape(NBTag tag) {
            Color = tag["Color"].GetColor();
            var pointsTag = (NBTList)tag["Points"];
            Points = new PointF[pointsTag.Tags.Length];
            for (int i = 0; i < Points.Length; i++) {
                Points[i] = pointsTag[i].GetPointF();
            }
            if (tag.Contains(nameof(PreviousState)))
                PreviousState = new Shape(tag[nameof(PreviousState)]);
        }


        public NBTag SerializeNBT(string tagName = "Shape") {
            NBTCompound tag = new NBTCompound(tagName);
            tag.Append( "Color", Color );
            NBTList points = new NBTList( "Points", NBTType.PointF, Points.Length );
            for( int p = 0; p < Points.Length; p++ ) {
                points[p] = new NBTag( NBTType.PointF, null, Points[p], points );
            }
            tag.Append( points );
            if (PreviousState != null)
                tag.Append(PreviousState.SerializeNBT("PreviousState"));

            return tag;
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


        public override string ToString() {
            return String.Format("Shape(" + Color + ")");
        }
    }
}
