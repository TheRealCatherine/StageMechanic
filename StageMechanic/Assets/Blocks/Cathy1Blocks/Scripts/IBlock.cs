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
    /// Should return the underlying gameobject in the scene associated with this block.
    /// Callers should check for null as there is no strict requirement for all blocks
    /// to have in-scene components.
    /// </summary>
    GameObject GameObject
    {
        get;
    }

    /// <summary>
    /// This property should be true if the player is able to move the block during
    /// play mode via some direct means such as pushing or pulling. This property
    /// is only about direct contact movement, not indirect means such as triggers.
    /// If IsFixedPosition is true, the value of this property should be ignored.
    /// If this property is true then WeightFactor should be taken into account to
    /// determine how quickly this movement happens.
    /// </summary>
    bool IsMovableByPlayer
    {
        get;
        set;
    }

    /// <summary>
    /// This property should be true only if the block cannot be moved by any means,
    /// direct or indirect - including gravity. If this property is true both
    /// IsMovableByPlayer and GravityFactor should be ignored, as should any attempts to
    /// set the Position property.
    /// </summary>
    bool IsFixedPosition
    {
        get;
        set;
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
    /// Multiplier determining how quickly or slowly
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

    /// <summary>
    /// Create and return a new JSON delegate for this Block
    /// This is because GameObjects cannot directly be used
    /// [DataContract] classes.This is primarily used to
    /// serialize the Block information for saving level data.
    /// </summary>
    /// <returns></returns>
    BlockJsonDelegate GetJsonDelegate();
}
