/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using System;

/// <summary>
/// Like the other *JsonDelegate classes, this one is responsible for being a
/// DataContract type that is used directly by C# to serialize and deserialize
/// to JSON. This class is not intended for usage outside of this context.
/// </summary>
[DataContract(Name="Block")]
public class BlockJsonDelegate {

	IBlock _block;
	public IBlock Block {
		get {
			return _block;
		}
		set {
			_block = value;
		}
	}

	string _name = null;
	string _type = null;
	Vector3 _pos;
    Quaternion _rot;


	public BlockJsonDelegate( IBlock block ) {
		_block = block;
	}

	public BlockJsonDelegate() {
	}

    /// <summary>
    /// See <see cref="IBlock.Name"/> for information about this property
    /// </summary>
	[DataMember(Name="Name",Order=1)]
	public string Name {
		get {
			Debug.Assert (_block != null);
			return _block.Name;
		}
		set {
			_name = value;
		}
	}

    /// <summary>
    /// See <see cref="IBlock.TypeName"/> for information about this property
    /// </summary>
	[DataMember(Name="Type",Order=2)]
	public string Type {
		get {
			Debug.Assert (_block != null);
			return _block.TypeName;
		}
		set {
			_type = value;
		}
	}

    /// <summary>
    /// See <see cref="IBlock.Position"/> for information about this property
    /// </summary>
    [DataMember(Name="Position",Order=3)]
	public Vector3 Position {
		get {
			Debug.Assert (_block != null);
            return _block.Position;
		}
		set {
			_pos = value;
		}
	}

    /// <summary>
    /// See <see cref="IBlock.Rotation"/> for information about this property
    /// </summary>
    [DataMember(Name="Rotation",Order =3)]
    public Quaternion Rotation
    {
        get
        {
            Debug.Assert(_block != null);
            return _block.Rotation;
        }
        set
        {
            _rot = value;
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
	internal void OnDeserialedMethod(StreamingContext context)
	{
		Debug.Assert (_name != null);
		Debug.Assert (_type != null);

        //TODO support different block factories
		IBlock newBlock = StageCollection.BlockManager.Cathy1BlockFactory.CreateBlock (_pos, _rot, _type, StageCollection.BlockManager.ActiveFloor);
		newBlock.Name = _name;
	}
}
