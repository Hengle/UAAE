using UnityTools;

namespace AssetsAdvancedEditor.Assets
{
    public class ModMakerReplacerItem
    {
        public AssetID AssetID;
        public AssetsReplacer Replacer;
        public string DisplayText => ToString();

        public ModMakerReplacerItem(AssetID assetId, AssetsReplacer replacer)
        {
            AssetID = assetId;
            Replacer = replacer;
        }

        public override string ToString()
        {
            return Replacer is AssetsRemover ? $"Remove path id {Replacer.GetPathID()}" : $"Replace path id {Replacer.GetPathID()}";
        }
    }
}
