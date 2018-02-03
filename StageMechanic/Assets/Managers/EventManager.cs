/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {

    public Cathy1EventFactory Cathy1EventFactory;
    public static List<Cathy1AbstractEvent> EventList = new List<Cathy1AbstractEvent>();
    public static EventManager Instance;
    public GameObject Stage;

    private void Awake()
    {
        Instance = this;
    }

    public void CreatePlayerStartLocation(int playerNumber, Vector3 pos, Quaternion rotation)
    {
        Debug.Log("Assigning " + playerNumber + " start position: " + pos.ToString());
        Cathy1PlayerStartLocation ev = Cathy1EventFactory.CreateEvent(pos, rotation, Cathy1AbstractEvent.EventType.PlayerStart) as Cathy1PlayerStartLocation;
        ev.PlayerNumber = playerNumber;
        ev.transform.parent = Stage.gameObject.transform;
        if(EventList.Count > playerNumber)
        {
            Destroy(EventList[playerNumber]);
            EventList[playerNumber] = ev;
        }
        else
        {
            for (int x = EventList.Count; x <= playerNumber; ++x)
            {
                EventList.Add(null);
            }
            EventList[playerNumber] = ev;
        }

        if(PlayerManager.PlayerStartLocations.Count > playerNumber)
            PlayerManager.PlayerStartLocations[playerNumber] = ev;
        else
        {
            for(int x = PlayerManager.PlayerStartLocations.Count;x<=playerNumber;++x)
            {
                PlayerManager.PlayerStartLocations.Add(null);
            }
            PlayerManager.PlayerStartLocations[playerNumber] = ev;
        }
    }

    public static void Clear()
    {
        foreach (Cathy1AbstractEvent ev in EventList)
            Destroy(ev.gameObject);
        EventList.Clear();
    }
}
