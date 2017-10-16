using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {

    public void CreatePlayerStartLocation(int playerNumber, Vector3 pos, Quaternion rotation)
    {
        Cathy1PlayerStartLocation ev = GetComponent<Cathy1EventFactory>().CreateEvent(pos, rotation, Cathy1AbstractEvent.EventType.PlayerStart) as Cathy1PlayerStartLocation;
        ev.PlayerNumber = playerNumber;
        PlayerManager.PlayerStartLocations.Add(ev);
    }
}
