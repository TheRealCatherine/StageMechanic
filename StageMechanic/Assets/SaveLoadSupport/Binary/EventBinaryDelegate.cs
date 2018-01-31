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
    public string[] PropertyKeys;
    public string[] PropertyValues;

    public EventBinaryDelegate(Cathy1AbstractEvent ev) {
        PositionX = ev.Position.x;
        PositionY = ev.Position.y;
        PositionZ = ev.Position.z;
        Dictionary<string, string> properties = ev.Properties;
        PropertyKeys = properties.Keys.ToArray();
        PropertyValues = properties.Values.ToArray();
    }

    [OnDeserialized]
    private void OnDeserialedMethod(StreamingContext context)
    {
        Dictionary<string, string> properties = new Dictionary<string, string>();
        for (int i = 0; i < PropertyKeys.Length; ++i)
        {
            properties.Add(PropertyKeys[i], PropertyValues[i]);
        }
        Quaternion rotation = Quaternion.identity;
        int playerNumber = 0;
        if (properties.ContainsKey("PlayerNumber"))
            playerNumber = int.Parse(properties["PlayerNumber"]);
        PlayerManager.Instance.GetComponent<EventManager>().CreatePlayerStartLocation(playerNumber, new Vector3(PositionX, PositionY, PositionZ), rotation);
    }

}

