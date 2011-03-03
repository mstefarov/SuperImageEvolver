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