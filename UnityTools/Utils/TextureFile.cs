﻿using System.IO;

namespace UnityTools
{
    public class TextureFile
    {
        public string m_Name;
        public int m_ForcedFallbackFormat;
        public bool m_DownscaleFallback;
        public int m_Width;
        public int m_Height;
        public int m_CompleteImageSize;
        public int m_TextureFormat;
        public int m_MipCount;
        public bool m_MipMap;
        public bool m_IsReadable;
        public bool m_ReadAllowed;
        public bool m_StreamingMipmaps;
        public int m_StreamingMipmapsPriority;
        public int m_ImageCount;
        public int m_TextureDimension;
        public GLTextureSettings m_TextureSettings;
        public int m_LightmapFormat;
        public int m_ColorSpace;
        public byte[] pictureData;
        public StreamingInfo m_StreamData;

        public struct GLTextureSettings
        {
            public int m_FilterMode;
            public int m_Aniso;
            public float m_MipBias;
            public int m_WrapMode;
            public int m_WrapU;
            public int m_WrapV;
            public int m_WrapW;
        }

        public struct StreamingInfo
        {
            public ulong offset;
            public uint size;
            public string path;
        }

        public static TextureFile ReadTextureFile(AssetTypeValueField baseField)
        {
            var texture = new TextureFile();
            AssetTypeValueField tempField;

            texture.m_Name = baseField.Get("m_Name").Value.AsString();

            if (!(tempField = baseField.Get("m_ForcedFallbackFormat")).IsDummy())
                texture.m_ForcedFallbackFormat = tempField.Value.AsInt();

            if (!(tempField = baseField.Get("m_DownscaleFallback")).IsDummy())
                texture.m_DownscaleFallback = tempField.Value.AsBool();

            texture.m_Width = baseField.Get("m_Width").Value.AsInt();

            texture.m_Height = baseField.Get("m_Height").Value.AsInt();

            if (!(tempField = baseField.Get("m_CompleteImageSize")).IsDummy())
                texture.m_CompleteImageSize = tempField.Value.AsInt();

            texture.m_TextureFormat = baseField.Get("m_TextureFormat").Value.AsInt();

            if (!(tempField = baseField.Get("m_MipCount")).IsDummy())
                texture.m_MipCount = tempField.Value.AsInt();

            if (!(tempField = baseField.Get("m_MipMap")).IsDummy())
                texture.m_MipMap = tempField.Value.AsBool();

            texture.m_IsReadable = baseField.Get("m_IsReadable").Value.AsBool();

            if (!(tempField = baseField.Get("m_ReadAllowed")).IsDummy())
                texture.m_ReadAllowed = tempField.Value.AsBool();

            if (!(tempField = baseField.Get("m_StreamingMipmaps")).IsDummy())
                texture.m_StreamingMipmaps = tempField.Value.AsBool();

            if (!(tempField = baseField.Get("m_StreamingMipmapsPriority")).IsDummy())
                texture.m_StreamingMipmapsPriority = tempField.Value.AsInt();

            texture.m_ImageCount = baseField.Get("m_ImageCount").Value.AsInt();

            texture.m_TextureDimension = baseField.Get("m_TextureDimension").Value.AsInt();

            var textureSettings = baseField.Get("m_TextureSettings");

            texture.m_TextureSettings.m_FilterMode = textureSettings.Get("m_FilterMode").Value.AsInt();

            texture.m_TextureSettings.m_Aniso = textureSettings.Get("m_Aniso").Value.AsInt();

            texture.m_TextureSettings.m_MipBias = textureSettings.Get("m_MipBias").Value.AsFloat();

            if (!(tempField = textureSettings.Get("m_WrapMode")).IsDummy())
                texture.m_TextureSettings.m_WrapMode = tempField.Value.AsInt();

            if (!(tempField = textureSettings.Get("m_WrapU")).IsDummy())
                texture.m_TextureSettings.m_WrapU = tempField.Value.AsInt();

            if (!(tempField = textureSettings.Get("m_WrapV")).IsDummy())
                texture.m_TextureSettings.m_WrapV = tempField.Value.AsInt();

            if (!(tempField = textureSettings.Get("m_WrapW")).IsDummy())
                texture.m_TextureSettings.m_WrapW = tempField.Value.AsInt();

            if (!(tempField = baseField.Get("m_LightmapFormat")).IsDummy())
                texture.m_LightmapFormat = tempField.Value.AsInt();

            if (!(tempField = baseField.Get("m_ColorSpace")).IsDummy())
                texture.m_ColorSpace = tempField.Value.AsInt();

            var imageData = baseField.Get("image data");
            if (imageData.TemplateField.valueType == EnumValueTypes.ByteArray)
            {
                texture.pictureData = imageData.Value.AsByteArray().data;
            }
            else
            {
                var imageDataSize = imageData.Value.AsArray().size;
                texture.pictureData = new byte[imageDataSize];
                for (var i = 0; i < imageDataSize; i++)
                {
                    texture.pictureData[i] = (byte)imageData[i].Value.AsInt();
                }
            }

            AssetTypeValueField streamData;

            if (!(streamData = baseField.Get("m_StreamData")).IsDummy())
            {
                texture.m_StreamData.offset = streamData.Get("offset").Value.AsUInt64();
                texture.m_StreamData.size = streamData.Get("size").Value.AsUInt();
                texture.m_StreamData.path = streamData.Get("path").Value.AsString();
            }

            return texture;
        }

