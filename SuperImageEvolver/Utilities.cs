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
    }
}
