namespace UnityTools
{
    public class AssetBundleBlockAndDirectoryList06
    {
        public ulong checksumLow;
        public ulong checksumHigh;
        public int blockCount;
        public AssetBundleBlockInfo06[] blockInf;
        public int directoryCount;
        public AssetBundleDirectoryInfo06[] dirInf;

        ///void Free();
        public void Read(long filePos, AssetsFileReader reader)
        {
            reader.Position = filePos;
            checksumLow = reader.ReadUInt64();
            checksumHigh = reader.ReadUInt64();
            blockCount = reader.ReadInt32();
            blockInf = new AssetBundleBlockInfo06[blockCount];
            for (var i = 0; i < blockCount; i++)
            {
                blockInf[i] = new AssetBundleBlockInfo06
                {
                    decompressedSize = reader.ReadUInt32(),
                    compressedSize = reader.ReadUInt32(),
                    flags = reader.ReadUInt16()
                };
            }

            directoryCount = reader.ReadInt32();
            dirInf = new AssetBundleDirectoryInfo06[directoryCount];
            for (var i = 0; i < directoryCount; i++)
            {
                dirInf[i] = new AssetBundleDirectoryInfo06
                {
                    offset = reader.ReadInt64(),
                    decompressedSize = reader.ReadInt64(),
                    flags = reader.ReadUInt32(),
                    name = reader.ReadNullTerminated()
                };
            }
        }
        //Write doesn't compress
        public void Write(AssetsFileWriter writer)
        {
            writer.Write(checksumHigh);
            writer.Write(checksumLow);
            blockCount = blockInf.Length;
            writer.Write(blockCount);
            for (var i = 0; i < blockCount; i++)
            {
                writer.Write(blockInf[i].decompressedSize);
                writer.Write(blockInf[i].compressedSize);
                writer.Write(blockInf[i].flags);
            }
            directoryCount = dirInf.Length;
            writer.Write(directoryCount);
            for (var i = 0; i < directoryCount; i++)
            {
                writer.Write(dirInf[i].offset);
                writer.Write(dirInf[i].decompressedSize);
                writer.Write(dirInf[i].flags);
                writer.WriteNullTerminated(dirInf[i].name);
            }
        }
    }
}
