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
            var fileInst = Workspace.LoadedFiles[item.FileID];
            var assetId = new AssetID(fileInst.path, item.PathID);
            var br = fileInst.file.reader;
            byte[] data;
            if (Workspace.ModifiedAssets.ContainsKey(assetId))
            {
                data = Workspace.ModifiedAssets[assetId].Data.ToArray();
            }
            else
            {
                br.Position = item.Position;
                data = br.ReadBytes((int)item.Size);
            }
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
