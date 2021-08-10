namespace AssetsAdvancedEditor.Assets
{
    public class AssetItem
    {
        public AssetContainer Cont { get; set; }
        public string Name { get; set; }
        public string ListName { get; set; }
        public string Container { get; set; }
        public string Type { get; set; }
        public uint TypeID { get; set; }
        public int FileID { get; set; }
        public long PathID { get; set; }
        public long Size { get; set; }
        public string Modified { get; set; }
        public long Position { get; set; }
        public ushort MonoID { get; set; }

        public string[] ToArray()
        {
            return new[]
            {
                ListName,
                Container,
                Type,
                TypeID.ToString(),
                FileID.ToString(),
                PathID.ToString(),
                Size.ToString(),
                Modified
            };
        }
    }
}
