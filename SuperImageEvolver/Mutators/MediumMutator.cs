using System;
using System.Drawing;


namespace SuperImageEvolver {

    public class MediumMutatorFactory : IModuleFactory {
        public Type ModuleType { get { return typeof( MediumMutator ); } }
        public string ID { get { return "std.MediumMutator.1"; } }
        public ModuleFunction Function { get { return ModuleFunction.Mutator; } }
        public ModulePreset[] Presets {
            get {
                return new[]{
                    new ModulePreset("Medium", ()=>new MediumMutator(), this )
                };
            }
        }
        public IModule GetInstance() { return new MediumMutator(); }
    }


    public sealed class MediumMutator : IMutator {
        DNA IMutator.Mutate( Random rand, DNA oldDNA, TaskState task ) {
            DNA newDNA = new DNA( oldDNA );
            if( rand.Next( 20 ) == 0 ) {
                MutatorHelper.SwapShapes( rand, newDNA );
            } else {
                MutateShape( rand, newDNA, newDNA.Shapes[rand.Next( newDNA.Shapes.Length )], task );
            }

            return newDNA;
        }


        static void MutateShape( Random rand, DNA dna, Shape shape, TaskState task ) {
            int maxOverlap = task.ProjectOptions.MaxOverlap;
            shape.PreviousState = shape.Clone() as Shape;
            switch( rand.Next( 9 ) ) {
                case 0:
                    shape.Color = Color.FromArgb( (byte)rand.Next( task.ProjectOptions.MinAlpha, 256 ), shape.Color.R, shape.Color.G, shape.Color.B );
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
                case 4:
                case 5:
                    shape.Points[rand.Next( shape.Points.Length )].X = rand.NextFloat( -maxOverlap, task.ImageWidth + maxOverlap );
                    dna.LastMutation = MutationType.ReplacePoint;
                    break;
                case 6:
                case 7:
                    shape.Points[rand.Next( shape.Points.Length )].Y = rand.NextFloat( -maxOverlap, task.ImageHeight + maxOverlap );
                    dna.LastMutation = MutationType.ReplacePoint;
                    break;
                case 8:
                    shape.Points[rand.Next( shape.Points.Length )].X = rand.NextFloat( -maxOverlap, task.ImageWidth + maxOverlap );
                    shape.Points[rand.Next( shape.Points.Length )].Y = rand.NextFloat( -maxOverlap, task.ImageHeight + maxOverlap );
                    dna.LastMutation = MutationType.ReplacePoints;
                    break;
            }
        }


        object ICloneable.Clone() {
            return new MediumMutator();
        }


        void IModule.ReadSettings( NBTag tag ) { }

        void IModule.WriteSettings( NBTag tag ) { }
    }
}