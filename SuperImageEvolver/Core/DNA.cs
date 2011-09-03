using System;
using System.IO;


namespace SuperImageEvolver {
    public sealed class DNA : ICloneable {
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


        public NBTag SerializeNBT() {
            NBTList tag = new NBTList( "Shapes", NBTType.Compound, Shapes.Length );
            for( int i = 0; i < Shapes.Length; i++ ) {
                tag[i] = Shapes[i].SerializeNBT();
            }
            return tag;
        }

        public DNA( Stream stream, int shapes, int vertices ) {
            Shapes = new Shape[shapes];
            for( int i = 0; i < Shapes.Length; i++ ) {
                Shapes[i] = new Shape( stream, vertices );
            }
        }
    }


    public sealed class Mutation {
        public Mutation( DNA previousDna, DNA newDna ) {
            PreviousDNA = previousDna;
            NewDNA = newDna;
            Timestamp = DateTime.UtcNow;
        }

        public DNA PreviousDNA { get; set; }
        public DNA NewDNA { get; set; }
        public DateTime Timestamp { get; set; }

        public double DivergenceDelta {
            get {
                return PreviousDNA.Divergence - NewDNA.Divergence;
            }
        }
    }
}