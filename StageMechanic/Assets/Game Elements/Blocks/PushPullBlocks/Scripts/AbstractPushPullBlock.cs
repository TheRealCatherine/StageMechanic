using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractPushPullBlock : AbstractBlock {

	public GameObject CurrentModel;

	public override void Awake()
	{
		base.Awake();
		GravityFactor = 0;
	}

	public abstract override string TypeName { get; set; }

	public abstract void ApplyTheme(PushPullBlockTheme theme);
}
