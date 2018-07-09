using System.Collections;
using UnityEngine;

public class BloxelsHazardBlock : AbstractBloxelsBlock
{
	public override string TypeName
	{
		get
		{
			return "Hazard";
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

	protected override void OnPlayerEnter(PlayerMovementEvent ev)
	{
		base.OnPlayerEnter(ev);
		ev.Player.TakeDamage(10f, "Hazard");
	}

	protected override void OnPlayerStay(PlayerMovementEvent ev)
	{
		base.OnPlayerStay(ev);
		ev.Player.TakeDamage(10f, "Hazard");
	}

}
