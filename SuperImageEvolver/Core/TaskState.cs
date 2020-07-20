using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Xml.Linq;

namespace SuperImageEvolver
{
    public sealed class TaskState {
        public int Shapes, Vertices;
        public int ImageWidth, ImageHeight;
        
        public volatile DNA CurrentMatch;
        public volatile DNA BestMatch;

        public ProjectOptions ProjectOptions = new ProjectOptions();

        public Bitmap OriginalImage;
        public Bitmap WorkingImageCopy;
        public Bitmap WorkingImageCopyClone;
        public BitmapData WorkingImageData;
        public string ProjectFileName;

        public volatile IInitializer Initializer = new SegmentedInitializer( Color.Black );
        public volatile IMutator Mutator = new HardMutator();
        public volatile IEvaluator Evaluator = new RGBEvaluator( false );

        public DateTime TaskStart;
        public DateTime LastImprovementTime;
        public long LastImprovementMutationCount;

        public TaskStatistics Stats { get; } = new TaskStatistics();

        public bool HasChangedSinceSave = true;

        public const int FormatVersion = 1;

        public readonly object ImprovementLock = new object();
        

        public TaskState( NBTag tag ) {
            if( FormatVersion != tag["FormatVersion"].GetInt() ) throw new FormatException( "Incompatible format." );
            Shapes = tag["Shapes"].GetInt();
            Vertices = tag["Vertices"].GetInt();
            TaskStart = DateTime.UtcNow.Subtract( TimeSpan.FromTicks( tag["ElapsedTime"].GetLong() ) );
            LastImprovementMutationCount = tag.GetLong("LastImprovementMutationCount", 0);

            ReadCoreConfig(tag);
            Stats.Read(tag);

            byte[] imageBytes = tag["ImageData"].GetBytes();
            using( MemoryStream ms = new MemoryStream( imageBytes ) ) {
                OriginalImage = new Bitmap( ms );
            }
        }


        public TaskState() {
        }


        public void SetEvaluator( IEvaluator newEvaluator ) {
            lock( ImprovementLock ) {
                if( OriginalImage != null && BestMatch != null ) {
                    using( Bitmap testCanvas = new Bitmap( ImageWidth, ImageHeight ) ) {
                        newEvaluator.Initialize( this );
                        BestMatch.Divergence = newEvaluator.CalculateDivergence( testCanvas, BestMatch, this, 1 );
                        CurrentMatch = BestMatch;
                    }
                }
                Evaluator = newEvaluator;
            }
        }


        public NBTCompound SerializeNBT(string rootTag = "SuperImageEvolver") {
            HasChangedSinceSave = false;
            NBTCompound tag = new NBTCompound(rootTag);
            tag.Append( "FormatVersion", FormatVersion );
            tag.Append( "Shapes", Shapes );
            tag.Append( "Vertices", Vertices );
            tag.Append( "ElapsedTime", DateTime.UtcNow.Subtract( TaskStart ).Ticks );
            tag.Append( "LastImprovementMutationCount", LastImprovementMutationCount );

            StoreCoreConfig(tag);
            Stats.Store(tag);

            byte[] imageData;
            using( MemoryStream ms = new MemoryStream() ) {
                lock( OriginalImage ) {
                    OriginalImage.Save( ms, ImageFormat.Png );
                }
                ms.Flush();
                imageData = new byte[ms.Length];
                Buffer.BlockCopy( ms.GetBuffer(), 0, imageData, 0, imageData.Length );
            }

            tag.Append( "ImageData", imageData );

            return tag;
        }


        public void ReadCoreConfig(NBTag tag)
        {
            ProjectOptions = new ProjectOptions(tag["ProjectOptions"]);

            BestMatch = new DNA(tag["BestMatch"]);
            CurrentMatch = BestMatch;

            Initializer = (IInitializer)ModuleManager.ReadModule(tag["Initializer"]);
            Mutator = (IMutator)ModuleManager.ReadModule(tag["Mutator"]);
            Evaluator = (IEvaluator)ModuleManager.ReadModule(tag["Evaluator"]);
        }


