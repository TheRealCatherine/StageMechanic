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

	public GameObject[] Cathy1Players;
	public GameObject[] Pusher1Players;

	private void Awake()
	{
		Instance = this;
	}

	public void CreatePlayerStartLocation(string palette, int playerNumber, Vector3 pos, Quaternion rotation)
	{
		if (palette == "Cathy1 Internal" || palette == "Cathy Internal")
			CreateCathy1PlayerStartLocation(playerNumber, pos, rotation);
		else if (palette == "Pusher Internal")
			CreatePusherPlayerStartLocation(playerNumber, pos, rotation);
	}

	public void CreateCathy1PlayerStartLocation(int playerNumber, Vector3 pos, Quaternion rotation)
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

		PlayerManager.Instance.Prefabs[playerNumber] = Cathy1Players[playerNumber];
	}

	public void CreatePusherPlayerStartLocation(int playerNumber, Vector3 pos, Quaternion rotation)
	{
		Debug.Log("Assigning " + playerNumber + " start position: " + pos.ToString());
		Cathy1PlayerStartLocation ev = Cathy1EventFactory.CreateEvent(pos, rotation, Cathy1AbstractEvent.EventType.PlayerStart) as Cathy1PlayerStartLocation;
		ev.PlayerNumber = playerNumber;
		ev.transform.parent = Stage.gameObject.transform;
		ev.Palette = "Pusher Internal";
		if (EventList.Count > playerNumber)
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

		if (PlayerManager.PlayerStartLocations.Count > playerNumber)
			PlayerManager.PlayerStartLocations[playerNumber] = ev;
		else
		{
			for (int x = PlayerManager.PlayerStartLocations.Count; x <= playerNumber; ++x)
			{
				PlayerManager.PlayerStartLocations.Add(null);
			}
			PlayerManager.PlayerStartLocations[playerNumber] = ev;
		}

		PlayerManager.Instance.Prefabs[playerNumber] = Pusher1Players[playerNumber];
	}

	public static void Clear()
	{
		foreach (Cathy1AbstractEvent ev in EventList)
			Destroy(ev.gameObject);
		EventList.Clear();
	}
}
