using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
class StageCollectionBinaryDelegate
{
    public List<StageBinaryDelegate> Stages = new List<StageBinaryDelegate>();

    /// <summary>
    /// Creates the delegate with the given stage added to the list of stages.
    /// </summary>
    /// <param name="stage"></param>
    public StageCollectionBinaryDelegate( StageBinaryDelegate stage )
    {
        Stages.Add(stage);
    }
}
