using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using AssetsAdvancedEditor.Assets;
using AssetsAdvancedEditor.Plugins;
using AssetsAdvancedEditor.Utils;
using AssetsAdvancedEditor.Winforms;
using UnityTools;

namespace TextAsset.Options
{
    public class ImportTextAssetOption : PluginOption
    {
        public ImportTextAssetOption() => Action = PluginAction.Import;

        public override bool IsValidForPlugin(AssetsManager am, List<AssetItem> selectedItems)
        {
            Description = selectedItems.Count > 1 ? "Batch import .txt" : "Import .txt";

            var classId = AssetHelper.FindAssetClassByName(am.classFile, "TextAsset").classId;

            foreach (var item in selectedItems)
            {
                if (item.TypeID != classId)
                    return false;
            }
            return true;
        }

        public override bool ExecutePlugin(IWin32Window owner, AssetsWorkspace workspace, List<AssetItem> selectedItems)
        {
            return selectedItems.Count > 1
                ? BatchImport(owner, workspace, selectedItems)
                : SingleImport(owner, workspace, selectedItems[0]);
        }

        public bool BatchImport(IWin32Window owner, AssetsWorkspace workspace, List<AssetItem> selectedItems)
        {
            var ofd = new OpenFolderDialog
            {
                Title = "Select import directory"
            };
            if (ofd.ShowDialog(owner) != DialogResult.OK)
                return false;

            var dir = ofd.Folder;
            var dialog = new BatchImport(selectedItems, dir, BatchImportType.Dump);
            if (dialog.ShowDialog(owner) != DialogResult.OK)
                return false;

            foreach (var batchItem in dialog.batchItems)
            {
                if (batchItem.HasMatchingFile)
                {
                    var item = batchItem.Item;
                    var baseField = workspace.GetBaseField(batchItem.Item);
                    var strBytes = File.ReadAllBytes(batchItem.ImportFile);
                    baseField.Get("m_Script").GetValue().Set(strBytes);
                    var savedAsset = baseField.WriteToByteArray();
                    var replacer = AssetModifier.CreateAssetReplacer(item, savedAsset);
                    workspace.AddReplacer(ref item, replacer, new MemoryStream(savedAsset));
                }
            }
            return true;
        }

        public bool SingleImport(IWin32Window owner, AssetsWorkspace workspace, AssetItem selectedItem)
        {
            var sfd = new OpenFileDialog
            {
                Title = @"Open text file",
                Filter = @"TXT file (*.txt)|*.txt",
            };
            if (sfd.ShowDialog(owner) != DialogResult.OK)
                return false;

            var baseField = workspace.GetBaseField(selectedItem);
            var strBytes = File.ReadAllBytes(sfd.FileName);
            baseField.Get("m_Script").GetValue().Set(strBytes);
            var savedAsset = baseField.WriteToByteArray();
            var replacer = AssetModifier.CreateAssetReplacer(selectedItem, savedAsset);
            workspace.AddReplacer(ref selectedItem, replacer, new MemoryStream(savedAsset));
            return true;
        }
    }
}
