using System.Collections.Generic;

namespace UnityTools
{
    public class AssetTypeInstance
    {
        public int BaseFieldCount;
        public List<AssetTypeValueField> BaseFields { get; }

        public AssetTypeInstance(AssetTypeTemplateField baseField, AssetsFileReader reader, long filePos)
            : this(new[] { baseField }, reader, filePos) { }

        public AssetTypeInstance(AssetTypeTemplateField[] baseFields, AssetsFileReader reader, long filePos)
        {
            reader.BigEndian = false;
            reader.Position = filePos;
            BaseFieldCount = baseFields.Length;
            BaseFields = new List<AssetTypeValueField>(BaseFieldCount);
            for (var i = 0; i < BaseFieldCount; i++)
            {
                var valueField = baseFields[i].MakeValue(reader);
                BaseFields.Add(valueField);
            }
        }

        public static AssetTypeValueField GetDummyAssetTypeField()
        {
            var valueField = new AssetTypeValueField
            {
                ChildrenCount = -1
            };
            return valueField;
        }

        public AssetTypeValueField GetBaseField(int index = 0)
        {
            if (index < 0 || index >= BaseFieldCount)
                return GetDummyAssetTypeField();
            return BaseFields[index];
        }

        public bool SetChildList(int index, List<AssetTypeValueField> children)
        {
            if (index < 0 || index >= BaseFieldCount)
                return false;
            var valueField = GetBaseField(index);
            valueField.SetChildrenList(children);
            return true;
        }
    }
}
