using System.Collections;
using UnityEngine;

public class Cathy1LaserBlock : Cathy1Block
{
	public ParticleSystem LaserEffect;

	public override void ApplyTheme(Cathy1BlockTheme theme)
	{
		Debug.Assert(theme.IdleLaser != null);
		Model1 = theme.IdleLaser;
		Model2 = theme.ActiveLaser;
		LaserEffect = theme.LaserEffect;
	}

	internal override void Start()
	{
		base.Start();
		StartCoroutine(DoLaserThingy());
	}

	private IEnumerator DoLaserThingy()
	{
		if (!BlockManager.PlayMode)
			ShowModel(1);
		for(; ;)
		{
			yield return new WaitForSeconds(5);
			if (Model2 != null)
				ShowModel(2);
			else
				ShowModel(1);
			if (LaserEffect != null)
				VisualEffectsManager.PlayEffect(this, LaserEffect, 1, 5, new Vector3(0, 0, 0), Quaternion.Euler(180,0,0));
			yield return new WaitForSeconds(5);
			ShowModel(1);
		}
	}
}
