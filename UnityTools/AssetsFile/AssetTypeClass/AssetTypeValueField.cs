using System.Collections.Generic;

namespace UnityTools
{
    public class AssetTypeValueField
    {
        public AssetTypeTemplateField TemplateField;

        public int ChildrenCount;
        public List<AssetTypeValueField> Children;
        public AssetTypeValue Value;

        public void Read(AssetTypeValue value, AssetTypeTemplateField template, List<AssetTypeValueField> children)
        {
            TemplateField = template;
            ChildrenCount = children.Count;
            Children = children;
            Value = value;
        }
        ///public ulong Write(AssetsFileWriter writer, FileStream writerPar, ulong filePos);

        public AssetTypeValueField this[string name]
        {
            get
            {
                foreach (var valueField in Children)
                {
                    if (valueField.TemplateField.name == name)
                    {
                        return valueField;
                    }
                }
                return AssetTypeInstance.GetDummyAssetTypeField();
            }
        }

        public AssetTypeValueField this[int index] => Children[index];

        public AssetTypeValueField Get(string name) => this[name];
        public AssetTypeValueField Get(int index) => this[index];

        public string GetName() => TemplateField.name;
        public string GetFieldType() => TemplateField.type;
        public void SetChildrenList(List<AssetTypeValueField> children)
        {
            Children = children;
            ChildrenCount = children.Count;
        }

        public bool IsDummy() => ChildrenCount == -1;

        ///public ulong GetByteSize(ulong filePos = 0);

        public static EnumValueTypes GetValueTypeByTypeName(string type)
        {
            switch (type.ToLower())
            {
                case "string":
                    return EnumValueTypes.String;
                case "sint8":
                case "sbyte":
                    return EnumValueTypes.Int8;
                case "uint8":
                case "char":
                case "byte":
                    return EnumValueTypes.UInt8;
                case "sint16":
                case "short":
                    return EnumValueTypes.Int16;
                case "uint16":
                case "unsigned short":
                case "ushort":
                    return EnumValueTypes.UInt16;
                case "sint32":
                case "int":
                    return EnumValueTypes.Int32;
                case "type*":
                    return EnumValueTypes.Int32;
                case "uint32":
                case "unsigned int":
                case "uint":
                    return EnumValueTypes.UInt32;
                case "sint64":
                case "long":
                    return EnumValueTypes.Int64;
                case "uint64":
                case "unsigned long":
                case "ulong":
                case "filesize":
                    return EnumValueTypes.UInt64;
                case "single":
                case "float":
                    return EnumValueTypes.Float;
                case "double":
                    return EnumValueTypes.Double;
                case "bool":
                    return EnumValueTypes.Bool;
                default:
                    return EnumValueTypes.None;
            }
        }
    }
}
