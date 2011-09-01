using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace SuperImageEvolver {
    public class ProjectOptions : ICloneable {
        public Color Matte { get; set; }
        public Color BackColor { get; set; }
        public int MaxOverlap { get; set; }
        public int MinAlpha { get; set; }

        public ProjectOptions() {
            Matte = Color.Transparent;
            BackColor = Color.Black;
            MaxOverlap = 8;
            MinAlpha = 1;

        }

        public object Clone() {
            return new ProjectOptions {
                Matte = Matte,
                MaxOverlap = MaxOverlap,
                MinAlpha = MinAlpha,
                BackColor = BackColor
            };
        }
    }
}
