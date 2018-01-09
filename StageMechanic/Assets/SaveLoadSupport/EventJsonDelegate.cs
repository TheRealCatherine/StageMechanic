using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[DataContract(Name="Event")]
public class EventJsonDelegate {
    public IEvent Event { get; set; }

    internal string _name = null;
    internal string _type = null;
    internal Vector3 _pos;
    internal Dictionary<string, string> _properties;

    public EventJsonDelegate( IEvent eventToSerialize )
    {
        Event = eventToSerialize;
    }

    public EventJsonDelegate() { }

    [DataMember(Name = "Name", Order = 10)]
    public string Name
    {
        get
        {
            Debug.Assert(Event != null);
            return Event.Name;
        }
        set
        {
            _name = value;
        }
    }

    //TODO
    [DataMember(Name = "Palette", Order = 20)]
    public string Palette
    {
        get
        {
            return "Cathy1 Internal";
        }
        set
        {

        }
    }

    [DataMember(Name = "Type", Order = 30)]
    public string Type
    {
        get
        {
            Debug.Assert(Event != null);
            return Event.TypeName;
        }
        set
        {
            _type = value;
        }
    }

    [DataMember(Name = "Position", Order = 40)]
    public Vector3 Position
    {
        get
        {
            Debug.Assert(Event != null);
            return Event.Position;
        }
        set
        {
            _pos = value;
        }
    }

    [DataMember(Name = "Properties", Order = 100)]
    public Dictionary<string, string> Properties
    {
        get
        {
            Debug.Assert(Event != null);
            return Event.Properties;
        }
        set
        {
            _properties = value;
        }
    }

    [OnDeserialized()]
    internal void OnDeserialedMethod(StreamingContext context)
    {
        Debug.Assert(_name != null);
        Debug.Assert(_type != null);

        //if (_properties.ContainsKey("Rotation"))
        //Quaternion rotation = Utility.StringToQuaternion(value["Rotation"]);
        //else
        Quaternion rotation = Quaternion.identity;
        //TODO support different block factories
        //IEvent newEvent = PlayerManager.Instance.GetComponent<EventManager>().GetComponent<Cathy1EventFactory>().CreateEvent(_pos, rotation, Cathy1AbstractEvent.EventType.PlayerStart);
        //newEvent.Name = _name;
        //newEvent.Properties = _properties;

        int playerNumber = 0;
        if (_properties.ContainsKey("PlayerNumber"))
            playerNumber = int.Parse(_properties["PlayerNumber"]);
        PlayerManager.Instance.GetComponent<EventManager>().CreatePlayerStartLocation(playerNumber, _pos, rotation);
    }
}
