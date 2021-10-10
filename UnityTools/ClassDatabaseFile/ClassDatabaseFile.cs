using System.Collections.Generic;
using System.IO;
using SevenZip.Compression.LZMA;
using UnityTools.Compression.LZ4;

namespace UnityTools
{
    public class ClassDatabaseFile
    {
        public bool valid;
        public ClassDatabaseFileHeader header;

        public List<ClassDatabaseType> classes;

        public byte[] stringTable;

        public bool Read(AssetsFileReader reader)
        {
            header = new ClassDatabaseFileHeader();
            header.Read(reader);
            if (header.header != "cldb" || header.fileVersion > 4 || header.fileVersion < 1)
            {
                valid = false;
                return valid;
            }
            classes = new List<ClassDatabaseType>();

            var classTablePos = reader.Position;
            var newReader = reader;
            if (header.compressionType != 0)
            {
                classTablePos = 0;
                MemoryStream ms;
                if (header.compressionType == 1) //lz4
                {
                    var uncompressedBytes = new byte[header.uncompressedSize];
                    using (var tempMs = new MemoryStream(reader.ReadBytes((int)header.compressedSize)))
                    {
                        var decoder = new Lz4DecoderStream(tempMs);
                        decoder.Read(uncompressedBytes, 0, (int)header.uncompressedSize);
                        decoder.Dispose();
                    }
                    ms = new MemoryStream(uncompressedBytes);
                }
                else if (header.compressionType == 2) //lzma
                {
                    using var tempMs = new MemoryStream(reader.ReadBytes((int)header.compressedSize));
                    ms = SevenZipHelper.StreamDecompress(tempMs);
                }
                else
                {
                    valid = false;
                    return valid;
                }

                newReader = new AssetsFileReader(ms)
                {
                    BigEndian = false
                };
            }

            newReader.Position = header.stringTablePos;
            stringTable = newReader.ReadBytes((int)header.stringTableLen);
            newReader.Position = classTablePos;
            var size = newReader.ReadUInt32();
            for (var i = 0; i < size; i++)
            {
                var cdt = new ClassDatabaseType();
                cdt.Read(newReader, header.fileVersion, header.flags);
                classes.Add(cdt);
            }
            valid = true;
            return valid;
        }

        public void Write(AssetsFileWriter writer)
        {
            header.Write(writer);
            writer.Write(classes.Count);
            foreach (var @class in classes)
            {
                @class.Write(writer, header.fileVersion, header.flags);
            }
            var stringTablePos = writer.Position;
            writer.Write(stringTable);
            var stringTableLen = writer.Position - stringTablePos;
            var fileSize = writer.Position;
            header.stringTablePos = (uint)stringTablePos;
            header.stringTableLen = (uint)stringTableLen;
            header.uncompressedSize = (uint)fileSize;
            writer.Position = 0;
            header.Write(writer);
        }

        public bool IsValid() => valid;
    }
}
