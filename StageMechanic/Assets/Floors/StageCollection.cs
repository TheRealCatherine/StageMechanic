using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;

[DataContract(Name="StageCollection")]
public class StageCollection {

	List<StageJSONDelegate> _stages = new List<StageJSONDelegate>();

	public StageCollection( StageJSONDelegate stage ) {
		_stages.Add(stage);
	}

	public StageCollection() {
	}

	[DataMember(Name="Name",Order=1)]
	public string Name {
		get {
			Debug.Assert (_stages != null);
			return "MyStages";
		}
		set {
			Debug.Assert (_stages != null);
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

	[DataMember(Name="Stages",Order=3)]
	public List<StageJSONDelegate> Stages {
		get {
			return _stages;
		}
		set {
			_stages = value;
		}
	}

}
