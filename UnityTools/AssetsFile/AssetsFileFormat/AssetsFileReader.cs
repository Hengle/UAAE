using System.IO;
using System.Text;

namespace UnityTools
{
    public class AssetsFileReader : BinaryReader
    {
        #region AssetsFile
        // todo
        #endregion

        #region AssetBundleFile
        public AssetBundleHeader Header;
        #endregion

        //todo, this should default to BigEndian = false
        //since it's more likely little endian than big endian
        public bool BigEndian = true;
        public AssetsFileReader(Stream stream) : base(stream) { }

        public override short ReadInt16()
        {
            unchecked
            {
                return BigEndian ? (short)ReverseShort((ushort)base.ReadInt16()) : base.ReadInt16();
            }
        }

        public override ushort ReadUInt16()
        {
            return BigEndian ? ReverseShort(base.ReadUInt16()) : base.ReadUInt16();
        }

        public int ReadInt24()
        {
            unchecked
            {
                return (int)BufferToUInt24(ReadBytes(3));
            }
        }

        public uint ReadUInt24()
        {
            return BufferToUInt24(ReadBytes(3));
        }

        public override int ReadInt32()
        {
            unchecked
            {
                return BigEndian ? (int)ReverseInt((uint)base.ReadInt32()) : base.ReadInt32();
            }
        }

        public override uint ReadUInt32()
        {
            return BigEndian ? ReverseInt(base.ReadUInt32()) : base.ReadUInt32();
        }

        public override long ReadInt64()
        {
            unchecked
            {
                return BigEndian ? (long)ReverseLong((ulong)base.ReadInt64()) : base.ReadInt64();
            }
        }

        public override ulong ReadUInt64()
        {
            return BigEndian ? ReverseLong(base.ReadUInt64()) : base.ReadUInt64();
        }

        public ushort ReverseShort(ushort value)
        {
            return (ushort)(((value & 0xFF00) >> 8) | (value & 0x00FF) << 8);
        }

        public uint ReverseInt(uint value)
        {
            value = (value >> 16) | (value << 16);
            return ((value & 0xFF00FF00) >> 8) | ((value & 0x00FF00FF) << 8);
        }

        public ulong ReverseLong(ulong value)
        {
            value = (value >> 32) | (value << 32);
            value = ((value & 0xFFFF0000FFFF0000) >> 16) | ((value & 0x0000FFFF0000FFFF) << 16);
            return ((value & 0xFF00FF00FF00FF00) >> 8) | ((value & 0x00FF00FF00FF00FF) << 8);
        }

        protected uint BufferToUInt24(byte[] buffer, int offset = 0)
        {
            return BigEndian ?
                unchecked((uint)(buffer[offset + 2] | (buffer[offset + 1] << 8) | (buffer[offset] << 16))) :
                unchecked((uint)(buffer[offset] | (buffer[offset + 1] << 8) | (buffer[offset + 2] << 16)));
        }

        public void Align()
        {
            Position = (Position + 3) & ~3;
        }

        public void Align8()
        {
            Position = (Position + 7) & ~7;
        }

        public void Align16()
        {
            Position = (Position + 15) & ~15;
        }

        public string ReadStringLength(int len)
        {
            return Encoding.UTF8.GetString(ReadBytes(len));
        }

        public string ReadNullTerminated()
        {
            var output = "";
            char curChar;
            while ((curChar = ReadChar()) != 0x00)
            {
                output += curChar;
            }
            return output;
        }

        public static string ReadNullTerminatedArray(byte[] bytes, uint pos)
        {
            var output = new StringBuilder();
            char curChar;
            while ((curChar = (char)bytes[pos]) != 0x00)
            {
                output.Append(curChar);
                pos++;
            }
            return output.ToString();
        }

        public string ReadCountString()
        {
            var length = ReadByte();
            return ReadStringLength(length);
        }

        public string ReadCountStringInt16()
        {
            var length = ReadUInt16();
            return ReadStringLength(length);
        }

        public string ReadCountStringInt32()
        {
            var length = ReadInt32();
            return ReadStringLength(length);
        }

        public long Position
        {
            get => BaseStream.Position;
            set => BaseStream.Position = value;
        }
    }
}
