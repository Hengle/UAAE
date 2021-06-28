using AssetsTools.NET;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using AssetsTools.NET.Extra.Decompressors.LZ4;
using SevenZip.Compression.LZMA;

namespace AssetsAdvancedEditor.Utils
{
    public static class Extensions
    {
        public static bool WildcardMatches(string test, string pattern, bool caseSensitive = true)
        {
            RegexOptions options = 0;
            if (!caseSensitive)
                options |= RegexOptions.IgnoreCase;

            return Regex.IsMatch(test, "^" + Regex.Escape(pattern).Replace("\\*", ".*") + "$", options);
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
