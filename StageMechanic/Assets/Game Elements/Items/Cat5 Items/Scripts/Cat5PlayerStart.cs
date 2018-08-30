/*  
 * Copyright (C) 2018 You're Perfect Studio. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System.Collections.Generic;
using UnityEngine;

public class Cat5PlayerStart : Cat5AbstractItem {

	public int PlayerNumber = 1;

	public override int Uses
	{
		get
		{
			return 0;
		}

		set
		{
			base.Uses = value;
		}
	}

	public override bool Collectable
	{
		get
		{
			return false;
		}

		set
		{
			base.Collectable = value;
		}
	}

	public override bool Trigger
	{
		get
		{
			return true;
		}

		set
		{
			base.Trigger = value;
		}
	}


	public override void ApplyTheme(Cat5ItemTheme theme)
	{
		Debug.Assert(theme.PlayerStartPlaceholder != null);
		Model1 = theme.PlayerStartPlaceholder;
		Model2 = theme.PlayerStartObject;
		Model3 = theme.PlayerAvatar;
	}

	/// <summary>
	/// The recognized properties for this item, includes base class properties
	/// </summary>
	public override Dictionary<string, DefaultValue> DefaultProperties
	{
		get
		{
			Dictionary<string, DefaultValue> ret = base.DefaultProperties;
			ret.Add("Player Number", new DefaultValue { TypeInfo = typeof(int), Value = "1" });
			return ret;
		}
	}

	/// <summary>
	/// The list of properties associated with this item.
	/// Includes bass-class properties.
	/// </summary>
	public override Dictionary<string, string> Properties
	{
		get
		{
			Dictionary<string, string> ret = base.Properties;
			if (CurrentModelNumber > 1)
				ret.Add("Player Number", PlayerNumber.ToString());
			return ret;
		}
		set
		{
			base.Properties = value;
			if (value.ContainsKey("Player Number"))
			{
				PlayerNumber = (int.Parse(value["Player Number"]));
			}
		}
	}

	public override void OnGameModeChanged(GameManager.GameMode newMode, GameManager.GameMode oldMode)
	{
		base.OnGameModeChanged(newMode, oldMode);
		if(newMode == GameManager.GameMode.Play)
		{
			PlayerManager.InstantiatePlayer(PlayerNumber, Position, Model3);
		}
	}

}
