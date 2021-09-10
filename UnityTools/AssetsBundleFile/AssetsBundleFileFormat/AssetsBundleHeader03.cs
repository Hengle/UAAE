﻿namespace UnityTools
{
    public class AssetBundleHeader03
    {
        public string signature;
        public uint fileVersion;
        public string minPlayerVersion;
        public string fileEngineVersion;
        public uint minimumStreamedBytes;
        public uint bundleDataOffs;
        public uint numberOfAssetsToDownload;
        public uint levelCount;
        public AssetsBundleOffsetPair[] levelList;
        public uint fileSize2;
        public uint unknown2;
        public byte unknown3;

        ///public bool Read(AssetsFileReader reader, LPARAM lPar, AssetsFileVerifyLogger errorLogger = NULL);
        ///public bool Write(AssetsFileWriter writer, LPARAM lPar, QWORD &curFilePos, AssetsFileVerifyLogger errorLogger = NULL);
        public uint bundleCount;
    }
}
