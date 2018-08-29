/*  
 * Copyright (C) 2018 You're Perfect Studio. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using UnityEngine;

public class Cat5Coin : Cat5AbstractItem {

	public int value = 1000;

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

	public override void OnPlayerContact(IPlayerCharacter player)
	{
		base.OnPlayerContact(player);
		(player as AbstractPlayerCharacter).Score += value;
	}
}
