using System;
using System.Drawing;

namespace SuperImageEvolver {
    public class SoftTranslateMutatorFactory : IModuleFactory {
        public Type ModuleType {
            get { return typeof( SoftTranslateMutator ); }
        }

        public string ID {
            get { return "std.SoftTranslateMutator.1"; }
        }

        public ModuleFunction Function {
            get { return ModuleFunction.Mutator; }
        }

        public ModulePreset[] Presets {
            get {
                return new[] {
                    new ModulePreset( "Soft Translate", () => ( new SoftTranslateMutator {
                        PreserveAspectRatio = true
                    } ), this ),
                    new ModulePreset( "Soft Translate/Skew", () => ( new SoftTranslateMutator {
                        PreserveAspectRatio = false
                    } ), this )
                };
            }
        }


        public IModule GetInstance() {
            return new SoftTranslateMutator {
                PreserveAspectRatio = true
            };
        }
    }


    sealed class SoftTranslateMutator : IMutator {

        public bool PreserveAspectRatio { get; set; }
        public bool EnableRotation { get; set; }

        public float MaxColorDelta { get; set; }
        public float MaxPosDelta { get; set; }


        public SoftTranslateMutator() {
            MaxColorDelta = 8;
            MaxPosDelta = 16;
        }


        public DNA Mutate( Random rand, DNA oldDNA, TaskState task ) {
            DNA newDNA = new DNA( oldDNA );

            Shape shape = newDNA.Shapes[rand.Next( newDNA.Shapes.Length )];
            int choice = rand.Next( ( EnableRotation ? 16 : 12 ) );
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
                    newDNA.SwapShapes( rand );
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
                    ChangeColor( rand, shape, task );
                    newDNA.LastMutation = MutationType.AdjustColor;
                    break;
            }
            return newDNA;
        }


        static void MoveShape( Random rand, Shape shape, TaskState task ) {
            RectangleF rect = shape.GetBoundaries();
            int maxOverlap = task.ProjectOptions.MaxOverlap;
            PointF delta = new PointF {
                X =
                    rand.NextFloat( Math.Max( -maxOverlap, -rect.X - maxOverlap ),
                                    Math.Min( maxOverlap, task.ImageWidth - rect.Right + maxOverlap ) ),
                Y =
                    rand.NextFloat( Math.Max( -maxOverlap, -rect.Y - maxOverlap ),
                                    Math.Min( maxOverlap, task.ImageHeight - rect.Bottom + maxOverlap ) )
            };
            for( int i = 0; i < shape.Points.Length; i++ ) {
                shape.Points[i].X += delta.X;
                shape.Points[i].Y += delta.Y;
            }
        }


        void ScaleShape( Random rand, Shape shape, TaskState task ) {
            RectangleF rect = shape.GetBoundaries();
            int maxOverlap = task.ProjectOptions.MaxOverlap;

            int maxWidth = (int)( Math.Min( rect.X, task.ImageWidth - rect.Right ) + rect.Width ) + maxOverlap * 2;
            int maxHeight = (int)( Math.Min( rect.Y, task.ImageHeight - rect.Bottom ) + rect.Height ) + maxOverlap * 2;

            int minWidthRatio = (int)Math.Max( 3, rect.Width - maxOverlap );
            int maxWidthRatio = (int)Math.Min( rect.Width + maxOverlap, maxWidth + 1 );
            double newWidthRatio =
                rand.Next( Math.Min( minWidthRatio, maxWidthRatio ), Math.Max( minWidthRatio, maxWidthRatio ) ) /
                rect.Width;

            int minHeightRatio = (int)Math.Max( 3, rect.Height - maxOverlap );
            int maxHeightRatio = (int)Math.Min( rect.Height + maxOverlap, maxHeight + 1 );
            double newHeightRatio =
                rand.Next( Math.Min( minHeightRatio, maxHeightRatio ), Math.Max( minHeightRatio, maxHeightRatio ) ) /
                rect.Height;
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
                shape.Points[i].X = (float)( rectCenter.X + ( shape.Points[i].X - rectCenter.X ) * newWidthRatio );
                shape.Points[i].Y = (float)( rectCenter.Y + ( shape.Points[i].Y - rectCenter.Y ) * newHeightRatio );
            }
        }


        void ChangeColor( Random rand, Shape shape, TaskState task ) {
            shape.PreviousState = shape.Clone() as Shape;
            int delta = rand.NextByte( 1, (int)( MaxColorDelta + 1 ) ) * ( rand.Next( 2 ) == 0 ? 1 : -1 );
            switch( rand.Next( 4 ) ) {
                case 0:
                    shape.Color =
                        Color.FromArgb(
                            Math.Max( task.ProjectOptions.MinAlpha, Math.Min( 255, shape.Color.A + delta ) ),
                            shape.Color.R, shape.Color.G, shape.Color.B );
                    break;
                case 1:
                    shape.Color = Color.FromArgb( shape.Color.A, Math.Max( 0, Math.Min( 255, shape.Color.R + delta ) ),
                                                  shape.Color.G, shape.Color.B );
                    break;
                case 2:
                    shape.Color = Color.FromArgb( shape.Color.A, shape.Color.R,
                                                  Math.Max( 0, Math.Min( 255, shape.Color.G + delta ) ), shape.Color.B );
                    break;
                case 3:
                    shape.Color = Color.FromArgb( shape.Color.A, shape.Color.R, shape.Color.G,
                                                  Math.Max( 0, Math.Min( 255, shape.Color.B + delta ) ) );
                    break;
            }
        }


        void RotateShape( Random rand, Shape shape ) {
            RectangleF rect = shape.GetBoundaries();
            PointF rectCenter = new PointF {
                X = rect.X + rect.Width / 2,
                Y = rect.Y + rect.Height / 2
            };

            double rotation = rand.NextDouble() * Math.PI * 2 * ( MaxPosDelta / 180 );

            for( int i = 0; i < shape.Points.Length; i++ ) {
                float alignedX = shape.Points[i].X - rectCenter.X;
                float alignedY = shape.Points[i].Y - rectCenter.Y;
                shape.Points[i].X =
                    (float)( rectCenter.X + alignedX * Math.Cos( rotation ) - alignedY * Math.Sin( rotation ) );
                shape.Points[i].Y =
                    (float)( rectCenter.Y + alignedX * Math.Sin( rotation ) + alignedY * Math.Cos( rotation ) );
            }
        }


        object ICloneable.Clone() {
            return new SoftTranslateMutator {
                EnableRotation = EnableRotation,
                MaxColorDelta = MaxColorDelta,
                MaxPosDelta = MaxPosDelta,
                PreserveAspectRatio = PreserveAspectRatio
            };
        }


        void IModule.ReadSettings( NBTag tag ) {}

        void IModule.WriteSettings( NBTag tag ) {}
    }
}