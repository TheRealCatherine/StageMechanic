using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cathy1PlayerStartLocation : Cathy1AbstractEvent, IPlayerEventExtension {
    public int PlayerNumber;

    int IPlayerEventExtension.PlayerNumber
    {
        get
        {
            return PlayerNumber;
        }

        set
        {
            PlayerNumber = value;
        }
    }

    public override void Awake()
    {
        base.Awake();
        Type = EventType.PlayerStart;
    }
}
