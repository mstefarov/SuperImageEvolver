using System.Drawing;
using System.IO;


namespace SuperImageEvolver {
    public interface IEvaluator : IModule {
        void Initialize( TaskState state );
        double CalculateDivergence( Bitmap testImage, DNA dna, TaskState state, double max );
    }
}