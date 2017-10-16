using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cathy1AbstractEvent : MonoBehaviour, IEvent
{
    public enum EventType
    {
        Custon,
        Player1Start,
        Player2Start,
        Goal,
        Checkpoint,
        EnemySpawn
    }

    internal EventType _type;

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

    virtual public string TypeName
    {
        get
        {
            switch (_type)
            {
                case EventType.Player1Start:
                    return "Player 1 Start";
                case EventType.Player2Start:
                    return "Player 2 Start";
                case EventType.Goal:
                    return "Goal";
                case EventType.Checkpoint:
                    return "Checkpoint";
                case EventType.EnemySpawn:
                    return "Enemy Spawn";
                case EventType.Custon:
                default:
                    return "Custom";
            }
        }
        set
        {
            switch (value)
            {
                case "Player 1 Start":
                    _type = EventType.Player1Start;
                    break;
                case "Player 2 Start":
                    _type = EventType.Player2Start;
                    break;
                case "Goal":
                    _type = EventType.Goal;
                    break;
                case "Checkpoint":
                    _type = EventType.Checkpoint;
                    break;
                case "Enemy Spawn":
                    _type = EventType.EnemySpawn;
                    break;
                case "Custom":
                default:
                    _type = EventType.Custon;
                    break;
            }
        }
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
