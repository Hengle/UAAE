namespace UnityTools
{
    public class AssetID
    {
        public string fileName;
        public long pathID;
        public AssetID(string fileName, long pathID)
        {
            this.fileName = fileName;
            this.pathID = pathID;
        }
        public override bool Equals(object obj)
        {
            if (!(obj is AssetID))
                return false;
            var cobj = (AssetID)obj;
            return cobj.fileName == fileName && cobj.pathID == pathID;
        }
        public override int GetHashCode()
        {
            var hash = 17;
            hash = hash * 23 + fileName.GetHashCode();
            hash = hash * 23 + pathID.GetHashCode();
            return hash;
        }
    }
}
