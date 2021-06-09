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
        public AssetsFileReader FileReader;
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
        public AssetContainer(AssetsFileReader reader, AssetItem item, AssetsFileInstance fileInst, AssetTypeInstance typeInst = null)
        {
            FileReader = reader;
            Item = item;
            FileInstance = fileInst;
            TypeInstance = typeInst;
        }
    }
}
