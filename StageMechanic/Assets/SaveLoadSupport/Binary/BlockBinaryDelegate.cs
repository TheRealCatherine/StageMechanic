using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

[Serializable]
public class BlockBinaryDelegate
{
    public string Name;
    public string Type;
    public float PositionX;
    public float PositionY;
    public float PositionZ;
    public string[] PropertyKeys;
    public string[] PropertyValues;

    public BlockBinaryDelegate(IBlock block)
    {
        Name = block.Name;
        Type = block.TypeName;
        PositionX = block.Position.x;
        PositionY = block.Position.y;
        PositionZ = block.Position.z;
        Dictionary<string, string> properties = block.Properties;
        PropertyKeys = properties.Keys.ToArray();
        PropertyValues = properties.Values.ToArray();
    }

    [OnDeserialized]
    private void OnDeserialedMethod(StreamingContext context)
    {
        //TODO support different block factories
        IBlock newBlock = BlockManager.CreateBlockAt(new UnityEngine.Vector3(PositionX,PositionY,PositionZ), "Cathy1 Internal", Type);
        newBlock.Name = Name;
        Dictionary<string,string> properties = new Dictionary<string, string>();
        for(int i=0;i<PropertyKeys.Length;++i)
        {
            properties.Add(PropertyKeys[i], PropertyValues[i]);
        }
        newBlock.Properties = properties;
    }
}
