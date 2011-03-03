using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperImageEvolver {
    static class Utilities {
        public static string ToCompactString( this TimeSpan span ) {
            return String.Format( "{0}.{1:00}:{2:00}:{3:00}",
                span.Days, span.Hours, span.Minutes, span.Seconds );
        }

        public static double NextDouble( this Random rand, double max ) {
            return rand.NextDouble() * max;
        }

        public static double NextDouble( this Random rand, double min, double max ) {
            return rand.NextDouble() * (max - min) + min; 
        }

        public static float NextFloat( this Random rand ) {
            return (float)rand.NextDouble();
        }

        public static float NextFloat( this Random rand, double max ) {
            return (float)(rand.NextDouble() * max);
        }

        public static float NextFloat( this Random rand, double min, double max ) {
            return (float)(rand.NextDouble() * (max - min) + min);
        }

        public static byte NextByte( this Random rand ) {
            return (byte)rand.Next( 256 );
        }

        public static byte NextByte( this Random rand, int max ) {
            return (byte)rand.Next( max );
        }

        public static byte NextByte( this Random rand, int min, int max ) {
            return (byte)rand.Next( min, max );
        }
    }
}
