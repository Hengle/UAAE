using System.IO;

namespace UnityTools
{
    public class AssetBundleReader : AssetsFileReader
    {
        public AssetBundleReader(Stream stream) : base(stream) { }
        public AssetBundleReader(AssetsFileReader reader) : base(reader.BaseStream) { }
    }
}
