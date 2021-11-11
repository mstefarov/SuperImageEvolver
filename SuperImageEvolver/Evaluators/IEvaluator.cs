using System.Drawing;

namespace SuperImageEvolver {
    public interface IEvaluator : IModule {
        bool Smooth { get; }

        void Initialize( TaskState state );

        double CalculateDivergence( Bitmap testImage, DNA dna, TaskState state, double maxAcceptableDivergence );

        void DrawDivergence(Bitmap testImage, DNA dna, TaskState state, bool invert, bool normalize);
    }
}
