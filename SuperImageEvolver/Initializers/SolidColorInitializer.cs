using System;
using System.Drawing;
using System.IO;

namespace SuperImageEvolver {

    public class SolidColorInitializerFactory : IModuleFactory {
        public Type ModuleType { get { return typeof( SolidColorInitializer ); } }
        public string ID { get { return "std.SolidColorInitializer.1"; } }
        public ModuleFunction Function { get { return ModuleFunction.Initializer; } }
        public ModulePreset[] Presets {
            get {
                return new ModulePreset[]{
                    new ModulePreset("Full Random", ()=>(new SolidColorInitializer(Color.Black)) )
                };
            }
        }
        public IModule GetInstance() { return new SolidColorInitializer(Color.Black); }
    }

    class SolidColorInitializer : IInitializer {
        public Color Color;

        public SolidColorInitializer( Color _color ) {
            Color = _color;
        }

        DNA IInitializer.Initialize( Random rand, TaskState task ) {
            DNA dna = new DNA();
            dna.Shapes = new DNA.Shape[task.Shapes];
            for( int i = 0; i < dna.Shapes.Length; i++ ) {
                DNA.Shape shape = new DNA.Shape();
                shape.Color = Color.FromArgb(0,Color.R,Color.G,Color.B);
                shape.Points = new PointF[task.Vertices];
                for( int j = 0; j < shape.Points.Length; j++ ) {
                    shape.Points[j].X = rand.Next( task.ImageWidth );
                    shape.Points[j].Y = rand.Next( task.ImageHeight );
                }
                dna.Shapes[i] = shape;
            }
            return dna;
        }


        object ICloneable.Clone() {
            return new SolidColorInitializer( Color );
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