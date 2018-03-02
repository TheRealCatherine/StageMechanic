/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlatformBinaryDelegate
{
    public List<BlockBinaryDelegate> Blocks = new List<BlockBinaryDelegate>();
    public List<EventBinaryDelegate> Events = new List<EventBinaryDelegate>();

    public PlatformBinaryDelegate(GameObject platform)
    {
        foreach (IBlock child in BlockManager.BlockCache)
        {
            if (child != null)
                Blocks.Add((child as AbstractBlock).GetBinaryDelegate());
        }
        foreach (IEvent ev in EventManager.EventList)
        {
            if (ev != null)
                Events.Add((ev as Cathy1AbstractEvent).GetBinaryDelegate());
        }
    }
}