using UnityTools;

namespace AssetsAdvancedEditor.Assets
{
    // Not to be confused with containers which are just
    // UABE's way of showing what export an asset is connected to.
    public class AssetContainer
    {
        public AssetsFileInstance FileInstance { get; }
        public AssetTypeInstance TypeInstance { get; }
        public AssetsFileReader FileReader { get; }
        //public AssetID AssetId => new (FileInstance.path, Item.PathID);
        public bool HasInstance => TypeInstance != null;

        //Existing assets
        public AssetContainer(AssetsFileInstance fileInst, AssetTypeInstance typeInst = null)
        {
            FileReader = fileInst.file.reader;
            FileInstance = fileInst;
            TypeInstance = typeInst;
        }

        //Newly created assets
        public AssetContainer(AssetsFileReader reader, AssetsFileInstance fileInst, AssetTypeInstance typeInst = null)
        {
            FileReader = reader;
            FileInstance = fileInst;
            TypeInstance = typeInst;
        }

        public AssetContainer(AssetContainer container, AssetTypeInstance typeInst)
        {
            FileReader = container.FileReader;
            FileInstance = container.FileInstance;
            TypeInstance = typeInst;
        }
    }
}
