using System;

namespace UnityTools
{
    [Flags]
    public enum AssetBundleHeaderFlags
    {
        CompressionTypeMask = 0x3F,

        BlocksAndDirectoryInfoCombined = 0x40,
        BlocksInfoAtTheEnd = 0x80,
        OldWebPluginCompatibility = 0x100,
    }
}
