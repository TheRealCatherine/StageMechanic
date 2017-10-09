/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;

[DataContract(Name="Platform")]
public class PlatformJsonDelegate {

	//TODO make private
	public GameObject _platform;

	public PlatformJsonDelegate( GameObject platform ) {
		_platform = platform;
	}

	public PlatformJsonDelegate() {
		_platform = GameObject.CreatePrimitive (PrimitiveType.Plane);
	}

	[DataMember(Name="Name",Order=1)]
	public string Name {
		get {
			Debug.Assert (_platform != null);
			return _platform.name;
		}
		set {
			if(_platform == null)
				_platform = GameObject.CreatePrimitive (PrimitiveType.Plane);
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
	public List<BlockJsonDelegate> Blocks {
		get {
			Debug.Assert (_platform != null);
			List<BlockJsonDelegate> ret = new List<BlockJsonDelegate> ();
			foreach (Transform child in _platform.transform) {
				Block block = child.gameObject.GetComponent<Block>();
				if(block != null)
					ret.Add (block.GetJsonDelegate ());
			}
			return ret;
		}
		set {
			Debug.Assert (_platform != null);
			foreach (BlockJsonDelegate del in value) {
                //TODO do this in a Deserialzed method but #NotLikeThiiiiiissssss
                if(del != null && del.Block != null && del.Block.gameObject != null)
				    del.Block.gameObject.transform.parent = _platform.transform;
			}
		}
	}

}