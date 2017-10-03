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

[DataContract(Name="StageCollection")]
public class StageCollection {

	List<StageJsonDelegate> _stages = new List<StageJsonDelegate>();
	static public BlockManager BlockManager;

	public StageCollection( StageJsonDelegate stage ) {
		_stages.Add(stage);
	}

	public StageCollection( BlockManager manager ) {
		BlockManager = manager;
	}

	[DataMember(Name="Name",Order=1)]
	public string Name {
		get {
			return "MyStages";
		}
		set {
			//TODO
		}
	}

	[DataMember(Name="BlockPalette",Order=2)]
	public string BlockPalette {
		get {
			return "Cathy1-internal";
		}
		set { }
	}

	[DataMember(Name="ItemPalette",Order=2)]
	public string ItemPalette {
		get {
			return "Cathy1-internal";
		}
		set { }
	}

	[DataMember(Name="ActionPalette",Order=2)]
	public string ActionPalette {
		get {
			return "Cathy1-internal";
		}
		set { }
	}

	[DataMember(Name="EnemyPalette",Order=2)]
	public string EnemyPalette {
		get {
			return "Cathy1-internal";
		}
		set { }
	}

	[DataMember(Name="RulesPalette",Order=2)]
	public string RulesPalette {
		get {
			return "Cathy1-internal";
		}
		set { }
	}

	[DataMember(Name="PlayersPalette",Order=2)]
	public string PlayersPalette {
		get {
			return "Cathy1-internal";
		}
		set { }
	}

	[DataMember(Name="PlatformPalette",Order=2)]
	public string PlatformPalette {
		get {
			return "Cathy1-internal";
		}
		set { }
	}

	[DataMember(Name="Stages",Order=3)]
	public List<StageJsonDelegate> Stages {
		get {
			return _stages;
		}
		set {
			_stages = value;
		}
	}

}
