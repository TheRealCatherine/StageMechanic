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