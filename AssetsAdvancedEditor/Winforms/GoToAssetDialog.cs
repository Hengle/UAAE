using System;
using System.Windows.Forms;
using AssetsAdvancedEditor.Assets;
using AssetsAdvancedEditor.Utils;

namespace AssetsAdvancedEditor.Winforms
{
    public partial class GoToAssetDialog : Form
    {
        private AssetsWorkspace workspace { get; }
        public int FileID;
        public long PathID;

        public GoToAssetDialog(AssetsWorkspace workspace)
        {
            InitializeComponent();
            this.workspace = workspace;
        }

        private void GoToAssetDialog_Load(object sender, EventArgs e)
        {
            var i = 0;
            foreach (var fileInst in workspace.LoadedFiles)
            {
                cboxFileID.Items.Add($"{i++} - {fileInst.name}");
            }
            cboxFileID.SelectedIndex = 0;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            FileID = cboxFileID.SelectedIndex;
            if (long.TryParse(boxPathID.Text, out var pathId))
            {
                PathID = pathId;
            }
            else
            {
                MsgBoxUtils.ShowErrorDialog("Path ID is invalid!");
            }
        }
    }
}
