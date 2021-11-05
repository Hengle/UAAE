using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AssetsAdvancedEditor.Assets;

namespace AssetsAdvancedEditor.Winforms
{
    public partial class BatchImport : Form
    {
        private BatchImportType batchType { get; }
        private string directory { get; }
        public List<BatchImportItem> batchItems;

        public BatchImport(List<AssetItem> selectedItems, string directory, BatchImportType batchType)
        {
            InitializeComponent();
            this.batchType = batchType;
            this.directory = directory;
            batchItems = new List<BatchImportItem>();

            var extensions = GetMatchingExtensions(batchType);
            foreach (var item in selectedItems)
            {
                var batchItem = new BatchImportItem
                {
                    Description = item.ListName,
                    File = Path.GetFileName(item.Cont.FileInstance.path),
                    PathID = item.PathID,
                    Type = item.Type,
                    Item = item
                };

                var allMatchingFiles = new List<string>();
                foreach (var ext in extensions)
                {
                    var endWith = batchItem.GetMatchName(ext, batchType);
                    var matchingFiles = Directory.GetFiles(directory, "*" + endWith).Select(Path.GetFileName).ToList();
                    allMatchingFiles.AddRange(matchingFiles);
                }
                batchItem.MatchingFiles = allMatchingFiles;
                batchItem.SelectedIndex = allMatchingFiles.Count > 0 ? 0 : -1;
                batchItems.Add(batchItem);
                affectedAssetsList.Items.Add(new ListViewItem(batchItem.ToArray()));
            }
        }

        private static string[] GetMatchingExtensions(BatchImportType batchType)
        {
            return batchType switch
            {
                BatchImportType.Dump => new[] { ".dat", ".txt" },
                BatchImportType.Image => new[] { ".png", ".tga" },
                BatchImportType.Mesh => new[] { ".fbx" },
                _ => null
            };
        }

        private void lboxMatchingFiles_MouseHover(object sender, EventArgs e)
        {
            var pos = PointToClient(Cursor.Position);
            var toolTip = new ToolTip
            {
                IsBalloon = true
            };
            toolTip.Show("Select an item to import.", this, pos, 1000);
        }

        private void lboxMatchingFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FailIfNothingSelected()) return;

            var batchItem = GetSelectedBatchItem();
            if (lboxMatchingFiles.SelectedIndex != -1)
            {
                batchItem.SelectedIndex = lboxMatchingFiles.SelectedIndex;
            }
        }

        private void affectedAssetsList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (FailIfNothingSelected()) return;

            var batchItem = GetSelectedBatchItem();
            lboxMatchingFiles.Items.Clear();
            foreach (var matchingFile in batchItem.MatchingFiles)
            {
                lboxMatchingFiles.Items.Add(matchingFile);
            }
            lboxMatchingFiles.SelectedIndex = batchItem.SelectedIndex;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < batchItems.Count; i++)
            {
                var batchItem = batchItems[i];
                if (batchItem.HasMatchingFile)
                {
                    batchItem.ImportFile = Path.Combine(directory, batchItem.GetSelectedFile());
                    continue;
                }
                batchItems.RemoveAt(i);
                i--;
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            var batchItem = GetSelectedBatchItem();

            OpenFileDialog ofd;
            switch (batchType)
            {
                case BatchImportType.Dump:
                {
                    ofd = new OpenFileDialog
                    {
                        Title = @"Import dump/raw asset",
                        Filter = @"UAAE text dump (*.txt)|*.txt|Raw Unity asset (*.dat)|*.dat|All types (*.*)|*.*"
                    };
                    break;
                }
                case BatchImportType.Image:
                {
                    ofd = new OpenFileDialog
                    {
                        Title = @"Import png/tga image",
                        Filter = @"PNG file (*.png)|*.png|TGA file (*.tga)|*.tga|All types (*.*)|*.*"
                    };
                    break;
                }
                default:
                    return;
            }
            if (ofd.ShowDialog() != DialogResult.OK) return;

            var newMatchingFile = ofd.FileName;
            batchItem.MatchingFiles.Insert(0, newMatchingFile);
            lboxMatchingFiles.Items.Insert(0, newMatchingFile);
            batchItem.SelectedIndex += 1;
        }

        private bool FailIfNothingSelected() => affectedAssetsList.SelectedItems.Count == 0;
        private BatchImportItem GetSelectedBatchItem() => batchItems[affectedAssetsList.SelectedIndices[0]];
    }
}
