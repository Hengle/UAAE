namespace UnityTools
{
    public class AssetTypeInstance
    {
        public int baseFieldCount;
        public AssetTypeValueField[] baseFields;
        public byte[] memoryToClear;
        public AssetTypeInstance(AssetTypeTemplateField[] baseFields, AssetsFileReader reader, long filePos)
        {
            reader.BigEndian = false;
            reader.Position = filePos;
            this.baseFieldCount = baseFields.Length;
            this.baseFields = new AssetTypeValueField[baseFieldCount];
            for (var i = 0; i < baseFieldCount; i++)
            {
                var templateBaseField = baseFields[i];
                var atvf = templateBaseField.MakeValue(reader);
                this.baseFields[i] = atvf;
            }
        }
        public AssetTypeInstance(AssetTypeTemplateField baseField, AssetsFileReader reader, long filePos)
            : this(new[] { baseField }, reader, filePos) { }
        ///public bool SetChildList(AssetTypeValueField valueField, AssetTypeValueField[] childrenList, uint childrenCount, bool freeMemory = true);
        ///public bool AddTempMemory(byte[] memory);

        public static AssetTypeValueField GetDummyAssetTypeField()
        {
            var atvf = new AssetTypeValueField();
            atvf.childrenCount = -1;
            return atvf;
        }

        public AssetTypeValueField GetBaseField(int index = 0)
        {
            if (index >= baseFieldCount)
                return GetDummyAssetTypeField();
            return baseFields[index];
        }
    }
}
