/*  
 * Copyright (C) 2018 You're Perfect Studio. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */

using System.Collections.Generic;
using UnityEngine;

public enum ItemLocationAffinity
{
	BlockRelative = 0,		//The item will move when the block associated with it moves
	WorldPosition			//The item is not associated 
}

public interface IItem : IPositionable, INameable, IPropertyable
{
    /// <summary>
    /// A string representation of the type of item. Note that this is
    /// used in save files and other places as well as UI. 
    /// </summary>
    string TypeName
    {
        get;
    }

	/// <summary>
	/// Defines if an event should "stick to" a block (ie move with it
	/// like an item) or should be stuck to a given coordinate.
	/// </summary>
	EventLocationAffinity LocationAffinity
	{
		get;
		set;
	}

	EventJsonDelegate GetJsonDelegate();

	/// <summary>
	/// Determines if the player can collect this item or if it should remain
	/// after the player makes contact with it.
	/// </summary>
	bool Collectable { get; set; }

    /// <summary>
    /// How much score should the player gain or lose by collecting this item?
    /// </summary>
    long Score { get; set; }


}
