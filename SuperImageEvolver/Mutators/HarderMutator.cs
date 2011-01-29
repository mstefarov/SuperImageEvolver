using System;
using System.Drawing;
using System.IO;


namespace SuperImageEvolver {

    public class HarderMutatorFactory : IModuleFactory {
        public Type ModuleType { get { return typeof( HarderMutator ); } }
        public string ID { get { return "std.HarderMutator.1"; } }
        public ModuleFunction Function { get { return ModuleFunction.Mutator; } }
        public ModulePreset[] Presets {
            get {
                return new ModulePreset[]{
                    new ModulePreset("Harder", ()=>(new HarderMutator()) )
                };
            }
        }
        public IModule GetInstance() { return new HarderMutator(); }
    }


    class HarderMutator : IMutator {
        public DNA Mutate( Random rand, DNA oldDNA, TaskState task ) {
            DNA newDNA = new DNA( oldDNA );
            DNA.Shape shape = newDNA.Shapes[rand.Next( newDNA.Shapes.Length )];
            shape.Color = Color.FromArgb( rand.Next( 256 ), rand.Next( 256 ), rand.Next( 256 ), rand.Next( 256 ) );
            for( int i = 0; i < shape.Points.Length; i++ ) {
                shape.Points[i] = new Point( rand.Next( task.ImageWidth ), rand.Next( task.ImageHeight ) );
            }
            newDNA.LastMutation = MutationType.ReplaceShape;
            return newDNA;
        }

        object ICloneable.Clone() {
            return new HarderMutator();
        }

        void IModule.ReadSettings( BinaryReader reader, int settingsLength ) { }

        void IModule.WriteSettings( BinaryWriter writer ) {
            writer.Write( 0 );
        }
    }
}
