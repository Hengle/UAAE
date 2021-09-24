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
            var reader = bundle.Reader;
            var start = (int)(bundle.Header.GetFileDataOffset() + bundle.Metadata.DirectoryInfo[index].Offset);
            var length = (int)bundle.Metadata.DirectoryInfo[index].DecompressedSize;
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
            var dirInf = bundle.Metadata.DirectoryInfo;
            for (var i = 0; i < dirInf.Length; i++)
            {
                var info = dirInf[i];
                if (info.Name == name)
                {
                    return LoadAssetFromBundle(bundle, i);
                }
            }
            return null;
        }

        public static byte[] LoadAssetDataFromBundle(AssetBundleFile bundle, string name)
        {
            var dirInf = bundle.Metadata.DirectoryInfo;
            for (var i = 0; i < dirInf.Length; i++)
            {
                var info = dirInf[i];
                if (info.Name == name)
                {
                    return LoadAssetDataFromBundle(bundle, i);
                }
            }
            return null;
        }

        public static List<AssetsFile> LoadAllAssetsFromBundle(AssetBundleFile bundle)
        {
            var files = new List<AssetsFile>();
            var reader = bundle.Reader;
            var dirInf = bundle.Metadata.DirectoryInfo;
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
            var reader = bundle.Reader;
            var dirInf = bundle.Metadata.DirectoryInfo;
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
                bundle.Metadata.Read(0, memReader);
            }
        }
    }
}
