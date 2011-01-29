using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;


namespace SuperImageEvolver {
    public class TaskState {
        public int Shapes, Vertices;
        public int ImageWidth, ImageHeight;

        public DNA BestMatch;

        public int ImprovementCounter, MutationCounter;

        public Bitmap Image;
        public BitmapData ImageData;
        public Bitmap BestMatchRender;

        public List<Mutation> MutationLog;

        public IInitializer Initializer;
        public IMutator Mutator;
        public IEvaluator Evaluator;

        public DateTime TaskStart;

        public object ImprovementLock = new object();

        public void SetEvaluator( IEvaluator newEvaluator ) {
            lock( ImprovementLock ) {
                Evaluator = newEvaluator;
                if( Image != null && BestMatch != null ) {
                    using( Bitmap testCanvas = new Bitmap( ImageWidth, ImageHeight ) ) {
                        newEvaluator.Initialize( this );
                        BestMatch.Divergence = newEvaluator.CalculateDivergence( testCanvas, BestMatch, this, 1 );
                    }
                }
            }
        }

        public const int FormatVersion = 1;
        public void Serialize( Stream stream ) {
            BinaryWriter writer = new BinaryWriter( stream );
            writer.Write( FormatVersion );
            writer.Write( Shapes );
            writer.Write( Vertices );
            BestMatch.Serialize( stream );
            writer.Write( ImprovementCounter );
            writer.Write( MutationCounter );
            writer.Write( ImprovementCounter );
            writer.Write( DateTime.UtcNow.Subtract( TaskStart ).Ticks );

            ModuleManager.WriteModule( Initializer, stream );
            ModuleManager.WriteModule( Mutator, stream );
            ModuleManager.WriteModule( Evaluator, stream );

            Image.Save( stream, ImageFormat.Bmp );
        }
    }
}
