/*  
 * Copyright (C) 2018 You're Perfect Studio. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System.Collections.Generic;
using UnityEngine;

public class Cat5PlayerStart : Cat5AbstractItem {

	private int _playerNumber = 1;
	public int PlayerNumber
	{

		get
		{
			return _playerNumber;
		}
		set
		{
			_playerNumber = value;
			ApplyTheme(_currentTheme);
		}
	}
		

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

	private Cat5ItemTheme _currentTheme;
	public override void ApplyTheme(Cat5ItemTheme theme)
	{
		if (theme == null)
			return;
		_currentTheme = theme;
		if (PlayerNumber == 1 || PlayerNumber > 3)
		{
			Debug.Assert(theme.PlayerStartPlaceholder != null);
			Model1 = theme.PlayerStartPlaceholder;
			Model2 = theme.PlayerStartObject;
			Model3 = theme.PlayerAvatar;
		}
		else if (PlayerNumber == 2)
		{
			Debug.Assert(theme.Player2StartPlaceholder != null);
			Model1 = theme.Player2StartPlaceholder;
			Model2 = theme.Player2StartObject;
			Model3 = theme.Player2Avatar;
		}
		else if (PlayerNumber == 3)
		{
			Debug.Assert(theme.Player3StartPlaceholder != null);
			Model1 = theme.Player3StartPlaceholder;
			Model2 = theme.Player3StartObject;
			Model3 = theme.Player3Avatar;
		}
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
