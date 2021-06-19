using System;
using System.Windows.Forms;
using AssetsAdvancedEditor.Assets;
using AssetsTools.NET;

namespace AssetsAdvancedEditor.Winforms
{
    public partial class VersionDialog : Form
    {
        public ClassDatabaseFile SelectedCldb;
        public VersionDialog(string unityVersion, ClassDatabasePackage tpk)
        {
            InitializeComponent();
            foreach (var cldb in tpk.files)
            {
                lboxVersionList.Items.Add(new ClassFileInfo(cldb));
            }
            lblInfo.Text = $@"There is no type database for {unityVersion}.";
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (lboxVersionList.SelectedItem is ClassFileInfo classTypeInf)
            {
                SelectedCldb = classTypeInf.Cldb;
            }
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e) => Close();
    }
}
