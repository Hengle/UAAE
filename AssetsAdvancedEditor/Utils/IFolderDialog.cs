﻿using System.Windows.Forms;

namespace AssetsAdvancedEditor.Utils
{
    public interface IFolderDialog
    {
        string InitialFolder { get; set; }
        string DefaultFolder { get; set; }
        string Folder { get; set; }
        string Title { get; set; }
        void Reset();
        DialogResult ShowDialog();
        DialogResult ShowDialog(IWin32Window owner);
        DialogResult ShowVistaDialog(IWin32Window owner);
        DialogResult ShowLegacyDialog(IWin32Window owner);
    }
}