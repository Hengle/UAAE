﻿namespace AssetsAdvancedEditor.Assets
{
    public class AssetDetailsListItem
    {
        public string Name { get; set; }
        public string Container { get; set; }
        public string Type { get; set; }
        public uint TypeID { get; set; }
        public int FileID { get; set; }
        public long PathID { get; set; }
        public int Size { get; set; }
        public string Modified { get; set; }
        public long Position { get; set; }
        public ushort MonoID { get; set; }
    }
}
