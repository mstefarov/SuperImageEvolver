using System;
using System.Drawing;
using System.IO;

namespace SuperImageEvolver {

    public class SegmentedInitializerFactory : IModuleFactory {
        public Type ModuleType { get { return typeof( SegmentedInitializer ); } }
        public string ID { get { return "std.SegmentedInitializer.1"; } }
        public ModuleFunction Function { get { return ModuleFunction.Initializer; } }
        public ModulePreset[] Presets {
            get {
                return new ModulePreset[]{
                    new ModulePreset("Segmented", ()=>(new SegmentedInitializer(Color.Black)) )
                };
            }
        }
        public IModule GetInstance() { return new SegmentedInitializer( Color.Black ); }
    }


    public class SegmentedInitializer : IInitializer {
        public Color Color;

        public SegmentedInitializer( Color _color ) {
            Color = _color;
        }

        public DNA Initialize( Random rand, TaskState task ) {
            DNA dna = new DNA();
            dna.Shapes = new DNA.Shape[task.Shapes];
            int shapesPersegment = task.Shapes / 9;
            int shapeCounter = 0;

            for( int i = 0; i < task.Shapes - shapesPersegment * 9; i++ ) {
                DNA.Shape shape = new DNA.Shape();
                shape.Color = Color.FromArgb( 0, Color.R, Color.G, Color.B );
                shape.Points = new Point[task.Vertices];
                for( int j = 0; j < shape.Points.Length; j++ ) {
                    shape.Points[j] = new Point( rand.Next( task.ImageWidth ), rand.Next( task.ImageHeight ) );
                }
                dna.Shapes[i] = shape;
                shapeCounter++;
            }

            for( int x = 0; x < 3; x++ ) {
                for( int y = 0; y < 3; y++ ) {
                    for( int i = 0; i < shapesPersegment; i++ ) {
                        DNA.Shape shape = new DNA.Shape();
                        shape.Color = Color.FromArgb( 0, Color.R, Color.G, Color.B );
                        shape.Points = new Point[task.Vertices];
                        for( int j = 0; j < shape.Points.Length; j++ ) {
                            shape.Points[j] = new Point( task.ImageWidth / 3 * x + rand.Next( task.ImageWidth / 3 ),
                                                         task.ImageHeight / 3 * y + rand.Next( task.ImageHeight / 3 ) );
                        }
                        dna.Shapes[shapeCounter] = shape;
                        shapeCounter++;
                    }
                }
            }
            return dna;
        }

        object ICloneable.Clone() {
            return new SegmentedInitializer( Color );
        }

        void IModule.ReadSettings( BinaryReader reader, int settingsLength ) {
            Color = Color.FromArgb( reader.ReadInt32() );
        }

        void IModule.WriteSettings( BinaryWriter writer ) {
            writer.Write( 4 );
            writer.Write( Color.ToArgb() );
        }
    }
}