        //default setting for unitytools
        //usually you have to cd to the assets file
        public byte[] GetTextureData() => GetTextureData(Directory.GetCurrentDirectory());

        //new functions since I didn't like the way unitytools handled it
        public byte[] GetTextureData(AssetsFileInstance inst) => GetTextureData(Path.GetDirectoryName(inst.path));

        public byte[] GetTextureData(AssetsFile file)
        {
            string path = null;
            if (file.readerPar is FileStream fs)
            {
                path = Path.GetDirectoryName(fs.Name);
            }
            return GetTextureData(path);
        }

        public byte[] GetTextureData(string rootPath)
        {
            if (m_StreamData.size != 0 && m_StreamData.path != string.Empty)
            {
                var fixedStreamPath = m_StreamData.path;
                if (!Path.IsPathRooted(fixedStreamPath) && rootPath != null)
                {
                    fixedStreamPath = Path.Combine(rootPath, fixedStreamPath);
                }
                if (File.Exists(fixedStreamPath))
                {
                    Stream stream = File.OpenRead(fixedStreamPath);
                    stream.Position = (long)m_StreamData.offset;
                    pictureData = new byte[m_StreamData.size];
                    stream.Read(pictureData, 0, (int)m_StreamData.size);
                }
                else
                {
                    return null;
                }
            }
            var width = m_Width;
            var height = m_Height;
            var texFmt = (TextureFormat)m_TextureFormat;
            return GetTextureDataFromBytes(pictureData, texFmt, width, height);
        }

        public static byte[] GetTextureDataFromBytes(byte[] data, TextureFormat texFmt, int width, int height)
        {
            return texFmt switch
            {
                TextureFormat.R8 => RGBADecoders.ReadR8(data, width, height),
                TextureFormat.R16 => RGBADecoders.ReadR16(data, width, height),
                TextureFormat.RG16 => RGBADecoders.ReadRG16(data, width, height),
                TextureFormat.RGB24 => RGBADecoders.ReadRGB24(data, width, height),
                TextureFormat.RGB565 => RGBADecoders.ReadRGB565(data, width, height),
                TextureFormat.RGBA32 => RGBADecoders.ReadRGBA32(data, width, height),
                TextureFormat.ARGB32 => RGBADecoders.ReadARGB32(data, width, height),
                TextureFormat.RGBA4444 => RGBADecoders.ReadRGBA4444(data, width, height),
                TextureFormat.ARGB4444 => RGBADecoders.ReadARGB4444(data, width, height),
                TextureFormat.Alpha8 => RGBADecoders.ReadAlpha8(data, width, height),
                TextureFormat.RHalf => RGBADecoders.ReadRHalf(data, width, height),
                TextureFormat.RGHalf => RGBADecoders.ReadRGHalf(data, width, height),
                TextureFormat.RGBAHalf => RGBADecoders.ReadRGBAHalf(data, width, height),
                TextureFormat.DXT1 => DXTDecoders.ReadDXT1(data, width, height),
                TextureFormat.DXT5 => DXTDecoders.ReadDXT5(data, width, height),
                TextureFormat.BC7 => BC7Decoder.ReadBC7(data, width, height),
                TextureFormat.ETC_RGB4 => ETCDecoders.ReadETC(data, width, height),
                TextureFormat.ETC2_RGB4 => ETCDecoders.ReadETC(data, width, height, true),
                _ => null
            };
        }
    }

