using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AssetsAdvancedEditor.Assets;
using AssetsAdvancedEditor.Plugins;

namespace AssetsAdvancedEditor.Winforms
{
    public partial class EditDialog : Form
    {
        private IWin32Window owner { get; }
        private AssetsWorkspace workspace { get; }
        private List<AssetContainer> selectedAssets { get; }
        public EditDialog(IWin32Window owner, AssetsWorkspace workspace, List<AssetContainer> selectedAssets)
        {
            InitializeComponent();

            this.owner = owner;
            this.workspace = workspace;
            this.selectedAssets = selectedAssets;
            var plugInfs = workspace.Pm.GetSupportedPlugins(selectedAssets);
            foreach (var plugInfo in plugInfs)
            {
                lboxPluginsList.Items.Add(plugInfo);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // todo
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            if (lboxPluginsList.SelectedItem is PluginMenuInfo menuPlugInf)
            {
                var plugOption = menuPlugInf.PluginOpt;
                plugOption.ExecutePlugin(owner, workspace, selectedAssets);
            }
        }
    }
}
