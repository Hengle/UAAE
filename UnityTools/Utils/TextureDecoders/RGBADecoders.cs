using System;
using UnityTools.Utils;

namespace UnityTools
{
    public static class RGBADecoders
    {
        public static byte[] ReadR8(byte[] input, int width, int height)
        {
            var output = new byte[width * height * 4];
            for (int inputPos = 0, outputPos = 0; outputPos < output.Length; inputPos += 1, outputPos += 4)
            {
                var r = input[inputPos];
                output[outputPos + 2] = r;
                output[outputPos + 3] = 0xFF;
            }
            return output;
        }

        public static byte[] ReadAlpha8(byte[] input, int width, int height)
        {
            var output = new byte[width * height * 4];
            for (int inputPos = 0, outputPos = 0; outputPos < output.Length; inputPos += 1, outputPos += 4)
            {
                var a = input[inputPos];
                output[outputPos + 0] = 0xFF;
                output[outputPos + 1] = 0xFF;
                output[outputPos + 2] = 0xFF;
                output[outputPos + 3] = a;
            }
            return output;
        }

        public static byte[] ReadR16(byte[] input, int width, int height)
        {
            var output = new byte[width * height * 4];
            for (int inputPos = 0, outputPos = 0; outputPos < output.Length; inputPos += 2, outputPos += 4)
            {
                var r = input[inputPos + 1];
                output[outputPos + 2] = r;
                output[outputPos + 3] = 0xFF;
            }
            return output;
        }

        public static byte[] ReadRHalf(byte[] input, int width, int height)
        {
            var output = new byte[width * height * 4];
            for (int inputPos = 0, outputPos = 0; outputPos < output.Length; inputPos += 2, outputPos += 4)
            {
                var r = ReadHalf(input, inputPos);

                output[outputPos + 2] = r;
                output[outputPos + 3] = 0xFF;
            }
            return output;
        }

        public static byte[] ReadRG16(byte[] input, int width, int height)
        {
            var output = new byte[width * height * 4];
            for (int inputPos = 0, outputPos = 0; outputPos < output.Length; inputPos += 2, outputPos += 4)
            {
                var r = input[inputPos + 0];
                var g = input[inputPos + 1];
                output[outputPos + 1] = g;
                output[outputPos + 2] = r;
                output[outputPos + 3] = 0xFF;
            }
            return output;
        }

        public static byte[] ReadRGHalf(byte[] input, int width, int height)
        {
            var output = new byte[width * height * 4];
            for (int inputPos = 0, outputPos = 0; outputPos < output.Length; inputPos += 2, outputPos += 4)
            {
                var r = ReadHalf(input, inputPos + 0);
                var g = ReadHalf(input, inputPos + 2);

                output[outputPos + 1] = g;
                output[outputPos + 2] = r;
                output[outputPos + 3] = 0xFF;
            }
            return output;
        }

        public static byte[] ReadRGB24(byte[] input, int width, int height)
        {
            var output = new byte[width * height * 4];
            for (int inputPos = 0, outputPos = 0; outputPos < output.Length; inputPos += 3, outputPos += 4)
            {
                var r = input[inputPos + 0];
                var g = input[inputPos + 1];
                var b = input[inputPos + 2];
                output[outputPos + 0] = b;
                output[outputPos + 1] = g;
                output[outputPos + 2] = r;
                output[outputPos + 3] = 0xFF;
            }
            return output;
        }

        public static byte[] ReadRGB565(byte[] input, int width, int height)
        {
            var output = new byte[width * height * 4];
            for (int inputPos = 0, outputPos = 0; outputPos < output.Length; inputPos += 2, outputPos += 4)
            {
                int rgb = BitConverter.ToUInt16(input, inputPos);

                var r = (rgb >> 11) & 0x1F;
                r = (r << 3) | (r & 7);

                var g = (rgb >> 5) & 0x3F;
                g = (g << 2) | (g & 3);

                var b = rgb & 0x1F;
                b = (b << 3) | (b & 7);

                output[outputPos + 0] = (byte)b;
                output[outputPos + 1] = (byte)g;
                output[outputPos + 2] = (byte)r;
                output[outputPos + 3] = 0xFF;
            }
            return output;
        }

        public static byte[] ReadARGB4444(byte[] input, int width, int height)
        {
            var output = new byte[width * height * 4];
            for (int inputPos = 0, outputPos = 0; outputPos < output.Length; inputPos += 2, outputPos += 4)
            {
                var a = input[inputPos + 1] >> 4;
                var r = input[inputPos + 1] & 0xf;
                var g = input[inputPos] >> 4;
                var b = input[inputPos] & 0xf;
                output[outputPos + 0] = (byte)((b << 4) | b);
                output[outputPos + 1] = (byte)((g << 4) | g);
                output[outputPos + 2] = (byte)((r << 4) | r);
                output[outputPos + 3] = (byte)((a << 4) | a);
            }
            return output;
        }

        public static byte[] ReadRGBA4444(byte[] input, int width, int height)
        {
            var output = new byte[width * height * 4];
            for (int inputPos = 0, outputPos = 0; outputPos < output.Length; inputPos += 2, outputPos += 4)
            {
                var r = input[inputPos + 1] >> 4;
                var g = input[inputPos + 1] & 0xf;
                var b = input[inputPos] >> 4;
                var a = input[inputPos] & 0xf;
                output[outputPos + 0] = (byte)((b << 4) | b);
                output[outputPos + 1] = (byte)((g << 4) | g);
                output[outputPos + 2] = (byte)((r << 4) | r);
                output[outputPos + 3] = (byte)((a << 4) | a);
            }
            return output;
        }

        public static byte[] ReadRGBAHalf(byte[] input, int width, int height)
        {
            var output = new byte[width * height * 4];
            for (int inputPos = 0, outputPos = 0; outputPos < output.Length; inputPos += 8, outputPos += 4)
            {
                var r = ReadHalf(input, inputPos + 0);
                var g = ReadHalf(input, inputPos + 2);
                var b = ReadHalf(input, inputPos + 4);
                var a = ReadHalf(input, inputPos + 6);
                output[outputPos + 0] = b;
                output[outputPos + 1] = g;
                output[outputPos + 2] = r;
                output[outputPos + 3] = a;
            }
            return output;
        }

        public static byte[] ReadARGB32(byte[] input, int width, int height)
        {
            var output = new byte[width * height * 4];
            for (int pos = 0; pos < output.Length; pos += 4)
            {
                var a = input[pos + 0];
                var r = input[pos + 1];
                var g = input[pos + 2];
                var b = input[pos + 3];
                output[pos + 0] = b;
                output[pos + 1] = g;
                output[pos + 2] = r;
                output[pos + 3] = a;
            }
            return output;
        }

        public static byte[] ReadRGBA32(byte[] input, int width, int height)
        {
            var output = new byte[width * height * 4];
            for (int pos = 0; pos < output.Length; pos += 4)
            {
                var r = input[pos + 0];
                var g = input[pos + 1];
                var b = input[pos + 2];
                var a = input[pos + 3];
                output[pos + 0] = b;
                output[pos + 1] = g;
                output[pos + 2] = r;
                output[pos + 3] = a;
            }
            return output;
        }

        private static byte ReadHalf(byte[] input, int pos)
        {
            var half = BitConverter.ToUInt16(input, pos);
            var single = HalfHelper.HalfToSingle(half);
            return (byte)Math.Round(Math.Max(Math.Min(single, 1f), 0f) * 255f);
        }
    }
}
