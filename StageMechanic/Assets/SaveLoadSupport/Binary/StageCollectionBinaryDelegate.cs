using System;
using System.Collections.Generic;

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
