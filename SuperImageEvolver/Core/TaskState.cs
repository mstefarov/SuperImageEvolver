using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Xml.Linq;

namespace SuperImageEvolver {
    public sealed class TaskState {
        public int Shapes, Vertices;
        public int ImageWidth, ImageHeight;

        public DNA BestMatch;

        public ProjectOptions ProjectOptions = new ProjectOptions();
        public int ImprovementCounter, MutationCounter;

        public Bitmap OriginalImage;
        public BitmapData OriginalImageData;
        public string ProjectFileName;

        public readonly List<Mutation> MutationLog = new List<Mutation>();

        public IInitializer Initializer = new SegmentedInitializer(Color.Black);
        public IMutator Mutator = new HardMutator();
        public IEvaluator Evaluator = new RGBEvaluator( false );

        public DateTime TaskStart;
        public DateTime LastImprovementTime;
        public long LastImprovementMutationCount;
        public Point ClickLocation;

        public const int FormatVersion = 1;

        public readonly object ImprovementLock = new object();

        public readonly Dictionary<MutationType, int> MutationCounts = new Dictionary<MutationType, int>();
        public readonly Dictionary<MutationType, double> MutationImprovements = new Dictionary<MutationType, double>();
        

        public void SetEvaluator( IEvaluator newEvaluator ) {
            lock( ImprovementLock ) {
                if( OriginalImage != null && BestMatch != null ) {
                    using( Bitmap testCanvas = new Bitmap( ImageWidth, ImageHeight ) ) {
                        newEvaluator.Initialize( this );
                        BestMatch.Divergence = newEvaluator.CalculateDivergence( testCanvas, BestMatch, this, 1 );
                    }
                }
                Evaluator = newEvaluator;
            }
        }


        public NBTCompound SerializeNBT() {
            NBTCompound tag = new NBTCompound( "SuperImageEvolver" );
            tag.Append( "FormatVersion", FormatVersion );
            tag.Append( "Shapes", Shapes );
            tag.Append( "Vertices", Vertices );
            tag.Append( "ImprovementCounter", ImprovementCounter );
            tag.Append( "MutationCounter", MutationCounter );
            tag.Append( "ElapsedTime", DateTime.UtcNow.Subtract( TaskStart ).Ticks );

            tag.Append( ProjectOptions.SerializeNBT() );

            tag.Append( "BestMatch", BestMatch.SerializeNBT() );
            tag.Append( "BestMatchDivergence", BestMatch.Divergence );

            NBTag initializerTag = ModuleManager.WriteModule( "Initializer", Initializer );
            tag.Append( initializerTag );

            NBTag mutatorTag = ModuleManager.WriteModule( "Mutator", Mutator );
            tag.Append( mutatorTag );

            NBTag evaluatorTag = ModuleManager.WriteModule( "Evaluator", Evaluator );
            tag.Append( evaluatorTag );

            byte[] imageData;
            using( MemoryStream ms = new MemoryStream() ) {
                OriginalImage.Save( ms, ImageFormat.Png );
                ms.Flush();
                imageData = new byte[ms.Length];
                Buffer.BlockCopy( ms.GetBuffer(), 0, imageData, 0, imageData.Length );
            }

            tag.Append( "ImageData", imageData );

            List<NBTCompound> statTags = new List<NBTCompound>();
            foreach( MutationType mtype in Enum.GetValues( typeof( MutationType ) ) ) {
                NBTCompound stat = new NBTCompound( "MutationTypeStat" );
                stat.Append( "Type", mtype.ToString() );
                stat.Append( "Count", MutationCounts[mtype] );
                stat.Append( "Sum", MutationImprovements[mtype] );
                statTags.Add( stat );
            }
            var stats = new NBTList( "MutationStats", NBTType.Compound, statTags.ToArray() );
            tag.Append( stats );

            return tag;
        }


        public TaskState() {
            foreach( MutationType mutype in Enum.GetValues( typeof( MutationType ) ) ) {
                MutationCounts[mutype] = 0;
                MutationImprovements[mutype] = 0;
            }
        }

        public TaskState( Stream stream ) : this() {
            BinaryReader reader = new BinaryReader( stream );
            if( reader.ReadInt32() != FormatVersion ) throw new FormatException();
            Shapes = reader.ReadInt32();
            Vertices = reader.ReadInt32();
            BestMatch = new DNA( stream, Shapes, Vertices );
            ImprovementCounter = reader.ReadInt32();
            MutationCounter = reader.ReadInt32();
            TaskStart = DateTime.UtcNow.Subtract( TimeSpan.FromTicks( reader.ReadInt64() ) );

            Initializer = (IInitializer)ModuleManager.ReadModule( stream );
            Mutator = (IMutator)ModuleManager.ReadModule( stream );
            Evaluator = (IEvaluator)ModuleManager.ReadModule( stream );

            int imageLength = reader.ReadInt32();
            using( MemoryStream ms = new MemoryStream( reader.ReadBytes( imageLength ) ) ) {
                OriginalImage = new Bitmap( Image.FromStream( ms ) );
            }

            int statCount = reader.ReadInt32();
            for( int i = 0; i < statCount; i++ ) {
                string statName = reader.ReadString();
                try {
                    MutationType mtype = (MutationType)Enum.Parse( typeof( MutationType ), statName );
                    MutationCounts[mtype] = reader.ReadInt32();
                    MutationImprovements[mtype] = reader.ReadDouble();
                } catch( ArgumentException ) { }
            }
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
            foreach( Shape shape in currentBestMatch.Shapes ) {
                root.Add( shape.SerializeSVG( svg ) );
            }
            doc.Add( root );
            
            return doc;
        }
    }
}
