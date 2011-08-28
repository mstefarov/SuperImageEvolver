using System;


namespace SuperImageEvolver {
    public interface IMutator : IModule {
        DNA Mutate( Random rand, DNA dna, TaskState task );
    }
}
