﻿/*  
 * Copyright (C) 2018 You're Perfect Studio. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public GameObject Stage;

    public static List<IPlayerCharacter> Avatars { get; set; } = new List<IPlayerCharacter>();
    internal static PlayerManager Instance;

    internal static bool _playMode = false;
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
            }
            else
            {
                DestroyAllPlayers();
            }
        }
    }

    public static int PlayerCount()
    {
		if (Avatars != null && Avatars.Count > 0)
			return Avatars.Count;
		else
			return ItemManager.GetItemsOfType("Player Start").Count;
    }

    public static IPlayerCharacter Player(int playerNumber = 0)
    {
        Debug.Assert(Avatars != null);
        if (Avatars.Count > playerNumber)
            return Avatars[playerNumber];
        return null;
    }

    public static void Clear()
    {
        DestroyAllPlayers();
    }

    public static void OnUndoStart()
    {
        DestroyAllPlayers();
    }

    public static void OnUndoFinish()
    {
    }

    public static void ShowAllPlayers(bool show = true)
    {
		foreach (IPlayerCharacter player in Avatars)
        {
            player.GameObject.SetActive(show);
        }
	}

	public static void HideAllPlayers()
    {
        ShowAllPlayers(false);
    }

    public static void DestroyAllPlayers()
    {
        foreach (IPlayerCharacter player in Avatars) {
            Destroy(player.GameObject);
        }
        Avatars.Clear();
    }


	public static void InstantiatePlayer( int playerNumber, Vector3 location, GameObject avatarPrefab )
    {
		Debug.Log("Spawning player 1 at " + location);
		//TODO support multiplayer
		if (Avatars.Count > 0)
			DestroyAllPlayers();
		Avatars.Add(Instantiate(avatarPrefab, location, Quaternion.identity, Instance.Stage.transform).GetComponent<IPlayerCharacter>());
        Avatars[0].GameObject.layer = BlockManager.Instance.Stage.layer;
        LoadKeybindings(0);
    }


    // Use this for initialization
    void Start () {
        Instance = this;
	}

	public static Vector3 Player1Location()
    {
        if(Avatars.Count>0 && Avatars[0] != null)
        {
            return Avatars[0].GameObject.transform.position;
        }
        //TODO not do it this way
        return new Vector3(-255, -255, -255);
    }

    public static Vector3 Player1FacingDirection()
    {
        Debug.Assert(Avatars.Count > 0 && Avatars[0] != null);
        return Avatars[0].FacingDirection;
    }

    public static void SetPlayer1FacingDirection(Vector3 direction)
    {
		//TODO this should be an assert but it fails when undoing from the death dialog, this is a temporaty fix for Prototype 2
        if(Avatars.Count > 0 && Avatars[0] != null && Avatars[0].GameObject != null && Avatars[0].GameObject.activeInHierarchy)
			Avatars[0].FacingDirection = direction;
    }

    public static string PlayerStateName( int playerNumber = 0 )
    {
        if (Avatars.Count > playerNumber && Avatars[playerNumber] != null)
        {
            return Avatars[playerNumber].StateNames[Avatars[playerNumber].CurrentStateIndex];
        }
        return "Hiding";
    }

    public static int PlayerState( int playerNumber = 0 )
    {
        if (Avatars.Count > playerNumber && Avatars[playerNumber] != null)
        {
            return Avatars[playerNumber].CurrentStateIndex;
        }
        return 0;
    }

    public static void SetPlayer1State(int state)
    {
		//TODO this should be an assert but it fails when undoing from the death dialog, this is a temporaty fix for Prototype 2
		if (Avatars.Count > 0 && Avatars[0] != null && Avatars[0].GameObject != null && Avatars[0].GameObject.activeInHierarchy)
            Avatars[0].CurrentStateIndex = state;
    }

    public static void SetPlayer1Location(Vector3 location)
    {
        if (Avatars.Count > 0 && Avatars[0] != null)
            Avatars[0].Position = location;
    }

    public static void Player1BoingyTo( Vector3 location )
    {
        (Avatars[0] as Cathy1PlayerCharacter).Boingy(location);
    }

    public static void Player1SlideForward()
    {
        (Avatars[0] as Cathy1PlayerCharacter).SlideForward();
    }

    public static float PlayerApplyInput(int playerNumber, List<string> inputs, Dictionary<string, string> parameters = null)
    {
        if (Avatars == null || Avatars.Count < playerNumber)
            return 0f;
        return Avatars[playerNumber].ApplyInput(inputs, parameters);
    }

    private static List<Dictionary<string, string[]>> keybindings = new List<Dictionary<string, string[]>>();

    private static void LoadKeybindings( int playerNumber )
    {
        Debug.Assert(playerNumber >= 0);
        Debug.Assert(Avatars != null);
        Debug.Assert(Avatars.Count > playerNumber);
        Debug.Assert(Avatars[playerNumber] != null);
        Debug.Assert(keybindings != null);

        Dictionary<string, string[]> actual = new Dictionary<string, string[]>();

        Dictionary<string, string[]> suggested = Avatars[playerNumber].SuggestedInputs;
        foreach(KeyValuePair<string,string[]> item in suggested) {
            if (PlayerPrefs.HasKey("Player" + playerNumber + "_Input_" + item.Key))
                actual.Add(item.Key, PlayerPrefs.GetString("Input_Mapping_Player" + playerNumber + "_" + item.Key).Split(','));
            else
                actual.Add(item.Key, item.Value);
        }

        while (keybindings.Count <= playerNumber)
            keybindings.Add(null);

        keybindings[playerNumber] = actual;
    }

    public static Dictionary<string,string[]> PlayerInputOptions(int playerNumber)
    {
        if (Avatars == null || Avatars.Count == 0)
            return null;
        Debug.Assert(keybindings != null);
        Debug.Assert(keybindings.Count > playerNumber);
        return keybindings[playerNumber];
    }

    public static void RemoveKeyBinding(int playerNumber, string action = null, string keyName = null)
    {
        Dictionary<string, string[]> newBindings = new Dictionary<string,string[]>(keybindings[playerNumber]);
        foreach (KeyValuePair<string, string[]> bindings in keybindings[playerNumber])
        {
            List<string> newList = new List<string>();
            foreach(string key in bindings.Value)
            {
                if ((action != null || action != bindings.Key)  && key != keyName)
                    newList.Add(key);
            }
            newBindings[bindings.Key] = newList.ToArray();
        }
        keybindings[playerNumber] = newBindings;
    }

    public static void AddKeyBinding(int playerNumber, string action, string keyName)
    {
        Dictionary<string, string[]> newBindings = new Dictionary<string, string[]>(keybindings[playerNumber]);
        foreach (KeyValuePair<string, string[]> bindings in keybindings[playerNumber])
        {
            List<string> newList = new List<string>(bindings.Value);
            if(bindings.Key == action || action == null)
                newList.Add(keyName);
            newBindings[bindings.Key] = newList.ToArray();
        }
        keybindings[playerNumber] = newBindings;
    }

    public static AbstractPlayerCharacter GetPlayerAt(Vector3 position, float radius = 0.1f)
    {
        return Utility.GetGameObjectAt<AbstractPlayerCharacter>(position, radius);
    }

    public static AbstractPlayerCharacter GetPlayerNear(Vector3 position, float radius = 0.1f)
    {
        return Utility.GetGameObjectNear<AbstractPlayerCharacter>(position, radius);
    }

    public static List<AbstractPlayerCharacter> GetPlayersNear(Vector3 position, float radius = 0.1f)
    {
        return Utility.GetGameObjectsNear<AbstractPlayerCharacter>(position, radius);
    }
}
