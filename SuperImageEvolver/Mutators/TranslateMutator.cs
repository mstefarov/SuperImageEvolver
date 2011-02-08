using System;
using System.Drawing;
using System.IO;


namespace SuperImageEvolver {

    public class TranslateMutatorFactory : IModuleFactory {
        public Type ModuleType { get { return typeof( TranslateMutator ); } }
        public string ID { get { return "std.TranslateMutator.1"; } }
        public ModuleFunction Function { get { return ModuleFunction.Mutator; } }
        public ModulePreset[] Presets {
            get {
                return new ModulePreset[]{
                    new ModulePreset("Translate", ()=>(new TranslateMutator{
                        PreserveAspectRatio=true
                    }) ),
                    new ModulePreset("Translate/Skew", ()=>(new TranslateMutator{
                        PreserveAspectRatio=false
                    }) )
                };
            }
        }
        public IModule GetInstance() {
            return new TranslateMutator {
                PreserveAspectRatio = true
            };
        }
    }


    class TranslateMutator : IMutator {

        public bool PreserveAspectRatio { get; set; }

        const int MaxOverlap = 5;

        public DNA Mutate( Random rand, DNA oldDNA, TaskState task ) {
            DNA newDNA = new DNA( oldDNA );

            DNA.Shape shape = newDNA.Shapes[rand.Next( newDNA.Shapes.Length )];
            switch( rand.Next( 4 ) ) {
                case 0:
                    shape.PreviousState = shape.Clone() as DNA.Shape;
                    MoveShape( rand,shape,task );
                    break;
                case 1:
                    shape.PreviousState = shape.Clone() as DNA.Shape;
                    ScaleShape( rand,shape,task );
                    break;
                case 2:
                    shape.PreviousState = shape.Clone() as DNA.Shape;
                    MoveShape( rand,shape,task );
                    ScaleShape( rand,shape,task );
                    break;
                case 3:
                    switch( rand.Next( 6 ) ) {
                        case 0:
                            shape.PreviousState = shape.Clone() as DNA.Shape;
                            shape.Color = Color.FromArgb( (byte)rand.Next( 256 ), shape.Color.R, shape.Color.G, shape.Color.B );
                            newDNA.LastMutation = MutationType.ReplaceColor;
                            break;
                        case 1:
                            shape.PreviousState = shape.Clone() as DNA.Shape;
                            shape.Color = Color.FromArgb( shape.Color.A, (byte)rand.Next( 256 ), shape.Color.G, shape.Color.B );
                            newDNA.LastMutation = MutationType.ReplaceColor;
                            break;
                        case 2:
                            shape.PreviousState = shape.Clone() as DNA.Shape;
                            shape.Color = Color.FromArgb( shape.Color.A, shape.Color.R, (byte)rand.Next( 256 ), shape.Color.B );
                            newDNA.LastMutation = MutationType.ReplaceColor;
                            break;
                        case 3:
                            shape.PreviousState = shape.Clone() as DNA.Shape;
                            shape.Color = Color.FromArgb( shape.Color.A, shape.Color.R, shape.Color.G, (byte)rand.Next( 256 ) );
                            newDNA.LastMutation = MutationType.ReplaceColor;
                            break;
                        case 4: {
                                int s1 = rand.Next( newDNA.Shapes.Length );
                                int s2;
                                do {
                                    s2 = rand.Next( newDNA.Shapes.Length );
                                } while( s1 == s2 );
                                if( s2 > s1 ) {
                                    for( int i = s1; i < s2; i++ ) {
                                        newDNA.Shapes[i] = newDNA.Shapes[i + 1];
                                    }
                                } else {
                                    for( int i = s1; i > s2; i-- ) {
                                        newDNA.Shapes[i] = newDNA.Shapes[i - 1];
                                    }
                                }
                                newDNA.Shapes[s2] = shape;
                                newDNA.LastMutation = MutationType.SwapShapes;
                            } break;
                        case 5: {
                                int s1 = rand.Next( newDNA.Shapes.Length );
                                int s2 = rand.Next( newDNA.Shapes.Length );
                                shape = newDNA.Shapes[s1];
                                newDNA.Shapes[s1] = newDNA.Shapes[s2];
                                newDNA.Shapes[s2] = shape;
                                newDNA.LastMutation = MutationType.SwapShapes;
                            } break;
                    }
                    break;
            }
            return newDNA;
        }


        void MoveShape( Random rand, DNA.Shape shape, TaskState task ) {
            RectangleF rect = shape.GetBoundaries();
            Point delta = new Point {
                X = rand.Next( (int)-rect.X - MaxOverlap, (int)(task.ImageWidth - rect.Right) + MaxOverlap ),
                Y = rand.Next( (int)-rect.Y - MaxOverlap, (int)(task.ImageHeight - rect.Bottom) + MaxOverlap )
            };
            for( int i = 0; i < shape.Points.Length; i++ ) {
                shape.Points[i].X += delta.X;
                shape.Points[i].Y += delta.Y;
            }
        }


        void ScaleShape( Random rand, DNA.Shape shape, TaskState task ) {
            RectangleF rect = shape.GetBoundaries();

            int maxWidth = (int)(Math.Min( rect.X, task.ImageWidth - rect.Right ) + rect.Width) + MaxOverlap*2;
            int maxHeight = (int)(Math.Min( rect.Y, task.ImageHeight - rect.Bottom ) + rect.Height) + MaxOverlap * 2;

            int newWidth = rand.Next( 3, maxWidth + 1 );
            int newHeight = rand.Next( 3, maxHeight + 1 );

            double newWidthRatio = newWidth / (double)rect.Width;
            double newHeightRatio = newHeight / (double)rect.Height;

            if( PreserveAspectRatio ) {
                newWidthRatio = Math.Min( newWidthRatio, newHeightRatio );
                newHeightRatio = Math.Min( newWidthRatio, newHeightRatio );
            }

            PointF rectCenter = new PointF {
                X = rect.X + rect.Width / 2f,
                Y = rect.Y + rect.Height / 2f
            };

            for( int i = 0; i < shape.Points.Length; i++ ) {
                shape.Points[i].X = (float)(rectCenter.X + (shape.Points[i].X - rectCenter.X) * newWidthRatio);
                shape.Points[i].Y = (float)(rectCenter.Y + (shape.Points[i].Y - rectCenter.Y) * newHeightRatio);
            }
        }


        object ICloneable.Clone() {
            return new TranslateMutator();
        }

        void IModule.ReadSettings( BinaryReader reader, int settingsLength ) {
        }

        void IModule.WriteSettings( BinaryWriter writer ) {
        }
    }
}