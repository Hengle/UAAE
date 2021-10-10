using UnityTools.Compression.LZ4;
using System.IO;
using SevenZip.Compression.LZMA;

namespace UnityTools
{
    public class ClassDatabasePackage
    {
        public bool valid;

        public ClassDatabasePackageHeader header;
        public ClassDatabaseFile[] files;
        public byte[] stringTable;

        public bool Read(AssetsFileReader reader)
        {
            header = new ClassDatabasePackageHeader();
            header.Read(reader);
            files = new ClassDatabaseFile[header.fileCount];
            var firstFile = reader.Position;
            var newReader = reader;
            if ((header.compressionType & 0x80) == 0) //multiple blocks
            {
                //untested!
                //the compression is handled by the cldbs themselves
                for (var i = 0; i < header.fileCount; i++)
                {
                    newReader.Position = firstFile + header.files[i].offset;
                    var data = newReader.ReadBytes((int)header.files[i].length);
                    using var ms = new MemoryStream(data);
                    using var r = new AssetsFileReader(ms);
                    files[i] = new ClassDatabaseFile();
                    files[i].Read(r);
                }
            }
            else
            {
                if ((header.compressionType & 0x20) == 0) //not uncompressed
                {
                    firstFile = 0;
                    var compressedSize = (int)(header.stringTableOffset - newReader.Position);
                    var uncompressedSize = (int)header.fileBlockSize;
                    MemoryStream ms;
                    if ((header.compressionType & 0x1f) == 1) //lz4
                    {
                        var uncompressedBytes = new byte[uncompressedSize];
                        using (var tempMs = new MemoryStream(newReader.ReadBytes(compressedSize)))
                        {
                            var decoder = new Lz4DecoderStream(tempMs);
                            decoder.Read(uncompressedBytes, 0, uncompressedSize);
                            decoder.Dispose();
                        }
                        ms = new MemoryStream(uncompressedBytes);
                    }
                    else if ((header.compressionType & 0x1f) == 2) //lzma
                    {
                        var dbg = newReader.ReadBytes(compressedSize);
                        using var tempMs = new MemoryStream(dbg);
                        ms = SevenZipHelper.StreamDecompress(tempMs, uncompressedSize);
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
                for (var i = 0; i < header.fileCount; i++)
                {
                    newReader.Position = firstFile + header.files[i].offset;
                    var data = newReader.ReadBytes((int)header.files[i].length);
                    using var ms = new MemoryStream(data);
                    using var r = new AssetsFileReader(ms);
                    files[i] = new ClassDatabaseFile();
                    files[i].Read(r);
                }
            }

            newReader = reader;
            newReader.Position = header.stringTableOffset;
            if ((header.compressionType & 0x40) == 0) //string table is compressed
            {
                if ((header.compressionType & 0x20) == 0) //not uncompressed
                {
                    var compressedSize = (int)header.stringTableLenCompressed;
                    var uncompressedSize = (int)header.stringTableLenUncompressed;
                    MemoryStream ms;
                    switch (header.compressionType & 0x1f)
                    {
                        //lz4
                        case 1:
                        {
                            var uncompressedBytes = new byte[uncompressedSize];
                            using (var tempMs = new MemoryStream(newReader.ReadBytes(compressedSize)))
                            {
                                var decoder = new Lz4DecoderStream(tempMs);
                                decoder.Read(uncompressedBytes, 0, uncompressedSize);
                                decoder.Dispose();
                            }
                            ms = new MemoryStream(uncompressedBytes);
                            break;
                        }
                        //lzma
                        case 2:
                        {
                            using var tempMs = new MemoryStream(newReader.ReadBytes(compressedSize));
                            ms = SevenZipHelper.StreamDecompress(tempMs, uncompressedSize);
                            break;
                        }
                        default:
                            valid = false;
                            return valid;
                    }

                    newReader = new AssetsFileReader(ms);
                    newReader.BigEndian = false;
                }
            }
            stringTable = newReader.ReadBytes((int)header.stringTableLenUncompressed);
            for (var i = 0; i < header.fileCount; i++)
            {
                files[i].stringTable = stringTable;
            }

            valid = true;
            return valid;
        }
    }
}
