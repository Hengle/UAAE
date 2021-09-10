namespace UnityTools
{
    public class RGBADecoders
    {
        public static byte[] ReadRGBA32(byte[] bytes, int width, int height)
        {
            var len = width * height * 4;
            byte t;
            for (var i = 0; i < len; i += 4)
            {
                t = bytes[i];
                bytes[i] = bytes[i + 2];
                bytes[i + 2] = t;
            }
            return bytes;
        }
        public static byte[] ReadRGBA4444(byte[] bytes, int width, int height)
        {
            var len = width * height * 2;
            var data = new byte[width * height * 4];
            for (var i = 0; i < len; i += 2)
            {
                var dataPos = i / 2 * 4;
                data[dataPos] = (byte)(bytes[i + 1] & 0xf);
                data[dataPos + 1] = (byte)(bytes[i] >> 4);
                data[dataPos + 2] = (byte)(bytes[i] & 0xf);
                data[dataPos + 3] = (byte)(bytes[i + 1] >> 4);
            }
            return data;
        }
        public static byte[] ReadARGB32(byte[] bytes, int width, int height)
        {
            var len = width * height * 4;
            byte t;
            for (var i = 0; i < len; i += 4)
            {
                t = bytes[i];
                bytes[i] = bytes[i + 3];
                bytes[i + 3] = t;
                t = bytes[i + 1];
                bytes[i + 1] = bytes[i + 2];
                bytes[i + 2] = t;
            }
            return bytes;
        }
        public static byte[] ReadARGB4444(byte[] bytes, int width, int height)
        {
            var len = width * height * 2;
            var data = new byte[width * height * 4];
            for (var i = 0; i < len; i += 2)
            {
                var dataPos = i / 2 * 4;
                var temp1 = bytes[i + 1] >> 4;
                var temp2 = bytes[i + 1] & 0xf;
                var temp3 = bytes[i] >> 4;
                var temp4 = bytes[i] & 0xf;
                data[dataPos] = (byte)((temp1 << 4) | temp1);
                data[dataPos + 1] = (byte)((temp2 << 4) | temp2);
                data[dataPos + 2] = (byte)((temp3 << 4) | temp3);
                data[dataPos + 3] = (byte)((temp4 << 4) | temp4);
            }
            return data;
        }
        public static byte[] ReadRGB24(byte[] bytes, int width, int height)
        {
            var len = width * height * 3;
            var data = new byte[width * height * 4];
            for (var i = 0; i < len; i += 3)
            {
                var dataPos = i / 3 * 4;
                data[dataPos] = bytes[i + 2];
                data[dataPos + 1] = bytes[i + 1];
                data[dataPos + 2] = bytes[i];
                data[dataPos + 3] = 0xFF;
            }
            return data;
        }
        public static byte[] ReadRG16(byte[] bytes, int width, int height)
        {
            var len = width * height * 2;
            var data = new byte[width * height * 4];
            for (var i = 0; i < len; i += 2)
            {
                var dataPos = i / 2 * 4;
                data[dataPos] = 0x00;
                data[dataPos + 1] = bytes[i + 1];
                data[dataPos + 2] = bytes[i];
                data[dataPos + 3] = 0xFF;
            }
            return data;
        }
        public static byte[] ReadR16(byte[] bytes, int width, int height)
        {
            var len = width * height * 2;
            var data = new byte[width * height * 4];
            for (var i = 0; i < len; i += 2)
            {
                var dataPos = i / 2 * 4;
                data[dataPos] = 0x00;
                data[dataPos + 1] = 0x00;
                data[dataPos + 2] = (byte)((bytes[i + 1] & bytes[i]) / 0x100);
                data[dataPos + 3] = 0xFF;
            }
            return data;
        }
        public static byte[] ReadR8(byte[] bytes, int width, int height)
        {
            var len = width * height;
            var data = new byte[width * height * 4];
            for (var i = 0; i < len; i++)
            {
                var dataPos = i * 4;
                data[dataPos] = 0x00;
                data[dataPos + 1] = 0x00;
                data[dataPos + 2] = bytes[i];
                data[dataPos + 3] = 0xFF;
            }
            return data;
        }
        public static byte[] ReadAlpha8(byte[] bytes, int width, int height)
        {
            var len = width * height;
            var data = new byte[width * height * 4];
            for (var i = 0; i < len; i++)
            {
                var dataPos = i * 4;
                data[dataPos] = 0xFF;
                data[dataPos + 1] = 0xFF;
                data[dataPos + 2] = 0xFF;
                data[dataPos + 3] = bytes[i];
            }
            return data;
        }
    }
}
