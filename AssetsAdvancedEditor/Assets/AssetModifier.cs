using UnityTools;

namespace AssetsAdvancedEditor.Assets
{
    public class AssetModifier
    {
        public static AssetsReplacer CreateAssetReplacer(AssetItem item, byte[] data)
        {
            return new AssetsReplacerFromMemory(item.FileID, item.PathID, item.TypeID, item.MonoID, data);
        }

        public static AssetsReplacer CreateAssetRemover(AssetItem item)
        {
            return new AssetsRemover(item.FileID, item.PathID, item.TypeID, item.MonoID);
        }

        public static BundleReplacer CreateBundleReplacer(string name, bool isSerialized, byte[] data)
        {
            return new BundleReplacerFromMemory(name, name, isSerialized, data, -1);
        }

        public static BundleReplacer CreateBundleRemover(string name, bool isSerialized, int bundleListIndex = -1)
        {
            return new BundleRemover(name, isSerialized, bundleListIndex);
        }
    }
}