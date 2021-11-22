using System.IO;

namespace UnityTools
{
    //To stay close to the original API, these methods will stay apart from their classes
    //If the original API ever adds these methods into their classes, these will be moved back
    public static class AssetWriters
    {
        //AssetTypeInstance
        public static void Write(this AssetTypeInstance instance, AssetsFileWriter writer)
        {
            for (var i = 0; i < instance.BaseFieldCount; i++)
            {
                instance.BaseFields[i].Write(writer);
            }
        }
        public static byte[] WriteToByteArray(this AssetTypeInstance instance, bool bigEndian = false)
        {
            byte[] data;
            using var ms = new MemoryStream();
            using var w = new AssetsFileWriter(ms);
            w.BigEndian = bigEndian;
            instance.Write(w);
            data = ms.ToArray();
            return data;
        }
        //AssetTypeValueField
        public static void Write(this AssetTypeValueField valueField, AssetsFileWriter writer, int depth = 0)
        {
            if (valueField.TemplateField.isArray)
            {
                if (valueField.TemplateField.valueType == EnumValueTypes.ByteArray)
                {
                    var byteArray = valueField.GetValue().value.asByteArray;

                    byteArray.size = (uint)byteArray.data.Length;
                    writer.Write(byteArray.size);
                    writer.Write(byteArray.data);
                    if (valueField.TemplateField.align) writer.Align();
                }
                else
                {
                    var array = valueField.GetValue().value.asArray;

                    array.size = valueField.ChildrenCount;
                    writer.Write(array.size);
                    for (var i = 0; i < array.size; i++)
                    {
                        valueField[i].Write(writer, depth + 1);
                    }
                    if (valueField.TemplateField.align) writer.Align();
                }
            }
            else
            {
                if (valueField.ChildrenCount == 0)
                {
                    switch (valueField.TemplateField.valueType)
                    {
                        case EnumValueTypes.Int8:
                            writer.Write(valueField.GetValue().value.asInt8);
                            if (valueField.TemplateField.align) writer.Align();
                            break;
                        case EnumValueTypes.UInt8:
                            writer.Write(valueField.GetValue().value.asUInt8);
                            if (valueField.TemplateField.align) writer.Align();
                            break;
                        case EnumValueTypes.Bool:
                            writer.Write(valueField.GetValue().value.asBool);
                            if (valueField.TemplateField.align) writer.Align();
                            break;
                        case EnumValueTypes.Int16:
                            writer.Write(valueField.GetValue().value.asInt16);
                            if (valueField.TemplateField.align) writer.Align();
                            break;
                        case EnumValueTypes.UInt16:
                            writer.Write(valueField.GetValue().value.asUInt16);
                            if (valueField.TemplateField.align) writer.Align();
                            break;
                        case EnumValueTypes.Int32:
                            writer.Write(valueField.GetValue().value.asInt32);
                            break;
                        case EnumValueTypes.UInt32:
                            writer.Write(valueField.GetValue().value.asUInt32);
                            break;
                        case EnumValueTypes.Int64:
                            writer.Write(valueField.GetValue().value.asInt64);
                            break;
                        case EnumValueTypes.UInt64:
                            writer.Write(valueField.GetValue().value.asUInt64);
                            break;
                        case EnumValueTypes.Float:
                            writer.Write(valueField.GetValue().value.asFloat);
                            break;
                        case EnumValueTypes.Double:
                            writer.Write(valueField.GetValue().value.asDouble);
                            break;
                        case EnumValueTypes.String:
                            writer.Write(valueField.GetValue().value.asString.Length);
                            writer.Write(valueField.GetValue().value.asString);
                            writer.Align();
                            break;
                    }
                }
                else
                {
                    for (var i = 0; i < valueField.ChildrenCount; i++)
                    {
                        valueField[i].Write(writer, depth + 1);
                    }
                    if (valueField.TemplateField.align) writer.Align();
                }
            }
        }
        public static byte[] WriteToByteArray(this AssetTypeValueField valueField, bool bigEndian = false)
        {
            byte[] data;
            using var ms = new MemoryStream();
            using var w = new AssetsFileWriter(ms);
            w.BigEndian = bigEndian;
            valueField.Write(w);
            data = ms.ToArray();
            return data;
        }
    }
}
