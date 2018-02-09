using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloxelsGroundBlock : AbstractBlock
{
	public override void Awake()
	{
		base.Awake();
		WeightFactor = 0;
	}

	public override string TypeName
	{
		get
		{
			return "Ground";
		}

		set
		{
			throw new System.NotImplementedException();
		}
	}
}
