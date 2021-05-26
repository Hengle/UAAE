using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using AssetsAdvancedEditor.Utils;
using AssetsTools.NET;
using AssetsTools.NET.Extra;

namespace AssetsAdvancedEditor.Winforms
{
    public partial class BundleLoader : Form
    {
        public AssetBundleFile Bundle;
        public BundleFileInstance BundleInst;
        public bool Loaded;

        public BundleLoader(BundleFileInstance bundleInst)
        {
            InitializeComponent();
            Bundle = bundleInst?.file;
            BundleInst = bundleInst;
        }

        private void BundleLoader_Load(object sender, EventArgs e)
        {
            if (BundleInst == null) return;
            var compType = Bundle.bundleHeader6.GetCompressionType();
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

        private void decompressButton_Click(object sender, EventArgs e)
        {
            if (BundleInst == null) return;
            //try
            //{
                var bw = new BackgroundWorker();
                if (Bundle.IsBundleDataCompressed())
                {
                    var bunDecomp = new BundleDecompression();
                    if (bunDecomp.ShowDialog() != DialogResult.OK) return;
                    var choice = bunDecomp.cboxDecompType.SelectedIndex;
                    switch (choice)
                    {
                        // Decompress to File
                        case 0:
                        {
                            var sfd = new SaveFileDialog
                            {
                                FileName = BundleInst.name + ".unpacked",
                                Filter = @"All types (*.*)|*.*"
                            };
                            if (sfd.ShowDialog() != DialogResult.OK) return;
                            lblNote.Text = @"Decompressing...";
                            btnDecompress.Enabled = false;
                            btnCompress.Enabled = false; 
                            bw.DoWork += delegate { DecompressToFile(sfd.FileName); };
                            break;
                        }
                        // Decompress to Memory
                        case 1:
                            lblNote.Text = @"Decompressing...";
                            btnDecompress.Enabled = false;
                            btnCompress.Enabled = false;
                            bw.DoWork += delegate { DecompressToMemory(); };
                            break;
                    }
                }
                else
                {
                    lblNote.Text = @"Decompressing...";
                    btnDecompress.Enabled = false;
                    btnCompress.Enabled = false;
                    bw.DoWork += delegate { DecompressToMemory(); };
                }
                bw.RunWorkerCompleted += delegate
                {
                    lblNote.Text = @"Done. Click Load to open the bundle.";
                    btnDecompress.Enabled = false;
                    btnLoad.Enabled = true;
                };
                bw.RunWorkerAsync();
            //}
            //catch (Exception ex)
            //{
            //    MsgBoxUtils.ShowErrorDialog("Something went wrong when unpacking the bundle:\n" + ex);
            //}
        }

        private void DecompressToFile(string savePath)
        {
            var bundleStream = File.Open(savePath, FileMode.Create);
            Bundle.Unpack(Bundle.reader, new AssetsFileWriter(bundleStream));

            bundleStream.Position = 0;

            var newBundle = new AssetBundleFile();
            newBundle.Read(new AssetsFileReader(bundleStream));

            Bundle.reader.Close();
            BundleInst.file = newBundle;
        }

        private void DecompressToMemory()
        {
            var bundleStream = new MemoryStream();
            Bundle.Unpack(Bundle.reader, new AssetsFileWriter(bundleStream));

            bundleStream.Position = 0;

            var newBundle = new AssetBundleFile();
            newBundle.Read(new AssetsFileReader(bundleStream));

            Bundle.reader.Close();
            BundleInst.file = newBundle;
        }

        private void compressButton_Click(object sender, EventArgs e)
        {
            MsgBoxUtils.ShowInfoDialog("Compression is not supported atm");
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            Loaded = true;
            Close();
        }
    }
}