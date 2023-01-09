using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Xml.Linq;
using static System.Windows.Forms.AxHost;

namespace SuperImageEvolver
{
    public sealed class TaskState {
        public int Shapes, Vertices;
        public int ImageWidth, ImageHeight;
        public int EvalImageWidth, EvalImageHeight;
        
        public volatile DNA CurrentMatch;
        public volatile DNA BestMatch;

        public ProjectOptions ProjectOptions = new ProjectOptions();

        public Bitmap OriginalImage;
        public Bitmap WorkingImageCopy; // Scaled to EvalScale
        private Bitmap _workingDataProvider; // Scaled to EvalScale
        public BitmapData WorkingImageData;
        public string ProjectFileName;

        public volatile IInitializer Initializer = new SegmentedInitializer( Color.Black );
        public volatile IMutator Mutator = new HardMutator();
        public volatile IEvaluator Evaluator = new RGBEvaluator( false );
        public volatile float EvalScale = 1.0f;

        public DateTime TaskStart;
        public DateTime LastImprovementTime;
        public long LastImprovementMutationCount;

        public TaskStatistics Stats { get; } = new TaskStatistics();

        public bool HasChangedSinceSave = true;
        public int ConfigVersion; // Used to ensure that "stale" best-matches are not accepted from clients after Evaluator was changed by server

        // V1: original
        // V2: adds EvalScale, removes Sloppy module, changes stats to long
        public const int FormatVersion = 2;

        public readonly object ImprovementLock = new object();
        

        public TaskState( NBTag tag ) {
            var format = tag["FormatVersion"].GetInt();
            if (format > FormatVersion )
                throw new FormatException( "Incompatible format: " + format);

            if(format == 1)
                UpgradeFromV1(tag);

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
            lock ( ImprovementLock ) {
                newEvaluator.Initialize(this);
                Evaluator = newEvaluator;
                RefreshBestMatchDivergence();
                HasChangedSinceSave = true;
            }
        }

        public void SetEvalScale(float scale) {
            lock (ImprovementLock) {
                EvalScale = scale;
                if (OriginalImage != null) {
                    SetOriginalImage(OriginalImage);
                    RefreshBestMatchDivergence();
                }
            }
        }

        private void RefreshBestMatchDivergence() {
            if (WorkingImageData != null && BestMatch != null && Evaluator != null) {
                using (Bitmap testCanvas = new Bitmap(EvalImageWidth, EvalImageHeight)) {
                    BestMatch.Divergence = Evaluator.CalculateDivergence(testCanvas, BestMatch, this, 1);
                }
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
            ConfigVersion = tag.GetInt(nameof(ConfigVersion), 0);
            CurrentMatch = BestMatch;

            Initializer = (IInitializer)ModuleManager.ReadModule(tag["Initializer"]);
            Mutator = (IMutator)ModuleManager.ReadModule(tag["Mutator"]);
            Evaluator = (IEvaluator)ModuleManager.ReadModule(tag["Evaluator"]);

            EvalScale = tag.GetFloat(nameof(EvalScale), 1);
        }


        public void StoreCoreConfig(NBTag tag)
        {
            tag.Append(ProjectOptions.SerializeNBT());

            tag.Append(BestMatch.SerializeNBT("BestMatch"));
            tag.Append( nameof(ConfigVersion), ConfigVersion );
            tag.Append( nameof(EvalScale), EvalScale );

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
            ImageWidth = image.Width;
            ImageHeight = image.Height;
            EvalImageWidth = (int)Math.Ceiling(ImageWidth * EvalScale);
            EvalImageHeight = (int)Math.Ceiling(ImageHeight * EvalScale);

            if (WorkingImageCopy != null)
                WorkingImageCopy.Dispose();

            WorkingImageCopy = new Bitmap(EvalImageWidth, EvalImageHeight);
            using (Graphics g = Graphics.FromImage(WorkingImageCopy))
            {
                g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
                g.Clear(ProjectOptions.Matte);
                g.DrawImage(OriginalImage, 0, 0, EvalImageWidth, EvalImageHeight);
            }

            if (WorkingImageData != null)
            {
                _workingDataProvider.UnlockBits(WorkingImageData);
                _workingDataProvider.Dispose();
            }
            _workingDataProvider = (Bitmap)WorkingImageCopy.Clone();
            WorkingImageData =
                _workingDataProvider.LockBits(new Rectangle(Point.Empty, WorkingImageCopy.Size),
                                             ImageLockMode.ReadOnly,
                                             WorkingImageCopy.PixelFormat);
        }

        private void UpgradeFromV1(NBTag tag) {
            // Upgrade stat counters to long
            tag["ImprovementCounter"].Set((long)tag.GetInt("ImprovementCounter", 0), NBTType.Long);
            tag["MutationCounter"].Set((long)tag.GetInt("MutationCounter", 0), NBTType.Long);
            tag["RiskyMoveCounter"].Set((long)tag.GetInt("RiskyMoveCounter", 0), NBTType.Long);
            tag["FailedRiskCounter"].Set((long)tag.GetInt("FailedRiskCounter", 0), NBTType.Long);

            // Replace "sloppyRGB" with "RGB" at 0.5 scale.
            var evaluator = tag["Evaluator"];
            string moduleID = evaluator["ID"].GetString();
            if (moduleID == "std.SloppyRGBEvaluator.1") {
                evaluator["ID"].Set("std.RGBEvaluator.1");
                tag.Append(nameof(EvalScale), 0.5f);
            } else if (moduleID == "std.LumaEvaluator.1") {
                evaluator["ID"].Set("std.PerceptualEvaluator.1");
            }

            tag["FormatVersion"].Set(FormatVersion);
        }
    }
}
