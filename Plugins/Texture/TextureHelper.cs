using System.IO;
using System.Linq;
using AssetsAdvancedEditor.Assets;
using UnityTools;

namespace Texture
{
    public static class TextureHelper
    {
        public static AssetTypeInstance GetByteArrayTexture(AssetsWorkspace workspace, AssetItem tex)
        {
            var textureTemp = workspace.GetTemplateField(tex);
            var image_data = textureTemp.children.FirstOrDefault(f => f.name == "image data");
            if (image_data == null)
                return null;
            image_data.valueType = EnumValueTypes.ByteArray;
            var texTypeInst = new AssetTypeInstance(new[] { textureTemp }, tex.Cont.FileReader, tex.Position);
            return texTypeInst;
        }

        public static byte[] GetRawTextureBytes(TextureFile texFile, AssetsFileInstance inst)
        {
            var rootPath = Path.GetDirectoryName(inst.path);
            if (texFile.m_StreamData.size != 0 && texFile.m_StreamData.path != string.Empty)
            {
                var fixedStreamPath = texFile.m_StreamData.path;
                if (!Path.IsPathRooted(fixedStreamPath) && rootPath != null)
                {
                    fixedStreamPath = Path.Combine(rootPath, fixedStreamPath);
                }
                if (File.Exists(fixedStreamPath))
                {
                    Stream stream = File.OpenRead(fixedStreamPath);
                    stream.Position = (long)texFile.m_StreamData.offset;
                    texFile.pictureData = new byte[texFile.m_StreamData.size];
                    stream.Read(texFile.pictureData, 0, (int)texFile.m_StreamData.size);
                }
                else
                {
                    return null;
                }
            }
            return texFile.pictureData;
        }

        public static bool GetResSTexture(TextureFile texFile, AssetItem item)
        {
            var parentBundle = item.Cont.FileInstance.parentBundle;
            var streamInfo = texFile.m_StreamData;
            if (!string.IsNullOrEmpty(streamInfo.path) && parentBundle != null)
            {
                //some versions apparently don't use archive:/
                var searchPath = streamInfo.path;
                if (searchPath.StartsWith("archive:/"))
                    searchPath = searchPath[9..];

                searchPath = Path.GetFileName(searchPath);

                var bundle = parentBundle.file;

                var reader = bundle.reader;
                var dirInf = bundle.bundleInf6.dirInf;
                foreach (var info in dirInf)
                {
                    if (info.name != searchPath) continue;
                    reader.Position = bundle.bundleHeader6.GetFileDataOffset() + info.offset + (long)streamInfo.offset;
                    texFile.pictureData = reader.ReadBytes((int)streamInfo.size);
                    texFile.m_StreamData.offset = 0;
                    texFile.m_StreamData.size = 0;
                    texFile.m_StreamData.path = "";
                    return true;
                }
                return false;
            }
            return true;
        }
    }
}
