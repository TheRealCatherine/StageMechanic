using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using System;

[DataContract(Name="Block")]
public class BlockJSONDelegate {

	Block _block;
	public Block Block {
		get {
			return _block;
		}
		set {
			_block = value;
		}
	}

	public BlockJSONDelegate( Block block ) {
		_block = block;
	}

	public BlockJSONDelegate() {
		_block = new Block ();
	}

	[DataMember(Name="Name",Order=1)]
	public string Name {
		get {
			Debug.Assert (_block != null);
			return _block.Name;
		}
		set {
			Debug.Assert (_block != null);
			_block.Name = value;
		}
	}

	[DataMember(Name="Type",Order=2)]
	public string Type {
		get {
			Debug.Assert (_block != null);
			return _block.Type.ToString ();
		}
		set {
			try {
				Block.BlockType type = (Block.BlockType)Enum.Parse (typeof(Block.BlockType), value);
				if (Enum.IsDefined (typeof(Block.BlockType), type))
					_block.Type = type;
			} catch (ArgumentException e) {
				Debug.Log (e.Message);
			}
		}
	}

	[DataMember(Name="GlobalPosition",Order=3)]
	public Vector3 GlobalPosition {
		get {
			Debug.Assert (_block != null);
			return _block.transform.position;
		}
		set {
			Debug.Assert (_block != null);
			_block.transform.position = value;
		}
	}

	[DataMember(Name="RelativePosition",Order=4)]
	public Vector3 RelativePosition {
		get {
			Debug.Assert (_block != null);
			return _block.transform.localPosition;
		}
		set {
			Debug.Assert (_block != null);
			_block.transform.localPosition = value;
		}
	}
}
