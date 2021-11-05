﻿using UnityTools;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using AssetsAdvancedEditor.Assets;
using SevenZip.Compression.LZMA;
using UnityTools.Compression.LZ4;

namespace AssetsAdvancedEditor.Utils
{
    public static class Extensions
    {
        public static string ReplaceInvalidFileNameChars(string filename)
        {
            return string.Join("_", filename.Split(Path.GetInvalidFileNameChars()));
        }

        public static void GetAssetItemFast(int fileId, AssetsFileInstance fileInst, AssetsWorkspace workspace, AssetFileInfoEx info, out AssetItem item)
        {
            var file = fileInst.file;
            var classId = info.curFileType;
            var cldb = workspace.Am.classFile;
            var cldbType = AssetHelper.FindAssetClassByID(cldb, classId);
            var reader = file.reader;
            string name;
            string type;
            var scriptIndex = AssetHelper.GetScriptIndex(file, info);

            const string container = "";
            var typeId = info.curFileType;
            var pathId = info.index;
            const string modified = "";
            var monoId = ushort.MaxValue;

            if (typeId is AssetClassID.MonoBehaviour)
            {
                monoId = (ushort)(uint.MaxValue - info.curFileTypeOrIndex);
            }

            item = new AssetItem
            {
                ListName = "Unnamed asset",
                Container = container,
                TypeID = typeId,
                FileID = fileId,
                PathID = pathId,
                Size = info.curFileSize,
                Modified = modified,
                Position = info.absoluteFilePos,
                MonoID = monoId
            };

            if (file.typeTree.hasTypeTree)
            {
                var ttType = classId is AssetClassID.MonoBehaviour ?
                    AssetHelper.FindTypeTreeTypeByScriptIndex(file.typeTree, scriptIndex) :
                    AssetHelper.FindTypeTreeTypeByID(file.typeTree, classId);

                type = ttType.Children[0].GetTypeString(ttType.stringTable);
                item.Type = type;
                switch (ttType.Children.Length)
                {
                    case > 1 when ttType.Children[1].GetNameString(ttType.stringTable) == "m_Name":
                    {
                        reader.Position = item.Position;
                        name = reader.ReadCountStringInt32();
                        item.Name = name;
                        if (name != "")
                        {
                            item.ListName = name;
                            return;
                        }
                        break;
                    }
                    default:
                    {
                        switch (type)
                        {
                            case "GameObject":
                            {
                                reader.Position = item.Position;
                                var size = reader.ReadInt32();
                                var componentSize = file.header.Version > 0x10 ? 0x0c : 0x10;
                                reader.Position += size * componentSize;
                                reader.Position += 0x04;
                                name = reader.ReadCountStringInt32();
                                item.Name = name;
                                if (name != "")
                                {
                                    item.ListName = $"{type} {name}";
                                }
                                return;
                            }
                            case "MonoBehaviour":
                            {
                                reader.Position = item.Position;
                                reader.Position += 0x1c;
                                name = reader.ReadCountStringInt32();
                                item.Name = name;
                                if (name != "")
                                {
                                    item.ListName = $"{type} {name}";
                                }
                                return;
                            }
                        }
                        break;
                    }
                }
                return;
            }

            if (cldbType == null)
            {
                item.Type = $"0x{classId:X8}";
                return;
            }

            type = cldbType.name.GetString(cldb);
            item.Type = type;
            switch (cldbType.fields.Count)
            {
                case 0:
                {
                    return;
                }
                case > 1 when cldbType.fields[1].fieldName.GetString(cldb) == "m_Name":
                {
                    reader.Position = item.Position;
                    name = reader.ReadCountStringInt32();
                    item.Name = name;
                    if (name != "")
                    {
                        item.ListName = name;
                    }
                    break;
                }
                default:
                {
                    switch (type)
                    {
                        case "GameObject":
                        {
                            reader.Position = item.Position;
                            var size = reader.ReadInt32();
                            var componentSize = file.header.Version > 0x10 ? 0x0c : 0x10;
                            reader.Position += size * componentSize;
                            reader.Position += 0x04;
                            name = reader.ReadCountStringInt32();
                            item.Name = name;
                            if (name != "")
                            {
                                item.ListName = $"{type} {name}";
                            }
                            return;
                        }
                        case "MonoBehaviour":
                        {
                            reader.Position = item.Position;
                            reader.Position += 0x1c;
                            name = reader.ReadCountStringInt32();
                            item.Name = name;
                            if (name != "")
                            {
                                item.ListName = $"{type} {name}";
                            }
                            return;
                        }
                    }
                    break;
                }
            }
        }

