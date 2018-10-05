/*  
 * Copyright (C) 2018 You're Perfect Studio. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System.Collections.Generic;
using UnityEngine;

public class Cat5SpecialCoin : Cat5Coin
{

	public int Type = 0;

	public override void ApplyTheme(Cat5ItemTheme theme)
	{
		switch (Type)
		{
			case 0:
			default:
				Model1 = theme.SpecialCollectableObject1;
				Model2 = theme.SpecialCollectableObject1;
				break;
			case 1:
				Model1 = theme.SpecialCollectableObject2;
				Model2 = theme.SpecialCollectableObject2;
				break;
			case 2:
				Model1 = theme.SpecialCollectableObject3;
				Model2 = theme.SpecialCollectableObject3;
				break;
			case 3:
				Model1 = theme.SpecialCollectableObject4;
				Model2 = theme.SpecialCollectableObject4;
				break;
		}
		CollectSound = theme.SpecialCollectableSound;
	}
}
