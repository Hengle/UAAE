using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace AssetsAdvancedEditor.Assets
{
    public class ModMakerItem
    {
        public string RootPath { get; set; }
        public string FullPath { get; set; }
        public string RelPath { get; set; }
        public List<ModMakerReplacerItem> Replacers { get; }

        public ModMakerItem(string fullPath)
        {
            Replacers = new List<ModMakerReplacerItem>();
            FullPath = fullPath;
            RootPath = "";
            RelPath = fullPath;
        }

        public ModMakerItem(string fullPath, string rootPath)
        {
            Replacers = new List<ModMakerReplacerItem>();
            FullPath = fullPath;
            RootPath = rootPath;
            RelPath = "";

            UpdateRootPath(rootPath);
        }

        public void UpdateRootPath(string rootPath)
        {
            RootPath = rootPath;
            RelPath = IsPathRootedSafe(rootPath) ? Path.GetRelativePath(rootPath, FullPath) : FullPath;
        }

        private bool IsPathRootedSafe(string path)
        {
            return !string.IsNullOrEmpty(path) && Path.IsPathRooted(path);
        }
    }
}
