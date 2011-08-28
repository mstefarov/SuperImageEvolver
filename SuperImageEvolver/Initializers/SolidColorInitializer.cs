using System;
using System.Drawing;

namespace SuperImageEvolver {

    public class SolidColorInitializerFactory : IModuleFactory {
        public Type ModuleType { get { return typeof( SolidColorInitializer ); } }
        public string ID { get { return "std.SolidColorInitializer.1"; } }
        public ModuleFunction Function { get { return ModuleFunction.Initializer; } }
        public ModulePreset[] Presets {
            get {
                return new[]{
                    new ModulePreset("Full Random", ()=>(new SolidColorInitializer(Color.Black)), this )
                };
            }
        }
        public IModule GetInstance() { return new SolidColorInitializer(Color.Black); }
    }


    sealed class SolidColorInitializer : IInitializer {
        public Color Color { get; set; }
        public int MaxOverlap { get; set; }
        public byte StartingAlpha { get; set; }

        public SolidColorInitializer( Color color ) {
            Color = color;
            MaxOverlap = 6;
            StartingAlpha = 1;
        }

        DNA IInitializer.Initialize( Random rand, TaskState task ) {
            DNA dna = new DNA {
                Shapes = new Shape[task.Shapes]
            };
            for( int i = 0; i< dna.Shapes.Length; i++ ) {
                Shape shape = new Shape {
                    Color = Color.FromArgb( StartingAlpha, Color.R, Color.G, Color.B ),
                    Points = new PointF[task.Vertices]
                };
                for( int j = 0; j < shape.Points.Length; j++ ) {
                    shape.Points[j] = new PointF( rand.NextFloat( -MaxOverlap, task.ImageWidth + MaxOverlap ),
                                                  rand.NextFloat( -MaxOverlap, task.ImageHeight + MaxOverlap ) );
                }
                dna.Shapes[i] = shape;
            }
            return dna;
        }


        object ICloneable.Clone() {
            return new SolidColorInitializer( Color ) {
                MaxOverlap = MaxOverlap
            };
        }

        void IModule.ReadSettings( NBTag tag ) { }
        void IModule.WriteSettings( NBTag tag ) { }
    }
}