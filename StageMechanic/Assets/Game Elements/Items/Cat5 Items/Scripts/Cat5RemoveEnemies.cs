/*  
 * Copyright (C) 2018 You're Perfect Studio. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System.Collections.Generic;
using UnityEngine;

public class Cat5RemoveEnemies : Cat5AbstractItem {

	public float Radius = 10f;
	private ParticleSystem Animation;

	public override void ApplyTheme(Cat5ItemTheme theme)
	{
		Debug.Assert(theme.EnemyRemovalPlaceholder != null);
		Model1 = theme.EnemyRemovalPlaceholder;
		Model2 = theme.EnemyRemovalObject;
		Animation = theme.EnemyRemovalAnimation;
	}

	public override Dictionary<string, DefaultValue> DefaultProperties
	{
		get
		{
			Dictionary<string, DefaultValue> ret = base.DefaultProperties;
			ret.Add("Radius", new DefaultValue { TypeInfo = typeof(float), Value = "10" });
			return ret;
		}
	}

	public override Dictionary<string, string> Properties
	{
		get
		{
			Dictionary<string, string> ret = base.Properties;
			if (Radius != 15f)
				ret.Add("Radius", Radius.ToString());
			return ret;
		}
		set
		{
			base.Properties = value;
			if (value.ContainsKey("Radius"))
				Radius = float.Parse(value["Radius"]);
		}
	}

	public override void OnPlayerActivate(IPlayerCharacter player)
	{
		base.OnPlayerActivate(player);
		//List<AbstractBlock> blocks = BlockManager.GetBlocksNear(player.Position, Radius);
		List<Vector3> locations = new List<Vector3>();
		foreach(AbstractBlock block in BlockManager.BlockCache)
		{
			if((block.GetComponent<BloxelsEnemyBlock>() != null || block.GetComponent<Cathy1Enemy>() != null)
				&& block.Position.y < (player.Position.y+10))
				locations.Add(block.Position);
		}
		foreach (Vector3 pos in locations)
		{
			if (Animation != null)
				VisualEffectsManager.PlayEffect(BlockManager.GetBlockNear(pos), Animation, 1, 0.5f);
			BlockManager.DestroyBlock(BlockManager.GetBlockNear(pos));
		}
	}

	public override void OnGameModeChanged(GameManager.GameMode newMode, GameManager.GameMode oldMode)
	{
		base.OnGameModeChanged(newMode, oldMode);
		if (newMode == GameManager.GameMode.StageEdit)
			ShowModel(1);
		else if (newMode == GameManager.GameMode.Play)
			ShowModel(2);
	}
}
