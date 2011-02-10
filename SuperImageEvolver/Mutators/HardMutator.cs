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
            int s1 = rand.Next( newDNA.Shapes.Length );
            DNA.Shape shape = newDNA.Shapes[s1];
            switch( rand.Next( 20 ) ) {
                case 0:
                    int s2;
                    do {
                        s2 = rand.Next( newDNA.Shapes.Length );
                    } while( s1 == s2 );
                    if( s2 > s1 ) {
                        for( int i = s1; i < s2; i++ ) {
                            newDNA.Shapes[i] = newDNA.Shapes[i+1];
                        }
                    } else {
                        for( int i = s1; i > s2; i-- ) {
                            newDNA.Shapes[i] = newDNA.Shapes[i - 1];
                        }
                    }
                    newDNA.Shapes[s2] = shape;
                    newDNA.LastMutation = MutationType.SwapShapes;
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

        void MutateShape( Random rand, DNA dna, DNA.Shape shape, TaskState task ) {
            shape.PreviousState = shape.Clone() as DNA.Shape;
            switch( rand.Next( 10 ) ) {
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
                    point.X = rand.Next( task.ImageWidth );
                    dna.LastMutation = MutationType.ReplacePoint;
                    break;
                case 2:
                case 3:
                    point.Y = rand.Next( task.ImageHeight );
                    dna.LastMutation = MutationType.ReplacePoint;
                    break;
                case 4:
                    point.X = rand.Next( task.ImageWidth );
                    point.Y = rand.Next( task.ImageHeight );
                    dna.LastMutation = MutationType.ReplacePoints;
                    break;
            }
            return point;
        }

        void RandomizeShape( Random rand, DNA.Shape shape, TaskState task ) {
            shape.PreviousState = shape.Clone() as DNA.Shape;
            shape.Color = Color.FromArgb( rand.Next( 256 ), rand.Next( 256 ), rand.Next( 256 ), rand.Next( 256 ) );
            for( int i = 0; i < shape.Points.Length; i++ ) {
                shape.Points[i] = new Point( rand.Next( task.ImageWidth ), rand.Next( task.ImageHeight ) );
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