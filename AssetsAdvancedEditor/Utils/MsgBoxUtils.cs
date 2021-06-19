using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AssetsAdvancedEditor.Utils
{
    public static class MsgBoxUtils
    {
        public static DialogResult ShowErrorDialog(string message)
        {
            return MessageBox.Show(message, @"ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static DialogResult ShowErrorDialog(IWin32Window owner, string message)
        {
            return MessageBox.Show(owner, message, @"ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static DialogResult ShowErrorDialog(string message, string caption, [Optional] MessageBoxButtons? buttons)
        {
            if (string.IsNullOrEmpty(caption))
            {
                caption = "ERROR";
            }
            return MessageBox.Show(message, caption, buttons ?? MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static DialogResult ShowErrorDialog(string message, MessageBoxButtons buttons, [Optional] string caption)
        {
            return ShowErrorDialog(message, caption, buttons);
        }

        public static DialogResult ShowInfoDialog(string message)
        {
            return MessageBox.Show(message, @"UAAE", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
        }

        public static DialogResult ShowInfoDialog(string message, string caption, [Optional] MessageBoxButtons? buttons)
        {
            if (string.IsNullOrEmpty(caption))
            {
                caption = "UAAE";
            }
            return MessageBox.Show(message, caption, buttons ?? MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
        }

        public static DialogResult ShowInfoDialog(string message, MessageBoxButtons buttons, [Optional] string caption)
        {
            return ShowInfoDialog(message, caption, buttons);
        }

        public static DialogResult ShowWarningDialog(string message)
        {
            return MessageBox.Show(message, @"Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        }

        public static DialogResult ShowWarningDialog(string message, string caption, [Optional] MessageBoxButtons? buttons)
        {
            if (string.IsNullOrEmpty(caption))
            {
                caption = "Warning";
            }
            return MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        }

        public static DialogResult ShowWarningDialog(string message, MessageBoxButtons buttons, [Optional] string caption)
        {
            return ShowWarningDialog(message, caption, buttons);
        }
    }
}
