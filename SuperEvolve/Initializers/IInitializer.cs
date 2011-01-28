using System;
using System.IO;

namespace SuperImageEvolver {
    public interface IInitializer : IModule {
        DNA Initialize( Random rand, TaskState task );
    }
}
