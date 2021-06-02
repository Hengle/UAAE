using System.IO;
using AssetsTools.NET;
using AssetsTools.NET.Extra;

namespace AssetsAdvancedEditor.Assets
{
    public class AssetData
    {
        public AssetItem Item;
        public AssetTypeInstance Instance;
        public MemoryStream Data;
        public AssetsReplacer Replacer;
        public AssetsFileInstance File;
    }
}
