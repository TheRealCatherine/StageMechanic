/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cathy1CrackedBlock : Cathy1Block {

    public int StepsRemaining = 2;

    public sealed override BlockType Type
    {
        get
        {
            if (StepsRemaining == 1)
                return Cathy1Block.BlockType.Crack1;
            return Cathy1Block.BlockType.Crack2;
        }
    }
}
