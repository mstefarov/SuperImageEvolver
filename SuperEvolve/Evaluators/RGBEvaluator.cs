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
                    new ModulePreset("RGB (Fast)", ()=>(new RGBEvaluator(false)) ),
                    new ModulePreset("RGB (Smooth)", ()=>(new RGBEvaluator(true)) )
                };
            }
        }
        public IModule GetInstance() { return new RGBEvaluator(); }
    }



    unsafe class RGBEvaluator : IEvaluator {

        bool smooth;
        double maxDivergence;


        public RGBEvaluator() { }

        public RGBEvaluator( bool _smooth ) {
            smooth = _smooth;
        }


        public void Initialize( TaskState state ) {
            maxDivergence = state.ImageWidth * state.ImageHeight * 3 * 255;
        }


        public double CalculateDivergence( Bitmap testImage, DNA dna, TaskState task, double max ) {
            long sum = 0;
            long roundedMax = (long)(max * maxDivergence + 1);
            using( Graphics g = Graphics.FromImage( testImage ) ) {
                g.Clear( Color.White );
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
                    sum += Math.Abs( *originalPointer - *testPointer ) +
                           Math.Abs( originalPointer[1] - testPointer[1] ) +
                           Math.Abs( originalPointer[2] - testPointer[2] );
                    originalPointer += 4;
                    testPointer += 4;
                }
                if( sum > roundedMax ) break;
            }
            testImage.UnlockBits( testData );
            return sum / maxDivergence;
        }


        object ICloneable.Clone() {
            return new RGBEvaluator( smooth );
        }

        void IModule.ReadSettings( BinaryReader reader, int settingsLength ) {
            smooth = reader.ReadBoolean();
        }

        void IModule.WriteSettings( BinaryWriter writer ) {
            writer.Write( 1 );
            writer.Write( smooth );
        }
        public IModuleFactory Factory { get; set; }
    }
}