using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;

[DataContract(Name="Stage")]
public class StageJSONDelegate {

	BlockManager _manager;

	public StageJSONDelegate( BlockManager manager ) {
		_manager = manager;
	}

	public StageJSONDelegate() {
		_manager = null;
	}

	[DataMember(Name="Name",Order=1)]
	public string Name {
		get {
			Debug.Assert (_manager != null);
			return _manager.name;
		}
		set {
			Debug.Assert (_manager != null);
			_manager.name = value;
		}
	}

	[DataMember(Name="Exits",Order=2)]
	public List<StageJSONDelegate> Exits {
		get;
		set;
	}

	[DataMember(Name="Platforms",Order=3)]
	public List<PlatformJSONDelegate> Platforms {
		get {
			Debug.Assert (_manager != null);
			List<PlatformJSONDelegate> ret = new List<PlatformJSONDelegate> ();
			//TODO export all platforms
			ret.Add (_manager.GetPlatformJSONDelegate());
			return ret;
		}
		set {
			Debug.Assert (_manager != null);
			//TODO create all platforms
			foreach (PlatformJSONDelegate platform in value) {
				_manager.ActiveFloor = platform._platform;
			}
		}
	}
}
