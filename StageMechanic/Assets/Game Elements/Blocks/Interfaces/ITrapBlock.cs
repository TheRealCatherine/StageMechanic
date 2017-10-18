/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A common interface for block types that damage/kill the
/// player, enemies, even other blocks upon contact.
/// </summary>
public interface ITrapBlock
{
    /// <summary>
    /// When true, the trap has not yet been sprung. That is to say making
    /// contact with one or more of its colliders will set it off.
    /// </summary>
    bool IsArmed { get; set; }

    /// <summary>
    /// When true the block is currently dealing damage.
    /// </summary>
    bool IsActive { get; set; }

    /// <summary>
    /// When true, the trap is in a state the trigger has been tripped but
    /// it is not yet dealing damage.
    /// </summary>
    bool IsTriggered { get; set; }

    /// <summary>
    /// The amount of time in milliseconds after a collider has been triggered to wait before
    /// moving into the IsTriggered state. This gives the player a chance to move away from
    /// the block before taking damage.
    /// </summary>
    int TriggerTime { get; set; }

    /// <summary>
    /// How long the block should continue dealing damage once entering the IsTriggered state.
    /// This may be set to float.Infinity and fload.nan should be treated as 0.
    /// </summary>
    float ActiveTime { get; set; }

    /// <summary>
    /// Request to the engine concerning how much damage the player at Epicenter should sustain once the
    /// block enters the IsTriggered state. May be float.PositiveInfinity to indicate the
    /// player should take infinity damage. Negative numbers should indicate that the player
    /// should _recover_ health from contact with the block. This property may also be
    /// float.nan and this should react the same as if it is set to 0 - no damage.
    /// </summary>
    float PlayerDamage { get; set; }

    /// <summary>
    /// How much damage enemies at Epicenter should take when making contact with this block. In some
    /// games enemies do not take damage from traps, in others it works the same as it does
    /// for players. See <see cref="PlayerDamage"/> for more information.
    /// </summary>
    float EnemyDamage { get; set; }

    /// <summary>
    /// Amount of damage items making contact with this block should sustain when it is triggered
    /// For example, triggering may cause items to sustain float.Infinity damage causing them to
    /// dissapear.
    /// </summary>
    float ItemDamage { get; set; }

    /// <summary>
    /// Amount of damage blocks around the Epicenter should sustain once the block enters
    /// the IsTriggered state. For example, bomb traps may cause surrounding blocks to take
    /// damage.
    /// </summary>
    float BlockDamage { get; set; }

    /// <summary>
    /// An indication to the engine that the player should continue taking damage after the
    /// trap has been triggered even when no longer making contact with the Epicenter.
    /// Duration of poison effect, removing the effect, etc are part of the game and player
    /// rules not part of the block mechanics.
    /// </summary>
    float PlayerPoisonDamage { get; set; }

    /// <summary>
    /// An indication to the engine that the enemies should continue taking damage after the
    /// trap has been triggered even when no longer making contact with the Epicenter.
    /// Duration of poison effect, removing the effect, etc are part of the game and player
    /// rules not part of the block mechanics.
    /// </summary>
    float EnemyPoisonDamage { get; set; }

    /// <summary>
    /// An indication to the engine that items should continue taking damage after the
    /// trap has been triggered even when no longer making contact with the Epicenter.
    /// Duration of poison effect, removing the effect, etc are part of the game and player
    /// rules not part of the block mechanics.
    /// </summary>
    float ItemPoisonDamage { get; set; }

    /// <summary>
    /// An indication to the engine that blocks should continue taking damage after the
    /// trap has been triggered even when no longer making contact with the Epicenter.
    /// Duration of poison effect, removing the effect, etc are part of the game and player
    /// rules not part of the block mechanics.
    /// </summary>
    float BlockPoisonDamage { get; set; }

    /// <summary>
    /// Position of the epicenter of the trap. This will usually be the same location as
    /// the trap itself, however some traps can act as remote triggers - for example
    /// a detonator for a remote bomb.
    /// </summary>
    Vector3 Epicenter { get; set; }

    /// <summary>
    /// Radius around the Epicenter in which players/blocks/items/enemies/etc should take damage
    /// </summary>
    Vector3 DamageRadius { get; set; }

    /// <summary>
    /// If this property is true then damage from this trap decreases with distance from the epicenter.
    /// Outside of DamageRadius will be 0 damage. If this is not set, everything with DamageRadius takes
    /// the same amount of damage.
    /// </summary>
    bool GradientDamage { get; set; }

    /// <summary>
    /// Defines the area in which a player may trigger this trap, for example does it only trigger when
    /// the player on top of the block or does touching it anywhere set it off?
    /// </summary>
    Collider PlayerTriggerCollider { get; set; }

    /// <summary>
    /// Defines the area in which a player may trigger this trap, for example does it only trigger when
    /// the player on top of the block or does touching it anywhere set it off?
    /// </summary>
    Collider EnemyTriggerCollider { get; set; }

    /// <summary>
    /// Defines the area in which a player may trigger this trap, for example does it only trigger when
    /// the player on top of the block or does touching it anywhere set it off?
    /// </summary>
    Collider ItemTriggerCollider { get; set; }

    /// <summary>
    /// Defines the area in which a player may trigger this trap, for example does it only trigger when
    /// the player on top of the block or does touching it anywhere set it off?
    /// </summary>
    Collider BlockTriggerCollider { get; set; }
}
