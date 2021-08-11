using UnityTools;
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
                var ttType = classId == 0x72 ?
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
            var reader = bundle.reader;
            reader.Position = bundle.bundleHeader6.GetBundleInfoOffset();
            MemoryStream blocksInfoStream;
            var compressedSize = (int)bundle.bundleHeader6.compressedSize;
            byte[] uncompressedBytes;
            switch (bundle.bundleHeader6.GetCompressionType())
            {
                case 1:
                {
                    uncompressedBytes = new byte[bundle.bundleHeader6.decompressedSize];
                    using (var ms = new MemoryStream(reader.ReadBytes(compressedSize)))
                    {
                        var decoder = SevenZipHelper.StreamDecompress(ms, compressedSize);
                        decoder.Read(uncompressedBytes, 0, (int)bundle.bundleHeader6.decompressedSize);
                        decoder.Dispose();
                    }
                    blocksInfoStream = new MemoryStream(uncompressedBytes);
                    break;
                }
                case 2:
                case 3:
                {
                    uncompressedBytes = new byte[bundle.bundleHeader6.decompressedSize];
                    using (var ms = new MemoryStream(reader.ReadBytes(compressedSize)))
                    {
                        var decoder = new Lz4DecoderStream(ms);
                        decoder.Read(uncompressedBytes, 0, (int)bundle.bundleHeader6.decompressedSize);
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

            var uncompressedInf = bundle.bundleInf6;
            if (bundle.bundleHeader6.GetCompressionType() != 0)
            {
                using var memReader = new AssetsFileReader(blocksInfoStream)
                {
                    Position = 0
                };
                uncompressedInf = new AssetBundleBlockAndDirectoryList06();
                uncompressedInf.Read(0, memReader);
            }

            return uncompressedInf.blockInf.Any(inf => inf.GetCompressionType() != 0);
        }
    }
}
