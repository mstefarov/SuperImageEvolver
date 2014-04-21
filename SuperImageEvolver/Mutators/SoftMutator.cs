using System;
using System.Drawing;

namespace SuperImageEvolver {
    public class SoftMutatorFactory : IModuleFactory {
        public Type ModuleType {
            get { return typeof( SoftMutator ); }
        }

        public string ID {
            get { return "std.SoftMutator.1"; }
        }

        public ModuleFunction Function {
            get { return ModuleFunction.Mutator; }
        }

        public ModulePreset[] Presets {
            get {
                return new[] {
                    new ModulePreset( "Soft", () => ( new SoftMutator( 8, 12 ) ), this ),
                    new ModulePreset( "Softer", () => ( new SoftMutator( 1, 2 ) ), this )
                };
            }
        }


        public IModule GetInstance() {
            return new SoftMutator( 1, 2 );
        }
    }


    sealed class SoftMutator : IMutator {
        public int MaxPosDelta { get; set; }
        public int MaxColorDelta { get; set; }


        public SoftMutator() {
            MaxPosDelta = 12;
            MaxColorDelta = 12;
        }


        public SoftMutator( int maxColorDelta, int maxPosDelta )
            : this() {
            MaxColorDelta = maxColorDelta;
            MaxPosDelta = maxPosDelta;
        }


        public DNA Mutate( Random rand, DNA oldDNA, TaskState task ) {
            DNA newDNA = new DNA( oldDNA );
            if( rand.Next( 20 ) == 0 ) {
                newDNA.SwapShapes( rand );
            } else {
                MutateShape( rand, newDNA, newDNA.Shapes[rand.Next( newDNA.Shapes.Length )], task );
            }

            return newDNA;
        }


        void MutateShape( Random rand, DNA dna, Shape shape, TaskState task ) {
            int maxOverlap = task.ProjectOptions.MaxOverlap;
            shape.PreviousState = shape.Clone() as Shape;
            int colorDelta = (byte)rand.Next( 1, MaxColorDelta + 1 ) * ( rand.Next( 2 ) == 0 ? 1 : -1 );
            float posDelta = (float)rand.NextDouble() * MaxPosDelta * ( rand.Next( 2 ) == 0 ? 1 : -1 );
            switch( rand.Next( 9 ) ) {
                case 0:
                    shape.Color =
                        Color.FromArgb(
                            Math.Max( task.ProjectOptions.MinAlpha, Math.Min( 255, shape.Color.A + colorDelta ) ),
                            shape.Color.R, shape.Color.G, shape.Color.B );
                    dna.LastMutation = MutationType.AdjustColor;
                    break;
                case 1:
                    shape.Color = Color.FromArgb( shape.Color.A,
                                                  Math.Max( 0, Math.Min( 255, shape.Color.R + colorDelta ) ),
                                                  shape.Color.G, shape.Color.B );
                    dna.LastMutation = MutationType.AdjustColor;
                    break;
                case 2:
                    shape.Color = Color.FromArgb( shape.Color.A, shape.Color.R,
                                                  Math.Max( 0, Math.Min( 255, shape.Color.G + colorDelta ) ),
                                                  shape.Color.B );
                    dna.LastMutation = MutationType.AdjustColor;
                    break;
                case 3:
                    shape.Color = Color.FromArgb( shape.Color.A, shape.Color.R, shape.Color.G,
                                                  Math.Max( 0, Math.Min( 255, shape.Color.B + colorDelta ) ) );
                    dna.LastMutation = MutationType.AdjustColor;
                    break;
                case 4:
                case 5:
                    int pt1 = rand.Next( shape.Points.Length );
                    shape.Points[pt1].X = Math.Max( -maxOverlap,
                                                    Math.Min( task.ImageWidth - 1 + maxOverlap,
                                                              shape.Points[pt1].X + posDelta ) );
                    dna.LastMutation = MutationType.AdjustPoint;
                    break;
                case 6:
                case 7:
                    int pt2 = rand.Next( shape.Points.Length );
                    shape.Points[pt2].Y = Math.Max( -maxOverlap,
                                                    Math.Min( task.ImageHeight - 1 + maxOverlap,
                                                              shape.Points[pt2].Y + posDelta ) );
                    dna.LastMutation = MutationType.AdjustPoint;
                    break;
                case 8:
                    int pt3 = rand.Next( shape.Points.Length );
                    shape.Points[pt3].X = Math.Max( -maxOverlap,
                                                    Math.Min( task.ImageWidth - 1 + maxOverlap,
                                                              shape.Points[pt3].X + posDelta ) );
                    shape.Points[pt3].Y = Math.Max( -maxOverlap,
                                                    Math.Min( task.ImageHeight - 1 + maxOverlap,
                                                              shape.Points[pt3].Y + posDelta ) );
                    dna.LastMutation = MutationType.AdjustPoints;
                    break;
            }
        }


        object ICloneable.Clone() {
            return new SoftMutator {
                MaxColorDelta = MaxColorDelta,
                MaxPosDelta = MaxPosDelta
            };
        }


        void IModule.ReadSettings( NBTag tag ) {}

        void IModule.WriteSettings( NBTag tag ) {}
    }
}