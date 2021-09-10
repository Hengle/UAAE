using System;
using System.Collections.Generic;

namespace UnityTools
{
    public class AssetTypeTemplateField
    {
        public string name;
        public string type;
        public EnumValueTypes valueType;
        public bool isArray;
        public bool align;
        public bool hasValue;
        public int childrenCount;
        public AssetTypeTemplateField[] children;

        ///public AssetTypeTemplateField()
        ///public void Clear()
        public bool From0D(Type_0D u5Type, int fieldIndex)
        {
            var field = u5Type.Children[fieldIndex];
            name = field.GetNameString(u5Type.stringTable);
            type = field.GetTypeString(u5Type.stringTable);
            valueType = AssetTypeValueField.GetValueTypeByTypeName(type);
            isArray = field.IsArray;
            align = (field.MetaFlag & 0x4000) != 0x00;
            hasValue = valueType != EnumValueTypes.None;

            var childrenIndexes = new List<int>();
            int thisDepth = u5Type.Children[fieldIndex].Level;
            for (var i = fieldIndex + 1; i < u5Type.ChildrenCount; i++)
            {
                if (u5Type.Children[i].Level == thisDepth + 1)
                {
                    childrenCount++;
                    childrenIndexes.Add(i);
                }
                if (u5Type.Children[i].Level <= thisDepth) break;
            }
            children = new AssetTypeTemplateField[childrenCount];
            var child = 0;
            for (var i = fieldIndex + 1; i < u5Type.ChildrenCount; i++)
            {
                if (u5Type.Children[i].Level == thisDepth + 1)
                {
                    children[child] = new AssetTypeTemplateField();
                    children[child].From0D(u5Type, childrenIndexes[child]);
                    child++;
                }
                if (u5Type.Children[i].Level <= thisDepth) break;
            }
            return true;
        }
        public bool FromClassDatabase(ClassDatabaseFile file, ClassDatabaseType type, uint fieldIndex)
        {
            var field = type.fields[(int)fieldIndex];
            name = field.fieldName.GetString(file);
            this.type = field.typeName.GetString(file);
            valueType = AssetTypeValueField.GetValueTypeByTypeName(this.type);
            isArray = field.isArray is 1;
            align = (field.flags2 & 0x4000) != 0x00;
            hasValue = valueType != EnumValueTypes.None;

            var childrenIndexes = new List<int>();
            int thisDepth = type.fields[(int)fieldIndex].depth;
            for (var i = (int)fieldIndex + 1; i < type.fields.Count; i++)
            {
                if (type.fields[i].depth == thisDepth + 1)
                {
                    childrenCount++;
                    childrenIndexes.Add(i);
                }
                if (type.fields[i].depth <= thisDepth) break;
            }
            children = new AssetTypeTemplateField[childrenCount];
            var child = 0;
            for (var i = (int)fieldIndex + 1; i < type.fields.Count; i++)
            {
                if (type.fields[i].depth == thisDepth + 1)
                {
                    children[child] = new AssetTypeTemplateField();
                    children[child].FromClassDatabase(file, type, (uint)childrenIndexes[child]);
                    child++;
                }
                if (type.fields[i].depth <= thisDepth) break;
            }
            return true;
        }
        ///public bool From07(TypeField_07 typeField)
        public AssetTypeValueField MakeValue(AssetsFileReader reader)
        {
            var valueField = new AssetTypeValueField
            {
                templateField = this
            };
            valueField = ReadType(reader, valueField);
            return valueField;
        }

