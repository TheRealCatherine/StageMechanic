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

public class Cathy1GoalBlock : Cathy1Block
{

    public AudioClip Applause;
    public string NextStageFilename;
    public string NextStageStartPos;

    public sealed override BlockType Type { get; } = BlockType.Goal;

    public override void Awake()
    {
        base.Awake();
        //Make block immobile
        WeightFactor = 0f;
    }

    bool _hasPlayedSound = false;
    virtual internal void HandlePlayer(PlayerMovementEvent ev)
    {
        if (ev.Location != PlayerMovementEvent.EventLocation.Top)
            return;
        string statename = ev.Player.StateNames[ev.Player.CurrentStateIndex];
        if (statename == "Idle" || statename == "Walk" || statename == "Center")
        {
            if (!_hasPlayedSound)
            {
                GetComponent<AudioSource>().PlayOneShot(Applause);
                _hasPlayedSound = true;
            }

            if (Input.anyKey)
            {
                if (!string.IsNullOrWhiteSpace(NextStageFilename) && PlayerPrefs.HasKey("LastLoadDir"))
                {
                    Uri location = new Uri(PlayerPrefs.GetString("LastLoadDir") + "/" + NextStageFilename);
                    Debug.Log("loading " + location.ToString());
                    BlockManager.TogglePlayMode();
                    BlockManager.BlocksFromJson(location);
                    BlockManager.TogglePlayMode();
                }
                else if(string.IsNullOrWhiteSpace(NextStageFilename)) {
                    Debug.Log("No next level specified");
                }
                else if(!PlayerPrefs.HasKey("LastLoadDir"))
                {
                    Debug.Log("Unknown file location");
                }
            }
        }
    }

    protected override void OnPlayerEnter(PlayerMovementEvent ev)
    {
        base.OnPlayerEnter(ev);
        HandlePlayer(ev);
    }

    protected override void OnPlayerStay(PlayerMovementEvent ev)
    {
        base.OnPlayerStay(ev);
        HandlePlayer(ev);
    }

    protected override void OnPlayerLeave(PlayerMovementEvent ev)
    {
        base.OnPlayerLeave(ev);
        _hasPlayedSound = false;
    }

    public override Dictionary<string, KeyValuePair<Type, string>> DefaultProperties
    {
        get
        {
            Dictionary<string, KeyValuePair<Type, string>> ret = base.DefaultProperties;
            ret.Add("Next Stage Filename", new KeyValuePair<Type, string>(typeof(string), ""));
            ret.Add("Next Stage Start Block Override", new KeyValuePair<Type, string>(typeof(string), ""));
            return ret;
        }
    }

    public override Dictionary<string, string> Properties
    {
        get
        {
            Dictionary<string, string> ret = base.Properties;
            if (!string.IsNullOrEmpty(NextStageFilename) && !string.IsNullOrWhiteSpace(NextStageFilename))
                ret.Add("Next Stage Filename", NextStageFilename);
            if (!string.IsNullOrEmpty(NextStageStartPos) && !string.IsNullOrWhiteSpace(NextStageStartPos))
                ret.Add("Next Stage Start Block Override", NextStageStartPos);
            return ret;
        }
        set
        {
            base.Properties = value;
            if (value.ContainsKey("Next Stage Filename"))
                NextStageFilename = value["Next Stage Filename"];
            if (value.ContainsKey("Next Stage Start Block Override"))
                NextStageStartPos = value["Next Stage Start Block Override"];
        }
    }
}
