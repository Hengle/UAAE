using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UnityTools
{
    /// <summary>
    /// Assets files contain binary serialized objects and optional run-time type information.
    /// They have file name extensions like .asset, .assets, .sharedassets but may also have no extension at all
    /// </summary>
    public class AssetsFile
    {
        public AssetsFileHeader header;
        public TypeTree typeTree;

        public PreloadList preloadTable;
        public AssetsFileDependencyList dependencies;
        public string unknownString;

        public uint AssetTablePos;
        public uint AssetCount;

        public AssetsFileReader reader;
        public Stream readerPar;

        public AssetsFile(AssetsFileReader reader)
        {
            this.reader = reader;
            readerPar = reader.BaseStream;
            
            header = new AssetsFileHeader();
            header.Read(reader);

            typeTree = new TypeTree();
            typeTree.Read(reader, header.Version);
            
            AssetCount = reader.ReadUInt32();
            reader.Align();
            AssetTablePos = (uint)reader.BaseStream.Position;

            var assetInfoSize = AssetFileInfo.GetSize(header.Version);
            if (0x0F <= header.Version && header.Version <= 0x10)
            {
                //for these two versions, the asset info is not aligned
                //for the last entry, so we have to do some weird stuff
                reader.BaseStream.Position += ((assetInfoSize + 3) >> 2 << 2) * (AssetCount - 1) + assetInfoSize;
            }
            else
            {
                reader.BaseStream.Position += AssetFileInfo.GetSize(header.Version) * AssetCount;
            }
            if (header.Version > 0x0B)
            {
                preloadTable = new PreloadList();
                preloadTable.Read(reader);
            }

            dependencies = new AssetsFileDependencyList();
            dependencies.Read(reader);
        }
        
        public void Close() => readerPar.Dispose();

        public void Write(AssetsFileWriter writer, long filePos, List<AssetsReplacer> replacers, uint fileID, ClassDatabaseFile typeMeta = null)
        {
            if (filePos == -1)
                filePos = writer.Position;
            else
                writer.Position = filePos;

            header.Write(writer);

            foreach (var replacer in replacers)
            {
                var replacerClassId = replacer.GetClassID();
                if (typeTree.unity5Types.All(t => t.ClassID != replacerClassId))
                {
                    var type_0D = new Type_0D
                    {
                        ClassID = replacer.GetClassID(),
                        IsStrippedType = false,
                        ScriptIndex = 0xFFFF,
                        TypeHash = new Hash128(),
                        ChildrenCount = 0,
                        stringTableLen = 0,
                        stringTable = ""
                    };

                    if (typeMeta != null)
                    {
                        var cldbType = AssetHelper.FindAssetClassByID(typeMeta, (uint)replacerClassId);
                        if (cldbType != null)
                        {
                            type_0D = C2T5.Cldb2TypeTree(typeMeta, cldbType);
                        }
                    }

                    typeTree.unity5Types.Add(type_0D);
                }
            }
            typeTree.Write(writer, header.Version);

            var initialSize = (int)(AssetFileInfo.GetSize(header.Version) * AssetCount);
            var newSize = (int)(AssetFileInfo.GetSize(header.Version) * (AssetCount + replacers.Count));
            var appendedSize = newSize - initialSize;
            reader.Position = AssetTablePos;

            var assetInfos = new List<AssetFileInfo>();
            var originalAssetInfos = new Dictionary<long, AssetFileInfo>();
            var currentReplacers = replacers.ToDictionary(r => r.GetPathID());
            uint currentOffset = 0;

            //calculate sizes/offsets for original assets, modify sizes if needed and skip those to be removed
            for (var i = 0; i < AssetCount; i++)
            {
                var info = new AssetFileInfo();
                info.Read(header.Version, reader);
                originalAssetInfos.Add(info.index, info);
                var newInfo = new AssetFileInfo
                {
                    index = info.index,
                    curFileOffset = currentOffset,
                    curFileSize = info.curFileSize,
                    curFileTypeOrIndex = info.curFileTypeOrIndex,
                    inheritedUnityClass = info.inheritedUnityClass,
                    scriptIndex = info.scriptIndex,
                    stripped = info.stripped
                };
                if (currentReplacers.TryGetValue(newInfo.index, out var replacer))
                {
                    currentReplacers.Remove(newInfo.index);
                    if (replacer.GetReplacementType() == AssetsReplacementType.AddOrModify)
                    {
                        var classIndex = replacer.GetMonoScriptID() == 0xFFFF ? typeTree.unity5Types.FindIndex(t => t.ClassID == replacer.GetClassID()) : typeTree.unity5Types.FindIndex(t => t.ClassID == replacer.GetClassID() && t.ScriptIndex == replacer.GetMonoScriptID());
                        newInfo = new AssetFileInfo
                        {
                            index = replacer.GetPathID(),
                            curFileOffset = currentOffset,
                            curFileSize = (uint)replacer.GetSize(),
                            curFileTypeOrIndex = classIndex,
                            inheritedUnityClass = (ushort)replacer.GetClassID(), //for older unity versions
                            scriptIndex = replacer.GetMonoScriptID(),
                            stripped = false
                        };
                    }
                    else if (replacer.GetReplacementType() == AssetsReplacementType.Remove)
                    {
                        continue;
                    }
                }
                currentOffset += newInfo.curFileSize;
                currentOffset = (currentOffset + 7) >> 3 << 3; //pad to 8 bytes

                assetInfos.Add(newInfo);
            }

            //calculate sizes/offsets for new assets
            foreach (var replacerPair in currentReplacers)
            {
                var replacer = replacerPair.Value;
                if (replacer.GetReplacementType() == AssetsReplacementType.AddOrModify)
                {
                    int classIndex;
                    if (replacer.GetMonoScriptID() == 0xFFFF)
                        classIndex = typeTree.unity5Types.FindIndex(t => t.ClassID == replacer.GetClassID());
                    else
                        classIndex = typeTree.unity5Types.FindIndex(t => t.ClassID == replacer.GetClassID() && t.ScriptIndex == replacer.GetMonoScriptID());
                    var info = new AssetFileInfo()
                    {
                        index = replacer.GetPathID(),
                        curFileOffset = currentOffset,
                        curFileSize = (uint)replacer.GetSize(),
                        curFileTypeOrIndex = classIndex,
                        inheritedUnityClass = (ushort)replacer.GetClassID(),
                        scriptIndex = replacer.GetMonoScriptID(),
                        stripped = false
                    };
                    currentOffset += info.curFileSize;
                    currentOffset = (currentOffset + 7) >> 3 << 3; //pad to 8 bytes

                    assetInfos.Add(info);
                }
            }

            currentReplacers.Clear();

            writer.Write(assetInfos.Count);
            writer.Align();
            foreach (var info in assetInfos)
            {
                info.Write(header.Version, writer);
            }

            preloadTable.Write(writer);

            dependencies.Write(writer);

            //temporary fix for secondarytypecount and friends
            if (header.Version >= 0x14)
            {
                writer.Write(0); //secondaryTypeCount
            }

            var metadataSize = (uint)(writer.Position - filePos - 0x13); //0x13 is header - "endianness byte"? (if that's what it even is)
            if (header.Version >= 0x16)
            {
                //remove larger variation fields as well
                metadataSize -= 0x1c;
            }

            //for padding only. if all initial data before assetData is more than 0x1000, this is skipped
            if (writer.Position < 0x1000)
            {
                while (writer.Position < 0x1000)
                {
                    writer.Write((byte)0x00);
                }
            }
            else
            {
                if (writer.Position % 16 == 0)
                    writer.Position += 16;
                else
                    writer.Align16();
            }

            var firstFileOffset = writer.Position;

            //write all asset data
            for (var i = 0; i < assetInfos.Count; i++)
            {
                var info = assetInfos[i];
                var replacer = replacers.FirstOrDefault(n => n.GetPathID() == info.index);
                if (replacer != null)
                {
                    if (replacer.GetReplacementType() == AssetsReplacementType.AddOrModify)
                    {
                        replacer.Write(writer);
                        if (i != assetInfos.Count - 1)
                            writer.Align8();
                    }
                }
                else
                {
                    if (!originalAssetInfos.TryGetValue(info.index, out var originalInfo)) continue;
                    reader.Position = header.DataOffset + originalInfo.curFileOffset;
                    var assetData = reader.ReadBytes((int)originalInfo.curFileSize);
                    writer.Write(assetData);
                    if (i != assetInfos.Count - 1)
                        writer.Align8();
                }
            }

            var newHeader = new AssetsFileHeader()
            {
                MetadataSize = header.MetadataSize,
                FileSize = header.FileSize,
                Version = header.Version,
                DataOffset = header.DataOffset,
                Endianness = header.Endianness,
                Reserved = header.Reserved,
                unknown = header.unknown,
                FromBundle = header.FromBundle
            };

            newHeader.DataOffset = firstFileOffset;

            var fileSizeMarker = writer.Position - filePos;

            reader.Position = newHeader.DataOffset;

            writer.Position = filePos;
            newHeader.MetadataSize = metadataSize;
            newHeader.FileSize = fileSizeMarker;
            newHeader.Write(writer);

            writer.Position = fileSizeMarker + filePos;
        }

        ///public bool GetAssetFile(ulong fileInfoOffset, AssetsFileReader reader, AssetFile buf, FileStream readerPar);
        ///public ulong GetAssetFileOffs(ulong fileInfoOffset, AssetsFileReader reader, FileStream readerPar);
        ///public bool GetAssetFileByIndex(ulong fileIndex, AssetFile buf, uint size, AssetsFileReader reader, FileStream readerPar);
        ///public ulong GetAssetFileOffsByIndex(ulong fileIndex, AssetsFileReader reader, FileStream readerPar);
        ///public bool GetAssetFileByName(string name, AssetFile buf, uint size, AssetsFileReader reader, FileStream readerPar);
        ///public ulong GetAssetFileOffsByName(string name, AssetsFileReader reader, FileStream readerPar);
        ///public ulong GetAssetFileInfoOffs(ulong fileIndex, AssetsFileReader reader, FileStream readerPar);
        ///public ulong GetAssetFileInfoOffsByName(string name, AssetsFileReader reader, FileStream readerPar);
        ///public ulong GetFileList(AssetsFileReader reader, FileStream readerPar);
        ///public bool VerifyAssetsFile(AssetsFileVerifyLogger logger = null);
    }
}
