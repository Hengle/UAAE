using System;
using System.IO;
using AssetsAdvancedEditor.Utils;
using AssetsTools.NET;
using AssetsTools.NET.Extra;

namespace AssetsAdvancedEditor.Assets
{
    public class AssetExporter
    {
        public AssetsWorkspace Workspace;
        public StreamWriter Writer;

        public AssetExporter(AssetsWorkspace workspace) => Workspace = workspace;

        public void ExportRawAsset(string path, AssetItem item)
        {
            var cont = Workspace.GetAssetContainer(item.FileID, item.PathID);
            var br = cont.FileReader;
            br.Position = item.Position;
            var data = br.ReadBytes((int)item.Size);
            File.WriteAllBytes(path, data);
        }

        public void ExportDump(string path, AssetItem item, DumpType dumpType)
        {
            using var fs = File.OpenWrite(path);
            using var writer = new StreamWriter(fs);
            ExportDump(writer, item, dumpType);
        }

        public void ExportDump(StreamWriter writer, AssetItem item, DumpType dumpType)
        {
            Writer = writer;
            try
            {
                var field = Workspace.GetBaseField(item);
                switch (dumpType)
                {
                    case DumpType.TXT:
                        RecurseTextDump(field);
                        break;
                    case DumpType.XML:
                        RecurseXmlDump();
                        break;
                    case DumpType.JSON:
                        RecurseJsonDump();
                        break;
                    default:
                        return;
                }
            }
            catch (Exception ex)
            {
                MsgBoxUtils.ShowErrorDialog("Something went wrong when writing the dump file.\n" + ex);
            }
        }

        private void RecurseTextDump(AssetTypeValueField field, int depth = 0)
        {
            var template = field.GetTemplateField();
            var align = template.align ? "1" : "0";
            var typeName = template.type;
            var fieldName = template.name;
            var isArray = template.isArray;

            //string's field isn't aligned but its array is
            if (template.valueType == EnumValueTypes.String)
                align = "1";

            //mainly to handle enum fields not having the int type name
            if (template.valueType != EnumValueTypes.None &&
                template.valueType != EnumValueTypes.Array &&
                template.valueType != EnumValueTypes.ByteArray)
            {
                typeName = CorrectTypeName(template.valueType);
            }

            if (isArray)
            {
                var sizeTemplate = template.children[0];
                var sizeAlign = sizeTemplate.align ? "1" : "0";
                var sizeTypeName = sizeTemplate.type;
                var sizeFieldName = sizeTemplate.name;
                var size = field.GetValue().AsArray().size;
                Writer.WriteLine($"{new string(' ', depth)}{align} {typeName} {fieldName} ({size} {(size != 1 ? "items" : "item")})");
                Writer.WriteLine($"{new string(' ', depth + 1)}{sizeAlign} {sizeTypeName} {sizeFieldName} = {size}");
                for (var i = 0; i < field.childrenCount; i++)
                {
                    Writer.WriteLine($"{new string(' ', depth + 1)}[{i}]");
                    RecurseTextDump(field.children[i], depth + 2);
                }
            }
            else
            {
                var value = "";
                if (field.GetValue() != null)
                {
                    var evt = field.GetValue().GetValueType();
                    if (evt == EnumValueTypes.String)
                    {
                        //only replace \ with \\ but not " with \" lol
                        //you just have to find the last "
                        var fixedStr = field.GetValue().AsString()
                            .Replace("\\", "\\\\")
                            .Replace("\r", "\\r")
                            .Replace("\n", "\\n");
                        value = $" = \"{fixedStr}\"";
                    }
                    else if (1 <= (int)evt && (int)evt <= 12)
                    {
                        value = $" = {field.GetValue().AsString()}";
                    }
                }
                Writer.WriteLine($"{new string(' ', depth)}{align} {typeName} {fieldName}{value}");

                for (var i = 0; i < field.childrenCount; i++)
                {
                    RecurseTextDump(field.children[i], depth + 1);
                }
            }
        }

        private static string CorrectTypeName(EnumValueTypes valueType)
        {
            return valueType switch
            {
                EnumValueTypes.Bool => "bool",
                EnumValueTypes.UInt8 => "UInt8",
                EnumValueTypes.Int8 => "SInt8",
                EnumValueTypes.UInt16 => "UInt16",
                EnumValueTypes.Int16 => "Int16",
                EnumValueTypes.UInt32 => "unsigned int",
                EnumValueTypes.Int32 => "int",
                EnumValueTypes.UInt64 => "UInt64",
                EnumValueTypes.Int64 => "SInt64",
                EnumValueTypes.Float => "float",
                EnumValueTypes.Double => "double",
                EnumValueTypes.String => "string",
                _ => "UnknownBaseType"
            };
        }

        private void RecurseXmlDump()
        {
            // todo
        }

        private void RecurseJsonDump()
        {
            // todo
        }
    }
}
