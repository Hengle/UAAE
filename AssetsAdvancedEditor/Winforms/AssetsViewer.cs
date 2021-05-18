using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AssetsAdvancedEditor.Assets;
using AssetsAdvancedEditor.Utils;
using AssetsTools.NET;
using AssetsTools.NET.Extra;

namespace AssetsAdvancedEditor.Winforms
{
    public partial class AssetsViewer : Form
    {
        public AssetWorkspace Workspace { get; }
        public AssetsManager Am { get; }
        public AssetsFileInstance MainFile { get; }
        public bool FromBundle { get; }

        public string AssetsFileName { get; }
        public string AssetsRootDir { get; }
        public string UnityVersion { get; }
        private readonly List<AssetDetailsListItem> _assetDetails;
        public Dictionary<BundleReplacer, MemoryStream> ModifiedFiles { get; private set; }

        //extra (todo)
        //AssetNameSearch assetNameSearch;
        //AssetSearch assetSearch;
        //MonobehaviourSearch monoSearch;
        //TransformSearch transformSearch;
        //Dependencies dependencies;

        public AssetsViewer(AssetsManager am, AssetsFileInstance instance, bool fromBundle = false)
        {
            InitializeComponent();

            Workspace = new AssetWorkspace(am, instance, fromBundle);
            Am = Workspace.Am;
            MainFile = Workspace.MainFile;
            FromBundle = Workspace.FromBundle;
            AssetsFileName = Workspace.AssetsFileName;
            AssetsRootDir = Workspace.AssetsRootDir;
            UnityVersion = instance.file.typeTree.unityVersion;
            SetFormText();
            _assetDetails = new List<AssetDetailsListItem>();
            ModifiedFiles = new Dictionary<BundleReplacer, MemoryStream>();
            LoadAssetsToList();
        }

        private void SetFormText() => Text = $@"Assets Info ({UnityVersion})";

        private void LoadAssetsToList()
        {
            MainFile.file.reader.bigEndian = false;
            Workspace.LoadedFiles.Add(MainFile);
            foreach (var info in MainFile.table.assetFileInfo) 
                AddAssetItem(MainFile, info);

            var id = 1;
            foreach (var dep in MainFile.dependencies.Where(dep => dep != null))
            {
                dep.file.reader.bigEndian = false;
                foreach (var inf in dep.table.assetFileInfo)
                    AddAssetItem(dep, inf, id);
                Workspace.LoadedFiles.Add(dep);
                id++;
            }
        }

        private void AddAssetItem(AssetsFileInstance file, AssetFileInfoEx info, int fileId = 0)
        {
            var thisFile = file.file;
            var cldb = Am.classFile;
            var cldbType = AssetHelper.FindAssetClassByID(cldb, info.curFileType);
            var name = AssetHelper.GetAssetNameFast(thisFile, cldb, info); //handles both cldb and typetree
            const string container = "";
            string type;
            var typeId = info.curFileType;
            var pathId = info.index;
            var size = (int)info.curFileSize;
            const string modified = "";
            ushort monoId = 0xFFFF;

            var hasTypeTree = thisFile.typeTree.hasTypeTree;
            if (hasTypeTree)
            {
                var ttType = AssetHelper.FindTypeTreeTypeByID(thisFile.typeTree, typeId);
                if (ttType == null)
                {
                    type = $"0x{typeId:X8}";
                }
                else if (ttType.typeFieldsEx.Length != 0)
                {
                    type = ttType.typeFieldsEx[0].GetTypeString(ttType.stringTable);
                }
                else
                {
                    type = cldbType != null ?
                        cldbType.name.GetString(cldb) : $"0x{typeId:X8}";
                }
            }
            else
            {
                type = cldbType != null ?
                    cldbType.name.GetString(cldb) : $"0x{typeId:X8}";
            }

            switch (typeId)
            {
                case 0x01:
                    name = $"{type} {name}";
                    break;
                case 0x72:
                    monoId = (ushort)(0xFFFFFFFF - info.curFileTypeOrIndex);
                    if (type != name)
                        name = $"{type} {name}";
                    break;
            }

            if (!HasName(cldb, cldbType)) 
                name = "Unnamed asset";

            _assetDetails.Add(new AssetDetailsListItem
            {
                Name = name,
                Container = container,
                Type = type,
                TypeID = typeId,
                FileID = fileId,
                PathID = pathId,
                Size = size,
                Modified = modified,
                Position = info.absoluteFilePos,
                MonoID = monoId
            });
            var item = new[]
            {
                name,
                container,
                type,
                typeId.ToString(),
                fileId.ToString(),
                pathId.ToString(),
                size.ToString(),
                modified
            };

            assetList.Items.Add(new ListViewItem(item));
        }

