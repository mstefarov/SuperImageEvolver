using System;
using System.Drawing;
using System.IO;

namespace SuperImageEvolver {

    public class RadialInitializerFactory : IModuleFactory {
        public Type ModuleType { get { return typeof( RadialInitializer ); } }
        public string ID { get { return "std.SquareInitializer.1"; } }
        public ModuleFunction Function { get { return ModuleFunction.Initializer; } }
        public ModulePreset[] Presets {
            get {
                return new ModulePreset[]{
                    new ModulePreset("Radial", ()=>(new RadialInitializer(Color.Black)) )
                };
            }
        }
        public IModule GetInstance() { return new RadialInitializer( Color.Black ); }
    }


    public class RadialInitializer : IInitializer {
        public Color Color;

        public RadialInitializer( Color _color ) {
            Color = _color;
        }

        public DNA Initialize( Random rand, TaskState task ) {
            DNA dna = new DNA();
            dna.Shapes = new DNA.Shape[task.Shapes];
            for( int i = 0; i < task.Shapes; i++ ) {
                DNA.Shape shape = new DNA.Shape();
                shape.Color = Color.FromArgb( 0, Color.R, Color.G, Color.B );
                shape.Points = new PointF[task.Vertices];
                int radius = rand.Next( 2, Math.Min( task.ImageWidth, task.ImageHeight ) / 2 );
                Point center = new Point( rand.Next( radius, task.ImageWidth - radius ), rand.Next( radius, task.ImageHeight - radius ) );
                for( int j = 0; j < shape.Points.Length; j++ ) {
                    double t = j * Math.PI * 2 / shape.Points.Length + Math.PI / task.Vertices;
                    shape.Points[j].X = center.X + (int)(Math.Cos( t ) * radius);
                    shape.Points[j].Y = center.Y + (int)(Math.Sin( t ) * radius);
                }
                dna.Shapes[i] = shape;
                if( shape.GetBoundaries().Width == 0 || shape.GetBoundaries().Height == 0 ) {
                    continue;
                }
            }
            return dna;
        }

        object ICloneable.Clone() {
            return new RadialInitializer( Color );
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