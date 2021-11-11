using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace SuperImageEvolver {
    public class LumaEvaluatorFactory : IModuleFactory {
        public Type ModuleType {
            get { return typeof( LumaEvaluator ); }
        }

        public string ID {
            get { return "std.LumaEvaluator.1"; }
        }

        public ModuleFunction Function {
            get { return ModuleFunction.Evaluator; }
        }

        public ModulePreset[] Presets {
            get {
                return new[] {
                    new ModulePreset( "RGB+Luma (Fast)", () => ( new LumaEvaluator( false ) ), this ),
                    new ModulePreset( "RGB+Luma (Smooth)", () => ( new LumaEvaluator( true ) ), this )
                };
            }
        }


        public IModule GetInstance() {
            return new LumaEvaluator();
        }
    }


    sealed unsafe class LumaEvaluator : IEvaluator {

        public void Initialize( TaskState state ) {}

        public bool Smooth { get; set; }

        public double LumaWeight { get; set; } = 2;

        public LumaEvaluator() {}


        public LumaEvaluator( bool smooth ) {
            Smooth = smooth;
        }


        public double CalculateDivergence( Bitmap testImage, DNA dna, TaskState state, double maxAcceptableDivergence ) {
            double maxDivergence = state.ImageWidth * state.ImageHeight * (100 + 100 / LumaWeight);
            double colorDeltaScale = (100 / Math.Sqrt(255 * 255 * 2)) / LumaWeight;
            double sum = 0;
            long roundedMax = (long)(maxAcceptableDivergence * maxDivergence + 1);
            DrawDna(testImage, dna, state);

            BitmapData testData = testImage.LockBits(new Rectangle(Point.Empty, testImage.Size),
                                                      ImageLockMode.ReadOnly,
                                                      PixelFormat.Format32bppArgb);
            for (int i = 0; i < state.ImageHeight; i++) {
                byte* originalPointer = (byte*)state.WorkingImageData.Scan0 + state.WorkingImageData.Stride * i;
                byte* testPointer = (byte*)testData.Scan0 + testData.Stride * i;
                for (int j = 0; j < state.ImageWidth; j++) {

                    var original = ToLab(originalPointer[0], originalPointer[1], originalPointer[2]);
                    var test = ToLab(testPointer[0], testPointer[1], testPointer[2]);

                    double LDelta = Math.Abs(original.L - test.L);
                    double abDelta = Math.Sqrt((original.a - test.a) * (original.a - test.a) + (original.b - test.b) * (original.b - test.b)) * colorDeltaScale;

                    sum += LDelta + abDelta;

                    originalPointer += 4;
                    testPointer += 4;
                }
                if (sum > roundedMax) break;
            }
            testImage.UnlockBits(testData);
            return sum / maxDivergence;
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

        public void DrawDivergence(Bitmap testImage, DNA dna, TaskState state, bool invert, bool normalize) {
            double pixelDivergenceMultiplier = 255/(100 + 100 / LumaWeight);
            double colorDeltaScale = (100 / Math.Sqrt(255 * 255 * 2)) / LumaWeight;
            DrawDna(testImage, dna, state);
            BitmapData testData = testImage.LockBits(new Rectangle(Point.Empty, testImage.Size),
                                          ImageLockMode.ReadOnly,
                                          PixelFormat.Format32bppArgb);
            
            float maxObservedDivergence = 0.001f;
            for (int i = 0; i < state.ImageHeight; i++) {
                byte* originalPointer = (byte*)state.WorkingImageData.Scan0 + state.WorkingImageData.Stride * i;
                byte* testPointer = (byte*)testData.Scan0 + testData.Stride * i;
                for (int j = 0; j < state.ImageWidth; j++) {

                    var original = ToLab(originalPointer[0], originalPointer[1], originalPointer[2]);
                    var test = ToLab(testPointer[0], testPointer[1], testPointer[2]);

                    double LDelta = Math.Abs(original.L - test.L);
                    double abDelta = Math.Sqrt((original.a - test.a) * (original.a - test.a) + (original.b - test.b) * (original.b - test.b)) * colorDeltaScale;

                    var divergence = (float)((LDelta + abDelta) * pixelDivergenceMultiplier);
                    maxObservedDivergence = Math.Max(maxObservedDivergence, divergence);
                    *(float*)testPointer = divergence;

                    originalPointer += 4;
                    testPointer += 4;
                }
            }
            
            float multiplier = 255/maxObservedDivergence;
            for (int i = 0; i < state.ImageHeight; i++) {
                byte* testPointer = (byte*)testData.Scan0 + testData.Stride * i;
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
            return new LumaEvaluator( Smooth );
        }


        void IModule.ReadSettings(NBTag tag) {
            Smooth = tag.GetBool(nameof(Smooth), Smooth);
        }

        void IModule.WriteSettings(NBTag tag) {
            tag.Append(nameof(Smooth), Smooth);
        }

        // XN/YN/ZN are illuminant D65 tristimulus values
        const double
            XN = 95.0489,
            YN = 100.000,
            ZN = 108.884;

        // these constant are used in CIEXYZ -> CIELAB conversion
        public const double
            LinearThreshold = (6/29d)*(6/29d)*(6/29d),
            LinearMultiplier = (1/3d)*(29/6d)*(29/6d),
            LinearConstant = (4/29d);

        // Conversion from RGB to CIELAB, using illuminant D65.
        static LabColor ToLab(byte b, byte g, byte r) {
            // RGB are assumed to be in [0...255] range
            double R = r/255d;
            double G = g/255d;
            double B = b/255d;

            // CIEXYZ coordinates are normalized to [0...1]
            double x = 0.4124564*R + 0.3575761*G + 0.1804375*B;
            double y = 0.2126729*R + 0.7151522*G + 0.0721750*B;
            double z = 0.0193339*R + 0.1191920*G + 0.9503041*B;

            double xRatio = x/XN;
            double yRatio = y/YN;
            double zRatio = z/ZN;
            
            return new LabColor(
                // L is normalized to [0...100]
                116*XyzToLab(yRatio) - 16,
                500*(XyzToLab(xRatio) - XyzToLab(yRatio)),
                200*(XyzToLab(yRatio) - XyzToLab(zRatio)));
        }

        static double XyzToLab(double ratio) {
            if (ratio > LinearThreshold) {
                return Math.Pow(ratio, 1/3d);
            } else {
                return LinearMultiplier*ratio + LinearConstant;
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
}
