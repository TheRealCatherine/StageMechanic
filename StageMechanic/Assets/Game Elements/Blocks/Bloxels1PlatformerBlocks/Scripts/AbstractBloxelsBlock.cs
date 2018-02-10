using UnityEngine;
using System.Collections;

public abstract class AbstractBloxelsBlock : AbstractBlock
{

	public override void Awake()
	{
		base.Awake();
		GravityFactor = 0;
	}

	public abstract override string TypeName { get; set; }

	public abstract void ApplyTheme(AbstractBlockTheme theme);
}
