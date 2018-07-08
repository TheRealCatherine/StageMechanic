public class BloxelsCoinBlock : AbstractBloxelsBlock
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
			return "Coin";
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

	//TODO handle this via "event"-style method overrides from AbstractBlock instead of in Update
	internal override void Update()
	{
		base.Update();
		if (BlockManager.PlayMode)
		{
			AbstractPlayerCharacter player = PlayerManager.GetPlayerNear(Position);
			if (player != null)
			{
				//TODO access via AbstractBlock.Properties
				player.Score += 100;
				BlockManager.DestroyBlock(this);
				return;
			}
			AbstractBlock block = BlockManager.GetBlockNear(Position);
			if (block != null && block != this)
			{
				BlockManager.DestroyBlock(this);
				return;
			}
		}
	}
}
