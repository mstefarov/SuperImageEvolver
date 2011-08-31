using System;
using System.Drawing;


namespace SuperImageEvolver {

    public class HardishMutatorFactory : IModuleFactory {
        public Type ModuleType { get { return typeof( HardishMutator ); } }
        public string ID { get { return "std.HardishMutator.1"; } }
        public ModuleFunction Function { get { return ModuleFunction.Mutator; } }
        public ModulePreset[] Presets {
            get {
                return new[]{
                    new ModulePreset("Hardish", ()=>new HardishMutator(), this )
                };
            }
        }
        public IModule GetInstance() { return new HardishMutator(); }
    }


    sealed class HardishMutator : IMutator {

        public int MaxPosDelta { get; set; }
        public int MaxColorDelta { get; set; }

        public HardishMutator() {
            MaxPosDelta = 10;
            MaxColorDelta = 10;
        }


        public DNA Mutate( Random rand, DNA oldDNA, TaskState task ) {
            DNA newDNA = new DNA( oldDNA );
            int s1 = rand.Next( newDNA.Shapes.Length );
            Shape shape = newDNA.Shapes[s1];
            if( rand.Next( 20 ) == 0 ) {
                MutatorHelper.SwapShapes( rand, newDNA );
            } else {
                MutateShape( rand, newDNA, shape, task );
            }
            return newDNA;
        }



        void MutateShape( Random rand, DNA dna, Shape shape, TaskState task ) {
            shape.PreviousState = shape.Clone() as Shape;
            int colorDelta = (byte)rand.Next( 1, MaxColorDelta + 1 ) * (rand.Next( 2 ) == 0 ? 1 : -1);
            switch( rand.Next( 10 ) ) {
                case 0:
                    shape.Color = Color.FromArgb( Math.Max( task.ProjectOptions.MinAlpha, Math.Min( 255, shape.Color.A + colorDelta ) ), shape.Color.R, shape.Color.G, shape.Color.B );
                    dna.LastMutation = MutationType.AdjustColor;
                    break;
                case 1:
                    shape.Color = Color.FromArgb( shape.Color.A, Math.Max( 0, Math.Min( 255, shape.Color.R + colorDelta ) ), shape.Color.G, shape.Color.B );
                    dna.LastMutation = MutationType.AdjustColor;
                    break;
                case 2:
                    shape.Color = Color.FromArgb( shape.Color.A, shape.Color.R, Math.Max( 0, Math.Min( 255, shape.Color.G + colorDelta ) ), shape.Color.B );
                    dna.LastMutation = MutationType.AdjustColor;
                    break;
                case 3:
                    shape.Color = Color.FromArgb( shape.Color.A, shape.Color.R, shape.Color.G, Math.Max( 0, Math.Min( 255, shape.Color.B + colorDelta ) ) );
                    dna.LastMutation = MutationType.AdjustColor;
                    break;

                default:
                    int index = rand.Next( shape.Points.Length );
                    shape.Points[index] = MutatePoint( rand, dna, shape.Points[index], task );
                    if( rand.Next( 2 ) == 0 ) {
                        index = (index + 1) % shape.Points.Length;
                        shape.Points[index] = MutatePoint( rand, dna, shape.Points[index], task );
                        if( rand.Next( 2 ) == 0 ) {
                            index = (index + 1) % shape.Points.Length;
                            shape.Points[index] = MutatePoint( rand, dna, shape.Points[index], task );
                        }
                    }
                    dna.LastMutation = MutationType.AdjustPoints;
                    break;
            }
        }


        PointF MutatePoint( Random rand, DNA dna, PointF point, TaskState task ) {
            float posDelta = (float)rand.NextDouble() * MaxPosDelta * (rand.Next( 2 ) == 0 ? 1 : -1);
            int maxOverlap = task.ProjectOptions.MaxOverlap;
            switch( rand.Next( 5 ) ) {
                case 0:
                case 1:
                    point.X = Math.Max( -maxOverlap, Math.Min( task.ImageWidth - 1 + maxOverlap, point.X + posDelta ) );
                    dna.LastMutation = MutationType.AdjustPoint;
                    break;
                case 2:
                case 3:
                    point.Y = Math.Max( -maxOverlap, Math.Min( task.ImageHeight - 1 + maxOverlap, point.Y + posDelta ) );
                    dna.LastMutation = MutationType.AdjustPoint;
                    break;
                case 4:
                    point.X = Math.Max( -maxOverlap, Math.Min( task.ImageWidth - 1 + maxOverlap, point.X + posDelta ) );
                    point.Y = Math.Max( -maxOverlap, Math.Min( task.ImageHeight - 1 + maxOverlap, point.Y + posDelta ) );
                    dna.LastMutation = MutationType.AdjustPoints;
                    break;
            }
            return point;
        }


        object ICloneable.Clone() {
            return new HardishMutator {
                MaxColorDelta = MaxColorDelta,
                MaxPosDelta = MaxPosDelta
            };
        }

        void IModule.ReadSettings( NBTag tag ) { }

        void IModule.WriteSettings( NBTag tag ) { }
    }
}