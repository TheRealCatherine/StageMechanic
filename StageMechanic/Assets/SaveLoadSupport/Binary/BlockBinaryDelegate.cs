using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class BlockBinaryDelegate
{
    public string Name;
    public string Type;
    public float PositionX;
    public float PositionY;
    public float PositionZ;
    public Dictionary<string, string> Properties;

    public BlockBinaryDelegate(IBlock block)
    {
        Name = block.Name;
        Type = block.TypeName;
        PositionX = block.Position.x;
        PositionY = block.Position.y;
        PositionZ = block.Position.z;
        Properties = block.Properties;
    }

    [OnDeserialized]
    private void OnDeserialedMethod(StreamingContext context)
    {
        //TODO support different block factories
        IBlock newBlock = BlockManager.CreateBlockAt(new UnityEngine.Vector3(PositionX,PositionY,PositionZ), "Cathy1 Internal", Type);
        newBlock.Name = Name;
        newBlock.Properties = Properties;
    }
}
