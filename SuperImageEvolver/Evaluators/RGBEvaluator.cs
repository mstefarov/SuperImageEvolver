using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;


namespace SuperImageEvolver {

    public class RGBEvaluatorFactory : IModuleFactory {
        public Type ModuleType { get { return typeof( RGBEvaluator ); } }
        public string ID { get { return "std.RGBEvaluator.1"; } }
        public ModuleFunction Function { get { return ModuleFunction.Evaluator; } }
        public ModulePreset[] Presets {
            get {
                return new[]{
                    new ModulePreset("RGB (Fast)", ()=>(new RGBEvaluator(false)), this ),
                    new ModulePreset("RGB (Smooth)", ()=>(new RGBEvaluator(true)), this )
                };
            }
        }
        public IModule GetInstance() { return new RGBEvaluator(); }
    }


    sealed unsafe class RGBEvaluator : IEvaluator {

        public bool Smooth { get; set; }
        public bool Emphasized { get; set; }
        public double EmphasisAmount { get; set; }

        double maxDivergence;


        public RGBEvaluator() {
            Smooth = true;
            Emphasized = false;
            EmphasisAmount = 2;
        }

        public RGBEvaluator( bool smooth )
            : this() {
            Smooth = smooth;
        }


        public void Initialize( TaskState state ) { }


        public double CalculateDivergence( Bitmap testImage, DNA dna, TaskState task, double max ) {

            if( Emphasized ) {
                if( EmphasisAmount == 2 ) {
                    maxDivergence = 3L * task.ImageWidth * task.ImageHeight * 255L * 255L;
                } else {
                    maxDivergence = (long)(3L * task.ImageWidth * task.ImageHeight * Math.Pow( 255, EmphasisAmount ));
                }
            } else {
                maxDivergence = 3L * task.ImageWidth * task.ImageHeight * 255L;
            }

            long sum = 0;
            long roundedMax = (long)(max * maxDivergence + 1);
            using( Graphics g = Graphics.FromImage( testImage ) ) {
                g.Clear( Color.White );
                g.SmoothingMode = (Smooth ? SmoothingMode.HighQuality : SmoothingMode.HighSpeed);

                for( int i = 0; i < dna.Shapes.Length; i++ ) {
                    g.FillPolygon( new SolidBrush( dna.Shapes[i].Color ), dna.Shapes[i].Points, FillMode.Alternate );
                }
            }
            byte* originalPointer, testPointer;

            BitmapData testData = testImage.LockBits( new Rectangle( Point.Empty, testImage.Size ),
                                                      ImageLockMode.ReadOnly,
                                                      PixelFormat.Format32bppArgb );

            if( Emphasized ) {
                if( EmphasisAmount == 2 ) {
                    for( int i = 0; i < task.ImageHeight; i++ ) {
                        originalPointer = (byte*)task.OriginalImageData.Scan0 + task.OriginalImageData.Stride * i;
                        testPointer = (byte*)testData.Scan0 + testData.Stride * i;
                        for( int j = 0; j < task.ImageWidth; j++ ) {
                            int b = Math.Abs( *originalPointer - *testPointer );
                            int g = Math.Abs( originalPointer[1] - testPointer[1] );
                            int r = Math.Abs( originalPointer[2] - testPointer[2] );
                            sum += r * r + b * b + g * g;
                            originalPointer += 4;
                            testPointer += 4;
                        }
                        if( sum > roundedMax ) break;
                    }
                } else {
                    for( int i = 0; i < task.ImageHeight; i++ ) {
                        originalPointer = (byte*)task.OriginalImageData.Scan0 + task.OriginalImageData.Stride * i;
                        testPointer = (byte*)testData.Scan0 + testData.Stride * i;
                        for( int j = 0; j < task.ImageWidth; j++ ) {
                            int b = Math.Abs( *originalPointer - *testPointer );
                            int g = Math.Abs( originalPointer[1] - testPointer[1] );
                            int r = Math.Abs( originalPointer[2] - testPointer[2] );
                            sum += (long)(Math.Pow( r, EmphasisAmount ) + Math.Pow( g, EmphasisAmount ) + Math.Pow( b, EmphasisAmount ));
                            originalPointer += 4;
                            testPointer += 4;
                        }
                        if( sum > roundedMax ) break;
                    }
                }
            } else {
                for( int i = 0; i < task.ImageHeight; i++ ) {
                    originalPointer = (byte*)task.OriginalImageData.Scan0 + task.OriginalImageData.Stride * i;
                    testPointer = (byte*)testData.Scan0 + testData.Stride * i;
                    for( int j = 0; j < task.ImageWidth; j++ ) {
                        int b = Math.Abs( *originalPointer - *testPointer );
                        int g = Math.Abs( originalPointer[1] - testPointer[1] );
                        int r = Math.Abs( originalPointer[2] - testPointer[2] );
                        sum += r + b + g;
                        originalPointer += 4;
                        testPointer += 4;
                    }
                    if( sum > roundedMax ) break;
                }
            }

            testImage.UnlockBits( testData );
            if( Emphasized ) {
                return Math.Pow( sum / maxDivergence, 1 / EmphasisAmount );
            } else {
                return sum / maxDivergence;
            }
        }


        object ICloneable.Clone() {
            return new RGBEvaluator {
                Smooth = Smooth,
                Emphasized = Emphasized,
                EmphasisAmount = EmphasisAmount
            };
        }


        void IModule.ReadSettings( NBTag tag ) { }

        void IModule.WriteSettings( NBTag tag ) { }

    }
}