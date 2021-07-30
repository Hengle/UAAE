using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AssetsAdvancedEditor.Plugins;
using AssetsAdvancedEditor.Utils;
using AssetsTools.NET;
using AssetsTools.NET.Extra;
using Mono.Cecil;

namespace AssetsAdvancedEditor.Assets
{
    public class AssetsWorkspace
    {
        public AssetsManager Am { get; }
        public PluginManager Pm { get; }
        public AssetsFileInstance MainInstance { get; }
        public bool FromBundle { get; }

        public List<AssetsFileInstance> LoadedFiles { get; }
        public Dictionary<AssetID, AssetContainer> LoadedContainers { get; }

        public Dictionary<string, AssetsFileInstance> LoadedFileLookup { get; }
        public Dictionary<string, AssemblyDefinition> LoadedAssemblies { get; }

        public AssetImporter Importer { get; }
        public AssetExporter Exporter { get; }

        public Dictionary<AssetID, AssetsReplacer> NewAssets { get; }
        public Dictionary<AssetID, Stream> NewAssetDatas { get; }

        public bool Modified { get; set; }
        public string AssetsFileName { get; }
        public string AssetsRootDir { get; }
        public string UnityVersion { get; }

        public AssetsWorkspace(AssetsManager am, AssetsFileInstance file, bool fromBundle = false)
        {
            Am = am;
            Pm = new PluginManager(am);
            Pm.LoadPluginsInDirectory("Plugins");
            MainInstance = file;
            FromBundle = fromBundle;

            LoadedFiles = new List<AssetsFileInstance>();
            LoadedContainers = new Dictionary<AssetID, AssetContainer>();

            LoadedFileLookup = new Dictionary<string, AssetsFileInstance>();
            LoadedAssemblies = new Dictionary<string, AssemblyDefinition>();

            Importer = new AssetImporter(this);
            Exporter = new AssetExporter(this);

            NewAssets = new Dictionary<AssetID, AssetsReplacer>();
            NewAssetDatas = new Dictionary<AssetID, Stream>();

            Modified = false;

            AssetsFileName = file.name;
            AssetsRootDir = Path.GetDirectoryName(AssetsFileName);
            UnityVersion = MainInstance.file.typeTree.unityVersion;
        }

        public void AddReplacer(AssetsReplacer replacer, MemoryStream previewStream = null)
        {
            if (replacer == null) return;
            var forInstance = LoadedFiles[replacer.GetFileID()];
            var assetId = new AssetID(forInstance.path, replacer.GetPathID());
            if (!LoadedContainers.TryGetValue(assetId, out var cont)) return;
            var item = cont.Item;

            if (NewAssets.ContainsKey(assetId))
                RemoveReplacer(replacer);

            NewAssets[assetId] = replacer;

            if (previewStream == null)
            {
                var newStream = new MemoryStream();
                var newWriter = new AssetsFileWriter(newStream);
                replacer.Write(newWriter);
                newStream.Position = 0;
                previewStream = newStream;
            }

            NewAssetDatas[assetId] = previewStream;

            if (replacer is AssetsRemover)
            {
                LoadedContainers.Remove(assetId);
            }
            else
            {
                var reader = new AssetsFileReader(previewStream);
                cont = MakeAssetContainer(item, reader);
                UpdateAssetInfo(cont);
            }

            Modified = true;
        }

        public void RemoveReplacer(AssetsReplacer replacer, bool closePreviewStream = true)
        {
            if (replacer == null) return;
            var forInstance = LoadedFiles[replacer.GetFileID()];
            var assetId = new AssetID(forInstance.path, replacer.GetPathID());

            NewAssets.Remove(assetId);
            if (NewAssetDatas.ContainsKey(assetId))
            {
                if (closePreviewStream)
                    NewAssetDatas[assetId].Close();
                NewAssetDatas.Remove(assetId);
            }

            Modified = NewAssets.Count != 0;
        }
		
		private void UpdateAssetInfo(AssetContainer cont)
        {
            var assetId = cont.AssetId;
            var replacer = NewAssets[assetId];
            Extensions.GetUAAENameFast(this, cont, out var type, out var name);

            var item = new AssetItem
            {
                Name = name,
                Type = type,
                TypeID = (uint)replacer.GetClassID(),
                FileID = replacer.GetFileID(),
                PathID = replacer.GetPathID(),
                Size = replacer.GetSize(),
                Modified = "*",
                MonoID = replacer.GetMonoScriptID()
            };

            LoadedContainers[assetId] = new AssetContainer(LoadedContainers[assetId], item);
        }

