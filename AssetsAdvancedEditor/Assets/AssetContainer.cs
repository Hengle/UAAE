using System.IO;
using AssetsTools.NET;
using AssetsTools.NET.Extra;

namespace AssetsAdvancedEditor.Assets
{
    // Remake of the AssetData.
    // Not to be confused with containers which are just
    // UABE's way of showing what export an asset is connected to.
    public class AssetContainer
    {
        public AssetItem Item;
        public AssetsFileInstance FileInstance;
        public AssetTypeInstance TypeInstance;
        public MemoryStream Data;
        public AssetsFileReader FileReader;
        public AssetsReplacer Replacer;
        public AssetID AssetId => new (FileInstance.path, Item.PathID);
        public bool HasInstance => TypeInstance != null;

        //Existing assets
        public AssetContainer(AssetItem item, AssetsFileInstance fileInst, AssetTypeInstance typeInst = null)
        {
            Item = item;
            FileReader = fileInst.file.reader;
            FileInstance = fileInst;
            TypeInstance = typeInst;
        }

        //Newly created assets
        public AssetContainer(MemoryStream ms, AssetItem item, AssetsReplacer replacer, AssetsFileInstance fileInst, AssetTypeInstance typeInst = null)
        {
            FileReader = new AssetsFileReader(ms);
            Item = item;
            Replacer = replacer;
            FileInstance = fileInst;
            TypeInstance = typeInst;
            Data = ms;
        }

        public AssetContainer(AssetItem item, AssetsReplacer replacer, AssetsFileInstance fileInst, AssetTypeInstance typeInst = null)
        {
            var ms = new MemoryStream();
            var writer = new AssetsFileWriter(ms);
            replacer.Write(writer);
            ms.Position = 0;
            FileReader = new AssetsFileReader(ms);
            Item = item;
            Replacer = replacer;
            FileInstance = fileInst;
            TypeInstance = typeInst;
            Data = ms;
        }
    }
}
