﻿using System.Runtime.InteropServices;
using AssetsTools.NET;
using AssetsTools.NET.Extra;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Texture
{
    public class TextureManager
    {
        public static byte[] ImportTexture(string file, TextureFormat format, out int width, out int height)
        {
            byte[] decData;
            using var image = Image.Load<Rgba32>(file);
            width = image.Width;
            height = image.Height;

            image.Mutate(i => i.Flip(FlipMode.Vertical));
            if (image.TryGetSinglePixelSpan(out var pixelSpan))
            {
                decData = MemoryMarshal.AsBytes(pixelSpan).ToArray();
            }
            else
            {
                return null; //rip
            }

            var encData = EncodeTexture(decData, width, height, format);
            return encData;
        }

        public static bool ExportTexture(byte[] encData, string file, int width, int height, TextureFormat format)
        {
            var decData = DecodeTexture(encData, width, height, format);
            if (decData == null)
                return false;

            var image = Image.LoadPixelData<Rgba32>(decData, width, height);
            image.Mutate(i => i.Flip(FlipMode.Vertical));
            if (file.EndsWith(".tga"))
            {
                image.SaveAsTga(file);
            }
            else
            {
                image.SaveAsPng(file);
            }
            return true;
        }

        public static byte[] DecodeTexture(byte[] data, int width, int height, TextureFormat format)
        {
            switch (format)
            {
                case TextureFormat.RGB9e5Float: //pls don't use (what is this?)
                case TextureFormat.DXT1Crunched:
                case TextureFormat.DXT5Crunched:
                case TextureFormat.ETC_RGB4Crunched:
                case TextureFormat.ETC2_RGBA8Crunched:
                    return null; // todo
                case TextureFormat.ARGB32:
                case TextureFormat.BGRA32New:
                case TextureFormat.RGBA32:
                case TextureFormat.RGB24:
                case TextureFormat.ARGB4444:
                case TextureFormat.RGBA4444:
                case TextureFormat.RGB565:
                case TextureFormat.Alpha8:
                case TextureFormat.R8:
                case TextureFormat.R16:
                case TextureFormat.RG16:
                case TextureFormat.RHalf:
                case TextureFormat.RGHalf:
                case TextureFormat.RGBAHalf:
                case TextureFormat.RFloat:
                case TextureFormat.RGFloat:
                case TextureFormat.RGBAFloat:
                case TextureFormat.YUV2:
                case TextureFormat.EAC_R:
                case TextureFormat.EAC_R_SIGNED:
                case TextureFormat.EAC_RG:
                case TextureFormat.EAC_RG_SIGNED:
                case TextureFormat.ETC_RGB4:
                case TextureFormat.ETC_RGB4_3DS:
                case TextureFormat.ETC_RGBA8_3DS:
                case TextureFormat.ETC2_RGB4:
                case TextureFormat.ETC2_RGBA1:
                case TextureFormat.ETC2_RGBA8:
                case TextureFormat.PVRTC_RGB2:
                case TextureFormat.PVRTC_RGBA2:
                case TextureFormat.PVRTC_RGB4:
                case TextureFormat.PVRTC_RGBA4:
                case TextureFormat.ASTC_RGB_4x4:
                case TextureFormat.ASTC_RGB_5x5:
                case TextureFormat.ASTC_RGB_6x6:
                case TextureFormat.ASTC_RGB_8x8:
                case TextureFormat.ASTC_RGB_10x10:
                case TextureFormat.ASTC_RGB_12x12:
                case TextureFormat.ASTC_RGBA_4x4:
                case TextureFormat.ASTC_RGBA_5x5:
                case TextureFormat.ASTC_RGBA_8x8:
                case TextureFormat.ASTC_RGBA_10x10:
                case TextureFormat.ASTC_RGBA_12x12:
                {
                    var dest = TextureWrapper.DecodeByPVRTexLib(data, (int)format, width, height);
                    return dest;
                }
                case TextureFormat.DXT1:
                    var dxt1 = DXTDecoders.ReadDXT1(data, width, height);
                    for (var i = 0; i < dxt1.Length; i += 4)
                    {
                        var temp = dxt1[i];
                        dxt1[i] = dxt1[i + 2];
                        dxt1[i + 2] = temp;
                    }
                    return dxt1;
                case TextureFormat.DXT5:
                    var dxt5 = DXTDecoders.ReadDXT5(data, width, height);
                    for (var i = 0; i < dxt5.Length; i += 4)
                    {
                        var temp = dxt5[i];
                        dxt5[i] = dxt5[i + 2];
                        dxt5[i + 2] = temp;
                    }
                    return dxt5;
                case TextureFormat.BC7:
                    var bc7 = BC7Decoder.ReadBC7(data, width, height);
                    for (var i = 0; i < bc7.Length; i += 4)
                    {
                        var temp = bc7[i];
                        bc7[i] = bc7[i + 2];
                        bc7[i + 2] = temp;
                    }
                    return bc7;
                case TextureFormat.BC6H:
                case TextureFormat.BC4:
                case TextureFormat.BC5:
                    return null; // todo
                default:
                    return null;
            }
        }

        public static byte[] EncodeTexture(byte[] data, int width, int height, TextureFormat format, int quality = 3)
        {
            switch (format)
            {
                case TextureFormat.RGB9e5Float: //pls don't use (what is this?)
                case TextureFormat.DXT1Crunched:
                case TextureFormat.DXT5Crunched:
                case TextureFormat.ETC_RGB4Crunched:
                case TextureFormat.ETC2_RGBA8Crunched:
                    return null; // todo
                case TextureFormat.ARGB32:
                case TextureFormat.BGRA32New:
                case TextureFormat.RGBA32:
                case TextureFormat.RGB24:
                case TextureFormat.ARGB4444:
                case TextureFormat.RGBA4444:
                case TextureFormat.RGB565:
                case TextureFormat.Alpha8:
                case TextureFormat.R8:
                case TextureFormat.R16:
                case TextureFormat.RG16:
                case TextureFormat.RHalf:
                case TextureFormat.RGHalf:
                case TextureFormat.RGBAHalf:
                case TextureFormat.RFloat:
                case TextureFormat.RGFloat:
                case TextureFormat.RGBAFloat:
                case TextureFormat.YUV2: //looks like this should be YUY2 and the api has a typo
                case TextureFormat.EAC_R:
                case TextureFormat.EAC_R_SIGNED:
                case TextureFormat.EAC_RG:
                case TextureFormat.EAC_RG_SIGNED:
                case TextureFormat.ETC_RGB4:
                case TextureFormat.ETC_RGB4_3DS:
                case TextureFormat.ETC_RGBA8_3DS:
                case TextureFormat.ETC2_RGB4:
                case TextureFormat.ETC2_RGBA1:
                case TextureFormat.ETC2_RGBA8:
                case TextureFormat.PVRTC_RGB2:
                case TextureFormat.PVRTC_RGBA2:
                case TextureFormat.PVRTC_RGB4:
                case TextureFormat.PVRTC_RGBA4:
                case TextureFormat.ASTC_RGB_4x4:
                case TextureFormat.ASTC_RGB_5x5:
                case TextureFormat.ASTC_RGB_6x6:
                case TextureFormat.ASTC_RGB_8x8:
                case TextureFormat.ASTC_RGB_10x10:
                case TextureFormat.ASTC_RGB_12x12:
                case TextureFormat.ASTC_RGBA_4x4:
                case TextureFormat.ASTC_RGBA_5x5:
                case TextureFormat.ASTC_RGBA_8x8:
                case TextureFormat.ASTC_RGBA_10x10:
                case TextureFormat.ASTC_RGBA_12x12:
                case TextureFormat.DXT1:
                case TextureFormat.DXT5:
                {
                    var dest = TextureWrapper.EncodeByPVRTexLib(data, (int)format, quality, 1, width, height);
                    return dest;
                }
                case TextureFormat.BC6H:
                case TextureFormat.BC7:
                {
                    var dest = TextureWrapper.EncodeByISPC(data, (int)format, quality, width, height);
                    return dest;
                }
                case TextureFormat.BC4:
                case TextureFormat.BC5:
                    return null; // todo
                default:
                    return null;
            }
        }
    }
}