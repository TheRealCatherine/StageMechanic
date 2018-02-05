/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System.Collections.Generic;
using UnityEngine;

public class Cathy1AbstractEvent : MonoBehaviour, IEvent
{

    public enum EventType
    {
        Custom,
        PlayerStart,
        Goal,
        Checkpoint,
        EnemySpawn
    }

    public virtual EventType Type { get; set; }

    public string Name
    {
        get
        {
            return name;
        }

        set
        {
            name = value;
        }
    }

    public virtual string TypeName
    {
        get
        {
            switch (Type)
            {
                case EventType.PlayerStart:
                    return "Player Start";
                case EventType.Goal:
                    return "Goal";
                case EventType.Checkpoint:
                    return "Checkpoint";
                case EventType.EnemySpawn:
                    return "Enemy Spawn";
                case EventType.Custom:
                default:
                    return "Custom";
            }
        }
        set
        {
            switch (value)
            {
                case "Player Start":
                    Type = EventType.PlayerStart;
                    break;
                case "Goal":
                    Type = EventType.Goal;
                    break;
                case "Checkpoint":
                    Type = EventType.Checkpoint;
                    break;
                case "Enemy Spawn":
                    Type = EventType.EnemySpawn;
                    break;
                case "Custom":
                default:
                    Type = EventType.Custom;
                    break;
            }
        }
    }

    public Vector3 Position
    {
        get
        {
            return transform.position;
        }
        set
        {
            transform.position = value;
        }
    }

    public virtual Dictionary<string, string> Properties
    {
        get
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            return ret;
        }
        set
        {
        
            //base.Properties = value;
            //TODO
        }
    }

    public EventLocationAffinity LocationAffinity { get; set; }

    virtual public EventJsonDelegate GetJsonDelegate()
    {
        return new EventJsonDelegate(this);
    }

    public EventBinaryDelegate GetBinaryDelegate()
    {
        return new EventBinaryDelegate(this);
    }

    /// <summary>
    /// Sets the name to a random GUID
    /// Called when this object is created in the scene. If overriding
    /// you may wish to call this base class in order to have the name
    /// set to a random GUID.
    /// </summary>
    public virtual void Awake()
    {
        name = System.Guid.NewGuid().ToString();
    }
}
