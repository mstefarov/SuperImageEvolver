using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;


namespace SuperImageEvolver {

    public class SloppyRGBEvaluatorFactory : IModuleFactory {
        public Type ModuleType { get { return typeof( SloppyRGBEvaluator ); } }
        public string ID { get { return "std.SloppyRGBEvaluator.1"; } }
        public ModuleFunction Function { get { return ModuleFunction.Evaluator; } }
        public ModulePreset[] Presets {
            get {
                return new[]{
                    new ModulePreset("RGB (Sloppy)", ()=>(new SloppyRGBEvaluator()), this )
                };
            }
        }
        public IModule GetInstance() { return new SloppyRGBEvaluator(); }
    }


    sealed unsafe class SloppyRGBEvaluator : IEvaluator {

        double maxDivergence;
        Bitmap halfResImage;
        BitmapData halfResData;

        public bool Smooth { get; set; }
        public bool Emphasized { get; set; }
        public double EmphasisAmount { get; set; }

        public SloppyRGBEvaluator() {
            Smooth = true;
            Emphasized = false;
            EmphasisAmount = 2;
        }


        public void Initialize( TaskState state ) {
            halfResImage = new Bitmap( state.ImageWidth / 2, state.ImageHeight / 2, PixelFormat.Format32bppArgb );
            using( Graphics g = Graphics.FromImage( halfResImage ) ) {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.DrawImage( state.WorkingImageCopy, 0, 0, halfResImage.Width, halfResImage.Height );
            }
            halfResData = halfResImage.LockBits( new Rectangle( Point.Empty, halfResImage.Size ),
                                                 ImageLockMode.ReadOnly,
                                                 PixelFormat.Format32bppArgb );
        }


        public double CalculateDivergence( Bitmap testImage, DNA dna, TaskState state, double max ) {
            if( Emphasized ) {
                if( EmphasisAmount == 2 ) {
                    maxDivergence = 3L * state.ImageWidth / 2L * state.ImageHeight / 2L * 255L * 255L;
                } else {
                    maxDivergence = (long)(3L * state.ImageWidth / 2L * state.ImageHeight / 2L * Math.Pow( 255, EmphasisAmount ));
                }
            } else {
                maxDivergence = 3L * state.ImageWidth / 2L * state.ImageHeight / 2L * 255L;
            }

            long sum = 0;
            long roundedMax = (long)(max * maxDivergence + 1);
            using( Graphics g = Graphics.FromImage( testImage ) ) {
                g.Clear( state.ProjectOptions.Matte );
                g.Transform = new Matrix( .5f, 0, 0, .5f, 0, 0 );
                g.SmoothingMode = (Smooth ? SmoothingMode.HighQuality : SmoothingMode.HighSpeed);
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
                    if( Emphasized ) {
                        if( EmphasisAmount == 2 ) {
                            sum += r * r + b * b + g * g;
                        } else {
                            sum += (long)(Math.Pow( r, EmphasisAmount ) + Math.Pow( g, EmphasisAmount ) + Math.Pow( b, EmphasisAmount ));
                        }
                    } else {
                        sum += r + b + g;
                    }
                    originalPointer += 4;
                    testPointer += 4;
                }
                if( sum > roundedMax ) break;
            }
            testImage.UnlockBits( testData );
            if( Emphasized ) {
                return Math.Pow( sum / maxDivergence, 1 / EmphasisAmount );
            } else {
                return sum / maxDivergence;
            }
        }


        object ICloneable.Clone() {
            return new SloppyRGBEvaluator {
                Smooth = Smooth,
                Emphasized = Emphasized,
                EmphasisAmount = EmphasisAmount
            };
        }



        void IModule.ReadSettings( NBTag tag ) { }

        void IModule.WriteSettings( NBTag tag ) { }

    }
}