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

        public LumaEvaluator() {}


        public LumaEvaluator( bool smooth ) {
            Smooth = smooth;
        }


        public double CalculateDivergence( Bitmap testImage, DNA dna, TaskState state, double max ) {
            double maxDivergence = state.ImageWidth * state.ImageHeight * 2 * 255;
            long sum = 0;
            long roundedMax = (long)( max * maxDivergence + 1 );
            using( Graphics g = Graphics.FromImage( testImage ) ) {
                g.Clear( state.ProjectOptions.Matte );
                g.SmoothingMode = ( Smooth ? SmoothingMode.HighQuality : SmoothingMode.HighSpeed );
                for( int i = 0; i < dna.Shapes.Length; i++ ) {
                    g.FillPolygon( new SolidBrush( dna.Shapes[i].Color ), dna.Shapes[i].Points, FillMode.Alternate );
                }
            }

            BitmapData testData = testImage.LockBits( new Rectangle( Point.Empty, testImage.Size ),
                                                      ImageLockMode.ReadOnly,
                                                      PixelFormat.Format32bppArgb );
            for( int i = 0; i < state.ImageHeight; i++ ) {
                byte* originalPointer = (byte*)state.WorkingImageData.Scan0 + state.WorkingImageData.Stride * i;
                byte* testPointer = (byte*)testData.Scan0 + testData.Stride * i;
                for( int j = 0; j < state.ImageWidth; j++ ) {

                    /*
                    int originalLuma = (299 * originalPointer[2] + 587 * originalPointer[1] + 114 * *originalPointer) / 1000;
                    int testLuma = (299 * testPointer[2] + 587 * testPointer[1] + 114 * *testPointer) / 1000;
                    int deltaLuma = (299 * Math.Abs( originalPointer[2] - testPointer[2] ) +
                                    587 * Math.Abs( originalPointer[1] - testPointer[1] ) +
                                    114 * Math.Abs( *originalPointer - *testPointer )) / 1500; // 0-511

                    int deltaU = Math.Abs( (originalPointer[2] - originalLuma) - (testPointer[2] - testLuma) ); // 0-255
                    int deltaV = Math.Abs( (*originalPointer - originalLuma) - (*testPointer - testLuma) ); // 0-255
                    
                    sum += deltaLuma + (deltaU + deltaV)/2;
                    */


                    int originalLumi =
                        ( Math.Min( Math.Min( originalPointer[2], originalPointer[1] ), *originalPointer ) +
                          Math.Max( Math.Max( originalPointer[2], originalPointer[1] ), *originalPointer ) ) / 2;
                    int testLumi = ( Math.Min( Math.Min( testPointer[2], testPointer[1] ), *testPointer ) +
                                     Math.Max( Math.Max( testPointer[2], testPointer[1] ), *testPointer ) ) / 2;

                    byte lumiDelta = (byte)Math.Abs( originalLumi - testLumi );


                    int oringinalChroma =
                        ( Math.Max( Math.Max( originalPointer[2], originalPointer[1] ), *originalPointer ) -
                          Math.Min( Math.Min( originalPointer[2], originalPointer[1] ), *originalPointer ) );
                    int testChroma = ( Math.Max( Math.Max( testPointer[2], testPointer[1] ), *testPointer ) -
                                       Math.Min( Math.Min( testPointer[2], testPointer[1] ), *testPointer ) );


                    sum += lumiDelta * 2 + (byte)( Math.Abs( oringinalChroma - testChroma ) * lumiDelta / 255 );

                    originalPointer += 4;
                    testPointer += 4;
                }
                if( sum > roundedMax ) break;
            }
            testImage.UnlockBits( testData );
            return sum / maxDivergence;
        }


        object ICloneable.Clone() {
            return new LumaEvaluator( Smooth );
        }


        void IModule.ReadSettings( NBTag tag ) {}

        void IModule.WriteSettings( NBTag tag ) {}
    }
}