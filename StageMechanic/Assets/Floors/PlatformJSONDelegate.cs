using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;

[DataContract(Name="Platform")]
public class PlatformJSONDelegate {

	//TODO make private
	public GameObject _platform;

	public PlatformJSONDelegate( GameObject platform ) {
		_platform = platform;
	}

	public PlatformJSONDelegate() {
		_platform = null;
	}

	[DataMember(Name="Name",Order=1)]
	public string Name {
		get {
			Debug.Assert (_platform != null);
			return _platform.name;
		}
		set {
			Debug.Assert (_platform != null);
			_platform.name = value;
		}
	}

	[DataMember(Name="GlobalPosition",Order=2)]
	public Vector3 GlobalPosition {
		get {
			Debug.Assert (_platform != null);
			return _platform.transform.position;
		}
		set {
			Debug.Assert (_platform != null);
			_platform.transform.position = value;
		}
	}

	[DataMember(Name="Blocks",Order=5)]
	public List<BlockJSONDelegate> Blocks {
		get {
			Debug.Assert (_platform != null);
			List<BlockJSONDelegate> ret = new List<BlockJSONDelegate> ();
			foreach (Transform child in _platform.transform) {
				Block block = child.gameObject.GetComponent (typeof(Block)) as Block;
				if(block != null)
					ret.Add (block.GetJSONDelegate ());
			}
			return ret;
		}
		set {
			Debug.Assert (_platform != null);
			foreach (BlockJSONDelegate del in value) {
				del.Block.gameObject.transform.parent = _platform.transform;
			}
		}
	}

}