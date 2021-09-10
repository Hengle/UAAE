namespace UnityTools
{
    public class AssetPPtr
    {
        public int fileID;
        public long pathID;
        public AssetPPtr(int fileID, long pathID)
        {
            this.fileID = fileID;
            this.pathID = pathID;
        }
    }
}
