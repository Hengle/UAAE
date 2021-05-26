using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AssetsAdvancedEditor.Assets;
using AssetsTools.NET;
using AssetsTools.NET.Extra;

namespace AssetsAdvancedEditor.Winforms
{
    public partial class Menu : Form
    {
        private AssetsManager _am;
        private BundleFileInstance _bundleInst;
        private BundleLoader _loader;
        private bool _modified;
        private readonly Dictionary<string, BundleReplacer> _modifiedFiles;

        public Menu()
        {
            InitializeComponent();
            _modifiedFiles = new Dictionary<string, BundleReplacer>();
            _modified = false;
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            _am = new AssetsManager();
            if (File.Exists("classdata.tpk"))
            {
                try
                {
                    _am.LoadClassPackage("classdata.tpk");
                    if (_am.classPackage.valid) return;
                    MsgBoxUtils.ShowErrorDialog("Invalid classdata.tpk file.");
                }
                catch (Exception ex)
                {
                    MsgBoxUtils.ShowErrorDialog("Can't load classdata.tpk:\n" + ex);
                }
            }
            else
            {
                MsgBoxUtils.ShowErrorDialog("Missing classdata.tpk by exe.\n" +
                                            "Please make sure it exists.");
            }
            Close();
            Environment.Exit(1);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_modified) AskSaveChanges();
            var ofd = new OpenFileDialog
            {
                Title = @"Open assets or bundle file",
                Filter = @"All types (*.*)|*.*|Unity content (*.unity3d;*.assets)|*.unity3d;*.assets|Bundle file (*.unity3d)|*.unity3d|Assets file (*.assets)|*.assets"
            };
            if (ofd.ShowDialog() != DialogResult.OK) return;
            var selectedFile = ofd.FileName;

            var fileType = AssetsBundleDetector.DetectFileType(selectedFile);

            CloseAllFiles();

            switch (fileType)
            {
                case DetectedFileType.AssetsFile:
                {
                    var fileInst = _am.LoadAssetsFile(selectedFile, true);
                    _am.LoadClassDatabaseFromPackage(fileInst.file.typeTree.unityVersion);
                    new AssetsViewer(_am, fileInst).ShowDialog();
                    break;
                }
                case DetectedFileType.BundleFile:
                    LoadBundle(selectedFile);
                    break;
                default:
                    MsgBoxUtils.ShowErrorDialog("Unable to read the file!\n" +
                                                "Invalid file or unknown (unsupported) version.");
                    break;
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_modified) AskSaveChanges();
            else CloseAllFiles();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog
            {
                Title = @"Save bundle file",
                Filter = @"All types (*.*)|*.*|Bundle file (*.unity3d)|*.unity3d"
            };
            if (sfd.ShowDialog() != DialogResult.OK) return;
            SaveBundle(sfd.FileName);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AskSaveChanges();
            if (!_modified)
                Application.Exit();
        }

        private void LoadBundle(string path)
        {
            _bundleInst = _am.LoadBundleFile(path, false);
            _loader = new BundleLoader(_bundleInst);
            _loader.ShowDialog();
            if (!_loader.Loaded) return;
            cboxBundleContents.Enabled = true;
            btnExport.Enabled = true;
            btnImport.Enabled = true;
            btnRemove.Enabled = true;
            btnInfo.Enabled = true;

            var infos = _bundleInst.file.bundleInf6.dirInf;
            cboxBundleContents.Items.Clear();
            foreach (var info in infos)
            {
                cboxBundleContents.Items.Add(info.name);
            }
            cboxBundleContents.SelectedIndex = 0;
            lblFileName.Text = _bundleInst.name;
        }

        private void SaveBundle(string path)
        {
            if (_bundleInst == null) return;
            using (var fs = File.OpenWrite(path))
            using (var writer = new AssetsFileWriter(fs))
            {
                _bundleInst.file.Write(writer, _modifiedFiles.Values.ToList());
            }
            _modified = false;
            var items = cboxBundleContents.Items.Cast<string>().Select(i => i.Replace(" *", ""));
            cboxBundleContents.Items.Clear();
            cboxBundleContents.Items.AddRange((object[])items);
        }

        private void CloseAllFiles()
        {
            _modifiedFiles.Clear();
            _modified = false;

            _am.UnloadAllAssetsFiles(true);
            _am.UnloadAllBundleFiles();

            cboxBundleContents.Items.Clear();

            lblFileName.Text = @"No file opened.";
        }

