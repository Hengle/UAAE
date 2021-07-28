using System;
using System.Runtime.InteropServices;

namespace Texture
{
    public class PInvoke
    {
        [DllImport("TexToolWrap.dll")]
        public static extern uint DecodeByCrunchUnity(IntPtr data, uint byteSize, IntPtr buf, int mode, uint width, uint height);

        [DllImport("TexToolWrap.dll")]
        public static extern uint DecodeByPVRTexLib(IntPtr data, IntPtr buf, int mode, uint width, uint height);

        [DllImport("TexToolWrap.dll")]
        public static extern uint EncodeByCrunchUnity(IntPtr data, IntPtr buf, int mode, int level, int space, uint width, uint height);

        [DllImport("TexToolWrap.dll")]
        public static extern uint EncodeByPVRTexLib(IntPtr data, IntPtr buf, int mode, int level, int space, uint width, uint height);

        [DllImport("TexToolWrap.dll")]
        public static extern uint EncodeByISPC(IntPtr data, IntPtr buf, int mode, int level, uint width, uint height);
    }
}
