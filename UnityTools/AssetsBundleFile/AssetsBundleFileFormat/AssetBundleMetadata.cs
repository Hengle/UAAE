namespace UnityTools
{
    /// <summary>
    /// Metadata about bundle's block or chunk
    /// </summary>
    public class AssetBundleMetadata
    {
        public Hash128 Hash;
        public int BlockCount;
        public AssetBundleBlockInfo[] BlocksInfo;
        public int DirectoryCount;
        public AssetBundleDirectoryInfo[] DirectoryInfo;

        public void Read(long filePos, AssetsFileReader reader)
        {
            reader.Position = filePos;
            Hash = new Hash128(reader);
            BlockCount = reader.ReadInt32();
            BlocksInfo = new AssetBundleBlockInfo[BlockCount];
            for (var i = 0; i < BlockCount; i++)
            {
                BlocksInfo[i] = new AssetBundleBlockInfo();
                BlocksInfo[i].Read(reader);
            }

            DirectoryCount = reader.ReadInt32();
            DirectoryInfo = new AssetBundleDirectoryInfo[DirectoryCount];
            for (var i = 0; i < DirectoryCount; i++)
            {
                DirectoryInfo[i] = new AssetBundleDirectoryInfo();
                DirectoryInfo[i].Read(reader);
            }
        }

        public void Write(AssetsFileWriter writer)
        {
            Hash.Write(writer);
            writer.Write(BlockCount);
            for (var i = 0; i < BlockCount; i++)
            {
                BlocksInfo[i].Write(writer);
            }

            writer.Write(DirectoryCount);
            for (var i = 0; i < DirectoryCount; i++)
            {
                DirectoryInfo[i].Write(writer);
            }
        }
    }
}
