using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloxelsEnemyBlock : AbstractBloxelsBlock
{

	public override string TypeName
	{
		get
		{
			return "Enemy";
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
