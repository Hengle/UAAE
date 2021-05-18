using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AssetsTools.NET;
using AssetsTools.NET.Extra;

namespace AssetsAdvancedEditor.Assets
{
    public class AssetImporter
    {
        public StreamReader Reader;
        public AssetsFileWriter Writer;

        public AssetsReplacer ImportRawAsset(AssetDetailsListItem item, FileStream fs)
        {
            var ms = new MemoryStream();
            fs.CopyTo(ms);
            return AssetModifier.CreateAssetReplacer(item, ms.ToArray());
        }

        public AssetsReplacer ImportDump(AssetWorkspace workspace, AssetDetailsListItem listItem, StreamReader reader, DumpType dumpType)
        {
            using var ms = new MemoryStream();
            Reader = reader;
            Writer = new AssetsFileWriter(ms);
            try
            {
                switch (dumpType)
                {
                    case DumpType.TXT:
                        ImportTextDumpLoop(workspace);
                        break;
                    case DumpType.XML:
                        // todo
                        break;
                    case DumpType.JSON:
                        // todo
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
            return AssetModifier.CreateAssetReplacer(listItem, ms.ToArray());
        }

        private void ImportTextDumpLoop(AssetWorkspace workspace)
        {
            var error = "";
            var cldb = workspace.Am.classFile;
            var alignStack = new Stack<bool>();

            var line = Reader.ReadLine();
            var assetType = line[2..line.LastIndexOf(' ')];
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
                    type = type.Remove(type.IndexOf(' '));

                    var fieldName = line[(typeName + type.Length + 1)..];
                    fieldName = fieldName.Remove(fieldName.IndexOf(' '));
                    //if (cldbType != null)
                    //{

                    //    var valid = cldbType.fields.Any(f =>
                    //        f.typeName.GetString(cldb) == type &&
                    //        f.fieldName.GetString(cldb) == fieldName &&
                    //        f.depth == thisDepth);

                    //    if (!valid)
                    //        error += $"This asset does not contain a field \"{fieldName}\" of type \"{type}\"\n";
                    //}

                    var success = WriteData(type, valueStr);

                    if (!success)
                        error += $"An error occurred while writing the value \"{valueStr}\" of type \"{type}\"\n";

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
    }
}
