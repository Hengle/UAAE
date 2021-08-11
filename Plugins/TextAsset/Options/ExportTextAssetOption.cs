using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using AssetsAdvancedEditor.Assets;
using AssetsAdvancedEditor.Plugins;
using AssetsAdvancedEditor.Utils;
using UnityTools;

namespace TextAsset.Options
{
    public class ExportTextAssetOption : PluginOption
    {
        public ExportTextAssetOption() => Action = PluginAction.Export;

        public override bool IsValidForPlugin(AssetsManager am, List<AssetItem> selectedItems)
        {
            Description = selectedItems.Count > 1 ? "Batch export .txt" : "Export .txt";

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
                ? BatchExport(owner, workspace, selectedItems)
                : SingleExport(owner, workspace, selectedItems[0]);
        }

        public bool BatchExport(IWin32Window owner, AssetsWorkspace workspace, List<AssetItem> selectedItems)
        {
            var ofd = new OpenFolderDialog
            {
                Title = "Select export directory"
            };
            if (ofd.ShowDialog(owner) != DialogResult.OK)
                return false;

            var dir = ofd.Folder;
            foreach (var item in selectedItems)
            {
                var baseField = workspace.GetBaseField(item);
                var strBytes = baseField.Get("m_Script").GetValue().AsStringBytes();
                var name = Extensions.ReplaceInvalidFileNameChars(item.Name);
                if (string.IsNullOrEmpty(name))
                {
                    name = "Unnamed asset";
                }
                var file = Path.Combine(dir, $"{name}-{item.Cont.FileInstance.name}-{item.PathID}.txt");
                File.WriteAllBytes(file, strBytes);
            }
            return true;
        }

        public bool SingleExport(IWin32Window owner, AssetsWorkspace workspace, AssetItem selectedItem)
        {
            var name = Extensions.ReplaceInvalidFileNameChars(selectedItem.Name);
            if (string.IsNullOrEmpty(name))
            {
                name = "Unnamed asset";
            }
            var sfd = new SaveFileDialog
            {
                Title = @"Save text file",
                Filter = @"TXT file (*.txt)|*.txt",
                FileName = $"{name}-{selectedItem.Cont.FileInstance.name}-{selectedItem.PathID}"
            };
            if (sfd.ShowDialog(owner) != DialogResult.OK)
                return false;

            var baseField = workspace.GetBaseField(selectedItem);
            var strBytes = baseField.Get("m_Script").GetValue().AsStringBytes();
            var file = sfd.FileName;
            File.WriteAllBytes(file, strBytes);
            return true;
        }
    }
}
