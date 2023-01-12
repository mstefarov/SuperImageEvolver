using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;

namespace SuperImageEvolver {
    public class WeightedRGBEvaluatorFactory : IModuleFactory {
        public Type ModuleType {
            get { return typeof( WeightedRGBEvaluator ); }
        }

        public string ID {
            get { return "std.WeightedRGBEvaluator.1"; }
        }

        public ModuleFunction Function {
            get { return ModuleFunction.Evaluator; }
        }

        public ModulePreset[] Presets {
            get {
                return new[] {
                    new ModulePreset( "Weighted RGB", () => ( new WeightedRGBEvaluator() ), this )
                };
            }
        }


        public IModule GetInstance() {
            return new WeightedRGBEvaluator();
        }
    }


    sealed unsafe class WeightedRGBEvaluator : IEvaluator {

        double maxDivergence;

        public bool Smooth { get; set; }

        // Range [(0,0)...(1,1)], with origin in top-left corner and max in bottom-right corner.
        // Default is (0.5, 0.5), meaning center of the image.
        public double EmphasisCenterX { get; set; }
        public double EmphasisCenterY { get; set; }
        
        // Range [0...1], with 0.0 meaning that just the center pixel getting the full weight,
        // and 1.0 meaning that the entire EmphasisRange radius is evaluated with emphasis.
        // Default is 0.25 meaning that a quarter of EmphasisRange gets full emphasis.
        public double CenterRange { get; set; }

        // Range [0...1], with 0.0 meaning 1 pixel in the center and 1.0 meaning 100% of the length of image diagonal.
        // Default is 0.5, meaning emphasis extends half-diagonal / one-radius.
        public double EmphasisRange { get; set; }

        // Range [0...1], with 0.0 meaning uniform weight and 1.0 meaning 100% in the center and 0% outside.
        // Default is 0.5, meaning center gets 100% weight and outside gets 50% weight.
        public double OutsideWeight { get; set; }

        /*
         *           1.0 |         /-----\
         *               |       /         \
         * OutsideWeight |-----/             \-----
         *               |          
         *           0.0 |_____.___.__.__.___._____
         *                            |<-----> EmphasisRange
         *             CenterRange <->|
         *                            | EmphasisCenter
         */

        public WeightedRGBEvaluator() {
            Smooth = true;
            EmphasisCenterX = 0.5;
            EmphasisCenterY = 0.5;
            CenterRange = 0.25;
            EmphasisRange = 0.5;
            OutsideWeight = 0.5;
        }

        public void Initialize( TaskState state ) {
        }

        private double GetWeight(int x, int y, int emphasisPixelX, int emphasisPixelY, double maxRadius) {
            double absDistToCenter = Math.Sqrt((x - emphasisPixelX) * (x - emphasisPixelX) +
                                               (y - emphasisPixelY) * (y - emphasisPixelY));
            double distToCenter = absDistToCenter / maxRadius;
            if (distToCenter > EmphasisRange) {
                return OutsideWeight;
            }

            double centerFraction = EmphasisRange * CenterRange; // as a fraction of total radius
            if (distToCenter < centerFraction) {
                return 1;
            } else {
                var rampRatio = (distToCenter - centerFraction) / (EmphasisRange - centerFraction); 
                return (1-rampRatio) + rampRatio * OutsideWeight; // lerp between 1 and OutsideWeight
            }
        }

        private double GetMaxRadius(int imageWidth, int imageHeight){
            return Math.Sqrt(imageWidth * imageWidth + imageHeight * imageHeight) / 2; // half of the image diagonal
        }

        private long GetMaxDivergence(int imageWidth, int imageHeight) {
            var maxRadius = GetMaxRadius(imageWidth, imageHeight);
            int emphasisPixelX = (int)Math.Round(EmphasisCenterX * imageWidth);
            int emphasisPixelY = (int)Math.Round(EmphasisCenterY * imageHeight);
            double maxDiv = 0;
            for (int x = 0; x < imageWidth; x++) {
                for (int y = 0; y < imageHeight; y++) {
                    maxDiv += 3d * 255d * GetWeight(x, y, emphasisPixelX, emphasisPixelY, maxRadius);
                }
            }
            return (long)Math.Ceiling(maxDiv);
        }

        public double CalculateDivergence(Bitmap testImage, DNA dna, TaskState state, double maxAcceptableDivergence) {
            long sum = 0;
            long maxDivergence = GetMaxDivergence(state.EvalImageWidth, state.EvalImageHeight);
            long roundedMax = (long)Math.Ceiling(maxAcceptableDivergence * maxDivergence + 1);
            var maxRadius = GetMaxRadius(state.EvalImageWidth, state.EvalImageHeight);
            int emphasisPixelX = (int)Math.Round(EmphasisCenterX * state.EvalImageWidth);
            int emphasisPixelY = (int)Math.Round(EmphasisCenterY * state.EvalImageHeight);

            dna.Draw(testImage, state, Smooth);
            BitmapData testData = testImage.LockBits(new Rectangle(Point.Empty, testImage.Size),
                                                     ImageLockMode.ReadOnly,
                                                     PixelFormat.Format32bppArgb);

            for (int i = 0; i < state.EvalImageHeight; i++) {
                var originalPointer = (byte*)state.WorkingImageData.Scan0 + state.WorkingImageData.Stride * i;
                var testPointer = (byte*)testData.Scan0 + testData.Stride * i;
                for (int j = 0; j < state.EvalImageWidth; j++) {
                    int b = Math.Abs(*originalPointer - *testPointer);
                    int g = Math.Abs(originalPointer[1] - testPointer[1]);
                    int r = Math.Abs(originalPointer[2] - testPointer[2]);
                    sum += (long)Math.Ceiling(GetWeight(j, i, emphasisPixelX,emphasisPixelY,maxRadius) * (r + b + g));
                    originalPointer += 4;
                    testPointer += 4;
                }
                if (sum > roundedMax) {
                    sum = maxDivergence;
                    break;
                }
            }

            testImage.UnlockBits(testData);
            return sum / (double)maxDivergence;
        }

