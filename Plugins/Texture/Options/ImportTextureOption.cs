using System.Collections.Generic;
using System.Windows.Forms;
using AssetsAdvancedEditor.Assets;
using AssetsAdvancedEditor.Plugins;
using AssetsTools.NET.Extra;

namespace Texture.Options
{
    public class ImportTextureOption : PluginOption
    {
        public ImportTextureOption() => Action = PluginAction.Import;

        public override bool IsValidForPlugin(AssetsManager am, List<AssetContainer> selectedAssets)
        {
            Description = "Batch import Png/Tga";

            if (selectedAssets.Count <= 1)
                return false;

            var classId = AssetHelper.FindAssetClassByName(am.classFile, "Texture2D").classId;

            foreach (var cont in selectedAssets)
            {
                if (cont.Item.TypeID != classId)
                    return false;
            }
            return true;
        }

        public override bool ExecutePlugin(IWin32Window owner, AssetsWorkspace workspace, List<AssetContainer> selectedAssets)
        {
            // todo
            return false;
        }
    }
}