        private void AskSaveChanges()
        {
            if (!_modified) return;
            var choice = MsgBoxUtils.ShowInfoDialog("Would you like to save the changes?");
            switch (choice)
            {
                case DialogResult.Yes:
                {
                    var sfd = new SaveFileDialog
                    {
                        Title = @"Save bundle file",
                        Filter = @"All types (*.*)|*.*|Bundle file (*.unity3d)|*.unity3d"
                    };
                    if (sfd.ShowDialog() != DialogResult.OK) return;
                    SaveBundle(sfd.FileName);
                    break;
                }
                case DialogResult.No:
                    CloseAllFiles();
                    break;
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new About().ShowDialog();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (_bundleInst == null || cboxBundleContents.SelectedItem == null) return;
            var index = cboxBundleContents.SelectedIndex;

            var bunAssetName = _bundleInst.file.bundleInf6.dirInf[index].name;
            var assetData = BundleHelper.LoadAssetDataFromBundle(_bundleInst.file, index);

            var sfd = new SaveFileDialog
            {
                Title = @"Save as...",
                Filter = @"All types (*.*)|*.*|Assets file (*.assets)|*.assets",
                FileName = bunAssetName
            };
            if (sfd.ShowDialog() != DialogResult.OK) return;
            File.WriteAllBytes(sfd.FileName, assetData);
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (_bundleInst == null) return;
            var ofd = new OpenFileDialog
            {
                Title = @"Open assets file",
                Filter = @"All types (*.*)|*.*|Assets file (*.assets)|*.assets"
            };
            if (ofd.ShowDialog() != DialogResult.OK) return;
            var i = 0;
            foreach (var file in ofd.FileNames)
            {
                var fileName = Path.GetFileName(file);
                var fileBytes = File.ReadAllBytes(file);
                var isSerialized = !(file.EndsWith(".resS") || file.EndsWith(".resource"));
                var replacer = AssetModifier.CreateBundleReplacer(fileName, isSerialized, fileBytes);
                if (!cboxBundleContents.Items.Contains(fileName))
                {
                    cboxBundleContents.Items.Add(fileName + " *");
                }
                else
                {
                    var item = cboxBundleContents.Items[i] + " *";
                    cboxBundleContents.Items.Remove(item);
                    cboxBundleContents.Items.Insert(i, item);
                }
                _modifiedFiles[fileName] = replacer;
                i++;
            }
            _modified = true;
        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            if (_bundleInst == null || cboxBundleContents.SelectedItem == null) return;
            var index = cboxBundleContents.SelectedIndex;

            var dirInf = BundleHelper.GetDirInfo(_bundleInst.file, index);
            var bunAssetName = dirInf.name;

            //When we make a modification to an assets file in the bundle,
            //we replace the assets file in the manager. This way, all we
            //have to do is not reload from the bundle if our assets file
            //has been modified
            MemoryStream assetStream;
            if (!_modifiedFiles.ContainsKey(bunAssetName))
            {
                var assetData = BundleHelper.LoadAssetDataFromBundle(_bundleInst.file, index);
                assetStream = new MemoryStream(assetData);
            }
            else
            {
                //unused if the file already exists
                assetStream = null;
            }

            //warning: does not update if you import an assets file onto
            //a file that wasn't originally an assets file
            var isAssetsFile = _bundleInst.file.IsAssetsFile(_bundleInst.file.reader, dirInf);

            if (isAssetsFile)
            {
                var assetMemPath = Path.Combine(_bundleInst.path, bunAssetName);
                var fileInst = _am.LoadAssetsFile(assetStream, assetMemPath, true);
                _am.LoadClassDatabaseFromPackage(fileInst.file.typeTree.unityVersion);

                var info = new AssetsViewer(_am, fileInst, true);
                info.Closing += AssetsViewerClosing;
                info.Show();
            }
            else
            {
                MsgBoxUtils.ShowErrorDialog("This doesn't seem to be a valid assets file.");
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            var selectedItem = cboxBundleContents.SelectedItem;
            if (selectedItem == null) return;
            var name = (string)selectedItem;
            var origName = name.Replace(" *", "");
            var isSerialized = !(origName.EndsWith(".resS") || origName.EndsWith(".resource"));
            if (_modifiedFiles.ContainsKey(origName))
            {
                _modifiedFiles.Remove(origName);
            }
            else
            {
                _modifiedFiles.Add(origName, AssetModifier.CreateBundleRemover(origName, isSerialized));
            }
            cboxBundleContents.Items.Remove(name);
            if (cboxBundleContents.Items.Count != 0)
                cboxBundleContents.SelectedIndex = 0;
        }

        private void AssetsViewerClosing(object sender, CancelEventArgs e)
        {
            if (sender == null) return;

            var window = (AssetsViewer)sender;

            if (window.ModifiedFiles.Count == 0) return;
            var bunDict = window.ModifiedFiles;

            var choice = MsgBoxUtils.ShowWarningDialog("This option is closed until the next update.\n" +
                                                       "Do you want to save assets files?");
            if (choice != DialogResult.Yes) return;
            foreach (var (key, value) in bunDict)
            {
                var sfd = new SaveFileDialog
                {
                    Title = @"Save as...",
                    Filter = @"All types (*.*)|*.*|Assets file (*.assets)|*.assets",
                    FileName = key.GetEntryName()
                };
                if (sfd.ShowDialog() != DialogResult.OK) continue;
                File.WriteAllBytes(sfd.FileName, value.ToArray());
            }

            _modified = true;
        }
    }
}