    public enum TextureFormat
    {
        Alpha8 = 1, //Unity 1.5 or earlier (already in 1.2.2 according to documentation)
        ARGB4444, //Unity 3.0 (already in 1.2.2)
        RGB24, //Unity 1.5 or earlier (already in 1.2.2)
        RGBA32, //Unity 3.2 (not sure about 1.2.2)
        ARGB32, //Unity 1.5 or earlier (already in 1.2.2)
        UNUSED06,
        RGB565, //Unity 3.0 (already in 1.2.2)
        UNUSED08,
        R16, //Unity 5.0
        DXT1, //Unity 2.0 (already in 1.2.2)
        UNUSED11, //(DXT3 in 1.2.2?)
        DXT5, //Unity 2.0
        RGBA4444, //Unity 4.1
        BGRA32New, //Unity 4.5
        RHalf, //Unity 5.0
        RGHalf, //Unity 5.0
        RGBAHalf, //Unity 5.0
        RFloat, //Unity 5.0
        RGFloat, //Unity 5.0
        RGBAFloat, //Unity 5.0
        YUY2, //Unity 5.0
        RGB9e5Float, //Unity 5.6
        UNUSED23,
        BC6H, //Unity 5.5
        BC7, //Unity 5.5
        BC4, //Unity 5.5
        BC5, //Unity 5.5
        DXT1Crunched, //Unity 5.0 //SupportsTextureFormat version codes 0 (original) and 1 (Unity 2017.3)
        DXT5Crunched, //Unity 5.0 //SupportsTextureFormat version codes 0 (original) and 1 (Unity 2017.3)
        PVRTC_RGB2, //Unity 2.6
        PVRTC_RGBA2, //Unity 2.6
        PVRTC_RGB4, //Unity 2.6
        PVRTC_RGBA4, //Unity 2.6
        ETC_RGB4, //Unity 3.0
        ATC_RGB4, //Unity 3.4, removed in 2018.1
        ATC_RGBA8, //Unity 3.4, removed in 2018.1
        BGRA32Old, //Unity 3.4, removed in Unity 4.5
        UNUSED38, //TexFmt_ATF_RGB_DXT1, added in Unity 3.5, removed in Unity 5.0
        UNUSED39, //TexFmt_ATF_RGBA_JPG, added in Unity 3.5, removed in Unity 5.0
        UNUSED40, //TexFmt_ATF_RGB_JPG, added in Unity 3.5, removed in Unity 5.0
        EAC_R, //Unity 4.5
        EAC_R_SIGNED, //Unity 4.5
        EAC_RG, //Unity 4.5
        EAC_RG_SIGNED, //Unity 4.5
        ETC2_RGB4, //Unity 4.5
        ETC2_RGBA1, //Unity 4.5 //R4G4B4A1
        ETC2_RGBA8, //Unity 4.5 //R8G8B8A8
        ASTC_RGB_4x4, //Unity 4.5
        ASTC_RGB_5x5, //Unity 4.5
        ASTC_RGB_6x6, //Unity 4.5
        ASTC_RGB_8x8, //Unity 4.5
        ASTC_RGB_10x10, //Unity 4.5
        ASTC_RGB_12x12, //Unity 4.5
        ASTC_RGBA_4x4, //Unity 4.5
        ASTC_RGBA_5x5, //Unity 4.5
        ASTC_RGBA_6x6, //Unity 4.5
        ASTC_RGBA_8x8, //Unity 4.5
        ASTC_RGBA_10x10, //Unity 4.5
        ASTC_RGBA_12x12, //Unity 4.5
        ETC_RGB4_3DS, //Unity 5.0
        ETC_RGBA8_3DS, //Unity 5.0
        RG16, //Unity 2017.1
        R8, //Unity 2017.1
        ETC_RGB4Crunched, //Unity 2017.3  //SupportsTextureFormat version code 1
        ETC2_RGBA8Crunched //Unity 2017.3  //SupportsTextureFormat version code 1
    }
}
