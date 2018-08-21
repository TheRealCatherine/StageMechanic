/*  
 * Copyright (C) 2018 You're Perfect Studio. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
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
