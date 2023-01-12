using System;

namespace SuperImageEvolver {
    public interface IInitializer : IModule {
        DNA Initialize( Random rand, TaskState task );

        void ReInitShape( Random rand, TaskState task, Shape shape, int shapeIndex );
    }
}
