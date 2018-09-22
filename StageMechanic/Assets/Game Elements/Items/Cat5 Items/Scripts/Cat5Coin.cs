/*  
 * Copyright (C) 2018 You're Perfect Studio. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System.Collections.Generic;
using UnityEngine;

public class Cat5Coin : Cat5AbstractItem {

	[SerializeField]
	private int Value;
	public const string SCORE = "Score";

	public override int Score
	{
		get { return Value;  }
		set { Value = value; }
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

	public override void ApplyTheme(Cat5ItemTheme theme)
	{
		Debug.Assert(theme.CoinPlaceholder != null);
		Model1 = theme.CoinPlaceholder;
		Model2 = theme.CoinObject;
	}

	public override Dictionary<string, DefaultValue> DefaultProperties
	{
		get
		{
			Dictionary<string, DefaultValue> ret = base.DefaultProperties;
			ret[SCORE] = new DefaultValue { TypeInfo = typeof(int), Value = Value.ToString() };
			return ret;
		}
	}

	public override Dictionary<string, string> Properties
	{
		get
		{
			Dictionary<string, string> ret = base.Properties;
			if (Score != Value)
				ret[SCORE] = Score.ToString();
			else
				ret.Remove(SCORE);
			return ret;
		}
		set
		{
			base.Properties = value;
		}
	}
}
