using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;

namespace SuperImageEvolver {
    public class PerceptualEvaluatorFactory : IModuleFactory {
        public Type ModuleType {
            get { return typeof( PerceptualEvaluator ); }
        }

        public string ID {
            get { return "std.PerceptualEvaluator.1"; }
        }

        public ModuleFunction Function {
            get { return ModuleFunction.Evaluator; }
        }

        public ModulePreset[] Presets {
            get {
                return new[] {
                    new ModulePreset( "Perceptual (Fast)", () => ( new PerceptualEvaluator( false ) ), this ),
                    new ModulePreset( "Perceptual (Smooth)", () => ( new PerceptualEvaluator( true ) ), this )
                };
            }
        }


        public IModule GetInstance() {
            return new PerceptualEvaluator();
        }
    }

    public enum ColorSpace { CieLuv, CieLab }

    sealed unsafe class PerceptualEvaluator : IEvaluator {

        public void Initialize( TaskState state ) {}

        public bool Smooth { get; set; }
        public double ColorImportance { get; set; } = 1.0;
        public ColorSpace ColorSpace { get;set; } = ColorSpace.CieLuv;

        public PerceptualEvaluator() {}


        public PerceptualEvaluator( bool smooth ) {
            Smooth = smooth;
        }

        // Computed by brute-force by finding Lab values for all colors in 8-bit sRGB space,
        // and comparing min/max values seen for each channel. This is approximate and max possible delta is probably lower
        // (I did not test all *pairs* of colors in sRGB, thinking it would be too time-consuming).
        const double MaxLuminosityDistance = 99.65492608793288; // same for Lab and Luv

        // a is [-85.92873731179773 .. 97.93924543262145]
        // b is [-107.53871506366706 .. 94.19722821213907]
        const double MaxLabColorDistance = 272.9557214785601;

        // u is [-82.7892735260603 .. 174.3355525774302]
        // v is [-133.53972745903033 .. 107.02214364689067]
        const double MaxLuvColorDistance = 352.1124678689069;

        private double GetMaxDivPerPixel() {
            double maxColorDiff = (ColorSpace == ColorSpace.CieLuv ? MaxLuvColorDistance : MaxLabColorDistance);
            return Math.Sqrt(MaxLuminosityDistance * MaxLuminosityDistance + (maxColorDiff * ColorImportance) * (maxColorDiff * ColorImportance));
        }

        public double CalculateDivergence( Bitmap testImage, DNA dna, TaskState state, double maxAcceptableDivergence ) {
            double maxTotalDivergence = state.EvalImageHeight * state.EvalImageWidth * GetMaxDivPerPixel();
            double sum = 0;
            long roundedMax = (long)(maxAcceptableDivergence * maxTotalDivergence + 1);
            dna.Draw(testImage, state, Smooth);

            BitmapData testData = testImage.LockBits(new Rectangle(Point.Empty, testImage.Size),
                                                     ImageLockMode.ReadOnly,
                                                     PixelFormat.Format32bppArgb);
            for (int i = 0; i < state.EvalImageHeight; i++) {
                byte* originalPointer = (byte*)state.WorkingImageData.Scan0 + state.WorkingImageData.Stride * i;
                byte* testPointer = (byte*)testData.Scan0 + testData.Stride * i;
                for (int j = 0; j < state.EvalImageWidth; j++) {
                    sum += GetDelta(originalPointer, testPointer);
                    originalPointer += 4;
                    testPointer += 4;
                }
                if (sum > roundedMax) break;
            }
            testImage.UnlockBits(testData);
            return sum / maxTotalDivergence;
        }

        private double GetDelta(byte* originalPointer, byte* testPointer) {
            LabColor original, test;
            if (ColorSpace == ColorSpace.CieLuv) {
                original = ToLuv(originalPointer[0], originalPointer[1], originalPointer[2]);
                test = ToLuv(testPointer[0], testPointer[1], testPointer[2]);
            } else {
                original = ToLab(originalPointer[0], originalPointer[1], originalPointer[2]);
                test = ToLab(testPointer[0], testPointer[1], testPointer[2]);
            }

            double deltaL = original.L - test.L;
            double deltaC1 = (original.a - test.a) * ColorImportance;
            double deltaC2 = (original.b - test.b) * ColorImportance;
            return Math.Sqrt(deltaL * deltaL + deltaC1 * deltaC1 + deltaC2 * deltaC2);
        }

        public void DrawDivergence(Bitmap testImage, DNA dna, TaskState state, bool invert, bool normalize) {
            Debug.Assert(testImage.Width == state.EvalImageWidth);
            Debug.Assert(testImage.Height == state.EvalImageHeight);
            double pixelDivergenceMultiplier = 255d / GetMaxDivPerPixel();

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
                    var divergence = (float)GetDelta(originalPointer, testPointer);
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
            return new PerceptualEvaluator(Smooth) {
                ColorImportance = ColorImportance,
                ColorSpace = ColorSpace,
            };
        }


