using System;

namespace SuperImageEvolver {
    public static class MutatorHelper {
        public static void SwapShapes( Random rand, DNA newDNA ) {
            int s1 = rand.Next( newDNA.Shapes.Length );
            Shape shape = newDNA.Shapes[s1];
            shape.PreviousState = shape.Clone() as Shape;
            if( rand.Next( 2 ) == 0 ) {
                int s2;
                do {
                    s2 = rand.Next( newDNA.Shapes.Length );
                } while( s1 == s2 );
                if( s2 > s1 ) {
                    for( int i = s1; i < s2; i++ ) {
                        newDNA.Shapes[i] = newDNA.Shapes[i + 1];
                    }
                } else {
                    for( int i = s1; i > s2; i-- ) {
                        newDNA.Shapes[i] = newDNA.Shapes[i - 1];
                    }
                }
                newDNA.Shapes[s2] = shape;
            } else {
                int s2 = rand.Next( newDNA.Shapes.Length );
                newDNA.Shapes[s1] = newDNA.Shapes[s2];
                newDNA.Shapes[s2] = shape;
            }
            newDNA.LastMutation = MutationType.SwapShapes;
        }
    }
}
