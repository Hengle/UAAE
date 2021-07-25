using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AssetsAdvancedEditor.Assets;
using AssetsAdvancedEditor.Plugins;
using AssetsAdvancedEditor.Utils;
using AssetsTools.NET;
using AssetsTools.NET.Extra;

namespace Texture.Options
{
    public class ExportTextureOption : PluginOption
    {
        public ExportTextureOption() => Action = PluginAction.Export;

        public override bool IsValidForPlugin(AssetsManager am, List<AssetContainer> selectedAssets)
        {
            Description = selectedAssets.Count > 1 ? "Batch export Png/Tga" : "Export Png/Tga";

            var classId = AssetHelper.FindAssetClassByName(am.classFile, "Texture2D").classId;

            foreach (var cont in selectedAssets)
            {
                if (cont.Item.TypeID != classId)
                    return false;
            }
            return true;
        }

        public override bool ExecutePlugin(IWin32Window owner, AssetsWorkspace workspace, List<AssetContainer> selectedAssets)
        {
            return selectedAssets.Count > 1
                ? BatchExport(owner, workspace, selectedAssets)
                : SingleExport(owner, workspace, selectedAssets[0]);
        }

        public bool BatchExport(IWin32Window owner, AssetsWorkspace workspace, List<AssetContainer> selectedAssets)
        {
            for (var i = 0; i < selectedAssets.Count; i++)
            {
                selectedAssets[i] = new AssetContainer(selectedAssets[i], TextureHelper.GetByteArrayTexture(workspace, selectedAssets[i]));
            }

            var ofd = new OpenFolderDialog
            {
                Title = "Select export directory"
            };
            if (ofd.ShowDialog(owner) != DialogResult.OK)
                return false;

            var dir = ofd.Folder;
            var errorBuilder = new StringBuilder();
            foreach (var cont in selectedAssets)
            {
                var errorAssetName = $"{Path.GetFileName(cont.FileInstance.path)}/{cont.AssetId.pathID}";
                var texBaseField = cont.TypeInstance.GetBaseField();
                var texFile = TextureFile.ReadTextureFile(texBaseField);

                //0x0 texture, usually called like Font Texture or smth
                if (texFile.m_Width == 0 && texFile.m_Height == 0)
                    continue;

                var file = Path.Combine(dir, $"{texFile.m_Name}-{Path.GetFileName(cont.FileInstance.path)}-{cont.Item.PathID}.png");

                //bundle resS
                if (!TextureHelper.GetResSTexture(texFile, cont))
                {
                    var resSName = Path.GetFileName(texFile.m_StreamData.path);
                    errorBuilder.AppendLine($"[{errorAssetName}]: resS was detected but {resSName} was not found in bundle");
                    continue;
                }

                var data = TextureHelper.GetRawTextureBytes(texFile, cont.FileInstance);

                if (data == null)
                {
                    var resSName = Path.GetFileName(texFile.m_StreamData.path);
                    errorBuilder.AppendLine($"[{errorAssetName}]: resS was detected but {resSName} was not found on disk");
                    continue;
                }

                var success = TextureManager.ExportTexture(data, file, texFile.m_Width, texFile.m_Height, (TextureFormat)texFile.m_TextureFormat);
                if (success) continue;
                errorBuilder.AppendLine($"[{errorAssetName}]: Failed to decode texture with '{(TextureFormat)texFile.m_TextureFormat}' format");
            }

            if (errorBuilder.Length > 0)
            {
                var firstLines = errorBuilder.ToString().Split('\n').Take(20).ToArray();
                var firstLinesStr = string.Join('\n', firstLines);
                MsgBoxUtils.ShowErrorDialog("Some errors occurred while exporting", firstLinesStr);
            }

            return true;
        }

        public bool SingleExport(IWin32Window owner, AssetsWorkspace workspace, AssetContainer selectedAsset)
        {
            var texField = TextureHelper.GetByteArrayTexture(workspace, selectedAsset).GetBaseField();
            var texFile = TextureFile.ReadTextureFile(texField);
            var sfd = new SaveFileDialog
            {
                Title = @"Save texture",
                Filter = @"PNG file (*.png)|*.png|TGA file (*.tga)|*.tga|All types (*.*)|*.*",
                FileName = $"{texFile.m_Name}-{Path.GetFileName(selectedAsset.FileInstance.path)}-{selectedAsset.Item.PathID}"
            };
            if (sfd.ShowDialog(owner) != DialogResult.OK)
                return false;

            var file = sfd.FileName;
            var errorAssetName = $"{Path.GetFileName(selectedAsset.FileInstance.path)}/{selectedAsset.AssetId.pathID}";

            //bundle resS
            if (!TextureHelper.GetResSTexture(texFile, selectedAsset))
            {
                var resSName = Path.GetFileName(texFile.m_StreamData.path);
                MsgBoxUtils.ShowErrorDialog($"[{errorAssetName}]: resS was detected but {resSName} was not found in bundle");
                return false;
            }

            var data = TextureHelper.GetRawTextureBytes(texFile, selectedAsset.FileInstance);

            if (data == null)
            {
                var resSName = Path.GetFileName(texFile.m_StreamData.path);
                MsgBoxUtils.ShowErrorDialog($"[{errorAssetName}]: resS was detected but {resSName} was not found on disk");
                return false;
            }

            var success = TextureManager.ExportTexture(data, file, texFile.m_Width, texFile.m_Height, (TextureFormat)texFile.m_TextureFormat);
            if (!success)
            {
                MsgBoxUtils.ShowErrorDialog(owner, $"[{errorAssetName}]: Failed to decode texture with '{(TextureFormat)texFile.m_TextureFormat}' format");
            }
            return success;
        }
    }
}