        void IModule.ReadSettings(NBTag tag) {
            Smooth = tag.GetBool(nameof(Smooth), Smooth);
            if (Enum.TryParse<ColorSpace>(tag.GetString(nameof(ColorSpace), nameof(ColorSpace.CieLuv)), true, out var newColorSpace))
                ColorSpace = newColorSpace;
            ColorImportance = tag.GetDouble(nameof(ColorImportance), ColorImportance);
        }

        void IModule.WriteSettings(NBTag tag) {
            tag.Append(nameof(Smooth), Smooth);
            tag.Append(nameof(ColorSpace), ColorSpace.ToString());
            tag.Append(nameof(ColorImportance), ColorImportance);
        }

        // XN/YN/ZN are illuminant D65 tristimulus values
        const double
            XN = 95.0489,
            YN = 100.000,
            ZN = 108.884;

        // these constant are used in CIEXYZ -> CIELAB conversion
        const double
            LinearThreshold = (6/29d)*(6/29d)*(6/29d),
            LinearMultiplier = 1 / (3 * (6 / 29d) * (6 / 29d)),
            LinearConstant = (4/29d);

        // these constants are used in CIEXYZ -> CIELUV conversion
        const double RefU = (4 * XN) / (XN + (15 * YN) + (3 * ZN));
        const double RefV = (9 * YN) / (XN + (15 * YN) + (3 * ZN));

        // Conversion from RGB to CIELAB, using illuminant D65.
        static LabColor ToLab(byte b, byte g, byte r) {
            // RGB are assumed to be in [0...255] range
            double R = srgbLUT[r];
            double G = srgbLUT[g];
            double B = srgbLUT[b];

            // CIEXYZ coordinates are normalized to [0...1]
            double x = 0.4124564*R + 0.3575761*G + 0.1804375*B;
            double y = 0.2126729*R + 0.7151522*G + 0.0721750*B;
            double z = 0.0193339*R + 0.1191920*G + 0.9503041*B;

            double xRatio = x/XN;
            double yRatio = y/YN;
            double zRatio = z/ZN;
            
            return new LabColor(
                // L is normalized to [0...100]
                (float)(116*XyzToLab(yRatio) - 16),
                (float)(500 *(XyzToLab(xRatio) - XyzToLab(yRatio))),
                (float)(200 *(XyzToLab(yRatio) - XyzToLab(zRatio))));
        }

        static double XyzToLab(double ratio) {
            if (ratio > LinearThreshold) {
                return Math.Pow(ratio, 1/3d);
            } else {
                return LinearMultiplier*ratio + LinearConstant;
            }
        }

        // Conversion from RGB to CIELAB, using illuminant D65.
        static LabColor ToLuv(byte b, byte g, byte r) {
            // RGB are assumed to be in [0...255] range
            double R = srgbLUT[r];
            double G = srgbLUT[g];
            double B = srgbLUT[b];

            // CIEXYZ coordinates are normalized to [0...1]
            double x = 0.4124564 * R + 0.3575761 * G + 0.1804375 * B;
            double y = 0.2126729 * R + 0.7151522 * G + 0.0721750 * B;
            double z = 0.0193339 * R + 0.1191920 * G + 0.9503041 * B;

            double u, v;
            if (x != 0 || y != 0 || z != 0) {
                u = (4 * x) / (x + (15 * y) + (3 * z));
                v = (9 * y) / (x + (15 * y) + (3 * z));
            } else { // pitch black, technically u/v is "undefined" in CIELUV but I don't want to deal with a NaN
                u = 0;
                v = 0;
            }

            y /= 100;
            if (y > 0.008856) y = Math.Pow(y, 1 / 3d);
            else y = (7.787 * y) + (16 / 116d);

            var cieL = (116 * y) - 16;
            var cieU = 13 * cieL * (u - RefU);
            var cieV = 13 * cieL * (v - RefV);
            return new LabColor(cieL, cieU, cieV);
        }

        private static readonly double[] srgbLUT = InitLinearLUT();

        // Creates a lookup table from 8-bit sRGB values to linear RGB values in [0...100] range.
        // Used for RGB-to-CIEXYZ conversion
        private static double[] InitLinearLUT() {
            var lut = new double[256];
            for (int i = 0; i < lut.Length; i++) {
                double value = i / 255d;
                double linearValue;
                if (value > 0.04045)
                    linearValue = Math.Pow((value + 0.055) / 1.055, 2.4);
                else
                    linearValue = value / 12.92;
                lut[i] = linearValue * 100;
            }
            return lut;
        }
    }

    readonly ref struct LabColor {
        public LabColor(double L, double a, double b) {
            this.L = L;
            this.a = a;
            this.b = b;
        }
        public readonly double L, a, b;
    }
}