        //Existing assets
        public AssetContainer MakeAssetContainer(AssetItem item, bool forceFromCldb = false)
        {
            var fileInst = LoadedFiles[item.FileID];
            var reader = fileInst.file.reader;
            var templateField = GetTemplateField(item, forceFromCldb);
            var typeInst = new AssetTypeInstance(templateField, reader, item.Position);
            return new AssetContainer(item, fileInst, typeInst);
        }

        //Newly created assets
        public AssetContainer MakeAssetContainer(AssetItem item, Stream ms, bool forceFromCldb = false)
        {
            var fileInst = LoadedFiles[item.FileID];
            using var reader = new AssetsFileReader(ms);
            var templateField = GetTemplateField(item, forceFromCldb);
            var typeInst = new AssetTypeInstance(templateField, reader, 0);
            return new AssetContainer(reader, item, fileInst, typeInst);
        }

        public AssetContainer MakeAssetContainer(AssetItem item, AssetsFileReader reader, bool forceFromCldb = false)
        {
            var fileInst = LoadedFiles[item.FileID];
            var templateField = GetTemplateField(item, forceFromCldb);
            var typeInst = new AssetTypeInstance(templateField, reader, 0);
            return new AssetContainer(reader, item, fileInst, typeInst);
        }

        public AssetContainer GetAssetContainer(AssetItem item, bool onlyInfo = false)
        {
            var fileInst = LoadedFiles[item.FileID];
            var assetId = new AssetID(fileInst.path, item.PathID);

            if (!LoadedContainers.TryGetValue(assetId, out var cont))
                return null;

            if (cont.HasInstance || onlyInfo)
                return cont;

            return NewAssetDatas.TryGetValue(assetId, out var newData) ? MakeAssetContainer(item, newData) : MakeAssetContainer(item);
        }

        public AssetItem GetAssetItem(int fileId, long pathId)
        {
            var fileInst = LoadedFiles[fileId];
            var assetId = new AssetID(fileInst.path, pathId);
            return LoadedContainers.TryGetValue(assetId, out var cont) ? cont.Item : null;
        }

        public AssetContainer GetAssetContainer(int fileId, long pathId)
        {
            var item = GetAssetItem(fileId, pathId);
            return item != null ? GetAssetContainer(item) : null;
        }

        public AssetContainer GetAssetContainer(AssetTypeValueField pptrField)
        {
            var fileId = pptrField.Get("m_FileID").GetValue().AsInt();
            var pathId = pptrField.Get("m_PathID").GetValue().AsInt64();
            return GetAssetContainer(fileId, pathId);
        }

        public void GenerateAssetsFileLookup()
        {
            foreach (var fileInst in LoadedFiles)
            {
                LoadedFileLookup[fileInst.path.ToLower()] = fileInst;
            }
        }

        public AssetTypeTemplateField GetTemplateField(AssetItem item, bool forceFromCldb = false)
        {
            var file = LoadedFiles[item.FileID].file;
            var hasTypeTree = file.typeTree.hasTypeTree;
            var baseField = new AssetTypeTemplateField();
            var scriptIndex = item.MonoID;
            var fixedId = AssetHelper.FixAudioID(item.TypeID);

            if (hasTypeTree && !forceFromCldb)
            {
                baseField.From0D(AssetHelper.FindTypeTreeTypeByScriptIndex(file.typeTree, scriptIndex), 0);
            }
            else
            {
                baseField.FromClassDatabase(Am.classFile, AssetHelper.FindAssetClassByID(Am.classFile, fixedId), 0);
            }
            return baseField;
        }

