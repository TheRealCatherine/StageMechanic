using System;
using System.Collections.Generic;

[Serializable]
class StageBinaryDelegate
{
    public List<PlatformBinaryDelegate> Platforms = new List<PlatformBinaryDelegate>();

    public StageBinaryDelegate()
    {
        Platforms.Add(BlockManager.GetPlatformBinaryDelegate());
    }
}
