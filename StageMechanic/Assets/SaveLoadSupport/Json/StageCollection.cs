/*  
 * Copyright (C) 2018 You're Perfect Studio. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

[DataContract(Name="StageCollection")]
[Serializable]
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
			return "Stage Collection";
		}
		set {
			//TODO
		}
	}

    [DataMember(Name="Version", Order =2)]
    public string Version
    {
        get
        {
            return "1.0 Cathy1";
        }
        set
        {
            //TODO
        }
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
