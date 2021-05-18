using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using AssetsAdvancedEditor.Assets;
using AssetsTools.NET.Extra;

namespace AssetsAdvancedEditor.Winforms
{
    public partial class AddAssetsDialog : Form
    {
        public AssetWorkspace Workspace;
        public List<AssetDetailsListItem> ListItems;
        private static long _nextPathId;
        public AddAssetsDialog(AssetWorkspace workspace)
        {
            InitializeComponent();
            Workspace = workspace;
            ListItems = new List<AssetDetailsListItem>();
        }

        private void AddAssetDialog_Load(object sender, EventArgs e)
        {
            var i = 0;
            foreach (var file in Workspace.LoadedFiles)
            {
                cboxFileID.Items.Add($"{i} - {file.name}");
                i++;
            }
            cboxFileID.SelectedIndex = 0;
            cboxTypePreset.SelectedIndex = 0;
            _nextPathId = GetLastPathId();
            boxPathID.Text = GetNextPathId().ToString();
        }

        private static long GetNextPathId() => ++_nextPathId;

        private long GetLastPathId()
        {
            var fileId = cboxFileID.SelectedIndex;
            var max = Workspace.LoadedFiles[fileId].table.assetFileInfo.Max(i => i.index);

            if (Workspace.NewAssets.Count == 0) return max;

            var max2 = Workspace.NewAssets
                .Where(i => i.Value.GetFileID() == fileId)
                .Max(j => j.Key.pathID);
            return max > max2 ? max : max2;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            var cldb = Workspace.Am.classFile;
            var typeTree = Workspace.MainFile.file.typeTree;
            string type;
            if (uint.TryParse(boxTypeNameOrID.Text, out var typeId))
            {
                var cldbType = AssetHelper.FindAssetClassByID(cldb, typeId);
                var hasTypeTree = typeTree.hasTypeTree;
                if (hasTypeTree)
                {
                    var ttType = AssetHelper.FindTypeTreeTypeByID(typeTree, typeId);
                    if (ttType == null)
                    {
                        type = $"0x{typeId:X8}";
                    }
                    else if (ttType.typeFieldsEx.Length != 0)
                    {
                        type = ttType.typeFieldsEx[0].GetTypeString(ttType.stringTable);
                    }
                    else
                    {
                        type = cldbType != null ?
                            cldbType.name.GetString(cldb) : $"0x{typeId:X8}";
                    }
                }
                else
                {
                    type = cldbType != null ?
                        cldbType.name.GetString(cldb) : $"0x{typeId:X8}";
                }
            }
            else
            {
                type = boxTypeNameOrID.Text;
                var cldbType = AssetHelper.FindAssetClassByName(cldb, type);
                var hasTypeTree = typeTree.hasTypeTree;
                if (hasTypeTree)
                {
                    var ttType = AssetHelper.FindTypeTreeTypeByName(typeTree, type);
                    if (ttType != null)
                    {
                        typeId = (uint)ttType.classId;
                    }
                    else
                    {
                        if (cldbType != null)
                        {
                            typeId = (uint)cldbType.classId;
                        }
                        else
                        {
                            typeId = 0x01;
                            type = "GameObject";
                        }
                    }
                }
                else
                {
                    if (cldbType != null)
                    {
                        typeId = (uint)cldbType.classId;
                    }
                    else
                    {
                        typeId = 0x01;
                        type = "GameObject";
                    }
                }
            }
            var fileId = cboxFileID.SelectedIndex;
            var pathId = Convert.ToInt64(boxPathID.Text);
            var monoId = typeId == 0x72 ? Convert.ToUInt16(boxMonoID.Text) : (ushort)0xFFFF;
            var count = Convert.ToInt32(boxCount.Text);
            for (var i = 0; i < count; i++)
            {
                ListItems.Add(new AssetDetailsListItem
                {
                    Type = type,
                    TypeID = typeId,
                    FileID = fileId,
                    PathID = pathId + i,
                    Modified = "*",
                    MonoID = monoId
                });
            }
        }

        private void cboxFileID_SelectedIndexChanged(object sender, EventArgs e)
        {
            _nextPathId = GetLastPathId();
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
