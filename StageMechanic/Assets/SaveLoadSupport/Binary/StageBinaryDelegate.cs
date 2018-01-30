using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
class StageBinaryDelegate
{
    public List<PlatformBinaryDelegate> Platforms = new List<PlatformBinaryDelegate>();

    public StageBinaryDelegate()
    {
        Platforms.Add(BlockManager.Instance.GetPlatformBinaryDelegate());
    }
}
