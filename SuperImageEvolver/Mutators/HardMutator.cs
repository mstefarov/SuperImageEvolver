using System;
using System.Drawing;


namespace SuperImageEvolver {

    public class HardMutatorFactory : IModuleFactory {
        public Type ModuleType { get { return typeof( HardMutator ); } }
        public string ID { get { return "std.HardMutator.1"; } }
        public ModuleFunction Function { get { return ModuleFunction.Mutator; } }
        public ModulePreset[] Presets {
            get {
                return new[]{
                    new ModulePreset("Hard", ()=>new HardMutator(), this )
                };
            }
        }
        public IModule GetInstance() { return new HardMutator(); }
    }


    sealed class HardMutator : IMutator {

        public int MaxOverlap { get; set; }

        public HardMutator(){
            MaxOverlap = 6;
        }

        public DNA Mutate( Random rand, DNA oldDNA, TaskState task ) {
            DNA newDNA = new DNA( oldDNA );
            int s1 = rand.Next( newDNA.Shapes.Length );
            Shape shape = newDNA.Shapes[s1];
            switch( rand.Next( 20 ) ) {
                case 0:
                    MutatorHelper.SwapShapes( rand, newDNA );
                    break;
                case 1:
                    RandomizeShape( rand, shape, task );
                    newDNA.LastMutation = MutationType.ReplaceShape;
                    break;
                default:
                    MutateShape( rand, newDNA, shape, task );
                    break;
            }
            return newDNA;
        }

        void MutateShape( Random rand, DNA dna, Shape shape, TaskState task ) {
            shape.PreviousState = shape.Clone() as Shape;
            switch( rand.Next( 10 ) ) {
                case 0:
                    shape.Color = Color.FromArgb( (byte)rand.Next( 1, 256 ), shape.Color.R, shape.Color.G, shape.Color.B );
                    dna.LastMutation = MutationType.ReplaceColor;
                    break;
                case 1:
                    shape.Color = Color.FromArgb( shape.Color.A, (byte)rand.Next( 256 ), shape.Color.G, shape.Color.B );
                    dna.LastMutation = MutationType.ReplaceColor;
                    break;
                case 2:
                    shape.Color = Color.FromArgb( shape.Color.A, shape.Color.R, (byte)rand.Next( 256 ), shape.Color.B );
                    dna.LastMutation = MutationType.ReplaceColor;
                    break;
                case 3:
                    shape.Color = Color.FromArgb( shape.Color.A, shape.Color.R, shape.Color.G, (byte)rand.Next( 256 ) );
                    dna.LastMutation = MutationType.ReplaceColor;
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
                    break;
            }
        }

        PointF MutatePoint( Random rand, DNA dna, PointF point, TaskState task ) {
            switch( rand.Next( 5 ) ) {
                case 0:
                case 1:
                    point.X = rand.NextFloat( -MaxOverlap, task.ImageWidth + MaxOverlap );
                    dna.LastMutation = MutationType.ReplacePoint;
                    break;
                case 2:
                case 3:
                    point.Y = rand.NextFloat( -MaxOverlap, task.ImageHeight + MaxOverlap );
                    dna.LastMutation = MutationType.ReplacePoint;
                    break;
                case 4:
                    point.X = rand.NextFloat( -MaxOverlap, task.ImageWidth + MaxOverlap );
                    point.Y = rand.NextFloat( -MaxOverlap, task.ImageHeight + MaxOverlap );
                    dna.LastMutation = MutationType.ReplacePoints;
                    break;
            }
            return point;
        }

        void RandomizeShape( Random rand, Shape shape, TaskState task ) {
            shape.PreviousState = shape.Clone() as Shape;
            shape.Color = Color.FromArgb( rand.Next( 1, 256 ), rand.Next( 256 ), rand.Next( 256 ), rand.Next( 256 ) );
            for( int i = 0; i < shape.Points.Length; i++ ) {
                shape.Points[i] = new PointF( rand.NextFloat( -MaxOverlap, task.ImageWidth + MaxOverlap ),
                                              rand.NextFloat( -MaxOverlap, task.ImageHeight + MaxOverlap ) );
            }
        }


        object ICloneable.Clone() {
            return new HardMutator {
                MaxOverlap = MaxOverlap
            };
        }

        void IModule.ReadSettings( NBTag tag ) { }

        void IModule.WriteSettings( NBTag tag ) { }
    }
}