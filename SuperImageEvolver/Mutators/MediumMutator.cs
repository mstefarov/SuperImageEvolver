using System;
using System.Drawing;
using System.IO;


namespace SuperImageEvolver {

    public class MediumMutatorFactory : IModuleFactory {
        public Type ModuleType { get { return typeof( MediumMutator ); } }
        public string ID { get { return "std.MediumMutator.1"; } }
        public ModuleFunction Function { get { return ModuleFunction.Mutator; } }
        public ModulePreset[] Presets {
            get {
                return new ModulePreset[]{
                    new ModulePreset("Medium", ()=>(new MediumMutator()) )
                };
            }
        }
        public IModule GetInstance() { return new MediumMutator(); }
    }

    public class MediumMutator : IMutator {
        DNA IMutator.Mutate( Random rand, DNA oldDNA, TaskState task ) {
            DNA newDNA = new DNA( oldDNA );
            switch( rand.Next( 12 ) ) {
                case 0:
                    int s1 = rand.Next( newDNA.Shapes.Length );
                    int s2 = rand.Next( newDNA.Shapes.Length );
                    DNA.Shape shape = newDNA.Shapes[s1];
                    newDNA.Shapes[s1] = newDNA.Shapes[s2];
                    newDNA.Shapes[s2] = shape;
                    newDNA.LastMutation = MutationType.SwapShapes;
                    break;
                default:
                    MutateShape( rand, newDNA, newDNA.Shapes[rand.Next( newDNA.Shapes.Length )], task );
                    break;
            }

            return newDNA;
        }

        void MutateShape( Random rand, DNA dna, DNA.Shape shape, TaskState task ) {
            shape.PreviousState = shape.Clone() as DNA.Shape;
            switch( rand.Next( 9 ) ) {
                case 0:
                    shape.Color = Color.FromArgb( (byte)rand.Next( 256 ), shape.Color.R, shape.Color.G, shape.Color.B );
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
                    shape.Points[rand.Next( shape.Points.Length )].X = rand.Next( task.ImageWidth );
                    dna.LastMutation = MutationType.ReplacePoint;
                    break;
                case 6:
                case 7:
                    shape.Points[rand.Next( shape.Points.Length )].Y = rand.Next( task.ImageHeight );
                    dna.LastMutation = MutationType.ReplacePoint;
                    break;
                case 8:
                    shape.Points[rand.Next( shape.Points.Length )].X = rand.Next( task.ImageWidth );
                    shape.Points[rand.Next( shape.Points.Length )].Y = rand.Next( task.ImageHeight );
                    dna.LastMutation = MutationType.ReplacePoints;
                    break;
            }
        }


        object ICloneable.Clone() {
            return new MediumMutator();
        }

        void IModule.ReadSettings( BinaryReader reader, int settingsLength ) { }

        void IModule.WriteSettings( BinaryWriter writer ) {
            writer.Write( 0 );
        }
    }
}