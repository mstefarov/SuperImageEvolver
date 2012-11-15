using System;
using System.IO;

namespace SuperImageEvolver {
    public sealed class DNA : ICloneable {
        public DNA() {}


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


        public DNA( NBTag tag ) {
            Divergence = tag["Divergence"].GetDouble();
            var shapesTag = (NBTList)tag["Shapes"];
            Shapes = new Shape[shapesTag.Tags.Length];
            for( int i = 0; i < Shapes.Length; i++ ) {
                Shapes[i] = new Shape( shapesTag[i] );
            }
        }


        public NBTag SerializeNBT( string tagName ) {
            NBTCompound compound = new NBTCompound( tagName );
            compound.Append( "Divergence", Divergence );
            NBTList tag = new NBTList( "Shapes", NBTType.Compound, Shapes.Length );
            for( int i = 0; i < Shapes.Length; i++ ) {
                tag[i] = Shapes[i].SerializeNBT();
            }
            compound.Append( tag );
            return compound;
        }


        public DNA( Stream stream, int shapes, int vertices ) {
            Shapes = new Shape[shapes];
            for( int i = 0; i < Shapes.Length; i++ ) {
                Shapes[i] = new Shape( stream, vertices );
            }
        }
    }
}