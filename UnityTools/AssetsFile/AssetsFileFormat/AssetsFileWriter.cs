﻿using System;
using System.IO;
using System.Text;

namespace UnityTools
{
    public class AssetsFileWriter : BinaryWriter
    {
        //todo, this should default to BigEndian = false
        //since it's more likely little endian than big endian
        public bool BigEndian = true;
        public AssetsFileWriter(string path) : base(File.Open(path, FileMode.Create, FileAccess.Write)) { }
        public AssetsFileWriter(FileStream fs) : base(fs) { }
        public AssetsFileWriter(MemoryStream ms) : base(ms) { }
        public AssetsFileWriter(Stream stream) : base(stream) { }
        
        public override void Write(short val)
        {
            unchecked
            {
                if (BigEndian)
                    base.Write((short)ReverseShort((ushort)val));
                else
                    base.Write(val);
            }
        }

        public override void Write(ushort val)
        {
            base.Write(BigEndian ? ReverseShort(val) : val);
        }

        public override void Write(int val)
        {
            unchecked
            {
                if (BigEndian)
                    base.Write((int)ReverseInt((uint)val));
                else
                    base.Write(val);
            }
        }

        public override void Write(uint val)
        {
            base.Write(BigEndian ? ReverseInt(val) : val);
        }

        public override void Write(long val)
        {
            unchecked
            {
                if (BigEndian)
                    base.Write((long)ReverseLong((ulong)val));
                else
                    base.Write(val);
            }
        }

        public override void Write(ulong val)
        {
            base.Write(BigEndian ? ReverseLong(val) : val);
        }

        public override void Write(string val)
        {
            base.Write(Encoding.UTF8.GetBytes(val));
        }

        public void WriteUInt24(uint val)
        {
            if (BigEndian)
                base.Write(BitConverter.GetBytes(ReverseInt(val)), 1, 3);
            else
                base.Write(BitConverter.GetBytes(val), 0, 3);
        }

        public void WriteInt24(int val)
        {
            unchecked
            {
                if (BigEndian)
                    base.Write(BitConverter.GetBytes((int)ReverseInt((uint)val)), 1, 3);
                else
                    base.Write(BitConverter.GetBytes(val), 0, 3);
            }
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

        public void Align()
        {
            while (Position % 4 != 0) Write((byte)0x00);
        }

        public void Align8()
        {
            while (Position % 8 != 0) Write((byte)0x00);
        }

        public void Align16()
        {
            while (Position % 16 != 0) Write((byte)0x00);
        }

        public void WriteNullTerminated(string text)
        {
            Write(text);
            Write((byte)0x00);
        }

        public void WriteCountString(string text)
        {
            if (Encoding.UTF8.GetByteCount(text) > 0xFF)
                throw new Exception("String is longer than 255! Use the Int32 variant instead!");
            Write((byte)Encoding.UTF8.GetByteCount(text));
            Write(text);
        }

        public void WriteCountStringInt16(string text)
        {
            if (Encoding.UTF8.GetByteCount(text) > 0xFFFF)
                throw new Exception("String is longer than 65535! Use the Int32 variant instead!");
            Write((ushort)Encoding.UTF8.GetByteCount(text));
            Write(text);
        }

        public void WriteCountStringInt32(string text)
        {
            Write(Encoding.UTF8.GetByteCount(text));
            Write(text);
        }

        public long Position
        {
            get => BaseStream.Position;
            set => BaseStream.Position = value;
        }

    }
}
