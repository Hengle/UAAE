namespace UnityTools
{
    public class AssetBundleDirectoryInfo
    {
        public long Offset;
        public long DecompressedSize;
        public uint Flags;
        public string Name;

        public void Read(AssetsFileReader reader, uint version)
        {
            if (version >= 6)
            {
                Offset = reader.ReadInt64();
                DecompressedSize = reader.ReadInt64();
                Flags = reader.ReadUInt32();
                Name = reader.ReadNullTerminated();
            }
            else
            {
                Name = reader.ReadNullTerminated();
                Offset = reader.ReadInt64();
                DecompressedSize = reader.ReadInt64();
            }
        }

        public void Write(AssetsFileWriter writer)
        {
            writer.Write(Offset);
            writer.Write(DecompressedSize);
            writer.Write(Flags);
            writer.WriteNullTerminated(Name);
        }

        public long GetAbsolutePos(AssetBundleHeader header)
        {
            return header.GetFileDataOffset() + Offset;
        }

        public long GetAbsolutePos(AssetBundleFile file)
        {
            return file.Header.GetFileDataOffset() + Offset;
        }
    }
}
