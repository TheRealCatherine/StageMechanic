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

public class Cathy1MysteryBlock : Cathy1Block
{
    public sealed override BlockType Type { get; } = BlockType.Random;

    public float Delay = DEFAULT_DELAY;
    public const float DEFAULT_DELAY = 0.05f;

    public readonly string[] PossibleTypes = {
        "Basic",
        "Immobile",
        "Cracked (2 Steps)",
        "Cracked (1 Step)",
        "Heavy",
        "Spike Trap",
        "Ice",
        "Small Bomb",
        "Large Bomb",
        "Spring",
        "Mystery",
        "Monster",
        "Vortex"};

    protected override void OnPlayerEnter(PlayerMovementEvent ev)
    {
        base.OnPlayerEnter(ev);
        StartCoroutine(HandlePlayer(ev));
    }

    bool _started = false;
    virtual internal IEnumerator HandlePlayer(PlayerMovementEvent ev)
    {
        if (ev.Location != PlayerMovementEvent.EventLocation.Top || _started)
            yield break;
        string statename = ev.Player.StateNames[ev.Player.CurrentStateIndex];
        if (statename == "Idle" || statename == "Walk" || statename == "Center")
        {
            _started = true;
            GetComponent<AudioSource>()?.Play();
            yield return new WaitForSeconds(Delay);
            System.Random rnd = new System.Random();
            int index = rnd.Next(PossibleTypes.Length);
            BlockManager.CreateBlockAt(Position, "Cathy1 Internal", PossibleTypes[index]);
        }
    }

    public override Dictionary<string, KeyValuePair<Type, string>> DefaultProperties
    {
        get
        {
            Dictionary<string, KeyValuePair<Type, string>> ret = base.DefaultProperties;
            ret.Add("Trigger Time (seconds)", new KeyValuePair<Type, string>(typeof(float), DEFAULT_DELAY.ToString()));
            return ret;
        }
    }

    public override Dictionary<string, string> Properties
    {
        get
        {
            Dictionary<string, string> ret = base.Properties;
            if (Delay != DEFAULT_DELAY)
                ret.Add("Trigger Time (seconds)", Delay.ToString());
            return ret;
        }
        set
        {
            base.Properties = value;
            if (value.ContainsKey("Trigger Time (seconds)"))
                Delay = float.Parse(value["Trigger Time (seconds)"]);
        }
    }
}
