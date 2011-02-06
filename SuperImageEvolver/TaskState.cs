using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Xml;
using System.Xml.Linq;


namespace SuperImageEvolver {
    public class TaskState {
        public int Shapes, Vertices;
        public int ImageWidth, ImageHeight;

        public DNA BestMatch;

        public int ImprovementCounter, MutationCounter;

        public Bitmap Image;
        public BitmapData ImageData;
        public Bitmap BestMatchRender;

        public List<Mutation> MutationLog = new List<Mutation>();

        public IInitializer Initializer = new SegmentedInitializer(Color.Black);
        public IMutator Mutator = new HardMutator();
        public IEvaluator Evaluator = new RGBEvaluator( false );

        public DateTime TaskStart;
        public DateTime LastImprovementTime;
        public long LastImprovementMutationCount;

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


        
        public XDocument SerializeSVG() {
            XDocument doc = new XDocument();
            XNamespace svg = "http://www.w3.org/2000/svg";
            XElement root = new XElement( svg+"svg" );
            root.Add( new XAttribute( "xmlns", svg ) );
            root.Add( new XAttribute( XNamespace.Xmlns + "xlink", "http://www.w3.org/1999/xlink" ) );
            root.Add( new XAttribute( "width", ImageWidth ) );
            root.Add( new XAttribute( "height", ImageHeight ) );
            DNA currentBestMatch = BestMatch;
            foreach( DNA.Shape shape in currentBestMatch.Shapes ) {
                root.Add( shape.SerializeSVG( svg ) );
            }
            doc.Add( root );
            
            return doc;
        }
    }
}
