using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;

namespace SuperImageEvolver {
    public class RGBEvaluatorFactory : IModuleFactory {
        public Type ModuleType {
            get { return typeof(RGBEvaluator); }
        }

        public string ID {
            get { return "std.RGBEvaluator.1"; }
        }

        public ModuleFunction Function {
            get { return ModuleFunction.Evaluator; }
        }

        public ModulePreset[] Presets {
            get {
                return new[] {
                    new ModulePreset( "RGB (Fast)", () => ( new RGBEvaluator( false ) ), this ),
                    new ModulePreset( "RGB (Smooth)", () => ( new RGBEvaluator( true ) ), this ),
                };
            }
        }

        public IModule GetInstance() {
            return new RGBEvaluator();
        }
    }


    sealed unsafe class RGBEvaluator : IEvaluator {

        public bool Smooth { get; set; }
        public bool Emphasized { get; set; }
        public double EmphasisAmount { get; set; }


        public RGBEvaluator() {
            Smooth = true;
            Emphasized = false;
            EmphasisAmount = 2;
        }


        public RGBEvaluator(bool smooth)
            : this() {
            Smooth = smooth;
        }


        public void Initialize(TaskState state) { }


        public double CalculateDivergence(Bitmap testImage, DNA dna, TaskState state, double maxAcceptableDivergence) {
            long sum = 0;
            long maxDivergence = GetMaxDivergence(state.EvalImageWidth, state.EvalImageHeight, Emphasized, EmphasisAmount);
            long roundedMax = (long)Math.Ceiling(maxAcceptableDivergence * maxDivergence + 1);

            dna.Draw(testImage, state, Smooth);
            BitmapData testData = testImage.LockBits(new Rectangle(Point.Empty, testImage.Size),
                                                     ImageLockMode.ReadOnly,
                                                     PixelFormat.Format32bppArgb);

            // The loop is duplicated to avoid ifs in the hot path
            if (Emphasized) {
                if (EmphasisAmount == 2) {
                    for (int i = 0; i < state.EvalImageHeight; i++) {
                        var originalPointer = (byte*)state.WorkingImageData.Scan0 + state.WorkingImageData.Stride * i;
                        var testPointer = (byte*)testData.Scan0 + testData.Stride * i;
                        for (int j = 0; j < state.EvalImageWidth; j++) {
                            int b = Math.Abs(*originalPointer - *testPointer);
                            int g = Math.Abs(originalPointer[1] - testPointer[1]);
                            int r = Math.Abs(originalPointer[2] - testPointer[2]);
                            sum += r * r + b * b + g * g;
                            originalPointer += 4;
                            testPointer += 4;
                        }
                        if (sum > roundedMax) {
                            sum = maxDivergence;
                            break;
                        }
                    }
                } else {
                    double doubleSum = 0;
                    for (int i = 0; i < state.EvalImageHeight; i++) {
                        var originalPointer = (byte*)state.WorkingImageData.Scan0 + state.WorkingImageData.Stride * i;
                        var testPointer = (byte*)testData.Scan0 + testData.Stride * i;
                        for (int j = 0; j < state.EvalImageWidth; j++) {
                            int b = Math.Abs(*originalPointer - *testPointer);
                            int g = Math.Abs(originalPointer[1] - testPointer[1]);
                            int r = Math.Abs(originalPointer[2] - testPointer[2]);
                            doubleSum += Math.Pow(r, EmphasisAmount) + Math.Pow(g, EmphasisAmount) + Math.Pow(b, EmphasisAmount);
                            originalPointer += 4;
                            testPointer += 4;
                        }
                        if (doubleSum > roundedMax) {
                            doubleSum = maxDivergence;
                            break;
                        }
                    }
                    sum = (long)doubleSum;
                }
            } else {
                for (int i = 0; i < state.EvalImageHeight; i++) {
                    var originalPointer = (byte*)state.WorkingImageData.Scan0 + state.WorkingImageData.Stride * i;
                    var testPointer = (byte*)testData.Scan0 + testData.Stride * i;
                    for (int j = 0; j < state.EvalImageWidth; j++) {
                        int b = Math.Abs(*originalPointer - *testPointer);
                        int g = Math.Abs(originalPointer[1] - testPointer[1]);
                        int r = Math.Abs(originalPointer[2] - testPointer[2]);
                        sum += r + b + g;
                        originalPointer += 4;
                        testPointer += 4;
                    }
                    if (sum > roundedMax) {
                        sum = maxDivergence;
                        break;
                    }
                }
            }

            testImage.UnlockBits(testData);
            if (Emphasized) {
                return Math.Pow(sum / maxDivergence, 1 / EmphasisAmount);
            } else {
                return sum / (double)maxDivergence;
            }
        }

        public void DrawDivergence(Bitmap testImage, DNA dna, TaskState state, bool invert, bool normalize) {
            Debug.Assert(testImage.Width == state.EvalImageWidth);
            Debug.Assert(testImage.Height == state.EvalImageHeight);
            double pixelDivergenceMultiplier;
            if (Emphasized) {
                pixelDivergenceMultiplier = 255d / (3 * Math.Pow(255, EmphasisAmount));
            } else {
                pixelDivergenceMultiplier = 255d / (3 * 255);
            }

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
                    float divergence;
                    if (Emphasized)
                        divergence = (float)((Math.Pow(r, EmphasisAmount) + Math.Pow(g, EmphasisAmount) + Math.Pow(b, EmphasisAmount)) * pixelDivergenceMultiplier);
                    else
                        divergence = (float)((r + g + b) * pixelDivergenceMultiplier);

                    maxObservedDivergence = Math.Max(maxObservedDivergence, divergence);
                    *(float*)testPointer = divergence;
                    originalPointer += 4;
                    testPointer += 4;
                }
            }

            // Convert per-pixel divergence from float, scaling or inverting as needed, to grayscale RGBA pixel values.
            float multiplier = 255 / maxObservedDivergence;
            for (int i = 0; i < state.EvalImageHeight; i++) {
                testPointer = (byte*)testData.Scan0 + testData.Stride * i;
                for (int j = 0; j < state.EvalImageWidth; j++) {
                    var val = *(float*)testPointer;
                    if (normalize)
                        val *= multiplier;
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


        object ICloneable.Clone() {
            return new RGBEvaluator {
                Smooth = Smooth,
                Emphasized = Emphasized,
                EmphasisAmount = EmphasisAmount
            };
        }


        void IModule.ReadSettings(NBTag tag) {
            Smooth = tag.GetBool(nameof(Smooth), Smooth);
            Emphasized = tag.GetBool(nameof(Emphasized), Emphasized);
            EmphasisAmount = tag.GetDouble(nameof(EmphasisAmount), EmphasisAmount);
        }

        void IModule.WriteSettings(NBTag tag) {
            tag.Append(nameof(Smooth), Smooth);
            tag.Append(nameof(Emphasized), Emphasized);
            tag.Append(nameof(EmphasisAmount), EmphasisAmount);
        }

        static long GetMaxDivergence(int imageWidth, int imageHeight, bool emphasized, double emphasisAmount) {
            if (emphasized) {
                return (long)(3L * imageWidth * imageHeight * Math.Pow(255, emphasisAmount));
            } else {
                return 3L * imageWidth * imageHeight * 255L;
            }
        }
    }
}
