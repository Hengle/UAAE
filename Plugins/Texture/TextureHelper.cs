using System.IO;
using System.Linq;
using AssetsAdvancedEditor.Assets;
using AssetsTools.NET;
using AssetsTools.NET.Extra;

namespace Texture
{
    public static class TextureHelper
    {
        public static AssetTypeInstance GetByteArrayTexture(AssetsWorkspace workspace, AssetContainer tex)
        {
            var textureTemp = workspace.GetTemplateField(tex.Item);
            var image_data = textureTemp.children.FirstOrDefault(f => f.name == "image data");
            if (image_data == null)
                return null;
            image_data.valueType = EnumValueTypes.ByteArray;
            var texTypeInst = new AssetTypeInstance(new[] { textureTemp }, tex.FileReader, tex.Item.Position);
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
                    stream.Position = texFile.m_StreamData.offset;
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

        public static bool GetResSTexture(TextureFile texFile, AssetContainer cont)
        {
            var streamInfo = texFile.m_StreamData;
            if (!string.IsNullOrEmpty(streamInfo.path) && cont.FileInstance.parentBundle != null)
            {
                //some versions apparently don't use archive:/
                var searchPath = streamInfo.path;
                if (searchPath.StartsWith("archive:/"))
                    searchPath = searchPath[9..];

                searchPath = Path.GetFileName(searchPath);

                var bundle = cont.FileInstance.parentBundle.file;

                var reader = bundle.reader;
                var dirInf = bundle.bundleInf6.dirInf;
                foreach (var info in dirInf)
                {
                    if (info.name != searchPath) continue;
                    reader.Position = bundle.bundleHeader6.GetFileDataOffset() + info.offset + streamInfo.offset;
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