        public AssetTypeValueField GetBaseField(AssetItem item)
        {
            var cont = GetAssetContainer(item);
            var fileInst = cont.FileInstance;
            if (item.TypeID == 0x72)
            {
                var tt = fileInst.file.typeTree;
                //check if typetree data exists already
                if (!tt.hasTypeTree || AssetHelper.FindTypeTreeTypeByScriptIndex(tt, item.MonoID) == null)
                {
                    var filePath = Path.GetDirectoryName(fileInst.parentBundle != null ? fileInst.parentBundle.path : fileInst.path);
                    var managedPath = Path.Combine(filePath ?? Environment.CurrentDirectory, "Managed");
                    if (!Directory.Exists(managedPath))
                    {
                        var ofd = new OpenFolderDialog
                        {
                            Title = @"Select a folder for assemblies"
                        };
                        if (ofd.ShowDialog() != DialogResult.OK)
                            return cont.TypeInstance?.GetBaseField();

                        managedPath = ofd.Folder;
                    }
                    var monoField = GetMonoBaseField(cont, managedPath);
                    if (monoField != null)
                        return monoField;
                }
            }
            return cont.TypeInstance?.GetBaseField();
        }

        public AssetTypeValueField GetBaseField(AssetContainer cont)
        {
            var item = cont.Item;
            var fileInst = cont.FileInstance;
            if (item.TypeID == 0x72)
            {
                var tt = fileInst.file.typeTree;
                //check if typetree data exists already
                if (!tt.hasTypeTree || AssetHelper.FindTypeTreeTypeByScriptIndex(tt, item.MonoID) == null)
                {
                    var filePath = Path.GetDirectoryName(fileInst.parentBundle != null ? fileInst.parentBundle.path : fileInst.path);
                    var managedPath = Path.Combine(filePath ?? Environment.CurrentDirectory, "Managed");
                    if (!Directory.Exists(managedPath))
                    {
                        var ofd = new OpenFolderDialog
                        {
                            Title = @"Select a folder for assemblies"
                        };
                        if (ofd.ShowDialog() != DialogResult.OK)
                            return cont.TypeInstance?.GetBaseField();

                        managedPath = ofd.Folder;
                    }
                    var monoField = GetMonoBaseField(cont, managedPath);
                    if (monoField != null)
                        return monoField;
                }
            }
            return cont.TypeInstance?.GetBaseField();
        }

        public AssetTypeValueField GetMonoBaseField(AssetContainer cont, string managedPath)
        {
            var file = cont.FileInstance.file;
            var item = cont.Item;
            var baseTemp = new AssetTypeTemplateField();
            baseTemp.FromClassDatabase(Am.classFile, AssetHelper.FindAssetClassByID(Am.classFile, item.TypeID), 0);
            var mainAti = new AssetTypeInstance(baseTemp, cont.FileReader, item.Position);
            var scriptIndex = item.MonoID;
            if (scriptIndex != 0xFFFF)
            {
                var monoScriptCont = GetAssetContainer(mainAti.GetBaseField().Get("m_Script"));
                if (monoScriptCont == null)
                    return null;

                var scriptBaseField = monoScriptCont.TypeInstance.GetBaseField();
                var scriptName = scriptBaseField.Get("m_Name").GetValue().AsString();
                var scriptNamespace = scriptBaseField.Get("m_Namespace").GetValue().AsString();
                var assemblyName = scriptBaseField.Get("m_AssemblyName").GetValue().AsString();
                var assemblyPath = Path.Combine(managedPath, assemblyName);

                if (scriptNamespace != string.Empty)
                    scriptName = scriptNamespace + "." + scriptName;

                if (File.Exists(assemblyPath))
                {
                    if (!LoadedAssemblies.ContainsKey(assemblyName))
                    {
                        LoadedAssemblies.Add(assemblyName, MonoDeserializer.GetAssemblyWithDependencies(assemblyPath));
                    }
                    var asmDef = LoadedAssemblies[assemblyName];

                    var mc = new MonoDeserializer();
                    mc.Read(scriptName, asmDef, file.header.format);
                    var monoTemplateFields = mc.children;

                    var templateField = baseTemp.children.Concat(monoTemplateFields).ToArray();
                    baseTemp.children = templateField;
                    baseTemp.childrenCount = baseTemp.children.Length;

                    mainAti = new AssetTypeInstance(baseTemp, cont.FileReader, item.Position);
                }
            }
            return mainAti.GetBaseField();
        }

        public void ClearModified()
        {
            foreach (var kvp in LoadedContainers)
            {
                kvp.Value.Item.Modified = "";
            }
        }
    }
}
