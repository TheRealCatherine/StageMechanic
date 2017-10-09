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

[DataContract(Name="Block")]
public class BlockJsonDelegate {

	Block _block;
	public Block Block {
		get {
			return _block;
		}
		set {
			_block = value;
		}
	}

	string _name = null;
	string _type = null;
	Vector3 _pos = new Vector3(-255,-255,-255);


	public BlockJsonDelegate( Block block ) {
		_block = block;
	}

	public BlockJsonDelegate() {
	}

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

	[DataMember(Name="Type",Order=2)]
	public string Type {
		get {
			Debug.Assert (_block != null);
			return _block.Type.ToString ();
		}
		set {
			_type = value;
		}
	}

	[DataMember(Name="RelativePosition",Order=2)]
	public Vector3 RelativePosition {
		get {
			Debug.Assert (_block != null);
			return _block.transform.localPosition;
		}
		set {
			_pos = value;
		}
	}

	[OnDeserialized()]
	internal void OnDeserialedMethod(StreamingContext context)
	{
		Debug.Assert (_name != null);
		Debug.Assert (_type != null);

        //TODO load/save rotation
		Block newBlock = StageCollection.BlockManager.Cathy1BlockFactory().CreateBlock (_pos, new Quaternion(0,0,0,0), _type);
		newBlock.Name = _name;
	}
}
