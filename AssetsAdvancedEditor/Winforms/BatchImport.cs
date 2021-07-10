using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AssetsAdvancedEditor.Assets;
using AssetsAdvancedEditor.Utils;

namespace AssetsAdvancedEditor.Winforms
{
    public partial class BatchImport : Form
    {
        private string directory { get; }
        public List<BatchImportItem> batchItems;

        public BatchImport(AssetsWorkspace workspace, List<AssetContainer> selectedAssets, string directory, params string[] extensions)
        {
            InitializeComponent();

            this.directory = directory;
            batchItems = new List<BatchImportItem>();

            var filesInDir = new List<string>();
            foreach (var ext in extensions)
            {
                filesInDir.AddRange(Directory.GetFiles(directory, "*" + ext));
            }

            foreach (var cont in selectedAssets)
            {
                var item = cont.Item;
                Extensions.GetUAAENameFast(workspace, item, out _, out var name);
                var batchItem = new BatchImportItem
                {
                    Description = name,
                    File = Path.GetFileName(cont.FileInstance.path),
                    PathID = cont.Item.PathID,
                    Type = item.Type,
                    Cont = cont
                };

                var matchingFiles = new List<string>();
                foreach (var ext in extensions)
                {
                    var endWith = batchItem.GetMatchName(ext);
                    matchingFiles.AddRange(filesInDir.Where(f => f.EndsWith(endWith)).Select(Path.GetFileName).ToList());
                }
                batchItem.MatchingFiles = matchingFiles;
                batchItem.SelectedIndex = matchingFiles.Count > 0 ? 0 : -1;
                batchItems.Add(batchItem);
                affectedAssetsList.Items.Add(new ListViewItem(batchItem.ToArray()));
            }
        }

        private void lboxMatchingFiles_MouseHover(object sender, EventArgs e)
        {
            var pos = PointToClient(Cursor.Position);
            new ToolTip().Show("Double click an item to move it up and use it for import.", this, pos, 3000);
        }

        private void lboxMatchingFiles_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (affectedAssetsList.SelectedItems.Count == 0) return;

            var batchItem = GetSelectedBatchItem();
            var selIndex = lboxMatchingFiles.SelectedIndex;
            if (selIndex == -1) return;

            var matchingFile = batchItem.MatchingFiles[selIndex];
            batchItem.MatchingFiles.RemoveAt(selIndex);
            batchItem.MatchingFiles.Insert(0, matchingFile);

            lboxMatchingFiles.Items.RemoveAt(selIndex);
            lboxMatchingFiles.Items.Insert(0, matchingFile);
        }

        private void affectedAssetsList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (affectedAssetsList.SelectedItems.Count == 0) return;

            var batchItem = GetSelectedBatchItem();
            lboxMatchingFiles.Items.Clear();
            foreach (var matchingFile in batchItem.MatchingFiles)
            {
                lboxMatchingFiles.Items.Add(matchingFile);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            foreach (var batchItem in batchItems)
            {
                if (batchItem.SelectedIndex != -1)
                {
                    batchItem.ImportFile = Path.Combine(directory, batchItem.MatchingFiles[0]);
                    continue;
                }
                batchItems.Remove(batchItem);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            var batchItem = GetSelectedBatchItem();

            var ofd = new OpenFileDialog
            {
                Title = @"Import dump/raw asset",
                Filter = @"UAAE text dump (*.txt)|*.txt|Raw Unity asset (*.dat)|*.dat"
            };
            if (ofd.ShowDialog() != DialogResult.OK) return;

            var newMatchingFile = ofd.FileName;
            batchItem.MatchingFiles.Insert(0, newMatchingFile);
            lboxMatchingFiles.Items.Insert(0, newMatchingFile);
        }

        private BatchImportItem GetSelectedBatchItem() => batchItems[affectedAssetsList.SelectedIndices[0]];
    }
}
