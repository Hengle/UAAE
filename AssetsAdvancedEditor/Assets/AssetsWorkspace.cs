using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AssetsAdvancedEditor.Utils;
using AssetsTools.NET;
using AssetsTools.NET.Extra;
using Mono.Cecil;

namespace AssetsAdvancedEditor.Assets
{
    public class AssetsWorkspace
    {
        public AssetsManager Am { get; }
        public AssetsFileInstance MainInstance { get; }
        public bool FromBundle { get; }

        public List<AssetsFileInstance> LoadedFiles { get; }
        public Dictionary<AssetID, AssetContainer> LoadedAssets { get; }
        public List<AssetItem> AssetItems { get; }

        public Dictionary<string, AssetsFileInstance> LoadedFileLookup { get; }
        public Dictionary<string, AssemblyDefinition> LoadedAssemblies { get; }

        public AssetImporter Importer { get; }
        public AssetExporter Exporter { get; }

        public Dictionary<AssetID, AssetContainer> ModifiedAssets { get; }

        public bool Modified { get; set; }
        public string AssetsFileName { get; }
        public string AssetsRootDir { get; }
        public string UnityVersion { get; }

        public AssetsWorkspace(AssetsManager am, AssetsFileInstance file, bool fromBundle = false)
        {
            Am = am;
            MainInstance = file;
            FromBundle = fromBundle;

            LoadedFiles = new List<AssetsFileInstance>();
            LoadedAssets = new Dictionary<AssetID, AssetContainer>();
            AssetItems = new List<AssetItem>();

            LoadedFileLookup = new Dictionary<string, AssetsFileInstance>();
            LoadedAssemblies = new Dictionary<string, AssemblyDefinition>();

            Importer = new AssetImporter(this);
            Exporter = new AssetExporter(this);

            ModifiedAssets = new Dictionary<AssetID, AssetContainer>();

            Modified = false;

            AssetsFileName = file.name;
            AssetsRootDir = Path.GetDirectoryName(AssetsFileName);
            UnityVersion = MainInstance.file.typeTree.unityVersion;
        }

        public void AddReplacer(AssetsReplacer replacer, MemoryStream previewStream = null)
        {
            var forInstance = LoadedFiles[replacer.GetFileID()];
            var assetId = new AssetID(forInstance.path, replacer.GetPathID());

            if (ModifiedAssets.ContainsKey(assetId))
                RemoveReplacer(ModifiedAssets[assetId].Replacer);

            if (previewStream == null)
            {
                var newStream = new MemoryStream();
                var newWriter = new AssetsFileWriter(newStream);
                replacer.Write(newWriter);
                newStream.Position = 0;
                previewStream = newStream;
            }

            if (replacer is not AssetsRemover)
            {
                var cont = new AssetContainer(previewStream, LoadedAssets[assetId].Item, replacer, forInstance);

                ModifiedAssets[assetId] = cont;
                LoadedAssets[assetId] = cont;
            }
            else
            {
                ModifiedAssets.Add(assetId, LoadedAssets[assetId]);
                LoadedAssets.Remove(assetId);
            }
            Modified = true;
        }

        public void AddReplacers(List<AssetsReplacer> replacers)
        {
            foreach (var replacer in replacers)
            {
                var forInstance = LoadedFiles[replacer.GetFileID()];
                var assetId = new AssetID(forInstance.path, replacer.GetPathID());

                if (ModifiedAssets.ContainsKey(assetId))
                    RemoveReplacer(ModifiedAssets[assetId].Replacer);

                var newStream = new MemoryStream();
                var newWriter = new AssetsFileWriter(newStream);
                replacer.Write(newWriter);
                newStream.Position = 0;

                if (replacer is not AssetsRemover)
                {
                    var cont = new AssetContainer(newStream, LoadedAssets[assetId].Item, replacer, forInstance);

                    ModifiedAssets[assetId] = cont;
                }
                else
                {
                    ModifiedAssets.Add(assetId, LoadedAssets[assetId]);
                    LoadedAssets.Remove(assetId);
                }
            }
            Modified = true;
        }

        public void RemoveReplacer(AssetsReplacer replacer, bool closePreviewStream = true)
        {
            var forInstance = LoadedFiles[replacer.GetFileID()];
            var assetId = new AssetID(forInstance.path, replacer.GetPathID());

            ModifiedAssets.Remove(assetId);
            if (ModifiedAssets.ContainsKey(assetId))
            {
                if (closePreviewStream)
                    ModifiedAssets[assetId].Data.Close();
                ModifiedAssets.Remove(assetId);
            }
            Modified = ModifiedAssets.Count != 0;
        }

        public void RemoveReplacers(List<AssetsReplacer> replacers)
        {
            foreach (var replacer in replacers)
            {
                var forInstance = LoadedFiles[replacer.GetFileID()];
                var assetId = new AssetID(forInstance.path, replacer.GetPathID());

                if (ModifiedAssets.ContainsKey(assetId))
                {
                    ModifiedAssets.Remove(assetId);
                }
            }
            Modified = ModifiedAssets.Count != 0;
        }

        public AssetContainer GetAssetContainer(int fileId, long pathId, bool onlyGetInfo = false, bool forceFromCldb = false)
        {
            var fileInst = LoadedFiles[fileId];
            var assetId = new AssetID(fileInst.path, pathId);
            AssetContainer cont;
            if (ModifiedAssets.ContainsKey(assetId))
            {
                cont = ModifiedAssets[assetId];

                if (onlyGetInfo || cont.HasInstance)
                    return cont;

                var templateField = GetTemplateField(cont.Item, forceFromCldb);
                var typeInst = new AssetTypeInstance(templateField, cont.FileReader, 0);
                cont.TypeInstance = typeInst;
            }
            else
            {
                var info = fileInst.table.GetAssetInfo(pathId);
                cont = LoadedAssets[assetId];

                if (onlyGetInfo || cont.HasInstance)
                    return cont;

                cont.TypeInstance = Am.GetTypeInstance(fileInst, info, forceFromCldb);
            }
            return cont;
        }

        public AssetContainer GetAssetContainer(AssetTypeValueField pptrField, bool onlyGetInfo = false, bool forceFromCldb = false)
        {
            var fileId = pptrField.Get("m_FileID").GetValue().AsInt();
            var pathId = pptrField.Get("m_PathID").GetValue().AsInt64();
            return GetAssetContainer(fileId, pathId, onlyGetInfo, forceFromCldb);
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
                baseField.From0D(
                    scriptIndex == 0xFFFF
                        ? AssetHelper.FindTypeTreeTypeByID(file.typeTree, fixedId)
                        : AssetHelper.FindTypeTreeTypeByScriptIndex(file.typeTree, scriptIndex), 0);
            }
            else
            {
                baseField.FromClassDatabase(Am.classFile, AssetHelper.FindAssetClassByID(Am.classFile, fixedId), 0);
            }
            return baseField;
        }

        public AssetTypeValueField GetBaseField(AssetItem item)
        {
            var cont = GetAssetContainer(item.FileID, item.PathID);
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
            foreach (var (assetId, cont) in LoadedAssets)
            {
                cont.Item.Modified = "";
                LoadedAssets[assetId] = cont;
            }
        }
    }
}
