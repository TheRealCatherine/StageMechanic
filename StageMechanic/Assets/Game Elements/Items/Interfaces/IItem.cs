/*  
 * Copyright (C) 2018 You're Perfect Studio. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */

using System.Collections.Generic;
using UnityEngine;

public interface IItem : IPositionable, INameable, IPropertyable, IHierarchical
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
	/// Items may be owned by a block. In this case their position should be relative to the block
	/// (ie it should move with the block)
	/// <quote>"When I move, you move. Just like that" --Ludacris</quote>
	/// </summary>
	IBlock OwningBlock
	{
		get;
		set;
	}

	/// <summary>
	/// Should return the underlying gameobject in the scene associated with this block.
	/// Callers should check for null as there is no strict requirement for all items
	/// to have in-scene components.
	/// </summary>
	GameObject GameObject
	{
		get;
	}

	ItemJsonDelegate GetJsonDelegate();

	Sprite Icon { get; }

	/// <summary>
	/// Determines if the player can collect this item or if it should remain
	/// after the player makes contact with it.
	/// </summary>
	bool Collectable { get; set; }

	/// <summary>
	/// Number of times item can be used
	/// </summary>
	int Uses { get; set; }

	/// <summary>
	/// Determines if this should be treated as a normal item, or if this is
	/// a special locaion.
	/// </summary>
	bool Trigger { get; set; }

    /// <summary>
    /// How much score should the player gain or lose by making contact this item?
    /// </summary>
    int Score { get; set; }

	/// <summary>
	/// Method to be called when a player activates the item from their inventory
	/// </summary>
	/// <param name="player"></param>
	void OnPlayerActivate(IPlayerCharacter player);

	/// <summary>
	/// Method to be called when a player makes contact with this item
	/// </summary>
	/// <param name="player"></param>
	void OnPlayerContact(IPlayerCharacter player);

	/// <summary>
	/// Method to be called when an enemy makes contact with this item
	/// </summary>
	/// <param name="enemy"></param>
	void OnEnemyContact(INonPlayerCharacter enemy);

	/// <summary>
	/// Method to be called when the owning block (if any) is being destroyed
	/// This could be used, for example, to reparent or clone the item before
	/// the block's destrctor destroys it also.
	/// </summary>
	void OnBlockDestroyed();

	/// <summary>
	/// Method to be called whenever the game mode changes, for example when switching
	/// between play mode and create mode
	/// </summary>
	/// <param name="newMode"></param>
	/// <param name="oldMode"></param>
	void OnGameModeChanged(GameManager.GameMode newMode, GameManager.GameMode oldMode);
}
