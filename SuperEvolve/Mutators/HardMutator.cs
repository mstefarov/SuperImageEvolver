using System;
using System.Drawing;
using System.IO;


namespace SuperImageEvolver {

    public class HardMutatorFactory : IModuleFactory {
        public Type ModuleType { get { return typeof( HardMutator ); } }
        public string ID { get { return "std.HardMutator.1"; } }
        public ModuleFunction Function { get { return ModuleFunction.Mutator; } }
        public ModulePreset[] Presets {
            get {
                return new ModulePreset[]{
                    new ModulePreset("Hard", ()=>(new HardMutator()) )
                };
            }
        }
        public IModule GetInstance() { return new HardMutator(); }
    }

    class HardMutator : IMutator {
        public DNA Mutate( Random rand, DNA oldDNA, TaskState task ) {
            DNA newDNA = new DNA( oldDNA );
            for( int i = rand.Next( newDNA.Shapes.Length ); i >= 0; i-- ) {
                MutateShape( rand, newDNA, newDNA.Shapes[rand.Next( newDNA.Shapes.Length )], task );
            }
            return newDNA;
        }

        void MutateShape( Random rand, DNA dna, DNA.Shape shape, TaskState task ) {
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
            return new HardMutator();
        }

        void IModule.ReadSettings( BinaryReader reader, int settingsLength ) {
        }

        void IModule.WriteSettings( BinaryWriter writer ) {
            writer.Write( 0 );
        }
    }
}