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
            for (int si = 0; si < task.Shapes; si++) {
                var shape = new Shape {
                    Points = new PointF[task.Vertices]
                };
                ReInitShape(rand, task, shape, si);
                dna.Shapes[si] = shape;
            }
            return dna;
        }


        public void ReInitShape(Random rand, TaskState task, Shape shape, int shapeIndex) {
            shape.Color = Color.FromArgb(StartingAlpha, Color.R, Color.G, Color.B);
            do {
                int maxRadius = (int)Math.Round(Math.Min(task.ImageWidth, task.ImageHeight) * MaxRadiusRatio);
                int radius = rand.Next(MinRadius, maxRadius);
                var center = new Point(rand.Next(radius - MaxOverlap, task.ImageWidth - radius + MaxOverlap),
                                       rand.Next(radius - MaxOverlap, task.ImageHeight - radius + MaxOverlap));
                double offsetAngle = (Angle + AngleDelta * shapeIndex) / 180 * Math.PI + Math.PI / task.Vertices;
                for (int v = 0; v < task.Vertices; v++) {
                    double t = v * Math.PI * 2 * Revolutions / task.Vertices + offsetAngle;
                    shape.Points[v].X = (float)(center.X + Math.Cos(t) * radius);
                    shape.Points[v].Y = (float)(center.Y + Math.Sin(t) * radius);
                }
            }
            while (shape.GetBoundaries().Width < 1 || shape.GetBoundaries().Height < 1);
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


        void IModule.ReadSettings(NBTag tag) {
            MaxOverlap = tag.GetInt(nameof(MaxOverlap), MaxOverlap);
            StartingAlpha = tag.GetByte(nameof(StartingAlpha), StartingAlpha);
            MinRadius = tag.GetInt(nameof(MinRadius), MinRadius);
            MaxRadiusRatio = tag.GetDouble(nameof(MaxRadiusRatio), MaxRadiusRatio);
            Angle = tag.GetDouble(nameof(Angle), Angle);
            AngleDelta = tag.GetDouble(nameof(AngleDelta), AngleDelta);
            Revolutions = tag.GetDouble(nameof(Revolutions), Revolutions);
        }

        void IModule.WriteSettings(NBTag tag) {
            tag.Append(nameof(MaxOverlap), MaxOverlap);
            tag.Append(nameof(StartingAlpha), StartingAlpha);
            tag.Append(nameof(MinRadius), MinRadius);
            tag.Append(nameof(MaxRadiusRatio), MaxRadiusRatio);
            tag.Append(nameof(Angle), Angle);
            tag.Append(nameof(AngleDelta), AngleDelta);
            tag.Append(nameof(Revolutions), Revolutions);
        }
    }
}