        public void DrawDivergence(Bitmap testImage, DNA dna, TaskState state, bool invert, bool normalize) {
            Debug.Assert(testImage.Width == state.EvalImageWidth);
            Debug.Assert(testImage.Height == state.EvalImageHeight);
            var maxRadius = GetMaxRadius(state.EvalImageWidth, state.EvalImageHeight);
            int emphasisPixelX = (int)Math.Round(EmphasisCenterX * state.EvalImageWidth);
            int emphasisPixelY = (int)Math.Round(EmphasisCenterY * state.EvalImageHeight);

            dna.Draw(testImage, state, Smooth);
            BitmapData testData = testImage.LockBits(new Rectangle(Point.Empty, testImage.Size),
                                                     ImageLockMode.ReadOnly,
                                                     PixelFormat.Format32bppArgb);

            // Compute per-pixel divergence as an array of 32-bit floats, stored raw in the testData buffer.
            float maxObservedDivergence = 0.001f; // avoid division by zero when normalizing
            byte* originalPointer, testPointer;
            for (int i = 0; i < state.EvalImageHeight; i++) {
                originalPointer = (byte*)state.WorkingImageData.Scan0 + state.WorkingImageData.Stride * i;
                testPointer = (byte*)testData.Scan0 + testData.Stride * i;
                for (int j = 0; j < state.EvalImageWidth; j++) {
                    int b = Math.Abs(*originalPointer - *testPointer);
                    int g = Math.Abs(originalPointer[1] - testPointer[1]);
                    int r = Math.Abs(originalPointer[2] - testPointer[2]);
                    float divergence = (float)((r + g + b) * GetWeight(i, j, emphasisPixelX, emphasisPixelY, maxRadius));
                    maxObservedDivergence = Math.Max(maxObservedDivergence, divergence);
                    *(float*)testPointer = divergence;
                    originalPointer += 4;
                    testPointer += 4;
                }
            }

            // Convert per-pixel divergence from float, scaling or inverting as needed, to grayscale RGBA pixel values.
            double maxDivergencePerPixel;
            if (normalize)
                maxDivergencePerPixel = maxObservedDivergence;
            else
                maxDivergencePerPixel = 3d * 255d;
            for (int i = 0; i < state.EvalImageHeight; i++) {
                testPointer = (byte*)testData.Scan0 + testData.Stride * i;
                for (int j = 0; j < state.EvalImageWidth; j++) {
                    var val = 255*(*(float*)testPointer) / maxDivergencePerPixel;
                    byte divByte = (byte)Math.Min(255, val);
                    if (invert)
                        divByte = (byte)(255 - divByte);
                    testPointer[0] = divByte;
                    testPointer[1] = divByte;
                    testPointer[2] = divByte;
                    testPointer[3] = 255;
                    testPointer += 4;
                }
            }

            testImage.UnlockBits(testData);
        }
        
        public Rectangle GetDebugCenterBoundary(TaskState state) {
            // Everything is "times 2" because internally the sloppy evaluator uses half resolution
            int cx = (int)Math.Round(EmphasisCenterX * state.ImageWidth);
            int cy = (int)Math.Round(EmphasisCenterY * state.ImageHeight);
            int cr = (int)Math.Ceiling(GetMaxRadius(state.ImageWidth, state.ImageHeight) * EmphasisRange * CenterRange);
            return new Rectangle(cx - cr, cy - cr, cr * 2, cr * 2);
        }

        public Rectangle GetDebugOuterBoundary(TaskState state) {
            int cx = (int)Math.Round(EmphasisCenterX * state.ImageWidth);
            int cy = (int)Math.Round(EmphasisCenterY * state.ImageHeight);
            int cr = (int)Math.Ceiling(GetMaxRadius(state.ImageWidth, state.ImageHeight) * EmphasisRange);
            return new Rectangle(cx - cr, cy - cr, cr * 2, cr * 2);
        }

        object ICloneable.Clone() {
            return new WeightedRGBEvaluator {
                Smooth = Smooth,
                EmphasisCenterX = EmphasisCenterX,
                EmphasisCenterY = EmphasisCenterY,
                CenterRange = CenterRange,
                EmphasisRange = EmphasisRange,
                OutsideWeight = OutsideWeight,
            };
        }

        void IModule.ReadSettings( NBTag tag ) {
            Smooth = tag.GetBool(nameof(Smooth), Smooth);
            EmphasisCenterX = tag.GetDouble(nameof(EmphasisCenterX), EmphasisCenterX);
            EmphasisCenterY = tag.GetDouble(nameof(EmphasisCenterY), EmphasisCenterY);
            CenterRange = tag.GetDouble(nameof(CenterRange), CenterRange);
            EmphasisRange = tag.GetDouble(nameof(EmphasisRange), EmphasisRange);
            OutsideWeight = tag.GetDouble(nameof(OutsideWeight), OutsideWeight);
        }

        void IModule.WriteSettings(NBTag tag) {
            tag.Append(nameof(Smooth), Smooth);
            tag.Append(nameof(EmphasisCenterX), EmphasisCenterX);
            tag.Append(nameof(EmphasisCenterY), EmphasisCenterY);
            tag.Append(nameof(CenterRange), CenterRange);
            tag.Append(nameof(EmphasisRange), EmphasisRange);
            tag.Append(nameof(OutsideWeight), OutsideWeight);
        }
    }
}
