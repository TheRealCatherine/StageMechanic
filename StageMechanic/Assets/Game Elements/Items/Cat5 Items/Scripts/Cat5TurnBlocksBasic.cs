/*  
 * Copyright (C) 2018 You're Perfect Studio. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System.Collections.Generic;
using UnityEngine;

public class Cat5TurnBlocksBasic : Cat5AbstractItem {

	public float Radius = 10f;
	private ParticleSystem Animation;

	public override void ApplyTheme(Cat5ItemTheme theme)
	{
		Debug.Assert(theme.SpecialBlockRemoverPlaceholder != null);
		Model1 = theme.SpecialBlockRemoverPlaceholder;
		Model2 = theme.SpecialBlockRemoverObject;
		Animation = theme.SpecialBlockRemoverAnimation;
		CollectSound = theme.SpecialBlockRemoverCollectSound;
		UseSound = theme.SpecialBlockRemoverUseSound;
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
			if (block.Position.y < (player.Position.y + 10))
			{
				if(block.TypeName != "Basic" && block.TypeName != "Goal" && block.DensityFactor >= 0.7f && block.GetComponent<BloxelsEnemyBlock>() == null)
					locations.Add(block.Position);
			}
		}
		foreach (Vector3 pos in locations)
		{
			BlockManager.CreateBlockAt(pos, "Cat5 Internal", "Basic");
			if (Animation != null)
				VisualEffectsManager.PlayEffect(BlockManager.GetBlockNear(pos), Animation, 2, 1.5f);
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
