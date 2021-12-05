﻿using UnityTools.Compression.LZ4;
using System.Collections.Generic;
using System.IO;
using SevenZip.Compression.LZMA;
using UnityTools.Utils;

namespace UnityTools
{
    public static class BundleHelper
    {
        public static byte[] LoadAssetDataFromBundle(AssetBundleFile bundle, int index)
        {
            bundle.GetFileRange(index, out var offset, out var length);
            var reader = bundle.Reader;
            reader.Position = offset;
            return reader.ReadBytes((int)length);
        }

        public static byte[] LoadAssetDataFromBundle(AssetBundleFile bundle, string name)
        {
            var index = bundle.GetFileIndex(name);
            if (index != -1)
                return LoadAssetDataFromBundle(bundle, index);

            return null;
        }

        public static AssetsFile LoadAssetFromBundle(AssetBundleFile bundle, int index)
        {
            bundle.GetFileRange(index, out var offset, out var length);
            var ss = new SegmentStream(bundle.Reader.BaseStream, offset, length);
            var reader = new AssetsFileReader(ss);
            return new AssetsFile(reader);
        }

        public static AssetsFile LoadAssetFromBundle(AssetBundleFile bundle, string name)
        {
            var index = bundle.GetFileIndex(name);
            if (index != -1)
                return LoadAssetFromBundle(bundle, index);

            return null;
        }

        public static List<byte[]> LoadAllAssetsDataFromBundle(AssetBundleFile bundle)
        {
            var files = new List<byte[]>();
            var fileCount = bundle.FileCount;
            for (var i = 0; i < fileCount; i++)
            {
                if (bundle.IsAssetsFile(i))
                {
                    files.Add(LoadAssetDataFromBundle(bundle, i));
                }
            }
            return files;
        }

        public static List<AssetsFile> LoadAllAssetsFromBundle(AssetBundleFile bundle)
        {
            var files = new List<AssetsFile>();
            var fileCount = bundle.FileCount;
            for (var i = 0; i < fileCount; i++)
            {
                if (bundle.IsAssetsFile(i))
                {
                    files.Add(LoadAssetFromBundle(bundle, i));
                }
            }
            return files;
        }

        public static AssetBundleFile UnpackBundle(AssetBundleFile file, bool freeOriginalStream = true)
        {
            var ms = new MemoryStream();
            file.Unpack(file.Reader, new AssetsFileWriter(ms));
            ms.Position = 0;

            var newFile = new AssetBundleFile();
            newFile.Read(new AssetsFileReader(ms));

            if (freeOriginalStream)
            {
                file.Reader.Close();
            }
            return newFile;
        }

        public static AssetBundleFile UnpackBundleToStream(AssetBundleFile file, Stream stream, bool freeOriginalStream = true)
        {
            file.Unpack(file.Reader, new AssetsFileWriter(stream));
            stream.Position = 0;

            var newFile = new AssetBundleFile();
            newFile.Read(new AssetsFileReader(stream));

            if (freeOriginalStream)
            {
                file.Reader.Close();
            }
            return newFile;
        }

        public static AssetBundleDirectoryInfo GetDirInfo(AssetBundleFile bundle, int index)
        {
            var dirInf = bundle.Metadata.DirectoryInfo;
            return dirInf[index];
        }

        public static AssetBundleDirectoryInfo GetDirInfo(AssetBundleFile bundle, string name)
        {
            var dirInf = bundle.Metadata.DirectoryInfo;
            foreach (var info in dirInf)
            {
                if (info.Name == name)
                {
                    return info;
                }
            }
            return null;
        }

        public static void UnpackInfoOnly(this AssetBundleFile bundle)
        {
            var reader = bundle.Reader;

            reader.Position = 0;
            if (!bundle.Read(reader, true))
                return;

            reader.Position = bundle.Header.GetBundleInfoOffset();
            MemoryStream blocksInfoStream;
            var compressedSize = (int)bundle.Header.CompressedSize;
            switch (bundle.Header.GetCompressionType())
            {
                case AssetBundleCompressionType.Lzma:
                {
                    using var mstream = new MemoryStream(reader.ReadBytes(compressedSize));
                    blocksInfoStream = SevenZipHelper.StreamDecompress(mstream);
                    break;
                }
                case AssetBundleCompressionType.Lz4:
                case AssetBundleCompressionType.Lz4HC:
                {
                    var uncompressedBytes = new byte[bundle.Header.DecompressedSize];
                    using (var ms = new MemoryStream(reader.ReadBytes(compressedSize)))
                    {
                        var decoder = new Lz4DecoderStream(ms);
                        decoder.Read(uncompressedBytes, 0, (int)bundle.Header.DecompressedSize);
                        decoder.Dispose();
                    }
                    blocksInfoStream = new MemoryStream(uncompressedBytes);
                    break;
                }
                default:
                    blocksInfoStream = null;
                    break;
            }
            if (bundle.Header.GetCompressionType() != 0)
            {
                using var memReader = new AssetsFileReader(blocksInfoStream)
                {
                    Position = 0
                };
                bundle.Metadata.Read(bundle.Header, memReader);
            }
        }
    }
}
