/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {

    public void CreatePlayerStartLocation(int playerNumber, Vector3 pos, Quaternion rotation)
    {
        Cathy1PlayerStartLocation ev = GetComponent<Cathy1EventFactory>().CreateEvent(pos, rotation, Cathy1AbstractEvent.EventType.PlayerStart) as Cathy1PlayerStartLocation;
        ev.PlayerNumber = playerNumber;
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
}
