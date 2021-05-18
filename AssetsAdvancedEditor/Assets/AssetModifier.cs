using AssetsTools.NET;

namespace AssetsAdvancedEditor.Assets
{
    public class AssetModifier
    {
        public static AssetsReplacer CreateAssetReplacer(AssetDetailsListItem listItem, byte[] data)
        {
            return new AssetsReplacerFromMemory(listItem.FileID, listItem.PathID, (int)listItem.TypeID, listItem.MonoID, data);
        }

        public static AssetsReplacer CreateAssetRemover(AssetDetailsListItem listItem)
        {
            return new AssetsRemover(listItem.FileID, listItem.PathID, (int)listItem.TypeID, listItem.MonoID);
        }

        public static BundleReplacer CreateBundleReplacer(string name, bool isSerialized, byte[] data)
        {
            return new BundleReplacerFromMemory(name, name, isSerialized, data, -1);
        }
    }
}
