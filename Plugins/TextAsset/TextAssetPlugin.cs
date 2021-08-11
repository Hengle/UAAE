using System.Collections.Generic;
using AssetsAdvancedEditor.Plugins;
using TextAsset.Options;

namespace TextAsset
{
    public class TextAssetPlugin : IPlugin
    {
        public PluginInfo Init()
        {
            var info = new PluginInfo
            {
                DisplayName = "TextAsset Import/Export",
                Options = new List<PluginOption>
                {
                    new ImportTextAssetOption(),
                    new ExportTextAssetOption()
                }
            };
            return info;
        }
    }
}
