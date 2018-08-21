/*  
 * Copyright (C) 2018 You're Perfect Studio. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
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
