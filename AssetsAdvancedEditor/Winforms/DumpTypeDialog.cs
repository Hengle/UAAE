﻿using System;
using System.Windows.Forms;
using AssetsAdvancedEditor.Assets;
using AssetsAdvancedEditor.Utils;

namespace AssetsAdvancedEditor.Winforms
{
    public partial class DumpTypeDialog : Form
    {
        public DumpType dumpType;

        public DumpTypeDialog()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (rbtnTXT.Checked)
                dumpType = DumpType.TXT;
            else if (rbtnXML.Checked)
                dumpType = DumpType.XML;
            else if (rbtnJSON.Checked)
                dumpType = DumpType.TXT; // todo
            else
            {
                MsgBoxUtils.ShowErrorDialog("You didn't choose any dump type!\n" +
                                            "Please go back and select it.\n");
                DialogResult = DialogResult.None;
            }
        }
    }
}
