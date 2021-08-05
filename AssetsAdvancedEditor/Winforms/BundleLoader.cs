using System;
using System.Windows.Forms;
using UnityTools;

namespace AssetsAdvancedEditor.Winforms
{
    public partial class BundleLoader : Form
    {
        public BundleFileInstance BundleInst;
        public bool Loaded;

        public BundleLoader(BundleFileInstance bundleInst)
        {
            InitializeComponent();
            BundleInst = bundleInst;
        }

        private void BundleLoader_Load(object sender, EventArgs e)
        {
            if (BundleInst == null) return;
            var compType = BundleInst.file.bundleHeader6.GetCompressionType();
            switch (compType)
            {
                case 0:
                    lblCompType.Text = @"None";
                    break;
                case 1:
                    lblCompType.Text = @"LZMA";
                    break;
                case 2 or 3:
                    lblCompType.Text = @"LZ4";
                    break;
                default:
                    lblCompType.Text = @"Unknown";
                    lblNote.Text = @"Looks like the bundle is packed with a custom compression type and cannot be unpacked!";
                    btnLoad.Enabled = false;
                    btnDecompress.Enabled = false;
                    btnCompress.Enabled = false;
                    return;
            }

            if (compType == 0)
            {
                lblNote.Text = @"Bundle is not compressed. You can load it or compress it.";
                btnCompress.Enabled = true;
                btnLoad.Enabled = true;
            }
            else
            {
                lblNote.Text = @"Bundle is compressed. You must decompress the bundle to load.";
                btnDecompress.Enabled = true;
            }
        }

        private void btnDecompress_Click(object sender, EventArgs e)
        {
            if (BundleInst == null) return;
            var bunDecomp = new BundleDecompression(BundleInst);
            if (bunDecomp.ShowDialog() != DialogResult.OK) return;
            lblNote.Text = @"Decompressing...";
            btnDecompress.Enabled = false;
            btnCompress.Enabled = false;
            bunDecomp.Bw.RunWorkerCompleted += delegate
            {
                lblNote.Text = @"Done. Click Load to open the bundle.";
                btnDecompress.Enabled = false;
                btnCompress.Enabled = true;
                btnLoad.Enabled = true;
            };
        }

        private void btnCompress_Click(object sender, EventArgs e)
        {
            if (BundleInst == null) return;
            var bunComp = new BundleCompression(BundleInst);
            if (bunComp.ShowDialog() == DialogResult.OK && bunComp.Compressed) Close();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            Loaded = true;
            Close();
        }
    }
}