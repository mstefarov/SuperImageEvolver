using System;
using System.Drawing;


namespace SuperImageEvolver {

    public class TranslateMutatorFactory : IModuleFactory {
        public Type ModuleType { get { return typeof( TranslateMutator ); } }
        public string ID { get { return "std.TranslateMutator.1"; } }
        public ModuleFunction Function { get { return ModuleFunction.Mutator; } }
        public ModulePreset[] Presets {
            get {
                return new[]{
                    new ModulePreset("Translate", ()=>(new TranslateMutator{
                        PreserveAspectRatio=true
                    }), this ),
                    new ModulePreset("Translate/Skew", ()=>(new TranslateMutator{
                        PreserveAspectRatio=false
                    }), this )
                };
            }
        }
        public IModule GetInstance() {
            return new TranslateMutator {
                PreserveAspectRatio = true
            };
        }
    }


    sealed class TranslateMutator : IMutator {

        public bool PreserveAspectRatio { get; set; }
        public bool EnableRotation { get; set; }
        public int MaxOverlap { get; set; }

        public TranslateMutator() {
            MaxOverlap = 6;
        }

        public DNA Mutate( Random rand, DNA oldDNA, TaskState task ) {
            DNA newDNA = new DNA( oldDNA );

            Shape shape = newDNA.Shapes[rand.Next( newDNA.Shapes.Length )];
            int choice = rand.Next( (EnableRotation? 16 : 12) );
            switch( choice ) {
                case 0:
                case 1:
                    shape.PreviousState = shape.Clone() as Shape;
                    MoveShape( rand, shape, task );
                    newDNA.LastMutation = MutationType.Move;
                    break;
                case 2:
                case 3:
                    shape.PreviousState = shape.Clone() as Shape;
                    ScaleShape( rand, shape, task );
                    newDNA.LastMutation = MutationType.Scale;
                    break;
                case 4:
                    shape.PreviousState = shape.Clone() as Shape;
                    ScaleShape( rand, shape, task );
                    MoveShape( rand, shape, task );
                    newDNA.LastMutation = MutationType.Transform;
                    break;
                case 5:
                    shape.PreviousState = shape.Clone() as Shape;
                    MoveShape( rand, shape, task );
                    ScaleShape( rand, shape, task );
                    newDNA.LastMutation = MutationType.Transform;
                    break;
                case 6:
                case 7:
                case 8:
                case 9:
                    shape.PreviousState = shape.Clone() as Shape;
                    ChangeColor( rand, shape );
                    newDNA.LastMutation = MutationType.ReplaceColor;
                    break;
                case 10:
                    MutatorHelper.SwapShapes( rand, newDNA );
                    newDNA.LastMutation = MutationType.SwapShapes;
                    break;
                case 11:
                    shape.PreviousState = shape.Clone() as Shape;
                    MoveShape( rand, shape, task );
                    ScaleShape( rand, shape, task );
                    ChangeColor( rand, shape );
                    newDNA.LastMutation = MutationType.ReplaceShape;
                    break;
                case 12:
                case 13:
                    shape.PreviousState = shape.Clone() as Shape;
                    RotateShape( rand, shape );
                    newDNA.LastMutation = MutationType.Rotate;
                    break;
                case 14:
                    shape.PreviousState = shape.Clone() as Shape;
                    MoveShape( rand, shape, task );
                    RotateShape( rand, shape );
                    newDNA.LastMutation = MutationType.Transform;
                    break;
                case 15:
                    shape.PreviousState = shape.Clone() as Shape;
                    ChangeColor( rand, shape );
                    newDNA.LastMutation = MutationType.ReplaceColor;
                    break;
            }
            return newDNA;
        }



        void MoveShape( Random rand, Shape shape, TaskState task ) {
            RectangleF rect = shape.GetBoundaries();
            PointF delta = new PointF {
                X = Next( rand, -rect.X - MaxOverlap, task.ImageWidth - rect.Right + MaxOverlap ),
                Y = Next( rand, -rect.Y - MaxOverlap, task.ImageHeight - rect.Bottom + MaxOverlap )
            };
            for( int i = 0; i < shape.Points.Length; i++ ) {
                shape.Points[i].X += delta.X;
                shape.Points[i].Y += delta.Y;
            }
        }


        void ScaleShape( Random rand, Shape shape, TaskState task ) {
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


        static void ChangeColor( Random rand, Shape shape ) {
            shape.PreviousState = shape.Clone() as Shape;
            switch( rand.Next( 4 ) ) {
                case 0:
                    shape.Color = Color.FromArgb( (byte)rand.Next( 1,256 ), shape.Color.R, shape.Color.G, shape.Color.B );
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


        static void RotateShape( Random rand, Shape shape ) {
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
            return new TranslateMutator {
                EnableRotation = EnableRotation,
                MaxOverlap = MaxOverlap,
                PreserveAspectRatio = PreserveAspectRatio
            };
        }

        void IModule.ReadSettings( NBTag tag ) { }

        void IModule.WriteSettings( NBTag tag ) { }
    }
}