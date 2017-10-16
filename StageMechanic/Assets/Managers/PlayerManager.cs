using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public static List<IPlayerEventExtension> PlayerStartLocations { get; set; } = new List<IPlayerEventExtension>();

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
        foreach (IPlayerEventExtension player in PlayerStartLocations) {
            Debug.Log(String.Format("Stopping Player {0}", player.PlayerNumber));
        }
    }

    private void SpawnPlayers()
    {
        Debug.Log("Spawning");
        foreach (IPlayerEventExtension player in PlayerStartLocations)
        {
            Debug.Log(String.Format("Starting Player {0}", player.PlayerNumber));
        }
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
