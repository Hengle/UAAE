namespace UnityTools
{
    public struct GUID128
    {
        public long mostSignificant;
        public long leastSignificant;
        public void Read(AssetsFileReader reader)
        {
            mostSignificant = reader.ReadInt64();
            leastSignificant = reader.ReadInt64();
        }

        public void Write(AssetsFileWriter writer)
        {
            writer.Write(mostSignificant);
            writer.Write(leastSignificant);
        }
    }
}
