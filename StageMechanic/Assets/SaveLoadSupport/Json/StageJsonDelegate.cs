﻿/*  
 * Copyright (C) 2018 You're Perfect Studio. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
[DataContract(Name="Stage")]
public class StageJsonDelegate {

	BlockManager _manager;

	public StageJsonDelegate( BlockManager manager ) {
		_manager = manager;
	}

	public StageJsonDelegate() {
		_manager = StageCollection.BlockManager;
	}

	[DataMember(Name="Name",Order=1)]
	public string Name {
		get {
			Debug.Assert (_manager != null);
			return _manager.Stage.name;
		}
		set {
			if (_manager == null)
				_manager = StageCollection.BlockManager;
			Debug.Assert (_manager != null);
			_manager.Stage.name = value;
		}
	}

	[DataMember(Name="Platforms",Order=3)]
	public List<PlatformJsonDelegate> Platforms {
		get {
			Debug.Assert (_manager != null);
			List<PlatformJsonDelegate> ret = new List<PlatformJsonDelegate> ();
			//TODO export all platforms
			ret.Add (BlockManager.GetPlatformJsonDelegate());
			return ret;
		}
		set {
			Debug.Assert (_manager != null);
			//TODO create all platforms
			foreach (PlatformJsonDelegate platform in value) {
				foreach (Transform child in platform._platform.transform) {
					child.parent = BlockManager.ActiveFloor.transform;
				}
				GameObject.Destroy (platform._platform);
				//TODO _manager.ActiveFloor = platform._platform;
			}
		}
	}
}
