using System;
using System.Drawing;
using System.IO;

namespace SuperImageEvolver {

    public class RadialInitializerFactory : IModuleFactory {
        public Type ModuleType { get { return typeof( RadialInitializer ); } }
        public string ID { get { return "std.RadialInitializer.1"; } }
        public ModuleFunction Function { get { return ModuleFunction.Initializer; } }
        public ModulePreset[] Presets {
            get {
                return new ModulePreset[]{
                    new ModulePreset("Radial", ()=>(new RadialInitializer(Color.Black)), this )
                };
            }
        }
        public IModule GetInstance() { return new RadialInitializer( Color.Black ); }
    }


    public class RadialInitializer : IInitializer {
        public Color Color { get; set; }
        public int MaxOverlap { get; set; }
        public byte StartingAlpha { get; set; }

        public RadialInitializer( Color _color ) {
            Color = _color;
            MaxOverlap = 6;
            StartingAlpha = 1;
        }


        public DNA Initialize( Random rand, TaskState task ) {
            DNA dna = new DNA();
            dna.Shapes = new Shape[task.Shapes];
            for( int i = 0; i < task.Shapes; i++ ) {
                Shape shape = new Shape();
                shape.Color = Color.FromArgb( StartingAlpha, Color.R, Color.G, Color.B );
                shape.Points = new PointF[task.Vertices];
                int radius = rand.Next( 2, Math.Min( task.ImageWidth, task.ImageHeight ) / 2 );
                Point center = new Point( rand.Next( radius - MaxOverlap, task.ImageWidth - radius + MaxOverlap ),
                                          rand.Next( radius - MaxOverlap, task.ImageHeight - radius + MaxOverlap ) );
                for( int j = 0; j < shape.Points.Length; j++ ) {
                    double t = j * Math.PI * 2 / shape.Points.Length + Math.PI / task.Vertices;
                    shape.Points[j].X = (float)(center.X + Math.Cos( t ) * radius);
                    shape.Points[j].Y = (float)(center.Y + Math.Sin( t ) * radius);
                }
                dna.Shapes[i] = shape;
                if( shape.GetBoundaries().Width == 0 || shape.GetBoundaries().Height == 0 ) {
                    continue;
                }
            }
            return dna;
        }

        object ICloneable.Clone() {
            return new RadialInitializer( Color ) {
                MaxOverlap = MaxOverlap
            };
        }


        void IModule.ReadSettings( NBTag tag ) { }

        void IModule.WriteSettings( NBTag tag ) { }
    }
}