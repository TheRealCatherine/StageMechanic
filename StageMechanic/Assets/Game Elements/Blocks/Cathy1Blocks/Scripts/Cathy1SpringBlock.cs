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

public class Cathy1SpringBlock : Cathy1Block
{
    public sealed override BlockType Type { get; } = BlockType.Spring;
    private const float DEFAULT_DELAY = 0.75f;
    private const float DEFAULT_DISTANCE = 8f;
    public Vector3 Distance = new Vector3(0f, DEFAULT_DISTANCE, 0f);
    public float Delay = DEFAULT_DELAY;

    private float waitPeriod = 0.0f;
    protected override void OnPlayerStay(PlayerMovementEvent ev)
    {
        base.OnPlayerEnter(ev);
        waitPeriod += Time.deltaTime;
        if (waitPeriod < Delay)
            return;
        StartCoroutine(HandlePlayer(ev));
    }


    virtual internal IEnumerator HandlePlayer(PlayerMovementEvent ev)
    {
        if (ev.Location != PlayerMovementEvent.EventLocation.Top)
            yield break;
        yield return new WaitForSeconds(Delay);
        if ((ev.Player as Cathy1PlayerCharacter)?.CurrentBlock != (this as IBlock))
            yield break;
        string statename = ev.Player.StateNames[ev.Player.CurrentStateIndex];
        if (statename == "Idle" || statename == "Walk" || statename == "Center")
        {
            waitPeriod = 0f;
            PlayerManager.Player1BoingyTo(transform.position + Vector3.up + Distance);
        }
    }

    public override Dictionary<string, DefaultValue> DefaultProperties
    {
        get
        {
            Dictionary<string, DefaultValue> ret = base.DefaultProperties;
            ret.Add("Trigger Time (seconds)",   new DefaultValue { TypeInfo = typeof(float), Value = DEFAULT_DELAY.ToString() } );
            ret.Add("Distance",                 new DefaultValue { TypeInfo = typeof(float), Value = DEFAULT_DISTANCE.ToString() } );
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
            if (Distance.y != DEFAULT_DISTANCE)
                ret.Add("Distance", Distance.y.ToString());
            return ret;
        }
        set
        {
            base.Properties = value;
            if (value.ContainsKey("Trigger Time (seconds)"))
                Delay = float.Parse(value["Trigger Time (seconds)"]);
            if (value.ContainsKey("Distance"))
            {
                float distance = float.Parse(value["Distance"]);
                Distance = new Vector3(0, distance, 0);
            }
        }
    }
}
