using System;
using System.Drawing;

namespace SuperImageEvolver {
    public sealed class ProjectOptions : ICloneable {
        public Color Matte { get; set; }
        public Color BackColor { get; set; }
        public int MaxOverlap { get; set; }
        public byte MinAlpha { get; set; }
        public TimeSpan RefreshRate { get; set; }


        public ProjectOptions() {
            Matte = Color.White;
            BackColor = Color.Black;
            MaxOverlap = 8;
            MinAlpha = 1;
            RefreshRate = TimeSpan.FromMilliseconds( 500 );
        }


        public object Clone() {
            return new ProjectOptions {
                Matte = Matte,
                MaxOverlap = MaxOverlap,
                MinAlpha = MinAlpha,
                BackColor = BackColor,
                RefreshRate = RefreshRate
            };
        }


        public NBTag SerializeNBT() {
            NBTCompound tag = new NBTCompound( "ProjectOptions" );
            tag.Append( "Matte", Matte );
            tag.Append( "BackColor", BackColor );
            tag.Append( "MaxOverlap", MaxOverlap );
            tag.Append( "MinAlpha", MinAlpha );
            tag.Append( "RefreshRate", RefreshRate.Ticks );
            return tag;
        }


        public ProjectOptions( NBTag tag )
            : this() {
            Matte = tag["Matte"].GetColor();
            BackColor = tag["BackColor"].GetColor();
            MaxOverlap = tag["MaxOverlap"].GetInt();
            MinAlpha = tag["MinAlpha"].GetByte();
            RefreshRate = new TimeSpan( tag.GetLong( "RefreshRate", RefreshRate.Ticks ) );
        }
    }
}