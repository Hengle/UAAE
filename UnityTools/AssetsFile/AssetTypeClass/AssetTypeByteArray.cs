namespace UnityTools
{
    public struct AssetTypeByteArray
    {
        public AssetTypeByteArray(byte[] data)
        {
            size = (uint)data.Length;
            this.data = data;
        }

        public uint size;
        public byte[] data;
    }
}
