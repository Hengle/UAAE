using System.Collections.Generic;

namespace AssetsAdvancedEditor.Assets
{
    public class BatchImportItem
    {
        public string Description { get; set; }
        public string File { get; set; }
        public long PathID { get; set; }

        public string Type { get; set; }
        public string ImportFile { get; set; }
        public AssetContainer Cont { get; set; }
        public List<string> MatchingFiles { get; set; }
        public int SelectedIndex { get; set; }

        public string GetMatchName(string ext)
        {
            return $"-{File}-{PathID}-{Type}{ext}";
        }

        public string[] ToArray()
        {
            return new[]
            {
                Description,
                File,
                PathID.ToString()
            };
        }
    }
}
