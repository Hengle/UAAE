namespace UnityTools
{
    public struct Hash128
    {
        public byte[] Data; //16 bytes
        public Hash128(byte[] data)
        {
            Data = data;
        }

        public Hash128(AssetsFileReader reader)
        {
            Data = reader.ReadBytes(16);
        }

        public void Write(AssetsFileWriter writer)
        {
            writer.Write(Data);
        }
    }
}
