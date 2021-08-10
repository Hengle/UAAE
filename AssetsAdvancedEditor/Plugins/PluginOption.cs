using System.Collections.Generic;
using System.Windows.Forms;
using AssetsAdvancedEditor.Assets;
using UnityTools;

namespace AssetsAdvancedEditor.Plugins
{
    public abstract class PluginOption
    {
        public PluginAction Action;
        public string Description;
        public abstract bool IsValidForPlugin(AssetsManager am, List<AssetItem> selectedItems);
        public abstract bool ExecutePlugin(IWin32Window owner, AssetsWorkspace workspace, List<AssetItem> selectedItems);
    }
}
