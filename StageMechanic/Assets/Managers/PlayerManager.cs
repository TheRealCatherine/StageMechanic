/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public GameObject Player1Prefab;

    public static List<Cathy1PlayerStartLocation> PlayerStartLocations { get; set; } = new List<Cathy1PlayerStartLocation>();
    public static List<Cathy1PlayerCharacter> Avatars { get; set; } = new List<Cathy1PlayerCharacter>();

    internal bool _playMode = false;
    public bool PlayMode
    {
        get
        {
            return _playMode;
        }
        set
        {
            _playMode = value;
            if(_playMode)
            {
                SpawnPlayers();
            }
            else
            {
                HidePlayers();
            }
        }
    }

    private void HidePlayers()
    {
        Debug.Log("Hiding");
        foreach (Cathy1PlayerCharacter player in Avatars) {
            Destroy(player.gameObject);
        }
        Avatars.Clear();
    }

    private void SpawnPlayers()
    {
        Debug.Log("Spawning");
        foreach (Cathy1PlayerStartLocation player in PlayerStartLocations)
        {
			Avatars.Add(Instantiate(Player1Prefab, player.transform.position+new Vector3(0f,1f,0f), player.transform.rotation, transform).GetComponent<Cathy1PlayerCharacter>());
        }
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void Player1Jump()
    {
        Avatars[0].Move(Vector3.up);
    }

	internal static void Player1MoveRight(bool pushpull)
    {
		if (pushpull)
			Avatars [0].PushPull (Vector3.right);
		else
			Avatars[0].Move(Vector3.right);
    }

	internal static void Player1MoveAway(bool pushpull)
    {
		if (pushpull)
			Avatars [0].PushPull (Vector3.forward);
		else
	        Avatars[0].Move(Vector3.forward);
    }

	internal static void Player1MoveLeft(bool pushpull)
    {
		if (pushpull)
			Avatars [0].PushPull (Vector3.left);
		else
	        Avatars[0].Move(Vector3.left);
    }

	internal static void Player1MoveCloser(bool pushpull)
    {
		if (pushpull)
			Avatars [0].PushPull (Vector3.back);
		else
	        Avatars[0].Move(Vector3.back);
    }
}
