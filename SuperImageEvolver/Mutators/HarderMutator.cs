using System;
using System.Drawing;

namespace SuperImageEvolver {
    public class HarderMutatorFactory : IModuleFactory {
        public Type ModuleType {
            get { return typeof( HarderMutator ); }
        }

        public string ID {
            get { return "std.HarderMutator.1"; }
        }

        public ModuleFunction Function {
            get { return ModuleFunction.Mutator; }
        }

        public ModulePreset[] Presets {
            get {
                return new[] {
                    new ModulePreset( "Harder", () => new HarderMutator(), this )
                };
            }
        }


        public IModule GetInstance() {
            return new HarderMutator();
        }
    }


    sealed class HarderMutator : IMutator {
        public double MaxPolygonArea { get; set; }


        public HarderMutator() {
            MaxPolygonArea = .5;
        }


        public DNA Mutate( Random rand, DNA oldDNA, TaskState task ) {
            int maxOverlap = task.ProjectOptions.MaxOverlap;
            DNA newDNA = new DNA( oldDNA );
            Shape shape = newDNA.Shapes[rand.Next( newDNA.Shapes.Length )];
            shape.PreviousState = shape.Clone() as Shape;
            shape.Color = Color.FromArgb( rand.Next( task.ProjectOptions.MinAlpha, 256 ), rand.NextByte(),
                                          rand.NextByte(), rand.NextByte() );
            double area, maxArea = MaxPolygonArea * task.ImageWidth * task.ImageHeight;
            do {
                for( int i = 0; i < shape.Points.Length; i++ ) {
                    shape.Points[i] = new PointF( rand.NextFloat( -maxOverlap, task.ImageWidth + maxOverlap ),
                                                  rand.NextFloat( -maxOverlap, task.ImageHeight + maxOverlap ) );
                }
                area = CalculateArea( shape.Points );
            } while( area > maxArea );
            newDNA.LastMutation = MutationType.ReplaceShape;
            return newDNA;
        }


        static double CalculateArea( PointF[] points ) {
            float minX = float.MaxValue,
                  maxX = float.MinValue,
                  minY = float.MaxValue,
                  maxY = float.MinValue;
            for( int i = 0; i < points.Length; i++ ) {
                minX = Math.Min( minX, points[i].X );
                maxX = Math.Max( maxX, points[i].X );
                minY = Math.Min( minY, points[i].Y );
                maxY = Math.Max( maxY, points[i].Y );
            }
            return ( maxX - minX ) * ( maxY - minY );
        }


        object ICloneable.Clone() {
            return new HarderMutator {
                MaxPolygonArea = MaxPolygonArea
            };
        }


        void IModule.ReadSettings( NBTag tag ) {}

        void IModule.WriteSettings( NBTag tag ) {}
    }
}