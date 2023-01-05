using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace SuperImageEvolver {
    public class SloppyRGBEvaluatorFactory : IModuleFactory {
        public Type ModuleType {
            get { return typeof(SloppyRGBEvaluator); }
        }

        public string ID {
            get { return "std.SloppyRGBEvaluator.1"; }
        }

        public ModuleFunction Function {
            get { return ModuleFunction.Evaluator; }
        }

        public ModulePreset[] Presets {
            get {
                return new[] {
                    new ModulePreset( "RGB (Sloppy)", () => ( new SloppyRGBEvaluator() ), this )
                };
            }
        }


        public IModule GetInstance() {
            return new SloppyRGBEvaluator();
        }
    }


    sealed unsafe class SloppyRGBEvaluator : IEvaluator {

        Bitmap halfResImage;

        public bool Smooth { get; set; }
        public bool Emphasized { get; set; }
        public double EmphasisAmount { get; set; }


        public SloppyRGBEvaluator() {
            Smooth = true;
            Emphasized = false;
            EmphasisAmount = 2;
        }


        public void Initialize(TaskState state) {
            halfResImage = new Bitmap(state.ImageWidth / 2, state.ImageHeight / 2, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(halfResImage)) {
                g.InterpolationMode = InterpolationMode.HighQualityBilinear;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.DrawImage(state.WorkingImageCopy, 0, 0, halfResImage.Width, halfResImage.Height);
            }
        }


        public double CalculateDivergence(Bitmap testImage, DNA dna, TaskState state, double maxAcceptableDivergence) {
            var halfResData = halfResImage.LockBits(new Rectangle(Point.Empty, halfResImage.Size),
                                                 ImageLockMode.ReadOnly,
                                                 PixelFormat.Format32bppArgb);
            double maxDivergence;
            maxDivergence = RgbEvaluatorUtils.GetMaxDivergence(state.ImageWidth / 2, state.ImageHeight / 2, Emphasized, EmphasisAmount);

            long sum = 0;
            long roundedMax = (long)Math.Ceiling(maxAcceptableDivergence * maxDivergence + 1);
            DrawDna(testImage, dna, state);

            BitmapData testData = testImage.LockBits(new Rectangle(Point.Empty, testImage.Size),
                                                      ImageLockMode.ReadOnly,
                                                      PixelFormat.Format32bppArgb);
            for (int i = 0; i < state.ImageHeight / 2; i++) {
                byte* originalPointer = (byte*)halfResData.Scan0 + halfResData.Stride * i;
                byte* testPointer = (byte*)testData.Scan0 + testData.Stride * i;
                for (int j = 0; j < state.ImageWidth / 2; j++) {
                    int b = Math.Abs(*originalPointer - *testPointer);
                    int g = Math.Abs(originalPointer[1] - testPointer[1]);
                    int r = Math.Abs(originalPointer[2] - testPointer[2]);
                    if (Emphasized) {
                        if (EmphasisAmount == 2) {
                            sum += r * r + b * b + g * g;
                        } else {
                            sum += (long)(Math.Pow(r, EmphasisAmount) + Math.Pow(g, EmphasisAmount) + Math.Pow(b, EmphasisAmount));
                        }
                    } else {
                        sum += r + b + g;
                    }
                    originalPointer += 4;
                    testPointer += 4;
                }
                if (sum > roundedMax) {
                    sum = (long)maxDivergence;
                    break;
                }
            }
            halfResImage.UnlockBits(halfResData);
            testImage.UnlockBits(testData);
            if (Emphasized) {
                return Math.Pow(sum / maxDivergence, 1 / EmphasisAmount);
            } else {
                return sum / maxDivergence;
            }
        }

        private void DrawDna(Bitmap testImage, DNA dna, TaskState state) {
            using Graphics g = Graphics.FromImage(testImage);
            g.Clear(state.ProjectOptions.Matte);
            g.Transform = new Matrix(.5f, 0, 0, .5f, 0, 0);
            g.SmoothingMode = (Smooth ? SmoothingMode.HighQuality : SmoothingMode.HighSpeed);
            for (int i = 0; i < dna.Shapes.Length; i++) {
                g.FillPolygon(new SolidBrush(dna.Shapes[i].Color), dna.Shapes[i].Points, FillMode.Alternate);
            }
        }

        public void DrawDivergence(Bitmap testImage, DNA dna, TaskState state, bool invert, bool normalize) {
            double pixelDivergenceMultiplier;
            if (Emphasized) {
                pixelDivergenceMultiplier = 255d / (3 * Math.Pow(255, EmphasisAmount));
            } else {
                pixelDivergenceMultiplier = 255d / (3 * 255);
            }

            DrawDna(testImage, dna, state);
            var testData = testImage.LockBits(new Rectangle(Point.Empty, testImage.Size),
                                              ImageLockMode.ReadWrite,
                                              PixelFormat.Format32bppArgb);
            var halfResData = halfResImage.LockBits(new Rectangle(Point.Empty, halfResImage.Size),
                                                    ImageLockMode.ReadOnly,
                                                    PixelFormat.Format32bppArgb);

            // Compute per-pixel divergence as an array of 32-bit floats, stored raw in the testData buffer.
            float maxObservedDivergence = 0.001f; // avoid division by zero when normalizing
            byte* originalPointer, testPointer;
            for (int i = 0; i < state.ImageHeight / 2; i++) {
                originalPointer = (byte*)halfResData.Scan0 + halfResData.Stride * i;
                testPointer = (byte*)testData.Scan0 + testData.Stride * (i + state.ImageHeight / 2) + (state.ImageWidth / 2) * 4; // bottom-right quadrant
                for (int j = 0; j < state.ImageWidth / 2; j++) {
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
            halfResImage.UnlockBits(halfResData);

            // Convert per-pixel divergence from float, scaling or inverting as needed, to grayscale RGBA pixel values.
            float multiplier = 255 / maxObservedDivergence;
            for (int i = 0; i < state.ImageHeight / 2; i++) {
                var testWritePointer = (byte*)testData.Scan0 + testData.Stride * i * 2;
                var testReadPointer = (byte*)testData.Scan0 + testData.Stride * (i + state.ImageHeight / 2) + (state.ImageWidth / 2) * 4; // bottom-right quadrant
                for (int j = 0; j < state.ImageWidth / 2; j++) {
                    var val = *(float*)testReadPointer;
                    if (normalize)
                        val *= multiplier;
                    byte divByte = (byte)Math.Min(255, val);
                    if (invert)
                        divByte = (byte)(255 - divByte);
                    testWritePointer[0] = divByte;
                    testWritePointer[1] = divByte;
                    testWritePointer[2] = divByte;
                    testWritePointer[3] = 255;
                    testWritePointer[4] = divByte;
                    testWritePointer[5] = divByte;
                    testWritePointer[6] = divByte;
                    testWritePointer[7] = 255;
                    testWritePointer[0 + testData.Stride] = divByte;
                    testWritePointer[1 + testData.Stride] = divByte;
                    testWritePointer[2 + testData.Stride] = divByte;
                    testWritePointer[3 + testData.Stride] = 255;
                    testWritePointer[4 + testData.Stride] = divByte;
                    testWritePointer[5 + testData.Stride] = divByte;
                    testWritePointer[6 + testData.Stride] = divByte;
                    testWritePointer[7 + testData.Stride] = 255;
                    testWritePointer += 8;
                    testReadPointer += 4;
                }
            }

            testImage.UnlockBits(testData);
        }


        object ICloneable.Clone() {
            return new SloppyRGBEvaluator {
                Smooth = Smooth,
                Emphasized = Emphasized,
                EmphasisAmount = EmphasisAmount,
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
    }

    static class RgbEvaluatorUtils {
        public static long GetMaxDivergence(int imageWidth, int imageHeight, bool emphasized, double emphasisAmount) {
            if (emphasized) {
                return (long)(3L * imageWidth * imageHeight * Math.Pow(255, emphasisAmount));
            } else {
                return 3L * imageWidth * imageHeight * 255L;
            }
        }
    }
}
