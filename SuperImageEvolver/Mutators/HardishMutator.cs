using System;
using System.Drawing;
using System.IO;


namespace SuperImageEvolver {

    public class HardishMutatorFactory : IModuleFactory {
        public Type ModuleType { get { return typeof( HardishMutator ); } }
        public string ID { get { return "std.HardishMutator.1"; } }
        public ModuleFunction Function { get { return ModuleFunction.Mutator; } }
        public ModulePreset[] Presets {
            get {
                return new ModulePreset[]{
                    new ModulePreset("Hardish", ()=>new HardishMutator(), this )
                };
            }
        }
        public IModule GetInstance() { return new HardishMutator(); }
    }


    class HardishMutator : IMutator {

        public int MaxOverlap { get; set; }
        public int MaxPosDelta { get; set; }
        public int MaxColorDelta { get; set; }

        public HardishMutator() {
            MaxOverlap = 6;
            MaxPosDelta = 10;
            MaxColorDelta = 10;
        }

        public HardishMutator( int _maxDelta )
            : this() {
            MaxPosDelta = _maxDelta;
            MaxColorDelta = _maxDelta;
        }


        public DNA Mutate( Random rand, DNA oldDNA, TaskState task ) {
            DNA newDNA = new DNA( oldDNA );
            int s1 = rand.Next( newDNA.Shapes.Length );
            Shape shape = newDNA.Shapes[s1];
            if( rand.Next( 20 ) == 0 ) {
                MutatorHelper.SwapShapes( rand, newDNA, task );
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
                    shape.Color = Color.FromArgb( Math.Max( 1, Math.Min( 255, (int)shape.Color.A + colorDelta ) ), shape.Color.R, shape.Color.G, shape.Color.B );
                    dna.LastMutation = MutationType.ReplaceColor;
                    break;
                case 1:
                    shape.Color = Color.FromArgb( shape.Color.A, Math.Max( 0, Math.Min( 255, (int)shape.Color.R + colorDelta ) ), shape.Color.G, shape.Color.B );
                    dna.LastMutation = MutationType.ReplaceColor;
                    break;
                case 2:
                    shape.Color = Color.FromArgb( shape.Color.A, shape.Color.R, Math.Max( 0, Math.Min( 255, (int)shape.Color.G + colorDelta ) ), shape.Color.B );
                    dna.LastMutation = MutationType.ReplaceColor;
                    break;
                case 3:
                    shape.Color = Color.FromArgb( shape.Color.A, shape.Color.R, shape.Color.G, Math.Max( 0, Math.Min( 255, (int)shape.Color.B + colorDelta ) ) );
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
            float posDelta = (float)rand.NextDouble() * MaxPosDelta * (rand.Next( 2 ) == 0 ? 1 : -1);
            switch( rand.Next( 5 ) ) {
                case 0:
                case 1:
                    point.X = Math.Max( -MaxOverlap, Math.Min( task.ImageWidth - 1 + MaxOverlap, point.X + posDelta ) );
                    dna.LastMutation = MutationType.ReplacePoint;
                    break;
                case 2:
                case 3:
                    point.Y = Math.Max( -MaxOverlap, Math.Min( task.ImageHeight - 1 + MaxOverlap, point.Y + posDelta ) );
                    dna.LastMutation = MutationType.ReplacePoint;
                    break;
                case 4:
                    point.X = Math.Max( -MaxOverlap, Math.Min( task.ImageWidth - 1 + MaxOverlap, point.X + posDelta ) );
                    point.Y = Math.Max( -MaxOverlap, Math.Min( task.ImageHeight - 1 + MaxOverlap, point.Y + posDelta ) );
                    dna.LastMutation = MutationType.ReplacePoints;
                    break;
            }
            return point;
        }


        object ICloneable.Clone() {
            return new HardishMutator {
                MaxOverlap = MaxOverlap,
                MaxColorDelta = MaxColorDelta,
                MaxPosDelta = MaxPosDelta
            };
        }

        void IModule.ReadSettings( NBTag tag ) { }

        void IModule.WriteSettings( NBTag tag ) { }
    }
}