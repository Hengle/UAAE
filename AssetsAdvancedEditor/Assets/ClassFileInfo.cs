using System.Linq;
using AssetsTools.NET;

namespace AssetsAdvancedEditor.Assets
{
    public class ClassFileInfo
    {
        public ClassDatabaseFile Cldb;
        public string Name;
        public ClassFileInfo(ClassDatabaseFile cldb)
        {
            Cldb = cldb;

            var unityVersions = cldb.header.unityVersions;
            var nonWildcardVersion = unityVersions.FirstOrDefault(v => !v.EndsWith(".*"));
            Name = nonWildcardVersion ?? unityVersions[0];
        }

        public override string ToString() => Name;
    }
}
