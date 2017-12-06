using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

/// <summary>
/// Like the other *JsonDelegate classes, this one is responsible for being a
/// DataContract type that is used directly by C# to serialize and deserialize
/// to JSON. This class is not intended for usage outside of this context.
/// </summary>
[DataContract(Name = "Cathy1BasicBlock")]
public class Cathy1BasicBlockJsonDelegate : BlockJsonDelegate {

    /// <summary>
    /// While loading from JSON the different properties are stored in temporary
    /// variables and then this method is called automatically on completion and
    /// the block is created. Applying values directly to the block as it loads
    /// causes more overhead as its position/type/etc change.
    /// </summary>
    /// <param name="context"></param>
    [OnDeserialized()]
    override internal void OnDeserialedMethod(StreamingContext context)
    {
        Debug.Assert(_name != null);
        Debug.Assert(_type != null);

        //TODO support different block factories
        IBlock newBlock = StageCollection.BlockManager.Cathy1BlockFactory.CreateBlock(_pos, _rot, Cathy1Block.BlockType.Basic, StageCollection.BlockManager.ActiveFloor);
        newBlock.Name = _name;
    }
}
