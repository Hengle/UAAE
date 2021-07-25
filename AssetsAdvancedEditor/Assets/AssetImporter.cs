﻿using System;
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
        public string Path;

        public AssetImporter(AssetsWorkspace workspace) => Workspace = workspace;

        public AssetsReplacer ImportRawAsset(string path, AssetItem item)
        {
            return AssetModifier.CreateAssetReplacer(item, File.ReadAllBytes(path));
        }

        public AssetsReplacer ImportDump(string path, AssetItem item, DumpType dumpType)
        {
            Path = path;
            using var ms = new MemoryStream();
            Writer = new AssetsFileWriter(ms)
            {
                bigEndian = false
            };
            try
            {
                switch (dumpType)
                {
                    case DumpType.TXT:
                    {
                        using var fs = File.OpenRead(path);
                        using var reader = new StreamReader(fs);
                        Reader = reader;
                        ImportTxtDumpLoop();
                        break;
                    }
                    case DumpType.XML:
                    {
                        ImportXmlDumpLoop();
                        break;
                    }
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

                    var success = WriteData(type, valueStr);

                    if (!success)
                        error += $"An error occurred while writing the value \"{valueStr}\" of type \"{type}\".\n";

                    if (align)
                        Writer.Align();
                }
                else
                {
                    alignStack.Push(align);
                }
            }

            if (error != "")
                throw new Exception(error);
        }

        private bool WriteData(string type, string value)
        {
            try
            {
                var valueType = AssetTypeValueField.GetValueTypeByTypeName(type);
                switch (valueType)
                {
                    case EnumValueTypes.Bool:
                        Writer.Write(bool.Parse(value));
                        break;
                    case EnumValueTypes.Int8:
                        Writer.Write(sbyte.Parse(value));
                        break;
                    case EnumValueTypes.UInt8:
                        Writer.Write(byte.Parse(value));
                        break;
                    case EnumValueTypes.Int16:
                        Writer.Write(short.Parse(value));
                        break;
                    case EnumValueTypes.UInt16:
                        Writer.Write(ushort.Parse(value));
                        break;
                    case EnumValueTypes.Int32:
                        Writer.Write(int.Parse(value));
                        break;
                    case EnumValueTypes.UInt32:
                        Writer.Write(uint.Parse(value));
                        break;
                    case EnumValueTypes.Int64:
                        Writer.Write(long.Parse(value));
                        break;
                    case EnumValueTypes.UInt64:
                        Writer.Write(ulong.Parse(value));
                        break;
                    case EnumValueTypes.Float:
                        Writer.Write(float.Parse(value));
                        break;
                    case EnumValueTypes.Double:
                        Writer.Write(double.Parse(value));
                        break;
                    case EnumValueTypes.String:
                        {
                            var firstQuote = value.IndexOf('"');
                            var lastQuote = value.LastIndexOf('"');
                            var valueStrFix = value[(firstQuote + 1)..(lastQuote - firstQuote)];
                            valueStrFix = valueStrFix
                                .Replace("\\\\", "\\")
                                .Replace("\\r", "\r")
                                .Replace("\\n", "\n");
                            Writer.WriteCountStringInt32(valueStrFix);
                            break;
                        }
                    case EnumValueTypes.None:
                    case EnumValueTypes.Array:
                    case EnumValueTypes.ByteArray:
                        return false;
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
