namespace UnityTools
{
    public sealed class Hash128
    {
        public byte[] data; //16 bytes
        public Hash128(byte[] data)
        {
            this.data = data;
        }

        public Hash128()
        {
            data = new byte[] { 0 };
        }

        public Hash128(AssetsFileReader reader)
        {
            data = reader.ReadBytes(16);
        }

        public void Write(AssetsFileWriter writer)
        {
            writer.Write(data);
        }
    }
}
