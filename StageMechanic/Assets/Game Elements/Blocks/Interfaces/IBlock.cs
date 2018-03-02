/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System.Collections.Generic;
using UnityEngine;

public enum BlockMotionState
{
	Unknown = 0,
	Grounded,
	Edged,
	Hovering,
	Falling,
	Moving,
	Sliding,
	Flying,
	Extended = 255
}

public interface IBlock : INameable, IPositionable, IRotatable, IPropertyable, IHierarchical, IPullable, IPushable
{

	/// <summary>
	/// A string representation of the type of block. Note that this is
	/// used in save files and other places as well as UI. Setting this value
	/// should change the type of the block to the specified type.
	/// </summary>
	/// <exception cref="BlockTypeExcpetion">
	/// May throw a BlockTypeException if the caller tries to set an invalid
	/// block type. Implentations may instead choose to handle this situation
	/// by setting the type to a default value or creating a new block type.
	/// </exception>
	string TypeName
	{
		get;
		set;
	}



	/// <summary>
	/// The state of the block in regards to motion. If state is not one of the built
	/// in types, this will return BlockState.Extended and MotionStateName should be consulted
	/// for the current state.
	/// </summary>
	BlockMotionState MotionState
	{
		get;
		set;
	}

	/// <summary>
	/// The state of the block in regards to motion. If not one of the built-int types
	/// IBlock.MotionState will be BlockMotionState.Extended
	/// </summary>
	string MotionStateName
	{
		get;
		set;
	}

	bool GravityEnabled
	{
		get;
		set;
	}

	/// <summary>
	/// A read-only property that is true if the block should not currently be affected by gravity.
	/// </summary>
	bool IsGrounded
	{
		get;
	}

	/// <summary>
	/// Should return the underlying gameobject in the scene associated with this block.
	/// Callers should check for null as there is no strict requirement for all blocks
	/// to have in-scene components.
	/// </summary>
	GameObject GameObject
	{
		get;
	}

	/// <summary>
	/// Multiplier determining how quickly or slowly the player can move the block.
	/// A value of 0 should indicate the block is not movable by the player and a
	/// negative value should either mean not movable by the player or may be used
	/// to indicate reverse mechanics.
	/// </summary>
	float WeightFactor
	{
		get;
		set;
	}

	/// <summary>
	/// Describes how the block reacts to gravity. At 1.0 the block
	/// should fall at a standard rate (determined by the world's
	/// rule set). At 0.0 it should not be affected by gravity.
	/// At 2.0 it will fall twice as fast; 0.5 falls more slowly.
	/// </summary>
	float GravityFactor
	{
		get;
		set;
	}

	float DensityFactor
	{
		get;
		set;
	}

	/// <summary>
	/// A list of items associated with this block. Typically
	/// destroying or moving the block will destroy or move
	/// associated items.
	/// </summary>
	List<GameObject> Items
	{
		get;
		set;
	}

	List<IEvent> Events
	{
		get;
		set;
	}



	/// <summary>
	/// Create and return a new JSON delegate for this Block
	/// This is because GameObjects cannot directly be used
	/// [DataContract] classes.This is primarily used to
	/// serialize the Block information for saving level data.
	/// </summary>
	/// <returns></returns>
	BlockJsonDelegate GetJsonDelegate();

}
