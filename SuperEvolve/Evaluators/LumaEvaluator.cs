using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;


namespace SuperImageEvolver {

    public class LumaEvaluatorFactory : IModuleFactory {
        public Type ModuleType { get { return typeof( LumaEvaluator ); } }
        public string ID { get { return "std.LumaEvaluator.1"; } }
        public ModuleFunction Function { get { return ModuleFunction.Evaluator; } }
        public ModulePreset[] Presets {
            get {
                return new ModulePreset[]{
                    new ModulePreset("RGB+Luma (Fast)", ()=>(new LumaEvaluator(false)) ),
                    new ModulePreset("RGB+Luma (Smooth)", ()=>(new LumaEvaluator(true)) )
                };
            }
        }
        public IModule GetInstance() { return new LumaEvaluator(); }
    }

    unsafe class LumaEvaluator : IEvaluator {

        double maxDivergence;

        public void Initialize( TaskState state ) {
            maxDivergence = state.ImageWidth * state.ImageHeight * 3 * 255;
        }

        bool smooth;
        public LumaEvaluator() { }
        public LumaEvaluator( bool _smooth ) {
            smooth = _smooth;
        }

        public double CalculateDivergence( Bitmap testImage, DNA dna, TaskState task, double max ) {
            long sum = 0;
            long roundedMax = (long)(max * maxDivergence + 1);
            using( Graphics g = Graphics.FromImage( testImage ) ) {
                g.Clear( Color.Red );
                if( smooth ) g.SmoothingMode = SmoothingMode.HighQuality;
                for( int i = 0; i < dna.Shapes.Length; i++ ) {
                    g.FillPolygon( new SolidBrush( dna.Shapes[i].Color ), dna.Shapes[i].Points, FillMode.Alternate );
                }
            }
            byte* originalPointer, testPointer;

            BitmapData testData = testImage.LockBits( new Rectangle( Point.Empty, testImage.Size ),
                                                      ImageLockMode.ReadOnly,
                                                      PixelFormat.Format32bppArgb );
            for( int i = 0; i < task.ImageHeight; i++ ) {
                originalPointer = (byte*)task.ImageData.Scan0 + task.ImageData.Stride * i;
                testPointer = (byte*)testData.Scan0 + testData.Stride * i;
                for( int j = 0; j < task.ImageWidth; j++ ) {
                    int originalLuma = (299 * originalPointer[2] + 587 * originalPointer[1] + 114 * *originalPointer) / 1000;
                    int testLuma = (299 * testPointer[2] + 587 * testPointer[1] + 114 * *testPointer) / 1000;

                    int deltaU = Math.Abs( (originalPointer[2] - originalLuma) - (testPointer[2] - testLuma) );
                    int deltaV = Math.Abs( (*originalPointer - originalLuma) - (*testPointer - testLuma) );

                    sum += Math.Abs( originalLuma - testLuma ) + deltaU + deltaV;

                    originalPointer += 4;
                    testPointer += 4;
                }
                if( sum > roundedMax ) break;
            }
            testImage.UnlockBits( testData );
            return sum / maxDivergence;
        }



        object ICloneable.Clone() {
            return new LumaEvaluator( smooth );
        }

        void IModule.ReadSettings( BinaryReader reader, int length ) {
            smooth = reader.ReadBoolean();
        }

        void IModule.WriteSettings( BinaryWriter writer ) {
            writer.Write( 1 );
            writer.Write( smooth );
        }
        public IModuleFactory Factory { get; set; }
    }
}