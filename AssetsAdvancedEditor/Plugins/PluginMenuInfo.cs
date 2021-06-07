namespace AssetsAdvancedEditor.Plugins
{
    public class PluginMenuInfo
    {
        public PluginInfo PluginInf;
        public PluginOption PluginOpt;
        public string DisplayName;

        public PluginMenuInfo(PluginInfo pluginInf, PluginOption pluginOpt)
        {
            PluginInf = pluginInf;
            PluginOpt = pluginOpt;
            DisplayName = pluginOpt.Description;
        }

        public override string ToString() => DisplayName;
    }
}
