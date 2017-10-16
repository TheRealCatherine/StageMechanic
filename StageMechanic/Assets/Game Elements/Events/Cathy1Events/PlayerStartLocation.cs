using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStartLocation : Cathy1AbstractEvent {

    public override void Awake()
    {
        base.Awake();
        _type = EventType.Player1Start;
    }
}
