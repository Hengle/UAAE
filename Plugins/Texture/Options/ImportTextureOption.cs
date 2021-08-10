using System.Collections.Generic;
using System.Windows.Forms;
using AssetsAdvancedEditor.Assets;
using AssetsAdvancedEditor.Plugins;
using UnityTools;

namespace Texture.Options
{
    public class ImportTextureOption : PluginOption
    {
        public ImportTextureOption() => Action = PluginAction.Import;

        public override bool IsValidForPlugin(AssetsManager am, List<AssetItem> selectedItems)
        {
            Description = "Batch import Png/Tga";

            if (selectedItems.Count <= 1)
                return false;

            var classId = AssetHelper.FindAssetClassByName(am.classFile, "Texture2D").classId;

            foreach (var item in selectedItems)
            {
                if (item.TypeID != classId)
                    return false;
            }
            return true;
        }

        public override bool ExecutePlugin(IWin32Window owner, AssetsWorkspace workspace, List<AssetItem> selectedItems)
        {
            // todo
            return false;
        }
    }
}
