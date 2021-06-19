using System;

namespace Texture
{
    public class TextureWrapper
    {
        public static byte[] DecodeByPVRTexLib(byte[] data, int mode, int width, int height)
        {
            var dest = new byte[width * height * 4];
            int size;
            unsafe
            {
                fixed (byte* dataPtr = data)
                fixed (byte* destPtr = dest)
                {
                    var dataIntPtr = (IntPtr)dataPtr;
                    var destIntPtr = (IntPtr)destPtr;
                    size = (int)PInvoke.DecodeByPVRTexLib(dataIntPtr, destIntPtr, mode, (uint)width, (uint)height);
                }
            }

            if (size <= 0)
                return null;

            Array.Resize(ref dest, size);
            return dest;
        }

        public static byte[] EncodeByPVRTexLib(byte[] data, int mode, int level, int space, int width, int height)
        {
            var dest = new byte[width * height * 16];
            int size;
            unsafe
            {
                fixed (byte* dataPtr = data)
                fixed (byte* destPtr = dest)
                {
                    var dataIntPtr = (IntPtr)dataPtr;
                    var destIntPtr = (IntPtr)destPtr;
                    size = (int)PInvoke.EncodeByPVRTexLib(dataIntPtr, destIntPtr, mode, level, space, (uint)width, (uint)height);
                }
            }

            if (size <= 0)
                return null;

            Array.Resize(ref dest, size);
            return dest;
        }

        public static byte[] EncodeByISPC(byte[] data, int mode, int level, int width, int height)
        {
            var dest = new byte[width * height * 4];
            int size;
            unsafe
            {
                fixed (byte* dataPtr = data)
                fixed (byte* destPtr = dest)
                {
                    var dataIntPtr = (IntPtr)dataPtr;
                    var destIntPtr = (IntPtr)destPtr;
                    size = (int)PInvoke.EncodeByISPC(dataIntPtr, destIntPtr, mode, level, (uint)width, (uint)height);
                }
            }

            if (size <= 0)
                return null;

            Array.Resize(ref dest, size);
            return dest;
        }
    }
}
