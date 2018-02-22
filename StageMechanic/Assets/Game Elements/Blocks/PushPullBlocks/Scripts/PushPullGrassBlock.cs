using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushPullGrassBlock : AbstractPushPullBlock
{

	public override string TypeName
	{
		get
		{
			return "Grass";
		}

		set
		{
			throw new System.NotImplementedException();
		}
	}

	public override void ApplyTheme(PushPullBlockTheme theme)
	{
		Debug.Assert(theme.Grass != null);
		CurrentModel = Instantiate(theme.Grass, transform);
	}
}
