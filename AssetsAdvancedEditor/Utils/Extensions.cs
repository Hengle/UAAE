using AssetsTools.NET;
using AssetsTools.NET.Extra;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AssetsTools.NET.Extra.Decompressors.LZ4;
using SevenZip.Compression.LZMA;

namespace AssetsAdvancedEditor.Utils
{
    public static class Extensions
    {
        public static AssetExternal GetExtAssetNewData(this AssetsManager am, AssetsFileInstance relativeTo, int fileId, long pathId,
                                               MemoryStream data, bool onlyGetInfo = false, bool forceFromCldb = false)
        {
            var ext = new AssetExternal();
            if (fileId == 0 && pathId == 0)
            {
                ext.info = null;
                ext.instance = null;
                ext.file = null;
            }
            else if (fileId != 0)
            {
                var dep = relativeTo.GetDependency(am, fileId - 1);
                ext.info = dep.table.GetAssetInfo(pathId);
                ext.instance = !onlyGetInfo ? am.GetTypeInstanceNewData(dep.file, ext.info, data, forceFromCldb) : null;
                ext.file = dep;
            }
            else
            {
                ext.info = relativeTo.table.GetAssetInfo(pathId);
                ext.instance = !onlyGetInfo ? am.GetTypeInstanceNewData(relativeTo.file, ext.info, data, forceFromCldb) : null;
                ext.file = relativeTo;
            }
            return ext;
        }

        public static AssetTypeInstance GetTypeInstanceNewData(this AssetsManager am, AssetsFile file, AssetFileInfoEx info,
                                                               MemoryStream data, bool forceFromCldb = false)
        {
            return new (am.GetTemplateBaseField(file, info, forceFromCldb), new AssetsFileReader(data), 0);
        }

        public static bool IsBundleDataCompressed(this AssetBundleFile bundle)
        {
            var reader = bundle.reader;
            reader.Position = bundle.bundleHeader6.GetBundleInfoOffset();
            MemoryStream blocksInfoStream;
            var compressedSize = (int)bundle.bundleHeader6.compressedSize;
            byte[] uncompressedBytes;
            switch (bundle.bundleHeader6.GetCompressionType())
            {
                case 1:
                    uncompressedBytes = new byte[bundle.bundleHeader6.decompressedSize];
                    using (var ms = new MemoryStream(reader.ReadBytes(compressedSize)))
                    {
                        var decoder = SevenZipHelper.StreamDecompress(ms, compressedSize);
                        decoder.Read(uncompressedBytes, 0, (int)bundle.bundleHeader6.decompressedSize);
                        decoder.Dispose();
                    }
                    blocksInfoStream = new MemoryStream(uncompressedBytes);
                    break;
                case 2:
                case 3:
                    uncompressedBytes = new byte[bundle.bundleHeader6.decompressedSize];
                    using (var ms = new MemoryStream(reader.ReadBytes(compressedSize)))
                    {
                        var decoder = new Lz4DecoderStream(ms);
                        decoder.Read(uncompressedBytes, 0, (int)bundle.bundleHeader6.decompressedSize);
                        decoder.Dispose();
                    }
                    blocksInfoStream = new MemoryStream(uncompressedBytes);
                    break;
                default:
                    blocksInfoStream = null;
                    break;
            }

            var uncompressedInf = bundle.bundleInf6;
            if (bundle.bundleHeader6.GetCompressionType() != 0)
            {
                using var memReader = new AssetsFileReader(blocksInfoStream)
                {
                    Position = 0
                };
                uncompressedInf = new AssetBundleBlockAndDirectoryList06();
                uncompressedInf.Read(0, memReader);
            }

            return uncompressedInf.blockInf.Any(inf => inf.GetCompressionType() != 0);
        }
    }
}
