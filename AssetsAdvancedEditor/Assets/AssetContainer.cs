using UnityTools;

namespace AssetsAdvancedEditor.Assets
{
    // Remake of the AssetData.
    // Not to be confused with containers which are just
    // UABE's way of showing what export an asset is connected to.
    public class AssetContainer
    {
        public AssetItem Item { get; }
        public AssetsFileInstance FileInstance { get; }
        public AssetTypeInstance TypeInstance { get; }
        public AssetsFileReader FileReader { get; }
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
            item.Position = 0;
            Item = item;
            FileInstance = fileInst;
            TypeInstance = typeInst;
        }

        public AssetContainer(AssetContainer container, AssetTypeInstance typeInst)
        {
            Item = container.Item;
            FileReader = container.FileReader;
            FileInstance = container.FileInstance;
            TypeInstance = typeInst;
        }

        public AssetContainer(AssetContainer container, AssetItem item)
        {
            Item = item;
            FileReader = container.FileReader;
            FileInstance = container.FileInstance;
            TypeInstance = container.TypeInstance;
        }
    }
}
