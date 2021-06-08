using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using AssetsAdvancedEditor.Utils;
using AssetsTools.NET;
using AssetsTools.NET.Extra;

namespace AssetsAdvancedEditor.Assets
{
    public class AssetImporter
    {
        public AssetsWorkspace Workspace;
        public StreamReader Reader;
        public AssetsFileWriter Writer;

        public AssetImporter(AssetsWorkspace workspace) => Workspace = workspace;

        public AssetsReplacer ImportRawAsset(string path, AssetItem item)
        {
            return AssetModifier.CreateAssetReplacer(item, File.ReadAllBytes(path));
        }

        public AssetsReplacer ImportDump(string path, AssetItem item, DumpType dumpType)
        {
            using var fs = File.OpenRead(path);
            using var reader = new StreamReader(fs);
            return ImportDump(reader, item, dumpType);
        }

        public AssetsReplacer ImportDump(StreamReader reader, AssetItem item, DumpType dumpType)
        {
            using var ms = new MemoryStream();
            Reader = reader;
            Writer = new AssetsFileWriter(ms)
            {
                bigEndian = false
            };
            try
            {
                switch (dumpType)
                {
                    case DumpType.TXT:
                        ImportTxtDumpLoop();
                        break;
                    case DumpType.XML:
                        ImportXmlDumpLoop();
                        break;
                    case DumpType.JSON:
                        ImportJsonDumpLoop();
                        break;
                    default:
                        return null;
                }
            }
            catch (Exception ex)
            {
                MsgBoxUtils.ShowErrorDialog("Something went wrong when reading the dump file:\n" + ex);
                return null;
            }
            return AssetModifier.CreateAssetReplacer(item, ms.ToArray());
        }

        private void ImportTxtDumpLoop()
        {
            var error = "";
            var cldb = Workspace.Am.classFile;
            var i = 2;
            var alignStack = new Stack<bool>();

            var line = Reader.ReadLine();
            var assetType = line?[2..line.LastIndexOf(' ')];
            var cldbType = AssetHelper.FindAssetClassByName(cldb, assetType);

            if (cldbType == null)
                error += $"Invalid or unknown asset type: \"{assetType}\"\n";

            while (true)
            {
                line = Reader.ReadLine();
                if (line == null) break;

                var thisDepth = 0;
                while (line[thisDepth] == ' ')
                    thisDepth++;

                if (line[thisDepth] == '[') // array index, ignore
                    continue;

                if (thisDepth < alignStack.Count)
                {
                    while (thisDepth < alignStack.Count)
                    {
                        if (alignStack.Pop())
                            Writer.Align();
                    }
                }

                var align = line.Substring(thisDepth, 1) == "1";
                var typeName = thisDepth + 2;
                var eqSign = line.IndexOf('=');
                var valueStr = line[(eqSign + 1)..].Trim();

                if (eqSign != -1)
                {
                    var type = line[typeName..];
                    type = type.StartsWith("unsigned") ?
                        type.Split()[0] + ' ' + type.Split()[1] : 
                        type.Split()[0];

                    var success = WriteData(valueStr);

                    if (!success)
                        error += $"An error occurred while writing the value \"{valueStr}\" of type \"{type}\".\n";

                    if (align)
                        Writer.Align();
                }
                else
                {
                    alignStack.Push(align);
                }
                i++;
            }

            if (error != "")
                throw new Exception(error);
        }

        private bool WriteData(object value)
        {
            try
            {
                switch (value)
                {
                    case bool asBool:
                        Writer.Write(asBool);
                        break;
                    case sbyte asInt8:
                        Writer.Write(asInt8);
                        break;
                    case byte asUInt8:
                        Writer.Write(asUInt8);
                        break;
                    case short asInt16:
                        Writer.Write(asInt16);
                        break;
                    case ushort asUInt16:
                        Writer.Write(asUInt16);
                        break;
                    case int asInt32:
                        Writer.Write(asInt32);
                        break;
                    case uint asUInt32:
                        Writer.Write(asUInt32);
                        break;
                    case long asInt64:
                        Writer.Write(asInt64);
                        break;
                    case ulong asUInt64:
                        Writer.Write(asUInt64);
                        break;
                    case float asFloat:
                        Writer.Write(asFloat);
                        break;
                    case double asDouble:
                        Writer.Write(asDouble);
                        break;
                    case string asString:
                        {
                            var firstQuote = asString.IndexOf('"');
                            var lastQuote = asString.LastIndexOf('"');
                            var valueStrFix = asString[(firstQuote + 1)..(lastQuote - firstQuote)];
                            valueStrFix = valueStrFix
                                .Replace("\\r", "\r")
                                .Replace("\\n", "\n");
                            Writer.WriteCountStringInt32(valueStrFix);
                            break;
                        }
                    default:
                        return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void ImportXmlDumpLoop()
        {
            // todo
        }

        private void ImportJsonDumpLoop()
        {
            // todo
        }
    }
}
