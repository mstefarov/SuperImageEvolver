using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;


namespace SuperImageEvolver {

    public class SloppyRGBEvaluatorFactory : IModuleFactory {
        public Type ModuleType { get { return typeof( SloppyRGBEvaluator ); } }
        public string ID { get { return "std.SloppyRGBEvaluator.1"; } }
        public ModuleFunction Function { get { return ModuleFunction.Evaluator; } }
        public ModulePreset[] Presets {
            get {
                return new ModulePreset[]{
                    new ModulePreset("RGB (Sloppy)", ()=>(new SloppyRGBEvaluator()) )
                };
            }
        }
        public IModule GetInstance() { return new SloppyRGBEvaluator(); }
    }



    unsafe class SloppyRGBEvaluator : IEvaluator {

        double maxDivergence;
        Bitmap halfResImage;
        BitmapData halfResData;

        public bool Smooth { get; set; }

        public SloppyRGBEvaluator() { }


        public void Initialize( TaskState state ) {
            halfResImage = new Bitmap( state.ImageWidth / 2, state.ImageHeight / 2, PixelFormat.Format32bppArgb );
            using( Graphics g = Graphics.FromImage( halfResImage ) ) {
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.DrawImage( state.Image, 0, 0, halfResImage.Width, halfResImage.Height );
            }
            maxDivergence = halfResImage.Width * halfResImage.Height * 3 * 255;
            halfResData = halfResImage.LockBits( new Rectangle( Point.Empty, halfResImage.Size ),
                                                 ImageLockMode.ReadOnly,
                                                 PixelFormat.Format32bppArgb );
        }


        public double CalculateDivergence( Bitmap testImage, DNA dna, TaskState task, double max ) {
            long sum = 0;
            long roundedMax = (long)(max * maxDivergence + 1);
            using( Graphics g = Graphics.FromImage( testImage ) ) {
                g.Clear( Color.White );
                g.Transform = new Matrix( .5f, 0, 0, .5f, 0, 0 );
                g.SmoothingMode = (Smooth ? SmoothingMode.HighQuality : SmoothingMode.HighSpeed);
                for( int i = 0; i < dna.Shapes.Length; i++ ) {
                    g.FillPolygon( new SolidBrush( dna.Shapes[i].Color ), dna.Shapes[i].Points, FillMode.Alternate );
                }
            }
            byte* originalPointer, testPointer;

            BitmapData testData = testImage.LockBits( new Rectangle( Point.Empty, testImage.Size ),
                                                      ImageLockMode.ReadOnly,
                                                      PixelFormat.Format32bppArgb );
            for( int i = 0; i < task.ImageHeight / 2; i++ ) {
                originalPointer = (byte*)halfResData.Scan0 + halfResData.Stride * i;
                testPointer = (byte*)testData.Scan0 + testData.Stride * i;
                for( int j = 0; j < task.ImageWidth / 2; j++ ) {
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
            return new SloppyRGBEvaluator();
        }


        void IModule.ReadSettings( BinaryReader reader, int settingsLength ) { }

        void IModule.WriteSettings( BinaryWriter writer ) {
            writer.Write( 0 );
        }

    }
}