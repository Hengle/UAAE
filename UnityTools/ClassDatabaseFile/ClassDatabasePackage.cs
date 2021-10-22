﻿using System;
using System.Collections.Generic;
using UnityTools.Compression.LZ4;
using System.IO;
using System.Text;
using SevenZip.Compression.LZMA;

namespace UnityTools
{
    public class ClassDatabasePackage
    {
        public bool valid;

        public ClassDatabasePackageHeader header;
        public List<ClassDatabaseFile> files;
        public byte[] stringTable;

        public bool Read(AssetsFileReader reader)
        {
            header = new ClassDatabasePackageHeader();
            header.Read(reader);
            files = new List<ClassDatabaseFile>();
            var firstFile = reader.Position;
            var newReader = reader;
            if ((header.compressionType & 0x80) == 0) //multiple compressed blocks
            {
                //untested!
                //the compression is handled by the cldbs themselves
                for (var i = 0; i < header.fileCount; i++)
                {
                    newReader.Position = firstFile + header.files[i].offset;
                    var data = newReader.ReadBytes((int)header.files[i].length);
                    using var ms = new MemoryStream(data);
                    using var r = new AssetsFileReader(ms);
                    var file = new ClassDatabaseFile();
                    file.Read(r);
                    files.Add(file);
                }
            }
            else //one compressed block
            {
                if ((header.compressionType & 0x20) == 0) //file block compressed
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
                    var file = new ClassDatabaseFile();
                    file.Read(r);
                    files.Add(file);
                }
            }

            newReader = reader;
            newReader.Position = header.stringTableOffset;
            if ((header.compressionType & 0x40) == 0) //string table is compressed
            {
                var compressedSize = (int)header.stringTableLenCompressed;
                var uncompressedSize = (int)header.stringTableLenUncompressed;
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
                    using var tempMs = new MemoryStream(newReader.ReadBytes(compressedSize));
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
            stringTable = newReader.ReadBytes((int)header.stringTableLenUncompressed);
            for (var i = 0; i < header.fileCount; i++)
            {
                files[i].stringTable = stringTable;
            }

            valid = true;
            return valid;
        }

        public void Write(AssetsFileWriter writer, int optimizeStringTable = 1, int compress = 1)
        {
            var filePos = writer.BaseStream.Position;

            //lol don't do this if compress is 0
            if ((compress & 0x80) == 0)
            {
                throw new NotImplementedException("Compression flag 0x80 must be used");
            }

            //compress 1 for lz4 and 2 for lzma
            //this is backwards from assets files

            //build string table
            var strTableBuilder = new StringBuilder();
            Dictionary<string, uint> strTableMap;
            strTableMap = optimizeStringTable != 0 ? new Dictionary<string, uint>() : null;

            foreach (var cldb in files)
            {
                foreach (var type in cldb.classes)
                {
                    AddStringTableEntry(cldb, strTableBuilder, strTableMap, ref type.name);

                    if (header.fileVersion == 4 && (cldb.header.flags & 1) != 0)
                    {
                        AddStringTableEntry(cldb, strTableBuilder, strTableMap, ref type.assemblyFileName);
                    }

                    var fields = type.fields;
                    for (var j = 0; j < fields.Count; j++)
                    {
                        var field = fields[j];
                        AddStringTableEntry(cldb, strTableBuilder, strTableMap, ref field.fieldName);
                        AddStringTableEntry(cldb, strTableBuilder, strTableMap, ref field.typeName);
                        fields[j] = field;
                    }
                }
            }

            header.fileCount = (uint)files.Count;
            header.Write(writer);

            using (var cldbMs = new MemoryStream())
            using (var cldbWriter = new AssetsFileWriter(cldbMs))
            {
                //annoyingly, files and header.files are two different lists...
                for (var i = 0; i < files.Count; i++)
                {
                    var cldb = files[i];
                    var cldbStartFilePos = cldbWriter.BaseStream.Position;

                    //does not support 0x80 self compression rn
                    cldb.Write(cldbWriter, 0, 0, false);
                    var cldbEndFilePos = cldbWriter.BaseStream.Position;

                    var cldbName = header.files[i].name;
                    header.files[i] = new ClassDatabaseFileRef()
                    {
                        offset = (uint)cldbStartFilePos,
                        length = (uint)(cldbEndFilePos - cldbStartFilePos),
                        name = cldbName
                    };
                }

                header.fileBlockSize = (uint)cldbMs.Length;

                cldbMs.Position = 0;
                if ((compress & 0x20) == 0) //compressed
                {
                    if ((compress & 0x1f) == 1) //lz4
                    {
                        var compressedBlock = LZ4Codec.Encode32HC(cldbMs.ToArray(), 0, (int)cldbMs.Length);
                        writer.Write(compressedBlock);
                    }
                    else if ((compress & 0x1f) == 2) //lzma
                    {
                        var compressedBlock = SevenZipHelper.Compress(cldbMs.ToArray());
                        writer.Write(compressedBlock);
                    }
                    else
                    {
                        throw new ArgumentException("File marked as compressed but no valid compression option set!");
                    }
                }
                else //write normally
                {
                    cldbMs.CopyToCompat(writer.BaseStream);
                }
            }

            header.stringTableOffset = (uint)writer.Position;

            var stringTableBytes = Encoding.ASCII.GetBytes(strTableBuilder.ToString());

            header.stringTableLenUncompressed = (uint)stringTableBytes.Length;

            if ((compress & 0x40) == 0) //string table is compressed
            {
                if ((compress & 0x1f) == 1) //lz4
                {
                    stringTableBytes = LZ4Codec.Encode32HC(stringTableBytes, 0, stringTableBytes.Length);
                }
                else if ((compress & 0x1f) == 2) //lzma
                {
                    stringTableBytes = SevenZipHelper.Compress(stringTableBytes);
                }
                else
                {
                    throw new ArgumentException("File marked as compressed but no valid compression option set!");
                }
            }

            header.stringTableLenCompressed = (uint)stringTableBytes.Length;

            writer.Write(stringTableBytes);

            writer.Position = filePos;
            header.compressionType = (byte)compress;
            header.Write(writer);
        }

        private void AddStringTableEntry(ClassDatabaseFile cldb, StringBuilder strTable, Dictionary<string, uint> strMap, ref ClassDatabaseFileString str)
        {
            var stringValue = str.GetString(cldb);

            if (strTable != null)
            {
                //search for string first and use that index if possible
                if (!strMap.ContainsKey(stringValue))
                {
                    strMap[stringValue] = (uint)strTable.Length;
                    strTable.Append(stringValue + '\0');
                }
                str.str.stringTableOffset = strMap[stringValue];
            }
            else
            {
                //always add string
                str.str.stringTableOffset = (uint)strTable.Length;
                strTable.Append(stringValue + '\0');
            }
        }

        public bool RemoveFile(int index)
        {
            if (files.Count < index)
            {
                files.RemoveAt(index);
                header.files.RemoveAt(index);
                return true;
            }
            return false;
        }

        public bool ImportFile(AssetsFileReader reader)
        {
            var cldb = new ClassDatabaseFile();
            var valid = cldb.Read(reader);
            if (valid)
            {
                files.Add(cldb);
                header.files.Add(new ClassDatabaseFileRef()
                {
                    offset = 0,
                    length = 0,
                    name = ""
                });
                return true;
            }
            return false;
        }
    }
}
