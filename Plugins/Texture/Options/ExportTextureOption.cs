using System.Collections.Generic;
using System.IO;
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

            foreach (var cont in selectedAssets)
            {
                var texBaseField = cont.TypeInstance.GetBaseField();
                var texFile = TextureFile.ReadTextureFile(texBaseField);

                //0x0 texture, usually called like Font Texture or smth
                if (texFile.m_Width == 0 && texFile.m_Height == 0)
                    continue;

                var file = Path.Combine(dir, $"{texFile.m_Name}-{Path.GetFileName(cont.FileInstance.path)}-{cont.Item.PathID}.png");

                //bundle resS
                if (!TextureHelper.GetResSTexture(texFile, cont))
                {
                    MsgBoxUtils.ShowErrorDialog(owner, ".resS was detected but no file was found in bundle!");
                    return false;
                }

                var data = TextureHelper.GetRawTextureBytes(texFile, cont.FileInstance);

                var success = TextureManager.ExportTexture(data, file, texFile.m_Width, texFile.m_Height, (TextureFormat)texFile.m_TextureFormat);
                if (success) continue;
                MsgBoxUtils.ShowErrorDialog(owner, $"Failed to decode texture\n({(TextureFormat)texFile.m_TextureFormat})");
                return false;
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

            //bundle resS
            if (!TextureHelper.GetResSTexture(texFile, selectedAsset))
            {
                MsgBoxUtils.ShowErrorDialog(owner, ".resS was detected but no file was found in bundle!");
                return false;
            }

            var data = TextureHelper.GetRawTextureBytes(texFile, selectedAsset.FileInstance);

            var success = TextureManager.ExportTexture(data, file, texFile.m_Width, texFile.m_Height, (TextureFormat)texFile.m_TextureFormat);
            if (!success)
            {
                MsgBoxUtils.ShowErrorDialog(owner, $"Failed to decode texture\n({(TextureFormat)texFile.m_TextureFormat})");
            }
            return success;
        }
    }
}
