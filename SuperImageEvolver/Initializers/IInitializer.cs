using System;

namespace SuperImageEvolver {
    public interface IInitializer : IModule {
        DNA Initialize( Random rand, TaskState task );
    }
}