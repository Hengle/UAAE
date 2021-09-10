using UnityTools.Compression.LZ4;
using System.Collections.Generic;
using System.IO;
using SevenZip.Compression.LZMA;

namespace UnityTools
{
    public static class BundleHelper
    {
        public static byte[] LoadAssetDataFromBundle(AssetBundleFile bundle, int index)
        {
            var reader = bundle.reader;
            var start = (int)(bundle.bundleHeader6.GetFileDataOffset() + bundle.bundleInf6.dirInf[index].offset);
            var length = (int)bundle.bundleInf6.dirInf[index].decompressedSize;
            reader.Position = start;
            return reader.ReadBytes(length);
        }

        public static AssetsFile LoadAssetFromBundle(AssetBundleFile bundle, int index)
        {
            var data = LoadAssetDataFromBundle(bundle, index);
            var ms = new MemoryStream(data);
            var r = new AssetsFileReader(ms);
            return new AssetsFile(r);
        }

        public static AssetsFile LoadAssetFromBundle(AssetBundleFile bundle, string name)
        {
            var dirInf = bundle.bundleInf6.dirInf;
            for (var i = 0; i < dirInf.Length; i++)
            {
                var info = dirInf[i];
                if (info.name == name)
                {
                    return LoadAssetFromBundle(bundle, i);
                }
            }
            return null;
        }

        public static byte[] LoadAssetDataFromBundle(AssetBundleFile bundle, string name)
        {
            var dirInf = bundle.bundleInf6.dirInf;
            for (var i = 0; i < dirInf.Length; i++)
            {
                var info = dirInf[i];
                if (info.name == name)
                {
                    return LoadAssetDataFromBundle(bundle, i);
                }
            }
            return null;
        }

        public static List<AssetsFile> LoadAllAssetsFromBundle(AssetBundleFile bundle)
        {
            var files = new List<AssetsFile>();
            var reader = bundle.reader;
            var dirInf = bundle.bundleInf6.dirInf;
            for (var i = 0; i < dirInf.Length; i++)
            {
                var info = dirInf[i];
                if (bundle.IsAssetsFile(reader, info))
                {
                    files.Add(LoadAssetFromBundle(bundle, i));
                }
            }
            return files;
        }

        public static List<byte[]> LoadAllAssetsDataFromBundle(AssetBundleFile bundle)
        {
            var files = new List<byte[]>();
            var reader = bundle.reader;
            var dirInf = bundle.bundleInf6.dirInf;
            for (var i = 0; i < dirInf.Length; i++)
            {
                var info = dirInf[i];
                if (bundle.IsAssetsFile(reader, info))
                {
                    files.Add(LoadAssetDataFromBundle(bundle, i));
                }
            }
            return files;
        }

        public static AssetBundleFile UnpackBundle(AssetBundleFile file, bool freeOriginalStream = true)
        {
            var ms = new MemoryStream();
            file.Unpack(file.reader, new AssetsFileWriter(ms));
            ms.Position = 0;

            var newFile = new AssetBundleFile();
            newFile.Read(new AssetsFileReader(ms));

            if (freeOriginalStream)
            {
                file.reader.Close();
            }
            return newFile;
        }

        public static AssetBundleDirectoryInfo06 GetDirInfo(AssetBundleFile bundle, int index)
        {
            var dirInf = bundle.bundleInf6.dirInf;
            return dirInf[index];
        }

        public static AssetBundleDirectoryInfo06 GetDirInfo(AssetBundleFile bundle, string name)
        {
            var dirInf = bundle.bundleInf6.dirInf;
            foreach (var info in dirInf)
            {
                if (info.name == name)
                {
                    return info;
                }
            }
            return null;
        }

        public static void UnpackInfoOnly(this AssetBundleFile bundle)
        {
            var reader = bundle.reader;

            reader.Position = 0;
            if (!bundle.Read(reader, true))
                return;

            reader.Position = bundle.bundleHeader6.GetBundleInfoOffset();
            MemoryStream blocksInfoStream;
            var compressedSize = (int)bundle.bundleHeader6.compressedSize;
            switch (bundle.bundleHeader6.GetCompressionType())
            {
                case 1:
                {
                    using var mstream = new MemoryStream(reader.ReadBytes(compressedSize));
                    blocksInfoStream = SevenZipHelper.StreamDecompress(mstream);
                    break;
                }
                case 2:
                case 3:
                {
                    var uncompressedBytes = new byte[bundle.bundleHeader6.decompressedSize];
                    using (var mstream = new MemoryStream(reader.ReadBytes(compressedSize)))
                    {
                        var decoder = new Lz4DecoderStream(mstream);
                        decoder.Read(uncompressedBytes, 0, (int)bundle.bundleHeader6.decompressedSize);
                        decoder.Dispose();
                    }
                    blocksInfoStream = new MemoryStream(uncompressedBytes);
                    break;
                }
                default:
                    blocksInfoStream = null;
                    break;
            }
            if (bundle.bundleHeader6.GetCompressionType() != 0)
            {
                using var memReader = new AssetsFileReader(blocksInfoStream)
                {
                    Position = 0
                };
                bundle.bundleInf6.Read(0, memReader);
            }
        }
    }
}
