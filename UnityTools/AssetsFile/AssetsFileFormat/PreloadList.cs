using System.Collections.Generic;

namespace UnityTools
{
    public class PreloadList
    {
        public int len;
        public List<AssetPPtr> items;

        public void Read(AssetsFileReader reader)
        {
            len = reader.ReadInt32();
            items = new List<AssetPPtr>();
            for (var i = 0; i < len; i++)
            {
                var fileId = reader.ReadInt32();
                reader.Align();
                var pathId = reader.ReadInt64();
                reader.Align();
                items.Add(new AssetPPtr(fileId, pathId));
            }
        }
        public void Write(AssetsFileWriter writer)
        {
            writer.Write(len);
            for (var i = 0; i < len; i++)
            {
                writer.Write(items[i].fileID);
                writer.Align();
                writer.Write(items[i].pathID);
                writer.Align();
            }
        }
    }
}
