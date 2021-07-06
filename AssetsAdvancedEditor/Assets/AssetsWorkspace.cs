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
        public List<AssetItem> LoadedAssets { get; }
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

        public delegate void AssetsWorkspaceItemUpdateEvent(AssetItem item, int index);
        public event AssetsWorkspaceItemUpdateEvent ItemUpdated;

        public AssetsWorkspace(AssetsManager am, AssetsFileInstance file, bool fromBundle = false)
        {
            Am = am;
            Pm = new PluginManager(am);
            Pm.LoadPluginsInDirectory("Plugins");
            MainInstance = file;
            FromBundle = fromBundle;

            LoadedFiles = new List<AssetsFileInstance>();
            LoadedAssets = new List<AssetItem>();
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
			var index = LoadedAssets.FindIndex(i => i.FileID == replacer.GetFileID() && i.PathID == replacer.GetPathID());
            var item = LoadedAssets[index];

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
                var cont = MakeAssetContainer(item, NewAssetDatas[assetId]);
                UpdateAssetInfo(cont, assetId, index);
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
		
		private void UpdateAssetInfo(AssetContainer cont, AssetID assetId, int index)
		{
			var field = GetBaseField(cont);
			var replacer = NewAssets[assetId];
            var classId = (uint)replacer.GetClassID();
            var nameValue = field.Get("m_Name").GetValue();
            var name = "";
            var type = field.GetFieldType();

            if (nameValue != null)
            {
                name = nameValue.AsString();
            }

            var item = new AssetItem
            {
				Name = name,
				Type = type,
                TypeID = classId,
                FileID = replacer.GetFileID(),
                PathID = replacer.GetPathID(),
                Size = replacer.GetSize(),
                Modified = "*",
                MonoID = replacer.GetMonoScriptID()
            };

            LoadedAssets[index] = item;
            LoadedContainers[assetId] = new AssetContainer(cont, item);
			ItemUpdated?.Invoke(item, index);
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

        public AssetContainer GetAssetContainer(AssetItem item)
        {
            var fileInst = LoadedFiles[item.FileID];
            var assetId = new AssetID(fileInst.path, item.PathID);

            if (!LoadedContainers.TryGetValue(assetId, out var cont))
                return null;

            if (cont.HasInstance)
                return cont;

            var newData = NewAssetDatas.ContainsKey(assetId);
            cont = newData ? MakeAssetContainer(item, NewAssetDatas[assetId]) : MakeAssetContainer(item);
            LoadedContainers[assetId] = cont;
            return cont;
        }

        public AssetContainer GetAssetContainer(int fileId, long pathId)
        {
            var item = LoadedAssets.FirstOrDefault(i => i.FileID == fileId && i.PathID == pathId);
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
            foreach (var item in LoadedAssets)
            {
                item.Modified = "";
            }
        }
    }
}
