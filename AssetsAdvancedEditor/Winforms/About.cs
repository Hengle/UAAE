using System.Diagnostics;
using System.Windows.Forms;

namespace AssetsAdvancedEditor.Winforms
{
    public partial class About : Form
    {
        public About() => InitializeComponent();

        private void firstLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openLink("https://github.com/Igor55x");
        }

        private void secondLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openLink("https://community.7daystodie.com/profile/418-derpopo");
        }

        private static void openLink(string url)
        {
            var proc = new Process
            {
                StartInfo =
                {
                    UseShellExecute = true,
                    FileName = url
                }
            };
            proc.Start();
        }
    }
}