using System;
using System.IO;

namespace SevenZip.Compression.LZMA
{
    public static class SevenZipHelper
    {
        //static int dictionary = 1 << 23;
        static int dictionary = 1 << 21;

        static int posStateBits = 2;
        static int litContextBits = 3;
        static int litPosBits = 0;
        static int algorithm = 2;

        static int numFastBytes = 32;

        static bool eos = false;

        static CoderPropID[] propIDs =
        {
            CoderPropID.DictionarySize,
            CoderPropID.PosStateBits,
            CoderPropID.LitContextBits,
            CoderPropID.LitPosBits,
            CoderPropID.Algorithm,
            CoderPropID.NumFastBytes,
            CoderPropID.MatchFinder,
            CoderPropID.EndMarker
        };

        // these are the default properties, keeping it simple for now:
        static object[] properties =
        {
            dictionary,
            posStateBits,
            litContextBits,
            litPosBits,
            algorithm,
            numFastBytes,
            "bt4",
            eos
        };

        public static byte[] Compress(byte[] inputBytes)
        {
            var inStream = new MemoryStream(inputBytes);
            var outStream = new MemoryStream();
            var encoder = new Encoder();
            encoder.SetCoderProperties(propIDs, properties);
            encoder.WriteCoderProperties(outStream);
            //don't write size
            //long fileSize = inStream.Length;
            //for (int i = 0; i < 8; i++)
            //    outStream.WriteByte((Byte)(fileSize >> (8 * i)));
            encoder.Code(inStream, outStream, -1, -1, null);
            return outStream.ToArray();
        }

        public static byte[] Decompress(byte[] inputBytes)
        {
            var newInStream = new MemoryStream(inputBytes);

            var decoder = new Decoder();

            newInStream.Seek(0, 0);
            var newOutStream = new MemoryStream();

            var properties2 = new byte[5];
            if (newInStream.Read(properties2, 0, 5) != 5)
                throw (new Exception("input .lzma is too short"));
            long outSize = 0;
            for (var i = 0; i < 8; i++)
            {
                var v = newInStream.ReadByte();
                if (v < 0)
                    throw (new Exception("Can't Read 1"));
                outSize |= ((long)(byte)v) << (8 * i);
            }
            decoder.SetDecoderProperties(properties2);

            var compressedSize = newInStream.Length - newInStream.Position;
            decoder.Code(newInStream, newOutStream, compressedSize, outSize, null);

            var b = newOutStream.ToArray();

            return b;
        }


        public static MemoryStream StreamDecompress(MemoryStream newInStream)
        {
            var decoder = new Decoder();

            newInStream.Seek(0, 0);
            var newOutStream = new MemoryStream();

            var properties2 = new byte[5];
            if (newInStream.Read(properties2, 0, 5) != 5)
                throw (new Exception("input .lzma is too short"));
            long outSize = 0;
            for (var i = 0; i < 8; i++)
            {
                var v = newInStream.ReadByte();
                if (v < 0)
                    throw (new Exception("Can't Read 1"));
                outSize |= ((long)(byte)v) << (8 * i);
            }
            decoder.SetDecoderProperties(properties2);

            var compressedSize = newInStream.Length - newInStream.Position;
            decoder.Code(newInStream, newOutStream, compressedSize, outSize, null);

            newOutStream.Position = 0;
            return newOutStream;
        }

        public static MemoryStream StreamDecompress(MemoryStream newInStream, long outSize)
        {
            var decoder = new Decoder();

            newInStream.Seek(0, 0);
            var newOutStream = new MemoryStream();

            var properties2 = new byte[5];
            if (newInStream.Read(properties2, 0, 5) != 5)
                throw (new Exception("input .lzma is too short"));
            decoder.SetDecoderProperties(properties2);

            var compressedSize = newInStream.Length - newInStream.Position;
            decoder.Code(newInStream, newOutStream, compressedSize, outSize, null);

            newOutStream.Position = 0;
            return newOutStream;
        }

        public static void StreamDecompress(Stream compressedStream, Stream decompressedStream, long compressedSize, long decompressedSize)
        {
            var basePosition = compressedStream.Position;
            var decoder = new Decoder();

            var properties = new byte[5];
            if (compressedStream.Read(properties, 0, 5) != 5)
                throw new Exception("input .lzma is too short");
            decoder.SetDecoderProperties(properties);

            decoder.Code(compressedStream, decompressedStream, compressedSize - 5, decompressedSize, null);
            compressedStream.Position = basePosition + compressedSize;
        }
    }
}
