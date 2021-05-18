using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AssetsAdvancedEditor
{
    public static class MsgBoxUtils
    {
        public static DialogResult ShowErrorDialog(string message, string caption = "")
        {
            if (string.IsNullOrEmpty(caption))
            {
                caption = "ERROR";
            }
            return MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static DialogResult ShowInfoDialog(string message, string caption = "")
        {
            if (string.IsNullOrEmpty(caption))
            {
                caption = "UAAE";
            }
            return MessageBox.Show(message, caption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
        }

        public static DialogResult ShowWarningDialog(string message, string caption = "")
        {
            if (string.IsNullOrEmpty(caption))
            {
                caption = "Warning";
            }
            return MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        }
    }
}