        private static bool HasName(ClassDatabaseFile cldb, ClassDatabaseType cldbType)
        {
            return cldbType != null && cldbType.fields.Any(f => f.fieldName.GetString(cldb) == "m_Name");
        }

        private static bool HasAnyField(ClassDatabaseType cldbType) => cldbType != null && cldbType.fields.Any();

        private void AddAssetItems(IEnumerable<AssetDetailsListItem> listItems)
        {
            foreach (var listItem in listItems)
            {
                Workspace.Modified = true;

                var name = listItem.Name;
                var typeId = listItem.TypeID;

                if (string.IsNullOrEmpty(listItem.Name))
                {
                    name = "Unnamed asset";
                }
                else
                {
                    if (typeId is 0x01 or 0x72)
                    {
                        name = $"{listItem.Type} {name}";
                    }
                }

                _assetDetails.Add(listItem);

                #region New asset
                var cldb = Am.classFile;
                var cldbType = AssetHelper.FindAssetClassByID(cldb, typeId);

                if (!HasName(cldb, cldbType))
                    name = "Unnamed asset";

                var templateField = new AssetTypeTemplateField();
                templateField.FromClassDatabase(cldb, cldbType, 0);
                var baseField = ValueBuilder.DefaultValueFieldFromTemplate(templateField);
                #endregion

                Workspace.AddReplacer(AssetModifier.CreateAssetReplacer(listItem, baseField.WriteToByteArray()));
                var item = new[]
                {
                    name,
                    listItem.Container,
                    listItem.Type,
                    typeId.ToString(),
                    listItem.FileID.ToString(),
                    listItem.PathID.ToString(),
                    listItem.Size.ToString(),
                    listItem.Modified
                };
                assetList.Items.Insert(0, new ListViewItem(item));
            }
        }

        private void RemoveAssetItems()
        {
            Workspace.Modified = true;

            var choice = MsgBoxUtils.ShowWarningDialog("Are you sure you want to remove the selected asset(s)?\n" +
                                                       "This will break any reference to this/these.");
            if (choice != DialogResult.Yes) return;

            foreach (ListViewItem item in assetList.SelectedItems)
            {
                var listItem = _assetDetails[item.Index];
                Workspace.AddReplacer(AssetModifier.CreateAssetRemover(listItem));
                _assetDetails.Remove(listItem);
                assetList.Items.Remove(item);
            }
        }

        //not used now, was originally for the size thing
        //private string ConvertSizes(uint size)
        //{
        //    if (size >= 1048576)
        //        return (size / 1048576f).ToString("N2") + "m";
        //    else if (size >= 1024)
        //        return (size / 1024f).ToString("N2") + "k";
        //    return size + "b";
        //}

        private List<AssetDetailsListItem> GetSelectedListItems()
        {
            return (from int index in assetList.SelectedIndices select _assetDetails[index]).ToList(); // probably wrong
        }

        private List<AssetTypeValueField> GetSelectedFieldsReplaced()
        {
            try
            {
                return GetSelectedExternalsReplaced().Select(ext => ext.instance.GetBaseField()).ToList();
            }
            catch
            {
                MsgBoxUtils.ShowErrorDialog("Unable to process the asset data!\n" +
                                            "This might be due to an incompatible type database.");
                return null;
            }
        }

        private IEnumerable<AssetExternal> GetSelectedExternalsReplaced(bool onlyInfo = false)
        {
            return from AssetDetailsListItem listItem in GetSelectedListItems()
                let file = Workspace.LoadedFiles[listItem.FileID]
                let assetId = new AssetID(file.path, listItem.PathID)
                select Workspace.NewAssetDatas.ContainsKey(assetId) ?
                    Am.GetExtAssetNewData(file, 0, listItem.PathID, Workspace.NewAssetDatas[assetId], onlyInfo) :
                    Am.GetExtAsset(file, 0, listItem.PathID, onlyInfo);
        }

