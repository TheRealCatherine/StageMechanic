using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

/// <summary>
/// Like the other *JsonDelegate classes, this one is responsible for being a
/// DataContract type that is used directly by C# to serialize and deserialize
/// to JSON. This class is not intended for usage outside of this context.
/// </summary>
[DataContract(Name = "Cathy1BombBlock")]
public class Cathy1BombBlockJsonDelegate : BlockJsonDelegate {

    internal string _size;

    public Cathy1BombBlockJsonDelegate( Cathy1BombBlock block ) : base(block)
    {
        
    }

    [DataMember(Name = "Size", Order = 4)]
    public string Size
    {
        get
        {
            Debug.Assert(_block != null);
            Cathy1BombBlock bombBlock = _block as Cathy1BombBlock;
            Debug.Assert(bombBlock != null);
            if(bombBlock.Size == Cathy1BombBlock.BombSize.Large)
                return "Large";
            return "Small";
        }
        set
        {
            _size = value;
        }
    }

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
        Debug.Assert(_size != null);

        IBlock newBlock;
        if (_size == "Large")
        //TODO support different block factories
            newBlock = StageCollection.BlockManager.Cathy1BlockFactory.CreateBlock(_pos, _rot, Cathy1Block.BlockType.Bomb2, StageCollection.BlockManager.ActiveFloor);
        else
            newBlock = StageCollection.BlockManager.Cathy1BlockFactory.CreateBlock(_pos, _rot, Cathy1Block.BlockType.Bomb1, StageCollection.BlockManager.ActiveFloor);
        newBlock.Name = _name;
    }
}
