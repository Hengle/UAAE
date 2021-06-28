using System;
using System.Windows.Forms;

namespace AssetsAdvancedEditor.Winforms.AssetSearch
{
    public partial class AssetNameSearch : Form
    {
        public bool ok;
        public string text;
        public bool isDown;
        public bool caseSensitive;
        public bool startAtSelection;

        public AssetNameSearch()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(boxName.Text))
            {
                ok = true;
                text = boxName.Text;
                isDown = rbtnDown.Checked;
                caseSensitive = chboxCaseSensitive.Checked;
                startAtSelection = chboxStartAtSelection.Checked;
            }
            else
            {
                ok = false;
                Close();
            }
        }
    }
}