        private void SetSelectedFieldsModified()
        {
            var listItems = GetSelectedListItems();
            for (var i = 0; i < listItems.Count; i++)
            {
                listItems[i].Modified = "*";
                assetList.SelectedItems[i].SubItems[7].Text = @"*";
            }
        }

        private void ClearModified()
        {
            var items = assetList.SelectedItems;
            for (var i = 0; i < items.Count; i++)
            {
                _assetDetails[i].Modified = "";
                assetList.SelectedItems[i].SubItems[7].Text = "";
            }
        }

        private void assetList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (_assetDetails.Count != assetList.Items.Count) return; // shouldn't happen
            var details = _assetDetails[e.ItemIndex];
            boxName.Text = details.Name;
            boxPathID.Text = details.PathID.ToString();
            boxFileID.Text = details.FileID.ToString();
            if (details.TypeID == 0x72)
            {
                boxType.Text = $@"0x{details.MonoID:X8} ({details.Type})";
                return;
            }
            if (uint.TryParse(details.Type, out var typeId))
            {
                boxType.Text = details.TypeID == typeId ?
                    $@"0x{details.TypeID:X8}" :
                    $@"0x{details.TypeID:X8} ({details.Type})";
            }
            else
            {
                boxType.Text = $@"0x{details.TypeID:X8} ({details.Type})";
            }
        }

