using System.Collections.Generic;
using AssetsAdvancedEditor.Plugins;
using Texture.Options;

namespace Texture
{
    public class TexturePlugin : IPlugin
    {
        public PluginInfo Init()
        {
            var info = new PluginInfo
            {
                DisplayName = "Texture Import/Export",
                Options = new List<PluginOption>
                {
                    new ImportTextureOption(),
                    new ExportTextureOption(),
                    new EditTextureOption()
                }
            };
            return info;
        }
    }
}
