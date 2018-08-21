/*  
 * Copyright (C) 2018 You're Perfect Studio. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using UnityEngine;

public interface IItemFactory {

	string Name
	{
		get;
	}

	string[] ItemTypeNames
	{
		get;
	}

	Sprite IconForType(string name);
	IItem CreateItem(Vector3 globalPosition, Quaternion globalRotation, string itemTypeName, GameObject parent = null);
	IItem CreateItem(string eventTypeName, IBlock parent);

}
