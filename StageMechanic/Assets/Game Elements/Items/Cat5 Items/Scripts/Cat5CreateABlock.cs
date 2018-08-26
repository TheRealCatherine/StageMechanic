/*  
 * Copyright (C) 2018 You're Perfect Studio. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using UnityEngine;

public class Cat5CreateABlock : Cat5AbstractItem {

	public override void ApplyTheme(Cat5ItemTheme theme)
	{
		Debug.Assert(theme.CreateBlocksPlaceholder != null);
		Model1 = theme.CreateBlocksPlaceholder;
		Model2 = theme.CreateBasicBlockObject;
		Model3 = theme.CreateImmobileBlockObject;
		Model4 = theme.CreateFloorBlocksObject;
	}

	public override void OnPlayerActivate(IPlayerCharacter player)
	{
		Debug.Log("Activate");
		base.OnPlayerActivate(player);
	}
}
