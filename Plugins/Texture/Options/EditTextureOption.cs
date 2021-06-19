using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using AssetsAdvancedEditor.Assets;
using AssetsAdvancedEditor.Plugins;
using AssetsTools.NET;
using AssetsTools.NET.Extra;

namespace Texture.Options
{
    public class EditTextureOption : PluginOption
    {
        public EditTextureOption() => Action = PluginAction.Import;

        public override bool IsValidForPlugin(AssetsManager am, List<AssetContainer> selectedAssets)
        {
            Description = "Edit texture";

            if (selectedAssets.Count != 1)
                return false;

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
            var cont = selectedAssets[0];

            var texField = TextureHelper.GetByteArrayTexture(workspace, cont).GetBaseField();
            var texFile = TextureFile.ReadTextureFile(texField);
            var editTexDialog = new EditTextureDialog(texFile, texField);
            var saved = editTexDialog.ShowDialog(owner) == DialogResult.OK;
            if (!saved) return false;

            var savedAsset = texField.WriteToByteArray();
            var replacer = AssetModifier.CreateAssetReplacer(cont.Item, savedAsset);

            workspace.AddReplacer(replacer, new MemoryStream(savedAsset));
            return true;
        }
    }
}
