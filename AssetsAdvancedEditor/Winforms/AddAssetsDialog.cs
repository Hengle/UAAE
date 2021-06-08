using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AssetsAdvancedEditor.Assets;
using AssetsAdvancedEditor.Utils;
using AssetsTools.NET;
using AssetsTools.NET.Extra;

namespace AssetsAdvancedEditor.Winforms
{
    public partial class AddAssetsDialog : Form
    {
        public AssetsWorkspace Workspace;
        public List<AssetItem> Items;
        public long NextPathId;
        public AddAssetsDialog(AssetsWorkspace workspace)
        {
            InitializeComponent();
            Workspace = workspace;
            Items = new List<AssetItem>();
        }

        private void AddAssetDialog_Load(object sender, EventArgs e)
        {
            var i = 0;
            foreach (var fileInst in Workspace.LoadedFiles)
            {
                cboxFileID.Items.Add($"{i++} - {fileInst.name}");
            }
            cboxFileID.SelectedIndex = 0;
            cboxTypePreset.SelectedIndex = 0;
            NextPathId = GetLastPathId();
            boxPathID.Text = GetNextPathId().ToString();
        }

        private long GetNextPathId() => ++NextPathId;

        private long GetLastPathId()
        {
            var fileId = cboxFileID.SelectedIndex;
            var lastId = Workspace.LoadedFiles[fileId].table.assetFileInfo.Max(i => i.index);

            if (Workspace.ModifiedAssets.Count == 0)
                return lastId;

            var newAssetsLastId = Workspace.ModifiedAssets
                .Where(i => i.Value.Replacer.GetFileID() == fileId)
                .Max(j => j.Key.pathID);
            return lastId > newAssetsLastId ? lastId : newAssetsLastId;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            var cldb = Workspace.Am.classFile;
            var fileInst = Workspace.LoadedFiles[cboxFileID.SelectedIndex];
            AssetTypeTemplateField templateField;

            var fileId = cboxFileID.SelectedIndex;
            if (!long.TryParse(boxPathID.Text, out var pathId))
            {
                MsgBoxUtils.ShowErrorDialog("Path ID is invalid!");
                return;
            }
            var type = boxTypeNameOrID.Text;
            int typeId;
            var createBlankAsset = chboxCreateBlankAssets.Checked;

            if (fileInst.file.typeTree.hasTypeTree)
            {
                if (!TryParseTypeTree(fileInst, type, createBlankAsset, out templateField, out typeId))
                {
                    if (!TryParseClassDatabase(type, createBlankAsset, out templateField, out typeId))
                    {
                        MsgBoxUtils.ShowErrorDialog("Class type is invalid!");
                        return;
                    }

                    //has typetree but had to lookup to cldb
                    //we need to add a new typetree entry because this is
                    //probably not a type that existed in this bundle
                    fileInst.file.typeTree.unity5Types.Add(C2T5.Cldb2TypeTree(cldb, typeId));
                }
            }
            else
            {
                if (!TryParseClassDatabase(type, createBlankAsset, out templateField, out typeId))
                {
                    MsgBoxUtils.ShowErrorDialog("Class type is invalid!");
                    return;
                }
            }

            var monoSelected = cboxTypePreset.SelectedIndex == 1;

            ushort monoId;
            if (typeId != 0x72 || !monoSelected)
            {
                monoId = ushort.MaxValue;
            }
            else
            {
                if (!ushort.TryParse(boxMonoID.Text, out monoId))
                {
                    MsgBoxUtils.ShowErrorDialog("Mono ID is invalid!");
                    return;
                }
            }

            if (!int.TryParse(boxCount.Text, out var count))
            {
                MsgBoxUtils.ShowErrorDialog("The count of assets being created is invalid!");
                return;
            }

            var conts = new List<AssetContainer>();
            byte[] assetBytes;
            if (createBlankAsset)
            {
                var baseField = ValueBuilder.DefaultValueFieldFromTemplate(templateField);
                assetBytes = baseField.WriteToByteArray();
            }
            else
            {
                assetBytes = Array.Empty<byte>();
            }
            for (var i = 0; i < count; i++)
            {
                var item = new AssetItem
                {
                    Type = type,
                    TypeID = (uint)typeId,
                    FileID = fileId,
                    PathID = pathId + i,
                    Modified = "*",
                    MonoID = monoId
                };
                Items.Add(item);
                var cont = new AssetContainer(new MemoryStream(assetBytes), item, AssetModifier.CreateAssetReplacer(item, assetBytes), fileInst);
                conts.Add(cont);
            }
            Workspace.AddContainers(conts);
            DialogResult = DialogResult.OK;
        }

        private bool TryParseClassDatabase(string type, bool createBlankAsset, out AssetTypeTemplateField templateField, out int typeId)
        {
            templateField = null;

            var cldb = Workspace.Am.classFile;
            ClassDatabaseType cldbType;
            bool needsTypeId;
            if (int.TryParse(type, out typeId))
            {
                cldbType = AssetHelper.FindAssetClassByID(cldb, (uint)typeId);
                needsTypeId = false;
            }
            else
            {
                cldbType = AssetHelper.FindAssetClassByName(cldb, type);
                needsTypeId = true;
            }

            if (cldbType == null)
            {
                return false;
            }

            if (needsTypeId)
            {
                typeId = cldbType.classId;
            }

            if (createBlankAsset)
            {
                templateField = new AssetTypeTemplateField();
                templateField.FromClassDatabase(cldb, cldbType, 0);
            }
            return true;
        }

        private bool TryParseTypeTree(AssetsFileInstance fileInst, string type, bool createBlankAsset, out AssetTypeTemplateField templateField, out int typeId)
        {
            templateField = null;

            var tt = fileInst.file.typeTree;
            Type_0D ttType;
            bool needsTypeId;
            if (int.TryParse(type, out typeId))
            {
                ttType = AssetHelper.FindTypeTreeTypeByID(tt, (uint)typeId);
                needsTypeId = false;
            }
            else
            {
                ttType = AssetHelper.FindTypeTreeTypeByName(tt, type);
                needsTypeId = true;
            }

            if (ttType == null)
            {
                return false;
            }

            if (needsTypeId)
            {
                typeId = ttType.classId;
            }

            if (createBlankAsset)
            {
                templateField = new AssetTypeTemplateField();
                templateField.From0D(ttType, 0);
            }
            return true;
        }

        private void cboxFileID_SelectedIndexChanged(object sender, EventArgs e)
        {
            NextPathId = GetLastPathId();
            boxPathID.Text = GetNextPathId().ToString();
        }

        private void cboxTypePreset_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cboxTypePreset.SelectedIndex)
            {
                case 0:
                    lblTypeNameOrID.Text = @"Type name/ID";
                    cboxMonoTypes.SendToBack();
                    break;
                case 1:
                    lblTypeNameOrID.Text = @"Class name";
                    cboxMonoTypes.BringToFront();
                    break;
            }
        }
    }
}
