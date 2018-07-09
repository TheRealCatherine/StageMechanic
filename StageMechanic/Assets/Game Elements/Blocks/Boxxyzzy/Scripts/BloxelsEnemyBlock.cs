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

	protected override void OnPlayerEnter(PlayerMovementEvent ev)
	{
		base.OnPlayerEnter(ev);
		ev.Player.TakeDamage(10f, "Enemy Contact");
	}

	protected override void OnPlayerStay(PlayerMovementEvent ev)
	{
		base.OnPlayerStay(ev);
		ev.Player.TakeDamage(10f, "Enemy Contact");
	}
}
