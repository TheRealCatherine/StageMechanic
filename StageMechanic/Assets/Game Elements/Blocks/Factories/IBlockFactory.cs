/*  
 * Copyright (C) 2018 You're Perfect Studio. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using UnityEngine;

public interface IBlockFactory
{

	string Name
	{
		get;
	}

	string[] BlockTypeNames
	{
		get;
	}

	Sprite IconForType(string name);
	IBlock CreateBlock(Vector3 globalPosition, Quaternion globalRotation, string blockTypeName, GameObject parent = null);
}
