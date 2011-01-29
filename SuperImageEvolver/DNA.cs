using System;
using System.Drawing;
using System.IO;


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
                Points = (Point[])other.Points.Clone();
            }

            public Color Color;
            public Point[] Points;

            public object Clone() {
                return new Shape( this );
            }

            public void Serialize( Stream stream ) {
                BinaryWriter writer = new BinaryWriter( stream );
                writer.Write( Color.ToArgb() );
                for( int p = 0; p < Points.Length; p++ ) {
                    writer.Write( (short)Points[p].X );
                    writer.Write( (short)Points[p].Y );
                }
            }
        }

        public void Serialize( Stream stream ) {
            for( int i = 0; i < Shapes.Length; i++ ) {
                Shapes[i].Serialize( stream );
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