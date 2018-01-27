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
    private const float DEFAULT_DELAY = 0.55f;
    private const float DEFAULT_DISTANCE = 6f;
    public Vector3 Distance = new Vector3(0f, DEFAULT_DISTANCE, 0f);
    public float Delay = DEFAULT_DELAY;


    IEnumerator DoBoigy()
    {
        if (!hasPlayer())
            yield break;
        yield return new WaitForSeconds(Delay);
        if (!hasPlayer())
            yield break;
        PlayerManager.Player1BoingyTo(transform.position + Vector3.up + Distance);
    }

    bool hasPlayer()
    {
        Vector3 player = PlayerManager.Player1Location();
        return (player == transform.position + Vector3.up && (PlayerManager.PlayerStateName() == "Idle" || PlayerManager.PlayerStateName() == "Walk" || PlayerManager.PlayerStateName() == "Center"));
    }

    internal override void Update()
    {
        base.Update();
        if (!BlockManager.PlayMode)
            return;
        if (!hasPlayer())
            return;
        StartCoroutine(DoBoigy());
        
    }

    public override Dictionary<string, KeyValuePair<Type, string>> DefaultProperties
    {
        get
        {
            Dictionary<string, KeyValuePair<Type, string>> ret = base.DefaultProperties;
            ret.Add("Trigger Time (seconds)", new KeyValuePair<Type, string>(typeof(float), DEFAULT_DELAY.ToString()));
            ret.Add("Distance", new KeyValuePair<Type, string>(typeof(float), DEFAULT_DISTANCE.ToString()));
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
