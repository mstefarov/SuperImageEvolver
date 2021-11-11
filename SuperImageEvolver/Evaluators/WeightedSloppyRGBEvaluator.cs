using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace SuperImageEvolver {
    public class WeightedSloppyRGBEvaluatorFactory : IModuleFactory {
        public Type ModuleType {
            get { return typeof( WeightedSloppyRGBEvaluator ); }
        }

        public string ID {
            get { return "std.WeightedSloppyRGBEvaluator.1"; }
        }

        public ModuleFunction Function {
            get { return ModuleFunction.Evaluator; }
        }

        public ModulePreset[] Presets {
            get {
                return new[] {
                    new ModulePreset( "Weighted RGB (Sloppy)", () => ( new WeightedSloppyRGBEvaluator() ), this )
                };
            }
        }


        public IModule GetInstance() {
            return new WeightedSloppyRGBEvaluator();
        }
    }


    sealed unsafe class WeightedSloppyRGBEvaluator : IEvaluator {

        double maxDivergence;
        Bitmap halfResImage;
        BitmapData halfResData;

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

        public WeightedSloppyRGBEvaluator() {
            Smooth = true;
            EmphasisCenterX = 0.5;
            EmphasisCenterY = 0.5;
            CenterRange = 0.25;
            EmphasisRange = 0.5;
            OutsideWeight = 0.5;
        }

        private int imageWidth, imageHeight;
        private double maxRadius;

        public void Initialize( TaskState state ) {
            imageWidth = state.ImageWidth / 2;
            imageHeight = state.ImageHeight / 2;
            maxRadius = Math.Sqrt(imageWidth * imageWidth + imageHeight * imageHeight) / 2; // half of the image diagonal

            halfResImage = new Bitmap( state.ImageWidth / 2, state.ImageHeight / 2, PixelFormat.Format32bppArgb );
            using( Graphics g = Graphics.FromImage( halfResImage ) ) {
                g.InterpolationMode = InterpolationMode.HighQualityBilinear;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.DrawImage( state.WorkingImageCopy, 0, 0, halfResImage.Width, halfResImage.Height );
            }
            halfResData = halfResImage.LockBits( new Rectangle( Point.Empty, halfResImage.Size ),
                                                 ImageLockMode.ReadOnly,
                                                 PixelFormat.Format32bppArgb );

            double maxDiv = 0;
            for (int x = 0; x < imageWidth; x++) {
                for (int y = 0; y < imageHeight; y++) {
                    maxDiv += 3d * 255d * GetWeight(x, y);
                }
            }
            maxDivergence = maxDiv;
        }

        private double GetWeight(int x, int y) {
            int emphasisPixelX = (int)Math.Round(EmphasisCenterX * imageWidth);
            int emphasisPixelY = (int)Math.Round(EmphasisCenterY * imageHeight);
            double distToCenter = Math.Sqrt((x - emphasisPixelX) * (x - emphasisPixelX) +
                                            (y - emphasisPixelY) * (y - emphasisPixelY));
            double ratioToCenter = distToCenter / maxRadius;
            if (ratioToCenter > EmphasisRange) {
                return OutsideWeight;
            }

            double centerRatio = EmphasisRange * CenterRange;
            if (ratioToCenter < centerRatio) {
                return 1;
            } else {
                var rampRatio = (ratioToCenter - centerRatio) / (EmphasisRange * (1 - CenterRange));
                return rampRatio + (1 - rampRatio) * OutsideWeight; // lerp between 1 and OutsideWeight
            }
        }

        public double CalculateDivergence( Bitmap testImage, DNA dna, TaskState state, double maxAcceptableDivergence ) {
            long sum = 0;
            long roundedMax = (long)( maxAcceptableDivergence * maxDivergence + 1 );
            using( Graphics g = Graphics.FromImage( testImage ) ) {
                g.Clear( state.ProjectOptions.Matte );
                g.Transform = new Matrix( .5f, 0, 0, .5f, 0, 0 );
                g.SmoothingMode = ( Smooth ? SmoothingMode.HighQuality : SmoothingMode.HighSpeed );
                for( int i = 0; i < dna.Shapes.Length; i++ ) {
                    g.FillPolygon( new SolidBrush( dna.Shapes[i].Color ), dna.Shapes[i].Points, FillMode.Alternate );
                }
            }

            BitmapData testData = testImage.LockBits( new Rectangle( Point.Empty, testImage.Size ),
                                                      ImageLockMode.ReadOnly,
                                                      PixelFormat.Format32bppArgb );
            for( int i = 0; i < state.ImageHeight / 2; i++ ) {
                byte* originalPointer = (byte*)halfResData.Scan0 + halfResData.Stride * i;
                byte* testPointer = (byte*)testData.Scan0 + testData.Stride * i;
                for( int j = 0; j < state.ImageWidth / 2; j++ ) {
                    int b = Math.Abs( *originalPointer - *testPointer );
                    int g = Math.Abs( originalPointer[1] - testPointer[1] );
                    int r = Math.Abs( originalPointer[2] - testPointer[2] );
                    sum += (long)Math.Ceiling(GetWeight(j, i) * (r + b + g));
                    originalPointer += 4;
                    testPointer += 4;
                }
                if( sum > roundedMax ) {
                    sum = (long)maxDivergence;
                    break;
                }
            }
            testImage.UnlockBits( testData );
            return sum / maxDivergence;
        }
        
        public Rectangle GetDebugCenterBoundary() {
            // Everything is "times 2" because internally the sloppy evaluator uses half resolution
            int cx = (int)Math.Round(EmphasisCenterX * imageWidth * 2);
            int cy = (int)Math.Round(EmphasisCenterY * imageHeight * 2);
            int cr = (int)Math.Ceiling(maxRadius * EmphasisRange * CenterRange * 2);
            return new Rectangle(cx - cr, cy - cr, cr * 2, cr * 2);
        }

        public Rectangle GetDebugOuterBoundary() {
            int cx = (int)Math.Round(EmphasisCenterX * imageWidth * 2);
            int cy = (int)Math.Round(EmphasisCenterY * imageHeight * 2);
            int cr = (int)Math.Ceiling(maxRadius * EmphasisRange * 2);
            return new Rectangle(cx - cr, cy - cr, cr * 2, cr * 2);
        }

        object ICloneable.Clone() {
            return new WeightedSloppyRGBEvaluator {
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
