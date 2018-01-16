/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Cathy1AbstractTrapBlock : Cathy1Block, ITrapBlock
{

    /// <summary>
    /// Used to indicate the type of trap
    /// </summary>
    public enum TrapBlockType
    {
        None = 0,
        Spike,
        SmallBomb,
        BigBomb,
        Vortex
    }

    public abstract TrapBlockType TrapType { get; }

    /// <summary>
    /// All blocks in Cathy1-style start out armed by default.
    /// </summary>
    public bool IsArmed { get; set; } = true;

    /// <summary>
    /// Bomb and spike traps start out not-triggered but vortexes are always
    /// triggered
    /// </summary>
    public bool IsTriggered { get; set; } = false;

    /// <summary>
    /// All Cathy1 traps start out inactive
    /// </summary>
    public bool IsActive { get; set; } = false;

    /// <summary>
    /// The amount of time the block spends in its damage-dealing state after
    /// entering the IsTriggered state before moving to IsArmed=false.
    /// For all Cath1 traps this is 1ms.
    /// </summary>
    public float ActiveTime
    {
        get
        {
            return 1.0f;
        }
        set { }
    }

    /// <summary>
    /// Bombs and spike traps have a trigger time but vortexes have 0
    /// </summary>
    public virtual float TriggerTime { get; set; } = 0;

    /// <summary>
    /// In Cathy1-style all traps deal infity damage to players and enemies
    /// </summary>
    public float PlayerDamage
    {
        get
        {
            return float.PositiveInfinity;
        }
        set { }
    }

    /// <summary>
    /// In Cathy1-style all traps deal infity damage to players and enemies.
    /// </summary>
    public float EnemyDamage
    {
        get
        {
            return float.PositiveInfinity;
        }
        set { }
    }

    /// <summary>
    /// In Cathy1 style items can be damaged by traps
    /// </summary>
    public float ItemDamage
    {
        get
        {
            return float.PositiveInfinity;
        }
        set { }
    }

    /// <summary>
    /// In Cath1 style Bombs and Vortexes damage blocks but
    /// spike traps do not.
    /// </summary>
    public float BlockDamage
    {
        get
        {
            return float.PositiveInfinity;
        }
        set { }
    }

    /// <summary>
    /// In Cathy1 style there is no poison element
    /// </summary>
    public float PlayerPoisonDamage
    {
        get
        {
            return 0.0f;
        }
        set { }
    }

    /// <summary>
    /// In Cathy1 style there is no poison element
    /// </summary>
    public float EnemyPoisonDamage
    {
        get
        {
            return 0.0f;
        }
        set { }
    }

    /// <summary>
    /// In Cathy1 style there is no poison element
    /// </summary>
    public float ItemPoisonDamage
    {
        get
        {
            return 0.0f;
        }
        set { }
    }

    /// <summary>
    /// In Cathy1 style there is no poison element
    /// </summary>
    public float BlockPoisonDamage
    {
        get
        {
            return 0.0f;
        }
        set { }
    }

    /// <summary>
    /// In Cathy1 style this is always the same location
    /// as the trap itself. Setting this will move the block.
    /// </summary>
    public Vector3 Epicenter
    {
        get
        {
            return Position;
        }
        set
        {
            Position = value;
        }
    }

    /// <summary>
    /// Spikes and vortexes have (0,1,0), bombs (1,1,1) or (3,3,3)
    /// </summary>
    public Vector3 DamageRadius { get; set; } = new Vector3(0, 1, 0);

    /// <summary>
    /// Damage in Cathy1-style is always 100%
    /// </summary>
    public bool GradientDamage
    {
        get
        {
            return false;
        }
        set { }
    }


}
