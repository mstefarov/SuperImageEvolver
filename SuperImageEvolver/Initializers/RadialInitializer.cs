using System;
using System.ComponentModel;
using System.Drawing;

namespace SuperImageEvolver {
    public class RadialInitializerFactory : IModuleFactory {
        public Type ModuleType {
            get { return typeof( RadialInitializer ); }
        }

        public string ID {
            get { return "std.RadialInitializer.1"; }
        }

        public ModuleFunction Function {
            get { return ModuleFunction.Initializer; }
        }

        public ModulePreset[] Presets {
            get {
                return new[] {
                    new ModulePreset( "Radial", () => ( new RadialInitializer( Color.Black ) ), this )
                };
            }
        }


        public IModule GetInstance() {
            return new RadialInitializer( Color.Black );
        }
    }


    public sealed class RadialInitializer : IInitializer {
        public Color Color { get; set; }
        [DefaultValue(6)]
        public int MaxOverlap { get; set; }
        [DefaultValue(1)]
        public byte StartingAlpha { get; set; }
        [DefaultValue(2)]
        public int MinRadius { get; set; }
        [DefaultValue(0.5)]
        public double MaxRadiusRatio { get; set; }
        [DefaultValue(0)]
        public double Angle { get; set; }
        [DefaultValue(0)]
        public double AngleDelta { get; set; }
        [DefaultValue(1)]
        public double Revolutions { get; set; }


        public RadialInitializer( Color color ) {
            Color = color;
            MaxOverlap = 6;
            StartingAlpha = 1;
            MinRadius = 2;
            MaxRadiusRatio = 0.5;
            Angle = 0;
            AngleDelta = 0;
            Revolutions = 1;
        }


        public DNA Initialize(Random rand, TaskState task) {
            var dna = new DNA {
                Shapes = new Shape[task.Shapes]
            };
            for (int s = 0; s < task.Shapes; s++) {
                bool alt = (s%2==0);
                var shape = new Shape {
                    Color = Color.FromArgb(StartingAlpha, alt?255-Color.R:Color.R, alt?255-Color.G:Color.G, alt?255-Color.B:Color.B),
                    Points = new PointF[task.Vertices]
                };
                int maxRadius = (int)Math.Round(Math.Min(task.ImageWidth, task.ImageHeight)*MaxRadiusRatio);
                int radius = rand.Next(MinRadius, maxRadius);
                var center = new Point(rand.Next(radius - MaxOverlap, task.ImageWidth - radius + MaxOverlap),
                                       rand.Next(radius - MaxOverlap, task.ImageHeight - radius + MaxOverlap));
                for (int v = 0; v < task.Vertices; v++) {
                    double t = v*Math.PI*2*Revolutions/task.Vertices + (Angle + AngleDelta*s)/180*Math.PI + Math.PI/task.Vertices;
                    shape.Points[v].X = (float)(center.X + Math.Cos(t)*radius);
                    shape.Points[v].Y = (float)(center.Y + Math.Sin(t)*radius);
                }
                if (shape.GetBoundaries().Width < 1 || shape.GetBoundaries().Height < 1) {
                    continue;
                }
                dna.Shapes[s] = shape;
            }
            return dna;
        }


        object ICloneable.Clone() {
            return new RadialInitializer(Color) {
                MaxOverlap = MaxOverlap,
                StartingAlpha = StartingAlpha,
                MinRadius = MinRadius,
                MaxRadiusRatio = MaxRadiusRatio,
                Angle = Angle,
                AngleDelta = AngleDelta,
                Revolutions = Revolutions,
            };
        }


        void IModule.ReadSettings( NBTag tag ) {}

        void IModule.WriteSettings( NBTag tag ) {}
    }
}