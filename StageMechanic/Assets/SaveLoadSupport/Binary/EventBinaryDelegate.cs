using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class EventBinaryDelegate
{

    public float PositionX;
    public float PositionY;
    public float PositionZ;
    public Dictionary<string, string> Properties;

    public EventBinaryDelegate(Cathy1AbstractEvent ev) {
        PositionX = ev.Position.x;
        PositionY = ev.Position.y;
        PositionZ = ev.Position.z;
        Properties = ev.Properties;
    }

    [OnDeserialized]
    private void OnDeserialedMethod(StreamingContext context)
    {
        Quaternion rotation = Quaternion.identity;
        int playerNumber = 0;
        if (Properties.ContainsKey("PlayerNumber"))
            playerNumber = int.Parse(Properties["PlayerNumber"]);
        PlayerManager.Instance.GetComponent<EventManager>().CreatePlayerStartLocation(playerNumber, new Vector3(PositionX, PositionY, PositionZ), rotation);
    }

}

