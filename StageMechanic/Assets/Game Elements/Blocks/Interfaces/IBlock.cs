/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;

public enum BlockMotionState
{
    Uknown = 0,
    Grounded,
    Edged,
    Hovering,
    Falling,
    Moving,
    Sliding,
    Extended = 255
}

public interface IBlock
{
    /// <summary>
    /// A human-readable identifier for the block. Typically implementations
    /// will auto-generate a guid in the case the user has not given a different
    /// name to the block. 
    /// </summary>
    /// <exception cref="BlockNameException">
    /// May throw a BlockNameException if the caller tries to set an invalid name.
    /// There is no inherent requirement for Names to be unique, however certain
    /// implementations may choose to impose this or other requirements on naming.
    /// </exception>
    string Name
    {
        get;
        set;
    }

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
    /// The block's position within the gameworld. This may be relative or global
    /// or have other meaning according to rulset. In initially implemented concepts
    /// this is the global position of the block, but implementors may use other
    /// meanings.
    /// </summary>
    Vector3 Position
    {
        get;
        set;
    }

    /// <summary>
    /// The blocks rotation. This may be the global rotation or relative to its parent
    /// or some other object, according to the defined ruleset. In initial implementations
    /// this is used as a global rotation.
    /// </summary>
    Quaternion Rotation
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

    bool PhysicsEnabled
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
    /// This should only be true if the block cannot be rotated by any means, even
    /// if everything around it is rotating. This is not expected to be a commonly
    /// used feature in any anticipated sitations, but implementors should take
    /// it into account.
    /// </summary>
    bool IsFixedRotation
    {
        get;
        set;
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
    /// The parent object in the scene of this block. This may be
    /// any type of GameObject and differnt game rules may react
    /// differently to different types of parent.
    /// </summary>
    GameObject Parent
    {
        get;
        set;
    }

    /// <summary>
    /// The list of children of this block. Thse can be any
    /// type of GameObject and different game rules may react
    /// different to different types of children.
    /// </summary>
    List<GameObject> Children
    {
        get;
        set;
    }

    Dictionary<string, KeyValuePair<string,string>> DefaultProperties
    {
        get;
    }

    /// <summary>
    /// Returns the set of non-default block-specific
    /// properties. This can be any arbitrary set of
    /// strings. Blocks do not have to include properties
    /// that are set to defaults in this property, to get
    /// a list of all recognized properties on a block use
    /// the DefaultProperties.
    /// </summary>
    Dictionary<string,string> Properties
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

	/// <summary>
	/// Determines whether this instance can be moved the specified direction distance.
	/// </summary>
	/// <returns><c>true</c> if this instance can be moved the specified direction distance; otherwise, <c>false</c>.</returns>
	/// <param name="direction">Direction.</param>
	/// <param name="distance">Distance.</param>
	bool CanBeMoved (Vector3 direction, int distance = 1);

	/// <summary>
	/// Move the specified direction and distance.
	/// </summary>
	/// <param name="direction">Direction.</param>
	/// <param name="distance">Distance.</param>
	bool Move (Vector3 direction, int distance = 1);
}
