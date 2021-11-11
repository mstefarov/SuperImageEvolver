using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace SuperImageEvolver {
    public class RGBEvaluatorFactory : IModuleFactory {
        public Type ModuleType {
            get { return typeof( RGBEvaluator ); }
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


        public RGBEvaluator( bool smooth )
            : this() {
            Smooth = smooth;
        }


        public void Initialize( TaskState state ) {}


        public double CalculateDivergence( Bitmap testImage, DNA dna, TaskState state, double maxAcceptableDivergence ) {

            var maxDivergence = GetMaxDivergence(state);
            double sum = 0;
            double roundedMax = (maxAcceptableDivergence * maxDivergence + 1);
            byte* originalPointer, testPointer;
            
            DrawDna(testImage, dna, state);
            BitmapData testData = testImage.LockBits(new Rectangle(Point.Empty, testImage.Size),
                                                      ImageLockMode.ReadOnly,
                                                      PixelFormat.Format32bppArgb);

            if (Emphasized) {
                if (EmphasisAmount == 2) {
                    for (int i = 0; i < state.ImageHeight; i++) {
                        originalPointer = (byte*)state.WorkingImageData.Scan0 + state.WorkingImageData.Stride * i;
                        testPointer = (byte*)testData.Scan0 + testData.Stride * i;
                        for (int j = 0; j < state.ImageWidth; j++) {
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
                    for (int i = 0; i < state.ImageHeight; i++) {
                        originalPointer = (byte*)state.WorkingImageData.Scan0 + state.WorkingImageData.Stride * i;
                        testPointer = (byte*)testData.Scan0 + testData.Stride * i;
                        for (int j = 0; j < state.ImageWidth; j++) {
                            int b = Math.Abs(*originalPointer - *testPointer);
                            int g = Math.Abs(originalPointer[1] - testPointer[1]);
                            int r = Math.Abs(originalPointer[2] - testPointer[2]);
                            sum += Math.Pow(r, EmphasisAmount) + Math.Pow(g, EmphasisAmount) +
                                   Math.Pow(b, EmphasisAmount);
                            originalPointer += 4;
                            testPointer += 4;
                        }
                        if (sum > roundedMax) {
                            sum = maxDivergence;
                            break;
                        }
                    }
                }
            } else {
                for (int i = 0; i < state.ImageHeight; i++) {
                    originalPointer = (byte*)state.WorkingImageData.Scan0 + state.WorkingImageData.Stride * i;
                    testPointer = (byte*)testData.Scan0 + testData.Stride * i;
                    for (int j = 0; j < state.ImageWidth; j++) {
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
                return sum / maxDivergence;
            }
        }

        private void DrawDna(Bitmap testImage, DNA dna, TaskState state) {
            using (Graphics g = Graphics.FromImage(testImage)) {
                g.Clear(state.ProjectOptions.Matte);
                g.SmoothingMode = (Smooth ? SmoothingMode.HighQuality : SmoothingMode.HighSpeed);

                for (int i = 0; i < dna.Shapes.Length; i++) {
                    g.FillPolygon(new SolidBrush(dna.Shapes[i].Color), dna.Shapes[i].Points, FillMode.Alternate);
                }
            }
        }

        private double GetMaxDivergence(TaskState state) {
            double maxDivergence;
            if (Emphasized) {
                if (EmphasisAmount == 2) {
                    maxDivergence = Math.Sqrt(3L * state.ImageWidth * state.ImageHeight * 255L * 255L);
                } else {
                    maxDivergence = Math.Pow(3L * state.ImageWidth * state.ImageHeight * Math.Pow(255, EmphasisAmount), 1/EmphasisAmount);
                }
            } else {
                maxDivergence = 3L * state.ImageWidth * state.ImageHeight * 255L;
            }

            return maxDivergence;
        }

        public void DrawDivergence(Bitmap testImage, DNA dna, TaskState state, bool invert, bool normalize) {
            double pixelDivergenceMultiplier;
            if (Emphasized) {
                pixelDivergenceMultiplier = 255d / Math.Pow(3 * Math.Pow(255 * 3, EmphasisAmount), 1 / EmphasisAmount);
            } else {
                pixelDivergenceMultiplier = 255d / (255 * 3);
            }

            DrawDna(testImage, dna, state);
            BitmapData testData = testImage.LockBits(new Rectangle(Point.Empty, testImage.Size),
                                                      ImageLockMode.ReadOnly,
                                                      PixelFormat.Format32bppArgb);
            float maxObservedDivergence = 0.001f;
            byte* originalPointer, testPointer;
            for (int i = 0; i < state.ImageHeight; i++) {
                originalPointer = (byte*)state.WorkingImageData.Scan0 + state.WorkingImageData.Stride * i;
                testPointer = (byte*)testData.Scan0 + testData.Stride * i;
                for (int j = 0; j < state.ImageWidth; j++) {
                    int b = Math.Abs(*originalPointer - *testPointer);
                    int g = Math.Abs(originalPointer[1] - testPointer[1]);
                    int r = Math.Abs(originalPointer[2] - testPointer[2]);
                    float divergence;
                    if (Emphasized)
                        divergence = (float)(Math.Pow(Math.Pow(r, EmphasisAmount) + Math.Pow(g, EmphasisAmount) + Math.Pow(b, EmphasisAmount), 1 / EmphasisAmount)*pixelDivergenceMultiplier);
                    else
                        divergence = (float)((r + g + b)*pixelDivergenceMultiplier);

                    maxObservedDivergence = Math.Max(maxObservedDivergence, divergence);
                    *(float*)testPointer = divergence;
                    originalPointer += 4;
                    testPointer += 4;
                }
            }
            
            float multiplier = 255/maxObservedDivergence;
            for (int i = 0; i < state.ImageHeight; i++) {
                testPointer = (byte*)testData.Scan0 + testData.Stride * i;
                for (int j = 0; j < state.ImageWidth; j++) {
                    var val = *(float*)testPointer;
                    if(normalize)
                        val*=multiplier;
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


        void IModule.ReadSettings( NBTag tag ) {
            Smooth = tag.GetBool(nameof(Smooth), Smooth);
            Emphasized = tag.GetBool(nameof(Emphasized), Emphasized);
            EmphasisAmount = tag.GetDouble(nameof(EmphasisAmount), EmphasisAmount);
        }

        void IModule.WriteSettings( NBTag tag ) {
            tag.Append(nameof(Smooth), Smooth);
            tag.Append(nameof(Emphasized), Emphasized);
            tag.Append(nameof(EmphasisAmount), EmphasisAmount);
        }
    }
}