        public AssetTypeValueField ReadType(AssetsFileReader reader, AssetTypeValueField valueField)
        {
            if (valueField.templateField.isArray)
            {
                if (valueField.templateField.childrenCount == 2)
                {
                    var sizeType = valueField.templateField.children[0].valueType;
                    if (sizeType is EnumValueTypes.Int32 or EnumValueTypes.UInt32)
                    {
                        if (valueField.templateField.valueType == EnumValueTypes.ByteArray)
                        {
                            valueField.childrenCount = 0;
                            valueField.children = new AssetTypeValueField[0];
                            var size = reader.ReadInt32();
                            var data = reader.ReadBytes(size);
                            if (valueField.templateField.align) reader.Align();
                            var byteArray = new AssetTypeByteArray
                            {
                                size = (uint)size,
                                data = data
                            };
                            valueField.value = new AssetTypeValue(EnumValueTypes.ByteArray, byteArray);
                        }
                        else
                        {
                            valueField.childrenCount = reader.ReadInt32();
                            valueField.children = new AssetTypeValueField[valueField.childrenCount];
                            for (var i = 0; i < valueField.childrenCount; i++)
                            {
                                valueField.children[i] = new AssetTypeValueField
                                {
                                    templateField = valueField.templateField.children[1]
                                };
                                valueField.children[i] = ReadType(reader, valueField.children[i]);
                            }
                            if (valueField.templateField.align) reader.Align();
                            var assetArray = new AssetTypeArray
                            {
                                size = valueField.childrenCount
                            };
                            valueField.value = new AssetTypeValue(EnumValueTypes.Array, assetArray);
                        }
                    }
                    else
                    {
                        throw new Exception($"Invalid array value type! Found an unexpected {sizeType} type instead!");
                    }
                }
                else
                {
                    throw new Exception("Invalid array!");
                }
            }
            else
            {
                var valType = valueField.templateField.valueType;
                if (valType != 0) valueField.value = new AssetTypeValue(valType, null);
                if (valType == EnumValueTypes.String)
                {
                    var length = reader.ReadInt32();
                    valueField.value.Set(reader.ReadBytes(length));
                    reader.Align();
                }
                else
                {
                    valueField.childrenCount = valueField.templateField.childrenCount;
                    if (valueField.childrenCount == 0)
                    {
                        valueField.children = new AssetTypeValueField[0];
                        switch (valueField.templateField.valueType)
                        {
                            case EnumValueTypes.Int8:
                                valueField.value.Set(reader.ReadSByte());
                                if (valueField.templateField.align) reader.Align();
                                break;
                            case EnumValueTypes.UInt8:
                            case EnumValueTypes.Bool:
                                valueField.value.Set(reader.ReadByte());
                                if (valueField.templateField.align) reader.Align();
                                break;
                            case EnumValueTypes.Int16:
                                valueField.value.Set(reader.ReadInt16());
                                if (valueField.templateField.align) reader.Align();
                                break;
                            case EnumValueTypes.UInt16:
                                valueField.value.Set(reader.ReadUInt16());
                                if (valueField.templateField.align) reader.Align();
                                break;
                            case EnumValueTypes.Int32:
                                valueField.value.Set(reader.ReadInt32());
                                break;
                            case EnumValueTypes.UInt32:
                                valueField.value.Set(reader.ReadUInt32());
                                break;
                            case EnumValueTypes.Int64:
                                valueField.value.Set(reader.ReadInt64());
                                break;
                            case EnumValueTypes.UInt64:
                                valueField.value.Set(reader.ReadUInt64());
                                break;
                            case EnumValueTypes.Float:
                                valueField.value.Set(reader.ReadSingle());
                                break;
                            case EnumValueTypes.Double:
                                valueField.value.Set(reader.ReadDouble());
                                break;
                        }
                    }
                    else
                    {
                        valueField.children = new AssetTypeValueField[valueField.childrenCount];
                        for (var i = 0; i < valueField.childrenCount; i++)
                        {
                            valueField.children[i] = new AssetTypeValueField
                            {
                                templateField = valueField.templateField.children[i]
                            };
                            valueField.children[i] = ReadType(reader, valueField.children[i]);
                        }
                        if (valueField.templateField.align) reader.Align();
                    }
                }
            }
            return valueField;
        }

        public AssetTypeTemplateField SearchChild(string name)
        {
            foreach (var child in children)
            {
                if (child.name == name)
                {
                    return child;
                }
            }
            return null;
        }
    }
}
