using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SuperImageEvolver {
    sealed unsafe partial class DiffCanvas : UserControl {
        public DiffCanvas() {
            InitializeComponent();
            DoubleBuffered = true;
        }


        public void Init(TaskState _state) {
            state = _state;
            if (state.OriginalImage != null) {
                canvasImage = new Bitmap(state.EvalImageWidth, state.EvalImageHeight);
                Zoom = zoom;
            }
        }


        #region Properties

        public bool Invert {
            get { return invert; }
            set {
                invert = value;
                Invalidate();
            }
        }

        bool invert;


        public bool ShowColor {
            get { return showColor; }
            set {
                showColor = value;
                Invalidate();
            }
        }

        bool showColor = true;


        public bool Exaggerate {
            get { return exaggerate; }
            set {
                exaggerate = value;
                Invalidate();
            }
        }

        bool exaggerate = true;


        public float Zoom {
            get { return zoom; }
            set {
                zoom = value;
                if (state != null) {
                    Size = new Size {
                        Width = (int)Math.Ceiling(state.ImageWidth * Zoom),
                        Height = (int)Math.Ceiling(state.ImageHeight * Zoom)
                    };
                }
                Invalidate();
            }
        }

        float zoom = 1;


        public bool ShowLastChange {
            get { return showLastChange; }
            set {
                showLastChange = value;
                Invalidate();
            }
        }

        bool showLastChange;

        #endregion


        const string PlaceholderText = "differences";
        Bitmap canvasImage;
        TaskState state;

        protected override void OnPaint(PaintEventArgs e) {
            Graphics g2 = e.Graphics;
            DNA currentMatch = state?.CurrentMatch;
            if (currentMatch != null) {
                e.Graphics.ScaleTransform(zoom, zoom);
                state.Evaluator.DrawDivergence(canvasImage, currentMatch, state, Invert, Exaggerate);

                g2.InterpolationMode = InterpolationMode.NearestNeighbor;
                g2.DrawImage(canvasImage, 0, 0, state.ImageWidth, state.ImageHeight);

                if (showLastChange) {
                    Pen lastChangePen1 = new Pen(state.ProjectOptions.LastChangeColor1, 2 / zoom);
                    Pen lastChangePen2 = new Pen(state.ProjectOptions.LastChangeColor2, 1 / zoom);
                    g2.SmoothingMode = (state.Evaluator.Smooth ? SmoothingMode.HighQuality : SmoothingMode.HighSpeed);
                    for (int i = 0; i < currentMatch.Shapes.Length; i++) {
                        if (currentMatch.Shapes[i].PreviousState != null) {
                            g2.DrawPolygon(lastChangePen1, currentMatch.Shapes[i].Points);
                            g2.DrawPolygon(lastChangePen2, currentMatch.Shapes[i].PreviousState.Points);
                        }
                    }
                }

            } else {
                SizeF align = g2.MeasureString(PlaceholderText, Font);
                g2.DrawString(PlaceholderText,
                               Font,
                               Brushes.White,
                               Width / 2f - align.Width / 2,
                               Height / 2f - align.Height / 2);
            }
            base.OnPaint(e);
        }
    }
}
