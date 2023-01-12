using System;
using System.Drawing;

namespace SuperImageEvolver {
    public class SegmentedInitializerFactory : IModuleFactory {
        public Type ModuleType {
            get { return typeof( SegmentedInitializer ); }
        }

        public string ID {
            get { return "std.SegmentedInitializer.1"; }
        }

        public ModuleFunction Function {
            get { return ModuleFunction.Initializer; }
        }

        public ModulePreset[] Presets {
            get {
                return new[] {
                    new ModulePreset( "Segmented", () => ( new SegmentedInitializer( Color.Black ) ), this )
                };
            }
        }


        public IModule GetInstance() {
            return new SegmentedInitializer( Color.Black );
        }
    }


    public sealed class SegmentedInitializer : IInitializer {
        public Color Color { get; set; }
        public int MaxOverlap { get; set; }
        public byte StartingAlpha { get; set; }


        public SegmentedInitializer( Color color ) {
            Color = color;
            MaxOverlap = 6;
            StartingAlpha = 1;
        }


        public DNA Initialize( Random rand, TaskState task ) {
            DNA dna = new DNA {
                Shapes = new Shape[task.Shapes]
            };
            int shapesPerSegment = task.Shapes / 9;
            int shapeCounter = 0;
            int remainder = task.Shapes - shapesPerSegment * 9;

            // shapes that did not fit into any of the 9 buckets
            for ( int i = 0; i < remainder; i++ ) {
                Shape shape = new Shape {
                    Points = new PointF[task.Vertices]
                };
                ReInitShape(rand, task, shape, i);
                dna.Shapes[i] = shape;
                shapeCounter++;
            }

            for( int x = 0; x < 3; x++ ) {
                for( int y = 0; y < 3; y++ ) {
                    for( int i = 0; i < shapesPerSegment; i++ ) {
                        Shape shape = new Shape {
                            Color = Color.FromArgb( StartingAlpha, Color.R, Color.G, Color.B ),
                            Points = new PointF[task.Vertices]
                        };
                        for( int j = 0; j < shape.Points.Length; j++ ) {
                            shape.Points[j] =
                                new PointF(
                                    rand.NextFloat( task.ImageWidth / 3f * x - MaxOverlap,
                                                    task.ImageWidth / 3f * ( x + 1 ) + MaxOverlap ),
                                    rand.NextFloat( task.ImageHeight / 3f * y - MaxOverlap,
                                                    task.ImageHeight / 3f * ( y + 1 ) + MaxOverlap ) );
                        }
                        dna.Shapes[shapeCounter] = shape;
                        shapeCounter++;
                    }
                }
            }
            return dna;
        }


        public void ReInitShape(Random rand, TaskState task, Shape shape, int shapeIndex) {
            shape.Color = Color.FromArgb(StartingAlpha, Color.R, Color.G, Color.B);
            for (int j = 0; j < shape.Points.Length; j++) {
                shape.Points[j] = new PointF(rand.NextFloat(-MaxOverlap, task.ImageWidth + MaxOverlap),
                                              rand.NextFloat(-MaxOverlap, task.ImageHeight + MaxOverlap));
            }
        }


        object ICloneable.Clone() {
            return new SegmentedInitializer( Color ) {
                MaxOverlap = MaxOverlap
            };
        }


        void IModule.ReadSettings( NBTag tag ) {}

        void IModule.WriteSettings( NBTag tag ) {}

    }
}
