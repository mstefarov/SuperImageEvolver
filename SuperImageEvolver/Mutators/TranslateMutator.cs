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
        public bool EnableRotation { get; set; }

        const int MaxOverlap = 5;

        public DNA Mutate( Random rand, DNA oldDNA, TaskState task ) {
            DNA newDNA = new DNA( oldDNA );

            DNA.Shape shape = newDNA.Shapes[rand.Next( newDNA.Shapes.Length )];
            int choice = rand.Next( (EnableRotation? 16 : 12) );
            switch( choice ) {
                case 0:
                case 1:
                    shape.PreviousState = shape.Clone() as DNA.Shape;
                    MoveShape( rand, shape, task );
                    newDNA.LastMutation = MutationType.Move;
                    break;
                case 2:
                case 3:
                    shape.PreviousState = shape.Clone() as DNA.Shape;
                    ScaleShape( rand, shape, task );
                    newDNA.LastMutation = MutationType.Scale;
                    break;
                case 4:
                    shape.PreviousState = shape.Clone() as DNA.Shape;
                    ScaleShape( rand, shape, task );
                    MoveShape( rand, shape, task );
                    newDNA.LastMutation = MutationType.Transform;
                    break;
                case 5:
                    shape.PreviousState = shape.Clone() as DNA.Shape;
                    MoveShape( rand, shape, task );
                    ScaleShape( rand, shape, task );
                    newDNA.LastMutation = MutationType.Transform;
                    break;
                case 6:
                case 7:
                case 8:
                case 9:
                    shape.PreviousState = shape.Clone() as DNA.Shape;
                    ChangeColor( rand, shape, task );
                    newDNA.LastMutation = MutationType.ReplaceColor;
                    break;
                case 10:
                    SwapShapes( rand, newDNA, task );
                    newDNA.LastMutation = MutationType.SwapShapes;
                    break;
                case 11:
                    shape.PreviousState = shape.Clone() as DNA.Shape;
                    MoveShape( rand, shape, task );
                    ScaleShape( rand, shape, task );
                    ChangeColor( rand, shape, task );
                    newDNA.LastMutation = MutationType.Transform;
                    break;
                case 12:
                case 13:
                    shape.PreviousState = shape.Clone() as DNA.Shape;
                    RotateShape( rand, shape, task );
                    newDNA.LastMutation = MutationType.Rotate;
                    break;
                case 14:
                    shape.PreviousState = shape.Clone() as DNA.Shape;
                    MoveShape( rand, shape, task );
                    RotateShape( rand, shape, task );
                    newDNA.LastMutation = MutationType.Rotate;
                    break;
                case 15:
                    shape.PreviousState = shape.Clone() as DNA.Shape;
                    ChangeColor( rand, shape, task );
                    newDNA.LastMutation = MutationType.ReplaceColor;
                    break;
            }
            return newDNA;
        }



        void MoveShape( Random rand, DNA.Shape shape, TaskState task ) {
            RectangleF rect = shape.GetBoundaries();
            PointF delta = new PointF {
                X = Next( rand, -rect.X - MaxOverlap, (int)(task.ImageWidth - rect.Right) + MaxOverlap ),
                Y = Next( rand, -rect.Y - MaxOverlap, (int)(task.ImageHeight - rect.Bottom) + MaxOverlap )
            };
            for( int i = 0; i < shape.Points.Length; i++ ) {
                shape.Points[i].X += delta.X;
                shape.Points[i].Y += delta.Y;
            }
        }


        void ScaleShape( Random rand, DNA.Shape shape, TaskState task ) {
            RectangleF rect = shape.GetBoundaries();

            int maxWidth = (int)(Math.Min( rect.X, task.ImageWidth - rect.Right ) + rect.Width) + MaxOverlap * 2;
            int maxHeight = (int)(Math.Min( rect.Y, task.ImageHeight - rect.Bottom ) + rect.Height) + MaxOverlap * 2;

            double newWidthRatio = rand.Next( 3, maxWidth + 1 ) / rect.Width;
            double newHeightRatio = rand.Next( 3, maxHeight + 1 ) / rect.Height;

            if( PreserveAspectRatio ) {
                newWidthRatio = Math.Min( newWidthRatio, newHeightRatio );
                newHeightRatio = newWidthRatio;
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


        void SwapShapes( Random rand, DNA newDNA, TaskState task ) {
            int s1 = rand.Next( newDNA.Shapes.Length );
            DNA.Shape shape = newDNA.Shapes[s1];
            shape.PreviousState = shape.Clone() as DNA.Shape;
            if( rand.Next( 2 ) == 0 ) {
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
            } else {
                int s2 = rand.Next( newDNA.Shapes.Length );
                newDNA.Shapes[s1] = newDNA.Shapes[s2];
                newDNA.Shapes[s2] = shape;
            }
        }


        void ChangeColor( Random rand, DNA.Shape shape, TaskState task ) {
            shape.PreviousState = shape.Clone() as DNA.Shape;
            switch( rand.Next( 4 ) ) {
                case 0:
                    shape.Color = Color.FromArgb( (byte)rand.Next( 256 ), shape.Color.R, shape.Color.G, shape.Color.B );
                    break;
                case 1:
                    shape.Color = Color.FromArgb( shape.Color.A, (byte)rand.Next( 256 ), shape.Color.G, shape.Color.B );
                    break;
                case 2:
                    shape.Color = Color.FromArgb( shape.Color.A, shape.Color.R, (byte)rand.Next( 256 ), shape.Color.B );
                    break;
                case 3:
                    shape.Color = Color.FromArgb( shape.Color.A, shape.Color.R, shape.Color.G, (byte)rand.Next( 256 ) );
                    break;
            }
        }


        void RotateShape( Random rand, DNA.Shape shape, TaskState task ) {
            RectangleF rect = shape.GetBoundaries();
            PointF rectCenter = new PointF {
                X = rect.X + rect.Width / 2,
                Y = rect.Y + rect.Height / 2
            };

            double rotation = rand.NextDouble() * Math.PI * 2;

            for( int i = 0; i < shape.Points.Length; i++ ) {
                float alignedX = shape.Points[i].X - rectCenter.X;
                float alignedY = shape.Points[i].Y - rectCenter.Y;
                shape.Points[i].X = (float)(rectCenter.X + alignedX * Math.Cos( rotation ) - alignedY * Math.Sin( rotation ));
                shape.Points[i].Y = (float)(rectCenter.Y + alignedX * Math.Sin( rotation ) + alignedY * Math.Cos( rotation ));
            }
        }


        static float Next( Random rand, double min, double max ) {
            return (float)(rand.NextDouble() * (max - min) + min);
        }
        

        object ICloneable.Clone() {
            return new TranslateMutator();
        }

        void IModule.ReadSettings( BinaryReader reader, int settingsLength ) {
            PreserveAspectRatio = reader.ReadBoolean();
        }

        void IModule.WriteSettings( BinaryWriter writer ) {
            writer.Write( 1 );
            writer.Write( PreserveAspectRatio );
        }
    }
}