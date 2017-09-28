using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[DataContract(Name="Block")]
public class BlockJSONDelegate {

	Block _block;

	public BlockJSONDelegate( Block block ) {
		_block = block;
	}

	public BlockJSONDelegate() {
		_block = new Block ();
	}

	[DataMember(Name="Name")]
	public string Name {
		get {
			Debug.Assert (_block != null);
			return _block.name;
		}
		set {
			Debug.Assert (_block != null);
			_block.name = value;
		}
	}

	[DataMember(Name="GlobalPosition")]
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

}
