using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloxelsStoryBlock : AbstractBloxelsBlock
{
	public override void Awake()
	{
		base.Awake();
		DensityFactor = 0.0f;
	}

	public override string TypeName
	{
		get
		{
			return "Story";
		}

		set
		{
			throw new System.NotImplementedException();
		}
	}

	public override void ApplyTheme(AbstractBlockTheme theme)
	{
		throw new System.NotImplementedException();
	}
}
