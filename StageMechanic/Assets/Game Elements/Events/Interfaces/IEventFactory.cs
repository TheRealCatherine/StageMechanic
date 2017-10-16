using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEventFactory {

    IEvent CreateEvent(Vector3 globalPosition, Quaternion globalRotation, int eventTypeIndex, GameObject parent = null);
    IEvent CreateEvent(Vector3 globalPosition, Quaternion globalRotation, string eventTypeName, GameObject parent = null);
    IEvent CreateEvent(int eventTypeIndex, IBlock parent);
    IEvent CreateEvent(string eventTypeName, IBlock parent);

}