        public void StoreCoreConfig(NBTag tag)
        {
            tag.Append(ProjectOptions.SerializeNBT());

            tag.Append(BestMatch.SerializeNBT("BestMatch"));

            NBTag initializerTag = ModuleManager.WriteModule("Initializer", Initializer);
            tag.Append(initializerTag);

            NBTag mutatorTag = ModuleManager.WriteModule("Mutator", Mutator);
            tag.Append(mutatorTag);

            NBTag evaluatorTag = ModuleManager.WriteModule("Evaluator", Evaluator);
            tag.Append(evaluatorTag);
        }


        public XDocument SerializeSVG() {
            XDocument doc = new XDocument();
            XNamespace svg = "http://www.w3.org/2000/svg";
            XElement root = new XElement( svg + "svg",
                new XAttribute( "xmlns", svg ),
                new XAttribute( XNamespace.Xmlns + "xlink", "http://www.w3.org/1999/xlink" ),
                new XAttribute( "width", ImageWidth ),
                new XAttribute( "height", ImageHeight ) );

            if( ProjectOptions.Matte != Color.White ) {
                string matteRGB = String.Format( "rgb({0},{1},{2})",
                                                 ProjectOptions.Matte.R,
                                                 ProjectOptions.Matte.G,
                                                 ProjectOptions.Matte.B );
                XElement fill = new XElement( svg+ "rect",
                    new XAttribute( "x", 0 ),
                    new XAttribute( "y", 0 ),
                    new XAttribute( "width", ImageWidth ),
                    new XAttribute( "height", ImageHeight ),
                    new XAttribute( "fill", matteRGB ),
                    new XAttribute( "fill-opacity", ProjectOptions.Matte.A/255f ) );
                root.Add( fill );
            }

            DNA currentBestMatch = BestMatch;
            foreach( Shape shape in currentBestMatch.Shapes ) {
                root.Add( shape.SerializeSVG( svg ) );
            }
            doc.Add( root );

            return doc;
        }


        public void SetBestMatch(DNA mutation)
        {
            BestMatch = mutation;
            LastImprovementTime = DateTime.UtcNow;
            LastImprovementMutationCount = Stats.MutationCounter;
            Stats.ImprovementCounter++;
            Stats.MutationDataLog.Add(new PointF {
                X = (float)DateTime.UtcNow.Subtract(TaskStart).TotalSeconds,
                Y = (float)mutation.Divergence
            });
            HasChangedSinceSave = true;
        }


        public void SetOriginalImage(Bitmap image)
        {
            OriginalImage = image;
            if (WorkingImageCopy != null)
            {
                WorkingImageCopy.Dispose();
            }

            WorkingImageCopy = new Bitmap(OriginalImage.Width, OriginalImage.Height);
            WorkingImageCopy.SetResolution(OriginalImage.HorizontalResolution, OriginalImage.VerticalResolution);
            using (Graphics g = Graphics.FromImage(WorkingImageCopy))
            {
                g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.Clear(ProjectOptions.Matte);
                g.DrawImageUnscaled(OriginalImage, Point.Empty);
            }

            if (WorkingImageData != null)
            {
                WorkingImageCopyClone.UnlockBits(WorkingImageData);
                WorkingImageCopyClone.Dispose();
            }
            WorkingImageCopyClone = (Bitmap)WorkingImageCopy.Clone();
            WorkingImageData =
                WorkingImageCopyClone.LockBits(new Rectangle(Point.Empty, OriginalImage.Size),
                                                     ImageLockMode.ReadOnly,
                                                     PixelFormat.Format32bppArgb);
            ImageWidth = OriginalImage.Width;
            ImageHeight = OriginalImage.Height;
        }
    }
}
