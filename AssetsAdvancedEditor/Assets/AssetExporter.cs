using System;
using System.IO;
using AssetsTools.NET;

namespace AssetsAdvancedEditor.Assets
{
    public class AssetExporter
    {
        public StreamWriter writer;

        public void ExportRawAsset(AssetsFile file, AssetDetailsListItem item, FileStream fs)
        {
            var br = file.reader;
            br.Position = item.Position;
            var data = br.ReadBytes(item.Size);
            fs.Write(data);
        }

        public void ExportDump(AssetTypeValueField baseField, StreamWriter writer, DumpType dumpType)
        {
            this.writer = writer;
            try
            {
                switch (dumpType)
                {
                    case DumpType.TXT:
                        RecurseTextDump(baseField);
                        break;
                    case DumpType.XML:
                        // todo
                        break;
                    case DumpType.JSON:
                        // todo
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

            if (isArray)
            {
                var sizeTemplate = template.children[0];
                var sizeAlign = sizeTemplate.align ? "1" : "0";
                var sizeTypeName = sizeTemplate.type;
                var sizeFieldName = sizeTemplate.name;
                var size = field.GetValue().AsArray().size;
                writer.WriteLine($"{new string(' ', depth)}{align} {typeName} {fieldName} ({size} {(size != 1 ? "items" : "item")})");
                writer.WriteLine($"{new string(' ', depth + 1)}{sizeAlign} {sizeTypeName} {sizeFieldName} = {size}");
                for (var i = 0; i < field.childrenCount; i++)
                {
                    writer.WriteLine($"{new string(' ', depth + 1)}[{i}]");
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
                writer.WriteLine($"{new string(' ', depth)}{align} {typeName} {fieldName}{value}");

                for (var i = 0; i < field.childrenCount; i++)
                {
                    RecurseTextDump(field.children[i], depth + 1);
                }
            }
        }
    }
}
