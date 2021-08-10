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
        public AssetItem Item { get; set; }
        public List<string> MatchingFiles { get; set; }
        public bool HasMatchingFile => MatchingFiles.Count > 0;

        public string GetMatchName(string ext, BatchImportType batchType)
        {
            if (batchType is BatchImportType.Image)
            {
                return $"-{File}-{PathID}{ext}";
            }
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