        public static void GetAssetNameFast(ClassDatabaseFile cldb, AssetItem item, out string type, out string listName, out string name)
        {
            var cont = item.Cont;
            var file = cont.FileInstance.file;
            var classId = item.TypeID;
            var cldbType = AssetHelper.FindAssetClassByID(cldb, classId);
            var reader = cont.FileReader;
            name = "";
            listName = "Unnamed asset";

            if (file.typeTree.hasTypeTree)
            {
                var ttType = classId is AssetClassID.MonoBehaviour ?
                    AssetHelper.FindTypeTreeTypeByScriptIndex(file.typeTree, item.MonoID) :
                    AssetHelper.FindTypeTreeTypeByID(file.typeTree, classId);

                type = ttType.Children[0].GetTypeString(ttType.stringTable);
                switch (ttType.Children.Length)
                {
                    case > 1 when ttType.Children[1].GetNameString(ttType.stringTable) == "m_Name":
                    {
                        reader.Position = item.Position;
                        name = reader.ReadCountStringInt32();
                        if (name != "")
                        {
                            listName = name;
                            return;
                        }
                        break;
                    }
                    default:
                    {
                        switch (type)
                        {
                            case "GameObject":
                            {
                                reader.Position = item.Position;
                                var size = reader.ReadInt32();
                                var componentSize = file.header.Version > 0x10 ? 0x0c : 0x10;
                                reader.Position += size * componentSize;
                                reader.Position += 0x04;
                                name = reader.ReadCountStringInt32();
                                if (name != "")
                                {
                                    listName = $"{type} {name}";
                                }
                                return;
                            }
                            case "MonoBehaviour":
                            {
                                reader.Position = item.Position;
                                reader.Position += 0x1c;
                                name = reader.ReadCountStringInt32();
                                if (name != "")
                                {
                                    listName = $"{type} {name}";
                                }
                                return;
                            }
                        }
                        break;
                    }
                }
                return;
            }

            if (cldbType == null)
            {
                type = $"0x{classId:X8}";
                return;
            }

            type = cldbType.name.GetString(cldb);
            switch (cldbType.fields.Count)
            {
                case 0:
                {
                    return;
                }
                case > 1 when cldbType.fields[1].fieldName.GetString(cldb) == "m_Name":
                {
                    reader.Position = item.Position;
                    name = reader.ReadCountStringInt32();
                    if (name != "")
                    {
                        listName = name;
                    }
                    break;
                }
                default:
                {
                    switch (type)
                    {
                        case "GameObject":
                        {
                            reader.Position = item.Position;
                            var size = reader.ReadInt32();
                            var componentSize = file.header.Version > 0x10 ? 0x0c : 0x10;
                            reader.Position += size * componentSize;
                            reader.Position += 0x04;
                            name = reader.ReadCountStringInt32();
                            if (name != "")
                            {
                                listName = $"{type} {name}";
                            }
                            return;
                        }
                        case "MonoBehaviour":
                        {
                            reader.Position = item.Position;
                            reader.Position += 0x1c;
                            name = reader.ReadCountStringInt32();
                            if (name != "")
                            {
                                listName = $"{type} {name}";
                            }
                            return;
                        }
                    }
                    break;
                }
            }
        }

        public static bool WildcardMatches(string test, string pattern, bool caseSensitive = true)
        {
            RegexOptions options = 0;
            if (!caseSensitive)
                options |= RegexOptions.IgnoreCase;

            return Regex.IsMatch(test, "^" + Regex.Escape(pattern).Replace("\\*", ".*") + "$", options);
        }

        public static bool IsBundleDataCompressed(this AssetBundleFile bundle)
        {
            var reader = bundle.Reader;
            reader.Position = bundle.Header.GetBundleInfoOffset();
            MemoryStream blocksInfoStream;
            var compressedSize = (int)bundle.Header.CompressedSize;
            byte[] uncompressedBytes;
            switch (bundle.Header.GetCompressionType())
            {
                case AssetBundleCompressionType.Lzma:
                {
                    uncompressedBytes = new byte[bundle.Header.DecompressedSize];
                    using (var ms = new MemoryStream(reader.ReadBytes(compressedSize)))
                    {
                        var decoder = SevenZipHelper.StreamDecompress(ms, compressedSize);
                        decoder.Read(uncompressedBytes, 0, (int)bundle.Header.DecompressedSize);
                        decoder.Dispose();
                    }
                    blocksInfoStream = new MemoryStream(uncompressedBytes);
                    break;
                }
                case AssetBundleCompressionType.Lz4:
                case AssetBundleCompressionType.Lz4HC:
                {
                    uncompressedBytes = new byte[bundle.Header.DecompressedSize];
                    using (var ms = new MemoryStream(reader.ReadBytes(compressedSize)))
                    {
                        var decoder = new Lz4DecoderStream(ms);
                        decoder.Read(uncompressedBytes, 0, (int)bundle.Header.DecompressedSize);
                        decoder.Dispose();
                    }
                    blocksInfoStream = new MemoryStream(uncompressedBytes);
                    break;
                }
                default:
                {
                    blocksInfoStream = null;
                    break;
                }
            }

            var uncompressedMetadata = bundle.Metadata;
            if (bundle.Header.GetCompressionType() != 0)
            {
                using var memReader = new AssetsFileReader(blocksInfoStream)
                {
                    Position = 0
                };
                uncompressedMetadata = new AssetBundleMetadata();
                uncompressedMetadata.Read(0, memReader, bundle.Header);
            }

            return uncompressedMetadata.BlocksInfo.Any(inf => inf.GetCompressionType() != 0);
        }
    }
}
