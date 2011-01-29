using System;
using System.Drawing;
using System.IO;


namespace SuperImageEvolver {

    public class SoftMutatorFactory : IModuleFactory {
        public Type ModuleType { get { return typeof( SoftMutator ); } }
        public string ID { get { return "std.SoftMutator.1"; } }
        public ModuleFunction Function { get { return ModuleFunction.Mutator; } }
        public ModulePreset[] Presets {
            get {
                return new ModulePreset[]{
                    new ModulePreset("Soft", ()=>(new SoftMutator(10)) ),
                    new ModulePreset("Softer", ()=>(new SoftMutator(2)) )
                };
            }
        }
        public IModule GetInstance() { return new SoftMutator( 2 ); }
    }


    class SoftMutator : IMutator {

        int maxDelta = 2;

        public SoftMutator( int _maxDelta ) {
            maxDelta = _maxDelta;
        }


        public DNA Mutate( Random rand, DNA oldDNA, TaskState task ) {
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
            int delta = (byte)rand.Next( -maxDelta, maxDelta + 1 );
            switch( rand.Next( 9 ) ) {
                case 0:
                    shape.Color = Color.FromArgb( Math.Max( 0, Math.Min( 255, (int)shape.Color.A + delta ) ), shape.Color.R, shape.Color.G, shape.Color.B );
                    dna.LastMutation = MutationType.AdjustColor;
                    break;
                case 1:
                    shape.Color = Color.FromArgb( shape.Color.A, Math.Max( 0, Math.Min( 255, (int)shape.Color.R + delta ) ), shape.Color.G, shape.Color.B );
                    dna.LastMutation = MutationType.AdjustColor;
                    break;
                case 2:
                    shape.Color = Color.FromArgb( shape.Color.A, shape.Color.R, Math.Max( 0, Math.Min( 255, (int)shape.Color.G + delta ) ), shape.Color.B );
                    dna.LastMutation = MutationType.AdjustColor;
                    break;
                case 3:
                    shape.Color = Color.FromArgb( shape.Color.A, shape.Color.R, shape.Color.G, Math.Max( 0, Math.Min( 255, (int)shape.Color.B + delta ) ) );
                    dna.LastMutation = MutationType.AdjustColor;
                    break;
                case 4:
                case 5:
                    int pt1 = rand.Next( shape.Points.Length );
                    shape.Points[pt1].X = Math.Max( 0, Math.Min( task.ImageWidth-1, shape.Points[pt1].X + delta ) );
                    dna.LastMutation = MutationType.AdjustPoint;
                    break;
                case 6:
                case 7:
                    int pt2 = rand.Next( shape.Points.Length );
                    shape.Points[pt2].Y = Math.Max( 0, Math.Min( task.ImageHeight - 1, shape.Points[pt2].Y + delta ) );
                    dna.LastMutation = MutationType.AdjustPoint;
                    break;
                case 8:
                    int pt3 = rand.Next( shape.Points.Length );
                    shape.Points[pt3].X = Math.Max( 0, Math.Min( task.ImageWidth - 1, shape.Points[pt3].X + delta ) );
                    shape.Points[pt3].Y = Math.Max( 0, Math.Min( task.ImageHeight - 1, shape.Points[pt3].Y + delta ) );
                    dna.LastMutation = MutationType.AdjustPoints;
                    break;
            }
        }


        object ICloneable.Clone() {
            return new SoftMutator( maxDelta );
        }

        void IModule.ReadSettings( BinaryReader reader, int settingsLength ) {
            maxDelta = reader.ReadInt32();
        }

        void IModule.WriteSettings( BinaryWriter writer ) {
            writer.Write( 4 );
            writer.Write( maxDelta );
        }
    }
}