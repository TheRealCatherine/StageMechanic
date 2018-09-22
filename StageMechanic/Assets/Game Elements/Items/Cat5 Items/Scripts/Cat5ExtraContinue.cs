/*  
 * Copyright (C) 2018 You're Perfect Studio. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System.Collections.Generic;
using UnityEngine;

public class Cat5ExtraContinue : Cat5AbstractItem {

	public int Continues = 1;

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

	public override void ApplyTheme(Cat5ItemTheme theme)
	{
		Debug.Assert(theme.OneUpPlaceholder != null);
		Model1 = theme.OneUpPlaceholder;
		Model2 = theme.OneUpObject;
	}

	public override Dictionary<string, DefaultValue> DefaultProperties
	{
		get
		{
			Dictionary<string, DefaultValue> ret = base.DefaultProperties;
			ret.Add("Continues", new DefaultValue { TypeInfo = typeof(int), Value = "1" });
			return ret;
		}
	}

	public override Dictionary<string, string> Properties
	{
		get
		{
			Dictionary<string, string> ret = base.Properties;
			if (Continues != 1)
				ret.Add("Continues", Continues.ToString());
			return ret;
		}
		set
		{
			base.Properties = value;
			if (value.ContainsKey("Continues"))
				Continues = int.Parse(value["Continues"]);
		}
	}

	public override void OnPlayerActivate(IPlayerCharacter player)
	{
		base.OnPlayerContact(player);
		(player as AbstractPlayerCharacter).Score += 5000;
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
