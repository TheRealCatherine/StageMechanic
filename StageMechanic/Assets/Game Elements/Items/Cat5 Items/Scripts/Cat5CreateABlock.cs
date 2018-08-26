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

	public override void ApplyTheme(Cat5ItemTheme theme)
	{
		Debug.Assert(theme.CreateBlocksPlaceholder != null);
		Model1 = theme.CreateBlocksPlaceholder;
		Model2 = theme.CreateBasicBlockObject;
		Model3 = theme.CreateImmobileBlockObject;
		Model4 = theme.CreateFloorBlocksObject;
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
	}
}
