using System;
using System.Drawing;

namespace SuperImageEvolver {
    public sealed class ProjectOptions : ICloneable {
        public Color Matte { get; set; }
        public Color BackColor { get; set; }
        public int MaxOverlap { get; set; }
        public byte MinAlpha { get; set; }
        public TimeSpan RefreshRate { get; set; }
        public Color WireframeColor { get; set; }
        public Color LastChangeColor1 { get; set; }
        public Color LastChangeColor2 { get; set; }


        public ProjectOptions() {
            Matte = Color.White;
            BackColor = Color.DimGray;
            MaxOverlap = 8;
            MinAlpha = 1;
            RefreshRate = TimeSpan.FromMilliseconds( 500 );
            WireframeColor = Color.Black;
            LastChangeColor1 = Color.White;
            LastChangeColor2 = Color.Black;
        }


        public object Clone() {
            return new ProjectOptions {
                Matte = Matte,
                MaxOverlap = MaxOverlap,
                MinAlpha = MinAlpha,
                BackColor = BackColor,
                RefreshRate = RefreshRate,
                WireframeColor = WireframeColor,
                LastChangeColor1 = LastChangeColor1,
                LastChangeColor2 = LastChangeColor2
            };
        }


        public NBTag SerializeNBT() {
            NBTCompound tag = new NBTCompound( "ProjectOptions" );
            tag.Append( "Matte", Matte );
            tag.Append( "BackColor", BackColor );
            tag.Append( "MaxOverlap", MaxOverlap );
            tag.Append( "MinAlpha", MinAlpha );
            tag.Append( "RefreshRate", RefreshRate.Ticks );
            tag.Append( "WireframeColor", WireframeColor );
            tag.Append( "LastChangeColor1", LastChangeColor1 );
            tag.Append( "LastChangeColor2", LastChangeColor2 );
            return tag;
        }


        public ProjectOptions( NBTag tag )
            : this() {
            Matte = tag["Matte"].GetColor();
            BackColor = tag["BackColor"].GetColor();
            MaxOverlap = tag["MaxOverlap"].GetInt();
            MinAlpha = tag["MinAlpha"].GetByte();
            RefreshRate = new TimeSpan( tag.GetLong( "RefreshRate", RefreshRate.Ticks ) );
            WireframeColor = tag.GetColor( "WireframeColor", WireframeColor );
            LastChangeColor1 = tag.GetColor( "LastChangeColor1", LastChangeColor1 );
            LastChangeColor2 = tag.GetColor( "LastChangeColor2", LastChangeColor2 );
        }
    }
}