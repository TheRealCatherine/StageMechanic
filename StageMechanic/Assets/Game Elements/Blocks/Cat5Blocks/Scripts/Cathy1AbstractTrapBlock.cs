/*  
 * Copyright (C) 2018 You're Perfect Studio. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System.Collections;
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
    public virtual bool IsArmed { get; set; } = true;

    /// <summary>
    /// Bomb and spike traps start out not-triggered but vortexes are always
    /// triggered
    /// </summary>
    public virtual bool IsTriggered { get; set; } = false;

    /// <summary>
    /// All Cathy1 traps start out inactive
    /// </summary>
    public virtual bool IsActive { get; set; } = false;

    /// <summary>
    /// The amount of time the block spends in its damage-dealing state after
    /// entering the IsTriggered state before moving to IsArmed=false.
    /// For all Cath1 traps this is 1ms.
    /// </summary>
    public virtual float ActiveTime
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
    public abstract float TriggerTime { get; set; }

    /// <summary>
    /// In Cathy1-style all traps deal infity damage to players and enemies
    /// </summary>
    public virtual float PlayerDamage
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
    public virtual float EnemyDamage
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
    public virtual float ItemDamage
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
    public virtual float BlockDamage
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
    public virtual float PlayerPoisonDamage
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
    public virtual float EnemyPoisonDamage
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
    public virtual float ItemPoisonDamage
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
    public virtual float BlockPoisonDamage
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
    /// Bomb blocks override this to allow creation of triggers
    /// </summary>
    public virtual Vector3 Epicenter
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
    public virtual Vector3 DamageRadius { get; set; } = new Vector3(0, 1, 0);

    /// <summary>
    /// Damage in Cathy1-style is always 100%
    /// </summary>
    public virtual bool GradientDamage
    {
        get
        {
            return false;
        }
        set { }
    }

    virtual internal void HandlePlayer(PlayerMovementEvent ev)
    {
        if (IsArmed == false || IsTriggered == true || ev.Location != PlayerMovementEvent.EventLocation.Top)
            return;
        string statename = ev.Player.StateNames[ev.Player.CurrentStateIndex];
        if (statename == "Idle" || statename == "Walk" || statename == "Center")
        {
            IsTriggered = true;
            StartCoroutine(HandleStep());
        }
    }

    abstract internal IEnumerator HandleStep();

    protected override void OnPlayerEnter(PlayerMovementEvent ev)
    {
        base.OnPlayerEnter(ev);
        HandlePlayer(ev);
    }

    protected override void OnPlayerStay(PlayerMovementEvent ev)
    {
        base.OnPlayerStay(ev);
        HandlePlayer(ev);
    }
}