        private void assetList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GetSelectedCount() is not 0) return;
            boxName.Text = "";
            boxPathID.Text = "";
            boxFileID.Text = "";
            boxType.Text = "";
        }

        private void btnViewData_Click(object sender, EventArgs e)
        {
            if (FailIfNothingSelected()) return;
            var baseFields = GetSelectedFieldsReplaced();
            if (baseFields == null) return;
            var cldb = Workspace.Am.classFile;
            var listItems = GetSelectedListItems();
            for (var i = 0; i < listItems.Count; i++)
            {
                var cldbType = AssetHelper.FindAssetClassByID(cldb, listItems[i].TypeID);
                if (cldbType != null)
                {
                    if (HasAnyField(cldbType)) continue;
                    MsgBoxUtils.ShowErrorDialog("This asset has no data to view.");
                }
                else
                {
                    MsgBoxUtils.ShowErrorDialog("Unknown asset format.");
                }
                baseFields.RemoveAt(i);
            }

            foreach (var baseField in baseFields)
                new AssetData(Workspace, baseField).ShowDialog();
        }

        private void AssetsViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Workspace.Modified) AskSaveChanges();
        }

        private void AskSaveChanges()
        {
            if (!Workspace.Modified) return;
            var choice = MsgBoxUtils.ShowInfoDialog("Would you like to save the changes?");
            switch (choice)
            {
                case DialogResult.Yes:
                    SaveFiles();
                    break;
                case DialogResult.No:
                    CloseFiles();
                    break;
            }
        }

        private void SaveFiles(bool overwrite = false)
        {
            var newAssetsList = Workspace.NewAssets.Values.ToList();

            if (FromBundle)
            {
                ModifiedFiles = WriteFilesToMemory(newAssetsList);
                Workspace.Modified = false;
                ClearModified();
            }
            else
            {
                if (overwrite)
                {
                    var choice = MsgBoxUtils.ShowWarningDialog("This action will overwrite the file.\n" +
                                                               "Are you sure you want to continue?");
                    if (choice != DialogResult.Yes) return;
                }
                WriteFiles(newAssetsList, overwrite);
                Workspace.Modified = false;
                ClearModified();
            }
        }

        public void WriteFiles(List<AssetsReplacer> replacers, bool overwrite = false)
        {
            var lastFileId = replacers.Max(r => r.GetFileID());
            for (var fileId = 0; fileId <= lastFileId; fileId++)
            {
                var id = fileId;
                var sortedAssetsList = replacers.Where(r => r.GetFileID() == id).ToList(); // sort the list of replacers by fileID
                var file = Workspace.LoadedFiles[fileId];
                if (overwrite)
                {
                    var path = file.path;
                    var tempPath = Path.Combine(Path.GetTempPath(), file.name);
                    using var fs = File.OpenWrite(tempPath);
                    using var writer = new AssetsFileWriter(fs);
                    file.file.Write(writer, 0, sortedAssetsList, 0);
                    Am.UnloadAssetsFile(path);
                    fs.Close();
                    File.Replace(tempPath, path, path + ".backup");
                    Workspace.LoadedFiles[fileId] = Am.LoadAssetsFile(path, false);
                }
                else
                {
                    var sfd = new SaveFileDialog
                    {
                        Title = @"Save as...",
                        Filter = @"All types (*.*)|*.*|Assets file (*.assets)|*.assets",
                        FileName = file.name
                    };
                    if (sfd.ShowDialog() != DialogResult.OK) continue;
                    using var fs = File.OpenWrite(sfd.FileName);
                    using var writer = new AssetsFileWriter(fs);
                    file.file.Write(writer, 0, sortedAssetsList, 0);
                }
            }
        }

        public Dictionary<BundleReplacer, MemoryStream> WriteFilesToMemory(List<AssetsReplacer> replacers)
        {
            var bunDict = new Dictionary<BundleReplacer, MemoryStream>();
            var lastFileId = replacers.Max(r => r.GetFileID());
            for (var fileId = 0; fileId < lastFileId; fileId++)
            {
                var id = fileId;
                var sortedAssetsList = replacers.Where(r => r.GetFileID() == id).ToList();
                using var ms = new MemoryStream();
                using var writer = new AssetsFileWriter(ms);
                if (fileId == 0)
                {
                    MainFile.file.Write(writer, 0, sortedAssetsList, 0);
                    bunDict.Add(AssetModifier.CreateBundleReplacer(AssetsFileName, true, ms.ToArray()), ms);
                }
                else
                {
                    var dep = MainFile.dependencies[fileId - 1];
                    dep.file.Write(writer, 0, sortedAssetsList, 0);
                    bunDict.Add(AssetModifier.CreateBundleReplacer(dep.name, true, ms.ToArray()), ms);
                }
            }
            return bunDict;
        }

        private void CloseFiles()
        {
            foreach (var dep in MainFile.dependencies)
            {
                Am.files.Remove(dep);
                dep.file.Close();
            }
            Am.files.Remove(MainFile);
            MainFile.file.Close();
            Workspace.Modified = false;
            Close();
        }

        #region // todo, batch import/export
        private void btnExportRaw_Click(object sender, EventArgs e)
        {
            if (FailIfNothingSelected()) return;
            var items = GetSelectedListItems();
            var count = GetSelectedCount();
            if (count is 1)
            {
                var item = items[0];
                var sfd = new SaveFileDialog
                {
                    Title = @"Save raw asset",
                    Filter = @"Raw Unity asset (*.dat)|*.dat",
                    FileName = $"{item.Name}-{AssetsFileName}-{item.PathID}"
                };
                if (sfd.ShowDialog() != DialogResult.OK) return;
                using var fs = File.OpenWrite(sfd.FileName);
                new AssetExporter().ExportRawAsset(MainFile.file, item, fs);
            }
            else
            {
                var fd = new FolderDialog
                {
                    Title = "Select a folder for the raw assets"
                };
                if (fd.ShowDialog() != DialogResult.OK) return;
                for (var i = 0; i < count; i++)
                {
                    var item = items[i];
                    var fileName = $"{item.Name}-{AssetsFileName}-{item.PathID}.dat";
                    using var fs = File.OpenWrite(Path.Combine(fd.Folder, fileName));
                    new AssetExporter().ExportRawAsset(MainFile.file, item, fs);
                }
            }
        }

        private void btnExportDump_Click(object sender, EventArgs e)
        {
            if (FailIfNothingSelected()) return;
            var items = GetSelectedListItems();
            var baseFields = GetSelectedFieldsReplaced();
            if (baseFields == null) return;
            var count = GetSelectedCount();
            if (count is 1)
            {
                var item = items[0];
                var field = baseFields[0];
                var sfd = new SaveFileDialog
                {
                    Title = @"Save dump",
                    Filter = @"UAAE text dump (*.txt)|*.txt",
                    FileName = $"{item.Name}-{AssetsFileName}-{item.PathID}-{item.Type}"
                };
                if (sfd.ShowDialog() != DialogResult.OK) return;
                var fs = File.OpenWrite(sfd.FileName);
                using var writer = new StreamWriter(fs);
                new AssetExporter().ExportDump(field, writer, DumpType.TXT);
            }
            else
            {
                var fd = new FolderDialog
                {
                    Title = "Select a folder for the dumps"
                };
                if (fd.ShowDialog() != DialogResult.OK) return;
                for (var i = 0; i < count; i++)
                {
                    var item = items[i];
                    var field = baseFields[i];
                    var fileName = $"{item.Name}-{AssetsFileName}-{item.PathID}-{item.Type}.txt";
                    var fs = File.OpenWrite(Path.Combine(fd.Folder, fileName));
                    using var writer = new StreamWriter(fs);
                    new AssetExporter().ExportDump(field, writer, DumpType.TXT);
                }
            }
        }

        private void btnImportRaw_Click(object sender, EventArgs e)
        {
            if (FailIfNothingSelected()) return;
            var items = GetSelectedListItems();
            var count = GetSelectedCount();
            if (count is not 1)
            {
                var choice = MsgBoxUtils.ShowWarningDialog("Only one raw asset will be imported.\n" +
                                              "Raw import does not support batch import atm.\n" +
                                              "Are you sure you want to continue?");
                if (choice != DialogResult.Yes) return;
            }
            var item = items[0];
            var ofd = new OpenFileDialog
            {
                Title = @"Import raw asset",
                Filter = @"Raw Unity asset (*.dat)|*.dat"
            };
            if (ofd.ShowDialog() != DialogResult.OK) return;
            using var fs = File.OpenRead(ofd.FileName);
            var replacer = new AssetImporter().ImportRawAsset(item, fs);
            Workspace.AddReplacer(replacer);
            SetSelectedFieldsModified();
        }

        private void btnImportDump_Click(object sender, EventArgs e)
        {
            if (FailIfNothingSelected()) return;
            var items = GetSelectedListItems();
            var count = GetSelectedCount();
            if (count is not 1)
            {
                var choice = MsgBoxUtils.ShowWarningDialog("Only one asset dump will be imported.\n" +
                                                           "Dump import does not support batch import atm.\n" +
                                                           "Are you sure you want to continue?");
                if (choice != DialogResult.Yes) return;
            }
            var item = items[0];
            var ofd = new OpenFileDialog
            {
                Title = @"Import dump",
                Filter = @"UAAE text dump (*.txt)|*.txt"
            };
            if (ofd.ShowDialog() != DialogResult.OK) return;
            using var fs = File.OpenRead(ofd.FileName);
            using var reader = new StreamReader(fs);
            var replacer = new AssetImporter().ImportDump(Workspace, item, reader, DumpType.TXT);
            if (replacer == null) return;
            Workspace.AddReplacer(replacer);
            SetSelectedFieldsModified();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            //if (FailIfNothingSelected()) return;
            // todo
        }
        #endregion

        private void addButton_Click(object sender, EventArgs e)
        {
            var addAssetDialog = new AddAssetsDialog(Workspace);
            if (addAssetDialog.ShowDialog() != DialogResult.OK) return;
            AddAssetItems(addAssetDialog.ListItems);
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            if (FailIfNothingSelected()) return;
            RemoveAssetItems();
        }

        private bool FailIfNothingSelected()
        {
            switch (GetSelectedCount())
            {
                case 0:
                    MsgBoxUtils.ShowErrorDialog("No item selected.");
                    return true;
                default:
                    return false;
            }
        }

        private int GetSelectedCount() => assetList.SelectedItems.Count;

        private void saveToolStripMenuItem_Click(object sender, EventArgs e) => SaveFiles(true);

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) => SaveFiles();

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Workspace.Modified) AskSaveChanges();
            else CloseFiles();
        }

        private void byNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // todo
        }

        private void binaryContentSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // todo
        }

        private void monobehaviourSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // todo
        }

        private void transformSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // todo
        }

        private void dependenciesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // todo
        }
    }
}