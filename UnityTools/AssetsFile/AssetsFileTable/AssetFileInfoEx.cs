using System;

namespace UnityTools
{
    public class AssetFileInfoEx : AssetFileInfo
    {
        public AssetClassID curFileType;
        public long absoluteFilePos;

        //recommend GetAssetNameFast if at possible
        public bool ReadName(AssetsFile file, out string str)
        {
            str = string.Empty;
            var reader = file.reader;
            if (AssetsFileExtra.HasName(curFileType))
            {
                reader.Position = absoluteFilePos;
                var length = Math.Min(reader.ReadInt32(), 99);
                str = reader.ReadStringLength(length);
                return true;
            }
            return false;
        }
    }
}
