using System;
using System.Drawing;
using System.IO;


namespace SuperImageEvolver {

    public class SoftTranslateMutatorFactory : IModuleFactory {
        public Type ModuleType { get { return typeof( SoftTranslateMutator ); } }
        public string ID { get { return "std.SoftTranslateMutator.1"; } }
        public ModuleFunction Function { get { return ModuleFunction.Mutator; } }
        public ModulePreset[] Presets {
            get {
                return new ModulePreset[]{
                    new ModulePreset("Soft Translate", ()=>(new SoftTranslateMutator{
                        PreserveAspectRatio=true
                    }), this),
                    new ModulePreset("Soft Translate/Skew", ()=>(new SoftTranslateMutator{
                        PreserveAspectRatio=false
                    }), this )
                };
            }
        }
        public IModule GetInstance() {
            return new SoftTranslateMutator {
                PreserveAspectRatio = true
            };
        }
    }


    class SoftTranslateMutator : IMutator {

        public bool PreserveAspectRatio { get; set; }
        public bool EnableRotation { get; set; }

        public int MaxOverlap { get; set; }
        public float MaxDelta { get; set; }

        public SoftTranslateMutator() {
            MaxOverlap = 6;
            MaxDelta = 12;
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
                    ChangeColor( rand, shape, task );
                    newDNA.LastMutation = MutationType.AdjustColor;
                    break;
                case 10:
                    SwapShapes( rand, newDNA, task );
                    newDNA.LastMutation = MutationType.SwapShapes;
                    break;
                case 11:
                    shape.PreviousState = shape.Clone() as Shape;
                    MoveShape( rand, shape, task );
                    ScaleShape( rand, shape, task );
                    ChangeColor( rand, shape, task );
                    newDNA.LastMutation = MutationType.Transform;
                    break;
                case 12:
                case 13:
                    shape.PreviousState = shape.Clone() as Shape;
                    RotateShape( rand, shape, task );
                    newDNA.LastMutation = MutationType.Rotate;
                    break;
                case 14:
                    shape.PreviousState = shape.Clone() as Shape;
                    MoveShape( rand, shape, task );
                    RotateShape( rand, shape, task );
                    newDNA.LastMutation = MutationType.Rotate;
                    break;
                case 15:
                    shape.PreviousState = shape.Clone() as Shape;
                    ChangeColor( rand, shape, task );
                    newDNA.LastMutation = MutationType.AdjustColor;
                    break;
            }
            return newDNA;
        }



        void MoveShape( Random rand, Shape shape, TaskState task ) {
            RectangleF rect = shape.GetBoundaries();
            PointF delta = new PointF {
                X = Next( rand, Math.Max(-MaxDelta,-rect.X - MaxOverlap), Math.Min(MaxDelta,task.ImageWidth - rect.Right + MaxOverlap) ),
                Y = Next( rand, Math.Max(-MaxDelta,-rect.Y - MaxOverlap), Math.Min(MaxDelta,task.ImageHeight - rect.Bottom + MaxOverlap) )
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

            double newWidthRatio = rand.Next( (int)Math.Max( 3, rect.Width - MaxDelta ), (int)Math.Min( rect.Width + MaxDelta, maxWidth + 1 ) ) / rect.Width;
            double newHeightRatio = rand.Next( (int)Math.Max( 3, rect.Height - MaxDelta ), (int)Math.Min( rect.Height + MaxDelta, maxHeight + 1 ) ) / rect.Height;
            //double newHeightRatio = rand.Next( 3, maxHeight + 1 ) / rect.Height;

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
            Shape shape = newDNA.Shapes[s1];
            shape.PreviousState = shape.Clone() as Shape;
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


        void ChangeColor( Random rand, Shape shape, TaskState task ) {
            shape.PreviousState = shape.Clone() as Shape;
            int delta = (byte)rand.Next( 1, (int)(MaxDelta + 1) ) * (rand.Next( 2 ) == 0 ? 1 : -1);
            switch( rand.Next( 4 ) ) {
                case 0:
                    shape.Color = Color.FromArgb( Math.Max( 1, Math.Min( 255, (int)shape.Color.A + delta ) ), shape.Color.R, shape.Color.G, shape.Color.B );
                    break;
                case 1:
                    shape.Color = Color.FromArgb( shape.Color.A, Math.Max( 0, Math.Min( 255, (int)shape.Color.R + delta ) ), shape.Color.G, shape.Color.B );
                    break;
                case 2:
                    shape.Color = Color.FromArgb( shape.Color.A, shape.Color.R, Math.Max( 0, Math.Min( 255, (int)shape.Color.G + delta ) ), shape.Color.B );
                    break;
                case 3:
                    shape.Color = Color.FromArgb( shape.Color.A, shape.Color.R, shape.Color.G, Math.Max( 0, Math.Min( 255, (int)shape.Color.B + delta ) ) );
                    break;
            }
        }


        void RotateShape( Random rand, Shape shape, TaskState task ) {
            RectangleF rect = shape.GetBoundaries();
            PointF rectCenter = new PointF {
                X = rect.X + rect.Width / 2,
                Y = rect.Y + rect.Height / 2
            };

            double rotation = rand.NextDouble() * Math.PI * 2 * (MaxDelta / 180);

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
            return new SoftTranslateMutator {
                EnableRotation = EnableRotation,
                MaxDelta = MaxDelta,
                MaxOverlap = MaxOverlap,
                PreserveAspectRatio = PreserveAspectRatio
            };
        }

        void IModule.ReadSettings( NBTag tag ) { }

        void IModule.WriteSettings( NBTag tag ) { }
    }
}