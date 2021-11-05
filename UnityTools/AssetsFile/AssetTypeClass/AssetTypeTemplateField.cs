﻿using System;
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
                TemplateField = this
            };
            valueField = ReadType(reader, valueField);
            return valueField;
        }

        public AssetTypeValueField ReadType(AssetsFileReader reader, AssetTypeValueField valueField)
        {
            if (valueField.TemplateField.isArray)
            {
                if (valueField.TemplateField.childrenCount == 2)
                {
                    var sizeType = valueField.TemplateField.children[0].valueType;
                    if (sizeType is EnumValueTypes.Int32 or EnumValueTypes.UInt32)
                    {
                        if (valueField.TemplateField.valueType == EnumValueTypes.ByteArray)
                        {
                            valueField.ChildrenCount = 0;
                            valueField.Children = new List<AssetTypeValueField>();
                            var size = reader.ReadInt32();
                            var data = reader.ReadBytes(size);
                            if (valueField.TemplateField.align) reader.Align();
                            var byteArray = new AssetTypeByteArray
                            {
                                size = (uint)size,
                                data = data
                            };
                            valueField.Value = new AssetTypeValue(EnumValueTypes.ByteArray, byteArray);
                        }
                        else
                        {
                            valueField.ChildrenCount = reader.ReadInt32();
                            valueField.Children = new List<AssetTypeValueField>(valueField.ChildrenCount);
                            for (var i = 0; i < valueField.ChildrenCount; i++)
                            {
                                valueField.Children.Add(new AssetTypeValueField
                                {
                                    TemplateField = valueField.TemplateField.children[1]
                                });
                                valueField.Children[i] = ReadType(reader, valueField.Children[i]);
                            }
                            if (valueField.TemplateField.align) reader.Align();
                            var assetArray = new AssetTypeArray
                            {
                                size = valueField.ChildrenCount
                            };
                            valueField.Value = new AssetTypeValue(EnumValueTypes.Array, assetArray);
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
                var valType = valueField.TemplateField.valueType;
                if (valType != 0) valueField.Value = new AssetTypeValue(valType, null);
                if (valType == EnumValueTypes.String)
                {
                    var length = reader.ReadInt32();
                    valueField.Value.Set(reader.ReadBytes(length));
                    reader.Align();
                }
                else
                {
                    valueField.ChildrenCount = valueField.TemplateField.childrenCount;
                    if (valueField.ChildrenCount == 0)
                    {
                        valueField.Children = new List<AssetTypeValueField>();
                        switch (valueField.TemplateField.valueType)
                        {
                            case EnumValueTypes.Int8:
                                valueField.Value.Set(reader.ReadSByte());
                                if (valueField.TemplateField.align) reader.Align();
                                break;
                            case EnumValueTypes.UInt8:
                            case EnumValueTypes.Bool:
                                valueField.Value.Set(reader.ReadByte());
                                if (valueField.TemplateField.align) reader.Align();
                                break;
                            case EnumValueTypes.Int16:
                                valueField.Value.Set(reader.ReadInt16());
                                if (valueField.TemplateField.align) reader.Align();
                                break;
                            case EnumValueTypes.UInt16:
                                valueField.Value.Set(reader.ReadUInt16());
                                if (valueField.TemplateField.align) reader.Align();
                                break;
                            case EnumValueTypes.Int32:
                                valueField.Value.Set(reader.ReadInt32());
                                break;
                            case EnumValueTypes.UInt32:
                                valueField.Value.Set(reader.ReadUInt32());
                                break;
                            case EnumValueTypes.Int64:
                                valueField.Value.Set(reader.ReadInt64());
                                break;
                            case EnumValueTypes.UInt64:
                                valueField.Value.Set(reader.ReadUInt64());
                                break;
                            case EnumValueTypes.Float:
                                valueField.Value.Set(reader.ReadSingle());
                                break;
                            case EnumValueTypes.Double:
                                valueField.Value.Set(reader.ReadDouble());
                                break;
                        }
                    }
                    else
                    {
                        valueField.Children = new List<AssetTypeValueField>(valueField.ChildrenCount);
                        for (var i = 0; i < valueField.ChildrenCount; i++)
                        {
                            valueField.Children.Add(new AssetTypeValueField
                            {
                                TemplateField = valueField.TemplateField.children[i]
                            });
                            valueField.Children[i] = ReadType(reader, valueField.Children[i]);
                        }
                        if (valueField.TemplateField.align) reader.Align();
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
