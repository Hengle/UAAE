using System.Collections.Generic;
using System.IO;
using System.Linq;
using AssetsAdvancedEditor.Utils;
using AssetsTools.NET;
using AssetsTools.NET.Extra;

namespace AssetsAdvancedEditor.Assets
{
    public class AssetsWorkspace
    {
        public AssetsManager Am { get; }
        public AssetsFileInstance MainFile { get; }
        public bool FromBundle { get; }

        public List<AssetsFileInstance> LoadedFiles { get; }
        public List<AssetDetailsListItem> LoadedAssets { get; }

        public AssetImporter Importer { get; }
        public AssetExporter Exporter { get; }

        public Dictionary<AssetID, AssetsReplacer> NewAssets { get; }
        public Dictionary<AssetID, MemoryStream> NewAssetDatas { get; }

        public bool Modified { get; set; }
        public string AssetsFileName { get; }
        public string AssetsRootDir { get; }
        public string UnityVersion { get; }

        public AssetsWorkspace(AssetsManager am, AssetsFileInstance file, bool fromBundle = false)
        {
            Am = am;
            MainFile = file;
            FromBundle = fromBundle;

            LoadedFiles = new List<AssetsFileInstance>();
            LoadedAssets = new List<AssetDetailsListItem>();

            Importer = new AssetImporter(this);
            Exporter = new AssetExporter(this);

            NewAssets = new Dictionary<AssetID, AssetsReplacer>();
            NewAssetDatas = new Dictionary<AssetID, MemoryStream>();

            Modified = false;

            AssetsFileName = file.name;
            AssetsRootDir = Path.GetDirectoryName(AssetsFileName);
            UnityVersion = MainFile.file.typeTree.unityVersion;
        }

        public void AddReplacer(AssetsReplacer replacer, MemoryStream previewStream = null)
        {
            var forInstance = LoadedFiles[replacer.GetFileID()];
            var assetId = new AssetID(forInstance.path, replacer.GetPathID());

            NewAssets[assetId] = replacer;
            if (previewStream == null)
            {
                var newStream = new MemoryStream();
                var newWriter = new AssetsFileWriter(newStream);
                replacer.Write(newWriter);
                newStream.Position = 0;
                NewAssetDatas[assetId] = newStream;
            }
            else
            {
                NewAssetDatas[assetId] = previewStream;
            }

            Modified = true;
        }

        public void RemoveReplacer(AssetsReplacer replacer, bool closePreviewStream = true)
        {
            var forInstance = LoadedFiles[replacer.GetFileID()];
            var assetId = new AssetID(forInstance.path, replacer.GetPathID());

            NewAssets.Remove(assetId);
            if (NewAssetDatas.ContainsKey(assetId))
            {
                if (closePreviewStream)
                    NewAssetDatas[assetId].Close();
                NewAssetDatas.Remove(assetId);
            }

            Modified = NewAssets.Any();
        }

        public AssetData GetAssetData(AssetDetailsListItem listItem, bool onlyGetInfo = false, bool forceFromCldb = false)
        {
            var file = LoadedFiles[listItem.FileID];
            var assetId = new AssetID(file.path, listItem.PathID);
            AssetData data;
            if (NewAssetDatas.ContainsKey(assetId))
            {
                data = new AssetData
                {
                    AssetItem = listItem,
                    Instance = !onlyGetInfo ? GetTypeInstanceNewData(file, listItem, NewAssetDatas[assetId], forceFromCldb) : null,
                    Replacer = NewAssets[assetId],
                    File = file
                };
            }
            else
            {
                var info = file.table.GetAssetInfo(listItem.PathID);
                data = new AssetData
                {
                    AssetItem = listItem,
                    Instance = !onlyGetInfo ? Am.GetTypeInstance(file, info, forceFromCldb) : null,
                    File = file
                };
            }
            return data;
        }

        public AssetTypeInstance GetTypeInstanceNewData(AssetsFileInstance file, AssetDetailsListItem listItem,
            MemoryStream ms, bool forceFromCldb = false)
        {
            return new (Am.MakeTemplateBaseField(file.file, listItem, forceFromCldb), new AssetsFileReader(ms), 0);
        }
    }
}
