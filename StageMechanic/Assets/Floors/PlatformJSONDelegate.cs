using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;

[DataContract(Name="Platform")]
public class PlatformJSONDelegate {

	GameObject _platform;

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

	[DataMember(Name="BlockPalette",Order=3)]
	public string BlockPalette {
		get {
			return "Cathy1-internal";
		}
		set { }
	}

	[DataMember(Name="ItemPalette",Order=3)]
	public string ItemPalette {
		get {
			return "Cathy1-internal";
		}
		set { }
	}

	[DataMember(Name="ActionPalette",Order=3)]
	public string ActionPalette {
		get {
			return "Cathy1-internal";
		}
		set { }
	}

	[DataMember(Name="EnemyPalette",Order=3)]
	public string EnemyPalette {
		get {
			return "Cathy1-internal";
		}
		set { }
	}

	[DataMember(Name="RulesPalette",Order=3)]
	public string RulesPalette {
		get {
			return "Cathy1-internal";
		}
		set { }
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