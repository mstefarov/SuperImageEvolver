// 
//  Authors:
//   *  Tyler Kennedy <tk@tkte.ch>
//   *  Matvei Stefarov <fragmer@gmail.com>
// 
//  Copyright (c) 2010-2011, Tyler Kennedy & Matvei Stefarov
// 
//  All rights reserved.
// 
//  Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
// 
//     * Redistributions of source code must retain the above copyright notice, this
//       list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright notice,
//       this list of conditions and the following disclaimer in
//       the documentation and/or other materials provided with the distribution.
//     * Neither the name of MCC nor the names of its contributors may be
//       used to endorse or promote products derived from this software without
//       specific prior written permission.
// 
//  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
//  "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
//  LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
//  A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
//  CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
//  EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
//  PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
//  PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
//  LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
//  NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
//  SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// 
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace SuperImageEvolver {
    public enum NBTType : byte {
        End,
        Byte,
        Short,
        Int,
        Long,
        Float,
        Double,
        Bytes,
        String,

        Bool,
        Color,
        Point,
        PointF,

        List,
        Compound
    }


    public class NBTag : IEnumerable<NBTag> {
        protected NBTType Type { get; set; }
        protected string Name { get; set; }
        object Payload { get; set; }
        NBTag Parent { get; set; }


        #region Constructors

        protected NBTag() {}


        public NBTag( NBTType type, NBTag parent ) {
            Type = type;
            Parent = parent;
        }


        public NBTag( NBTType type, string name, object payload, NBTag parent ) {
            Type = type;
            Name = name;
            Payload = payload;
            Parent = parent;
        }

        #endregion


        #region Shorthand Contructors

        public NBTag Append( NBTag tag ) {
            if( tag == null ) throw new ArgumentNullException( "tag" );
            if( tag == this ) throw new InvalidOperationException( "Cannot append tag to itself." );
            if( !( this is NBTCompound ) ) {
                throw new InvalidOperationException( "Can only append tags to compound tags." );
            }
            tag.Parent = this;
            this[tag.Name] = tag;
            return tag;
        }


        public NBTag Append( string name, byte value ) {
            return Append( new NBTag( NBTType.Byte, name, value, this ) );
        }


        public NBTag Append( string name, short value ) {
            return Append( new NBTag( NBTType.Short, name, value, this ) );
        }


        public NBTag Append( string name, int value ) {
            return Append( new NBTag( NBTType.Int, name, value, this ) );
        }


        public NBTag Append( string name, long value ) {
            return Append( new NBTag( NBTType.Long, name, value, this ) );
        }


        public NBTag Append( string name, float value ) {
            return Append( new NBTag( NBTType.Float, name, value, this ) );
        }


        public NBTag Append( string name, double value ) {
            return Append( new NBTag( NBTType.Double, name, value, this ) );
        }


        public NBTag Append( string name, byte[] value ) {
            if( value == null ) throw new ArgumentNullException( "value" );
            return Append( new NBTag( NBTType.Bytes, name, value, this ) );
        }


        public NBTag Append( string name, string value ) {
            if( value == null ) throw new ArgumentNullException( "value" );
            return Append( new NBTag( NBTType.String, name, value, this ) );
        }


        public NBTag Append( string name, bool value ) {
            return Append( new NBTag( NBTType.Bool, name, value, this ) );
        }


        public NBTag Append( string name, Color value ) {
            return Append( new NBTag( NBTType.Color, name, value, this ) );
        }


        public NBTag Append( string name, Point value ) {
            return Append( new NBTag( NBTType.Point, name, value, this ) );
        }


        public NBTag Append( string name, PointF value ) {
            return Append( new NBTag( NBTType.PointF, name, value, this ) );
        }


        public NBTag Append( string name, params NBTag[] tags ) {
            if( tags == null ) throw new ArgumentNullException( "tags" );
            NBTCompound compound = new NBTCompound( name );
            foreach( NBTag tag in tags ) {
                compound.Tags.Add( tag.Name, tag );
            }
            return Append( compound );
        }


        public NBTag Append( params NBTag[] tags ) {
            foreach( NBTag tag in tags ) {
                Append( tag );
            }
            return this;
        }

        #endregion


        #region Child Tag Manipulation

        public bool Contains( string name ) {
            if( this is NBTCompound ) {
                return ( (NBTCompound)this ).Tags.ContainsKey( name );
            } else {
                return false;
            }
        }


        public NBTag Remove( string name ) {
            if( this is NBTCompound ) {
                NBTag tag = ( this )[name];
                ( (NBTCompound)this ).Tags.Remove( name );
                return tag;
            } else {
                throw new NotSupportedException( "Can only Remove() from compound tags." );
            }
        }


        public NBTag Remove() {
            if( Parent is NBTCompound ) {
                Parent.Remove( Name );
                return this;
            } else {
                throw new NotSupportedException( "Cannot Remove() - no parent tag." );
            }
        }

        #endregion


        #region Loading

        public static NBTCompound ReadFile( string fileName ) {
            using( FileStream fs = File.OpenRead( fileName ) ) {
                using( GZipStream gs = new GZipStream( fs, CompressionMode.Decompress ) ) {
                    return ReadStream( gs );
                }
            }
        }


        public static NBTCompound ReadStream( Stream stream ) {
            BinaryReader reader = new BinaryReader( stream );
            return (NBTCompound)ReadTag( reader, (NBTType)reader.ReadByte(), null, null );
        }


        public static NBTag ReadTag( BinaryReader reader, NBTType type, string name, NBTag parent ) {
            if( name == null && type != NBTType.End ) {
                name = ReadString( reader );
            }
            switch( type ) {
                case NBTType.End:
                    return new NBTag( NBTType.End, parent );

                case NBTType.Byte:
                    return new NBTag( NBTType.Byte, name, reader.ReadByte(), parent );

                case NBTType.Short:
                    return new NBTag( NBTType.Short, name, reader.ReadInt16(), parent );

                case NBTType.Int:
                    return new NBTag( NBTType.Int, name, reader.ReadInt32(), parent );

                case NBTType.Long:
                    return new NBTag( NBTType.Long, name, reader.ReadInt64(), parent );

                case NBTType.Float:
                    return new NBTag( NBTType.Float, name, reader.ReadSingle(), parent );

                case NBTType.Double:
                    return new NBTag( NBTType.Double, name, reader.ReadDouble(), parent );

                case NBTType.Bytes:
                    int bytesLength = reader.ReadInt32();
                    return new NBTag( NBTType.Bytes, name, reader.ReadBytes( bytesLength ), parent );

                case NBTType.String:
                    return new NBTag( NBTType.String, name, ReadString( reader ), parent );


                case NBTType.Bool:
                    return new NBTag( NBTType.Bool, name, reader.ReadBoolean(), parent );

                case NBTType.Color:
                    return new NBTag( NBTType.Color, name, Color.FromArgb( reader.ReadInt32() ), parent );

                case NBTType.Point:
                    int iX = reader.ReadInt32();
                    int iY = reader.ReadInt32();
                    return new NBTag( NBTType.Point, name, new Point( iX, iY ), parent );

                case NBTType.PointF:
                    float fX = reader.ReadSingle();
                    float fY = reader.ReadSingle();
                    return new NBTag( NBTType.PointF, name, new PointF( fX, fY ), parent );


                case NBTType.List:
                    NBTList list = new NBTList {
                        Type = NBTType.List,
                        Name = name,
                        Parent = parent,
                        ListType = (NBTType)reader.ReadByte()
                    };
                    int listLength = reader.ReadInt32();
                    list.Tags = new NBTag[listLength];
                    for( int i = 0; i < listLength; i++ ) {
                        list.Tags[i] = ReadTag( reader, list.ListType, "", list );
                    }
                    return list;


                case NBTType.Compound:
                    NBTCompound compound = new NBTCompound( name ) {
                        Parent = parent
                    };
                    while( true ) {
                        NBTag childTag = ReadTag( reader, (NBTType)reader.ReadByte(), null, compound );
                        if( childTag.Type == NBTType.End ) break;
                        if( childTag.Name == null )
                            continue;
                        if( compound.Tags.ContainsKey( childTag.Name ) ) {
                            throw new IOException(
                                "NBT parsing error: null names and duplicate names are not allowed within a compound tags." );
                        } else {
                            compound.Tags.Add( childTag.Name, childTag );
                        }
                    }
                    return compound;

                default:
                    throw new IOException( "NBT parsing error: unknown tag type." );
            }
        }


        public static string ReadString( BinaryReader reader ) {
            short stringLength = reader.ReadInt16();
            return Encoding.UTF8.GetString( reader.ReadBytes( stringLength ) );
        }

        #endregion


        #region Saving

        public void WriteTag( string fileName ) {
            using( FileStream fs = File.OpenWrite( fileName ) ) {
                using( GZipStream gs = new GZipStream( fs, CompressionMode.Compress ) ) {
                    WriteTag( gs );
                }
            }
        }


        public void WriteTag( Stream stream ) {
            using( BinaryWriter writer = new BinaryWriter( stream ) ) {
                WriteTag( writer, true );
            }
        }


        public void WriteTag( BinaryWriter writer ) {
            WriteTag( writer, true );
        }


        public void WriteTag( BinaryWriter writer, bool writeType ) {
            if( writeType ) writer.Write( (byte)Type );
            if( Type == NBTType.End ) return;
            if( writeType ) WriteString( Name, writer );
            switch( Type ) {
                case NBTType.Byte:
                    writer.Write( (byte)Payload );
                    return;

                case NBTType.Short:
                    writer.Write( (short)Payload );
                    return;

                case NBTType.Int:
                    writer.Write( (int)Payload );
                    return;

                case NBTType.Long:
                    writer.Write( (long)Payload );
                    return;

                case NBTType.Float:
                    writer.Write( (float)Payload );
                    return;

                case NBTType.Double:
                    writer.Write( (double)Payload );
                    return;

                case NBTType.Bytes:
                    writer.Write( ( (byte[])Payload ).Length );
                    writer.Write( (byte[])Payload );
                    return;

                case NBTType.String:
                    WriteString( (string)Payload, writer );
                    return;

                case NBTType.Bool:
                    writer.Write( (bool)Payload );
                    return;

                case NBTType.Color:
                    writer.Write( ( (Color)Payload ).ToArgb() );
                    return;

                case NBTType.Point:
                    writer.Write( ( (Point)Payload ).X );
                    writer.Write( ( (Point)Payload ).Y );
                    break;

                case NBTType.PointF:
                    writer.Write( ( (PointF)Payload ).X );
                    writer.Write( ( (PointF)Payload ).Y );
                    break;


                case NBTType.List:
                    NBTList list = (NBTList)this;
                    writer.Write( (byte)list.ListType );
                    writer.Write( list.Tags.Length );

                    for( int i = 0; i < list.Tags.Length; i++ ) {
                        list.Tags[i].WriteTag( writer, false );
                    }
                    return;


                case NBTType.Compound:
                    foreach( NBTag tag in this ) {
                        tag.WriteTag( writer, true );
                    }
                    writer.Write( (byte)NBTType.End );
                    return;

                default:
                    return;
            }
        }


        public static void WriteString( string str, BinaryWriter writer ) {
            byte[] stringBytes = Encoding.UTF8.GetBytes( str );
            writer.Write( (short)stringBytes.Length );
            writer.Write( stringBytes );
        }

        #endregion


        #region Accessors

        public byte GetByte() {
            return (byte)Payload;
        }


        public short GetShort() {
            return (short)Payload;
        }


        public int GetInt() {
            return (int)Payload;
        }


        public long GetLong() {
            return (long)Payload;
        }


        public float GetFloat() {
            return (float)Payload;
        }


        public double GetDouble() {
            return (double)Payload;
        }


        public byte[] GetBytes() {
            return (byte[])Payload;
        }


        public string GetString() {
            return (string)Payload;
        }


        public bool GetBool() {
            return (bool)Payload;
        }


        public Color GetColor() {
            return (Color)Payload;
        }


        public Point GetPoint() {
            return (Point)Payload;
        }


        public PointF GetPointF() {
            return (PointF)Payload;
        }


        public void Set( object payload ) {
            Payload = payload;
        }


        object GetChild( string name, object defaultValue ) {
            return Contains( name ) ? this[name].Payload : defaultValue;
        }


        public byte GetByte( string name, byte defaultValue ) {
            return (byte)GetChild( name, defaultValue );
        }


        public short GetShort( string name, short defaultValue ) {
            return (short)GetChild( name, defaultValue );
        }


        public int GetInt( string name, int defaultValue ) {
            return (int)GetChild( name, defaultValue );
        }


        public long GetLong( string name, long defaultValue ) {
            return (long)GetChild( name, defaultValue );
        }


        public float GetFloat( string name, float defaultValue ) {
            return (float)GetChild( name, defaultValue );
        }


        public double GetDouble( string name, double defaultValue ) {
            return (double)GetChild( name, defaultValue );
        }


        public byte[] GetBytes( string name, byte[] defaultValue ) {
            return (byte[])GetChild( name, defaultValue );
        }


        public string GetString( string name, string defaultValue ) {
            return (string)GetChild( name, defaultValue );
        }


        public bool GetBool( string name, bool defaultValue ) {
            return (bool)GetChild( name, defaultValue );
        }


        public Color GetColor( string name, Color defaultValue ) {
            return (Color)GetChild( name, defaultValue );
        }


        public Point GetPoint( string name, Point defaultValue ) {
            return (Point)GetChild( name, defaultValue );
        }


        public PointF GetPointF( string name, PointF defaultValue ) {
            return (PointF)GetChild( name, defaultValue );
        }

        #endregion


        #region Indexers

        public NBTag this[ int index ] {
            get {
                if( this is NBTList ) {
                    return ( (NBTList)this ).Tags[index];
                } else {
                    throw new NotSupportedException();
                }
            }
            set {
                if( this is NBTList ) {
                    ( (NBTList)this ).Tags[index] = value;
                } else {
                    throw new NotSupportedException();
                }
            }
        }


        public NBTag this[ string key ] {
            get {
                if( this is NBTCompound ) {
                    return ( (NBTCompound)this ).Tags[key];
                } else {
                    throw new NotSupportedException();
                }
            }
            set {
                if( this is NBTCompound ) {
                    ( (NBTCompound)this ).Tags[key] = value;
                } else {
                    throw new NotSupportedException();
                }
            }
        }

        #endregion


        #region Enumerators

        public IEnumerator<NBTag> GetEnumerator() {
            return new NBTEnumerator( this );
        }


        IEnumerator IEnumerable.GetEnumerator() {
            return new NBTEnumerator( this );
        }


        public sealed class NBTEnumerator : IEnumerator<NBTag> {
            readonly NBTag[] tags;
            int index = -1;


            public NBTEnumerator( NBTag tag ) {
                if( tag is NBTCompound ) {
                    tags = new NBTag[( (NBTCompound)tag ).Tags.Count];
                    ( (NBTCompound)tag ).Tags.Values.CopyTo( tags, 0 );
                } else if( tag is NBTList ) {
                    tags = ( (NBTList)tag ).Tags;
                } else {
                    tags = new NBTag[0];
                }
            }


            public NBTag Current {
                get { return tags[index]; }
            }

            object IEnumerator.Current {
                get { return tags[index]; }
            }


            public bool MoveNext() {
                if( index < tags.Length ) index++;
                return index < tags.Length;
            }


            public void Reset() {
                index = -1;
            }


            public void Dispose() {}
        }

        #endregion


        #region ToString

        public string GetFullName() {
            string fullName = ToString();
            NBTag tag = this;
            while( tag.Parent != null ) {
                tag = tag.Parent;
                fullName = tag + "." + fullName;
            }
            return fullName;
        }


        public string GetIndentedName() {
            string fullName = ToString();
            NBTag tag = this;
            while( tag.Parent != null ) {
                tag = tag.Parent;
                fullName = "    " + fullName;
            }
            return fullName;
        }


        public override string ToString() {
            switch( Type ) {
                case NBTType.Bytes:
                    return String.Format( "{0}[{1}] {2}", Type, GetBytes().Length, Name );
                case NBTType.List:
                    NBTList list = (NBTList)this;
                    return String.Format( "{0}<{1}>[{2}] {3}", Type, list.ListType, list.Tags.Length, Name );
                case NBTType.Compound:
                    NBTCompound comp = (NBTCompound)this;
                    return String.Format( "{0}[{1}] {2}", Type, comp.Tags.Count, Name );
                case NBTType.String:
                    if( Payload != null ) {
                        return String.Format( "{0} {1} = \"{2}\"", Type, Name, Payload );
                    } else {
                        return String.Format( "{0} {1} = null", Type, Name );
                    }
                default:
                    return String.Format( "{0} {1} = {2}", Type, Name, Payload );
            }
        }


        public string ToString( bool recursive ) {
            string output = GetIndentedName() + Environment.NewLine;
            NBTList thisList = this as NBTList;
            if( thisList == null ||
                thisList.ListType == NBTType.Compound ||
                thisList.ListType == NBTType.Bytes ||
                thisList.ListType == NBTType.List ) {
                foreach( NBTag tag in this ) {
                    output += tag.ToString( recursive );
                }
            }
            return output;
        }

        #endregion
    }


    public sealed class NBTList : NBTag {
        public NBTList() {
            Type = NBTType.List;
        }


        public NBTList( string name, NBTType type, int count ) {
            Name = name;
            Type = NBTType.List;
            ListType = type;
            Tags = new NBTag[count];
        }


        public NBTList( string name, NBTType type, Array payloads ) {
            if( payloads == null ) throw new ArgumentNullException( "payloads" );
            Name = name;
            Type = NBTType.List;
            ListType = type;
            if( ListType == NBTType.Compound ) {
                if( !( payloads is NBTCompound[] ) )
                    throw new ArgumentException( "Expected NBTCompound[]", "payloads" );
                Tags = (NBTCompound[])payloads.Clone();
            } else if( ListType == NBTType.List ) {
                if( !( payloads is NBTList[] ) ) throw new ArgumentException( "Expected NBTList[]", "payloads" );
                Tags = (NBTList[])payloads.Clone();
            } else {
                Tags = new NBTag[payloads.Length];
                for( int i = 0; i < Tags.Length; i++ ) {
                    Tags[i] = new NBTag( ListType, null, payloads.GetValue( i ), this );
                }
            }
        }


        public NBTag[] Tags;
        public NBTType ListType;
    }


    public sealed class NBTCompound : NBTag {
        public NBTCompound( string name ) {
            Type = NBTType.Compound;
            Name = name;
        }


        public readonly Dictionary<string, NBTag> Tags = new Dictionary<string, NBTag>();
    }
}