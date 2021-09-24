using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AssetsAdvancedEditor.Assets;
using AssetsAdvancedEditor.Utils;
using UnityTools;

namespace AssetsAdvancedEditor.Winforms
{
    public partial class Menu : Form
    {
        public AssetsManager Am;
        public BundleFileInstance BundleInst;
        public BundleLoader Loader;
        public bool Modified;
        public Dictionary<string, BundleReplacer> ModifiedFiles;

        public Menu()
        {
            InitializeComponent();
            ModifiedFiles = new Dictionary<string, BundleReplacer>();
            Modified = false;
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            Am = new AssetsManager();
            var classDataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "classdata.tpk");
            if (File.Exists(classDataPath))
            {
                try
                {
                    Am.LoadClassPackage(classDataPath);
                    if (Am.classPackage.valid) return;
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
            if (Modified) AskSaveChanges();
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
                    var fileInst = Am.LoadAssetsFile(selectedFile, true);

                    if (!LoadOrAskCldb(fileInst))
                        return;

                    new AssetsViewer(Am, fileInst).ShowDialog();
                    break;
                }
                case DetectedFileType.BundleFile:
                {
                    LoadBundle(selectedFile);
                    break;
                }
                default:
                    MsgBoxUtils.ShowErrorDialog("Unable to read the file!\n" +
                                                "Invalid file or unknown (unsupported) version.");
                    break;
            }
        }

        private void MenuClose_Click(object sender, EventArgs e)
        {
            if (Modified) AskSaveChanges();
            else CloseAllFiles();
        }

        private void MenuSave_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog
            {
                Title = @"Save bundle file",
                Filter = @"All types (*.*)|*.*|Bundle file (*.unity3d)|*.unity3d"
            };
            if (sfd.ShowDialog() != DialogResult.OK) return;
            SaveBundle(sfd.FileName);
        }

        private void compressToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new BundleCompression(BundleInst).ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AskSaveChanges();
            if (!Modified)
                Application.Exit();
        }

        private void LoadBundle(string path)
        {
            BundleInst = Am.LoadBundleFile(path, false);
            Loader = new BundleLoader(BundleInst);
            Loader.ShowDialog();
            if (!Loader.Loaded) return;
            cboxBundleContents.Enabled = true;
            btnExport.Enabled = true;
            btnImport.Enabled = true;
            btnRemove.Enabled = true;
            btnInfo.Enabled = true;

            var infos = BundleInst.file.Metadata.DirectoryInfo;
            cboxBundleContents.Items.Clear();
            foreach (var info in infos)
            {
                cboxBundleContents.Items.Add(info.Name);
            }
            cboxBundleContents.SelectedIndex = 0;
            lblFileName.Text = BundleInst.name;
        }

        private void SaveBundle(string path)
        {
            if (BundleInst == null) return;
            using (var fs = File.OpenWrite(path))
            using (var writer = new AssetsFileWriter(fs))
            {
                BundleInst.file.Write(writer, ModifiedFiles.Values.ToList());
            }
            Modified = false;
            var items = cboxBundleContents.Items.Cast<string>().Select(i => i.Replace(" *", ""));
            cboxBundleContents.Items.Clear();
            cboxBundleContents.Items.AddRange((object[])items);
        }

        private void CloseAllFiles()
        {
            ModifiedFiles.Clear();
            Modified = false;

            Am.UnloadAllAssetsFiles(true);
            Am.UnloadAllBundleFiles();

            cboxBundleContents.Items.Clear();

            lblFileName.Text = @"No file opened.";
        }

        private bool LoadOrAskCldb(AssetsFileInstance fileInst)
        {
            var unityVersion = fileInst.file.typeTree.unityVersion;
            if (Am.LoadClassDatabaseFromPackage(unityVersion) == null)
            {
                var version = new VersionDialog(unityVersion, Am.classPackage);
                if (version.ShowDialog() != DialogResult.OK)
                    return false;

                if (version.SelectedCldb != null)
                    Am.classFile = version.SelectedCldb;
                else
                    return false;
            }
            return true;
        }

        private void AskSaveChanges()
        {
            if (!Modified) return;
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
            if (BundleInst == null || cboxBundleContents.SelectedItem == null) return;
            var index = cboxBundleContents.SelectedIndex;

            var bunAssetName = BundleInst.file.Metadata.DirectoryInfo[index].Name;
            var assetData = BundleHelper.LoadAssetDataFromBundle(BundleInst.file, index);

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
            if (BundleInst == null) return;
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
                var fileBytes = File.ReadAllBytes(fileName);
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
                ModifiedFiles[fileName] = replacer;
                i++;
            }
            Modified = true;
        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            if (BundleInst == null || cboxBundleContents.SelectedItem == null) return;
            var index = cboxBundleContents.SelectedIndex;

            var dirInf = BundleHelper.GetDirInfo(BundleInst.file, index);
            var bunAssetName = dirInf.Name;

            //When we make a modification to an assets file in the bundle,
            //we replace the assets file in the manager. This way, all we
            //have to do is not reload from the bundle if our assets file
            //has been modified
            MemoryStream assetStream;
            if (!ModifiedFiles.ContainsKey(bunAssetName))
            {
                var assetData = BundleHelper.LoadAssetDataFromBundle(BundleInst.file, index);
                assetStream = new MemoryStream(assetData);
            }
            else
            {
                //unused if the file already exists
                assetStream = null;
            }

            //warning: does not update if you import an assets file onto
            //a file that wasn't originally an assets file
            var isAssetsFile = BundleInst.file.IsAssetsFile(dirInf);

            if (isAssetsFile)
            {
                var assetMemPath = Path.Combine(BundleInst.path, bunAssetName);
                var fileInst = Am.LoadAssetsFile(assetStream, assetMemPath, true);

                if (!LoadOrAskCldb(fileInst))
                    return;

                var info = new AssetsViewer(Am, fileInst, true);
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
            if (ModifiedFiles.ContainsKey(origName))
            {
                ModifiedFiles.Remove(origName);
            }
            else
            {
                ModifiedFiles.Add(origName, AssetModifier.CreateBundleRemover(origName, isSerialized));
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

            foreach (var (bunRep, assetsStream) in bunDict)
            {
                var fileName = bunRep.GetOriginalEntryName();
                ModifiedFiles[fileName] = bunRep;

                //replace existing assets file in the manager
                var inst = Am.files.FirstOrDefault(i =>
                    string.Equals(i.name, fileName, StringComparison.CurrentCultureIgnoreCase));
                string assetsManagerName;

                if (inst != null)
                {
                    assetsManagerName = inst.path;
                    Am.files.Remove(inst);
                }
                else //shouldn't happen
                {
                    //we always load bundles from file, so this
                    //should always be somewhere on the disk
                    assetsManagerName = Path.Combine(BundleInst.path, fileName);
                }
                Am.LoadAssetsFile(assetsStream, assetsManagerName, false);
            }

            Modified = true;
        }
    }
}