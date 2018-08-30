/*  
 * Copyright (C) 2018 You're Perfect Studio. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System.Collections.Generic;
using UnityEngine;

public class Cat5CreateABlock : Cat5AbstractItem {

	public string BlockPalette = "Cat5 Internal";
	public string BlockType = "Basic";
	private ParticleSystem Animation;

	public override void ApplyTheme(Cat5ItemTheme theme)
	{
		if (BlockType == "Basic")
		{
			Debug.Assert(theme.CreateBasicBlocksPlaceholder != null);
			Model1 = theme.CreateBasicBlocksPlaceholder;
			Model2 = theme.CreateBasicBlockObject;
		}
		else
		{
			Debug.Assert(theme.CreateImmobileBlocksPlaceholder != null);
			Model1 = theme.CreateImmobileBlocksPlaceholder;
			Model2 = theme.CreateImmobileBlockObject;
		}
		Animation = theme.CreateBlockAnimation;
	}

	public override Dictionary<string, DefaultValue> DefaultProperties
	{
		get
		{
			Dictionary<string, DefaultValue> ret = base.DefaultProperties;
			ret.Add("Block Palette", new DefaultValue { TypeInfo = typeof(string), Value = "Cat5 Internal" });
			ret.Add("Block Type", new DefaultValue { TypeInfo = typeof(bool), Value = "Basic" });
			return ret;
		}
	}

	public override Dictionary<string, string> Properties
	{
		get
		{
			Dictionary<string, string> ret = base.Properties;
			if (BlockPalette != "Cat5 Internal")
				ret.Add("Block Palette", BlockPalette);
			if (BlockType != "Basic")
				ret.Add("Block Type", BlockType);
			return ret;
		}
		set
		{
			base.Properties = value;
			if (value.ContainsKey("Block Palette"))
				BlockPalette = value["Block Palette"];
			if (value.ContainsKey("Block Type"))
				BlockType = value["Block Type"];
		}
	}

	public override void OnPlayerActivate(IPlayerCharacter player)
	{
		BlockManager.CreateBlockAt(player.Position + player.FacingDirection, BlockPalette, BlockType);
		if (Animation != null)
			VisualEffectsManager.PlayEffect(BlockManager.GetBlockNear(player.Position + player.FacingDirection), Animation, 1, 1f);
	}

	public override void OnGameModeChanged(GameManager.GameMode newMode, GameManager.GameMode oldMode)
	{
		if (newMode == GameManager.GameMode.StageEdit)
			ShowModel(1);
		else if (newMode == GameManager.GameMode.Play)
			ShowModel(2);
	}
}
