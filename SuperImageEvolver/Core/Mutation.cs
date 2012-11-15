using System;

namespace SuperImageEvolver {
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
            get { return PreviousDNA.Divergence - NewDNA.Divergence; }
        }
    }
}