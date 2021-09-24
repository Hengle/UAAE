namespace UnityTools
{
    public struct AssetBundleScene
    {
        public uint CompressedSize;
        public uint DecompressedSize;

        public void Read(AssetsFileReader reader)
        {
            CompressedSize = reader.ReadUInt32();
            DecompressedSize = reader.ReadUInt32();
        }

        public void Write(AssetsFileWriter writer)
        {
            writer.Write(CompressedSize);
            writer.Write(DecompressedSize);
        }
    }
}