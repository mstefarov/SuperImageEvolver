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
        public byte MinAlpha { get; set; }

        public ProjectOptions() {
            Matte = Color.White;
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

        public NBTag SerializeNBT() {
            NBTCompound tag = new NBTCompound( "ProjectOptions" );
            tag.Append( "Matte", Matte );
            tag.Append( "BackColor", BackColor );
            tag.Append( "MaxOverlap", MaxOverlap );
            tag.Append( "MinAlpha", MinAlpha );
            return tag;
        }

        public ProjectOptions( NBTag tag ) {
            Matte = tag["Matte"].GetColor();
            BackColor = tag["BackColor"].GetColor();
            MaxOverlap = tag["MaxOverlap"].GetInt();
            MinAlpha = tag["MinAlpha"].GetByte();
        }
    }
}
