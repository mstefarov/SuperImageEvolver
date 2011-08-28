using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;


namespace SuperImageEvolver {

    public class RGBEvaluatorFactory : IModuleFactory {
        public Type ModuleType { get { return typeof( RGBEvaluator ); } }
        public string ID { get { return "std.RGBEvaluator.1"; } }
        public ModuleFunction Function { get { return ModuleFunction.Evaluator; } }
        public ModulePreset[] Presets {
            get {
                return new ModulePreset[]{
                    new ModulePreset("RGB (Fast)", ()=>(new RGBEvaluator(false)), this ),
                    new ModulePreset("RGB (Smooth)", ()=>(new RGBEvaluator(true)), this )
                };
            }
        }
        public IModule GetInstance() { return new RGBEvaluator(); }
    }


    unsafe class RGBEvaluator : IEvaluator {

        public bool Smooth { get; set; }
        public bool Emphasized { get; set; }
        public double EmphasisAmount { get; set; }

        double maxDivergence;


        public RGBEvaluator() {
            Smooth = true;
            Emphasized = false;
            EmphasisAmount = 2;
        }

        public RGBEvaluator( bool _smooth )
            : this() {
            Smooth = _smooth;
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
                        originalPointer = (byte*)task.ImageData.Scan0 + task.ImageData.Stride * i;
                        testPointer = (byte*)testData.Scan0 + testData.Stride * i;
                        for( int j = 0; j < task.ImageWidth; j++ ) {
                            int B = Math.Abs( *originalPointer - *testPointer );
                            int G = Math.Abs( originalPointer[1] - testPointer[1] );
                            int R = Math.Abs( originalPointer[2] - testPointer[2] );
                            sum += R * R + B * B + G * G;
                            originalPointer += 4;
                            testPointer += 4;
                        }
                        if( sum > roundedMax ) break;
                    }
                } else {
                    for( int i = 0; i < task.ImageHeight; i++ ) {
                        originalPointer = (byte*)task.ImageData.Scan0 + task.ImageData.Stride * i;
                        testPointer = (byte*)testData.Scan0 + testData.Stride * i;
                        for( int j = 0; j < task.ImageWidth; j++ ) {
                            int B = Math.Abs( *originalPointer - *testPointer );
                            int G = Math.Abs( originalPointer[1] - testPointer[1] );
                            int R = Math.Abs( originalPointer[2] - testPointer[2] );
                            sum += (long)(Math.Pow( R, EmphasisAmount ) + Math.Pow( G, EmphasisAmount ) + Math.Pow( B, EmphasisAmount ));
                            originalPointer += 4;
                            testPointer += 4;
                        }
                        if( sum > roundedMax ) break;
                    }
                }
            } else {
                for( int i = 0; i < task.ImageHeight; i++ ) {
                    originalPointer = (byte*)task.ImageData.Scan0 + task.ImageData.Stride * i;
                    testPointer = (byte*)testData.Scan0 + testData.Stride * i;
                    for( int j = 0; j < task.ImageWidth; j++ ) {
                        int B = Math.Abs( *originalPointer - *testPointer );
                        int G = Math.Abs( originalPointer[1] - testPointer[1] );
                        int R = Math.Abs( originalPointer[2] - testPointer[2] );
                        sum += R + B + G;
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