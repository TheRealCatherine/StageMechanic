/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cathy1BombBlock : Cathy1AbstractTrapBlock
{
    public Material ArmedStateMaterial;
    public Material TriggeredStateMaterial;
    public Material ActiveStateMaterial;
    public Material DisarmedStateMaterial;

    public enum BombSize
    {
        Small,
        Large
    }

    public BombSize Size = BombSize.Small;

    /// <summary>
    /// This class is used for both Bomb1 and Bomb2 types
    /// Setting to a different type should throw an exception
    /// for now just set it to the small one by default
    /// TODO
    /// </summary>
    public sealed override BlockType Type
    {
        get
        {
            if (Size == BombSize.Small)
                return Cathy1Block.BlockType.Bomb1;
            return Cathy1Block.BlockType.Bomb2;
        }
    }


    /// <summary>
    /// Indicate to the Cathy1 game rules that this is a spike trap
    /// </summary>
    public sealed override TrapBlockType TrapType
    {
        get
        {
            if (Size == BombSize.Small)
                return Cathy1AbstractTrapBlock.TrapBlockType.SmallBomb;
            return Cathy1AbstractTrapBlock.TrapBlockType.BigBomb;
        }
    }

    /// <summary>
    /// Returns the capsule collider associated with the block
    /// </summary>
    public override sealed Collider PlayerTriggerCollider
    {
        get
        {
            return GetComponent<CapsuleCollider>();
        }
        set { }
    }

    /// <summary>
    /// Items cannot trigger spike traps in Cathy1 style
    /// </summary>
    public override sealed Collider ItemTriggerCollider
    {
        get
        {
            return null;
        }
        set { }
    }

    /// <summary>
    /// Blocks cannot trigget spike traps in Cathy1 style
    /// </summary>
    public override sealed Collider BlockTriggerCollider
    {
        get
        {
            return null;
        }
        set { }
    }

    /// <summary>
    /// Add the spike trap specific properties.
    /// </summary>
    public override Dictionary<string, string> Properties
    {
        get
        {
            Dictionary<string, string> ret = base.Properties;
            if (Size == BombSize.Large)
                ret.Add("Bomb Radius", "Large");
            else
                ret.Add("Bomb Radius", "Small");
            return ret;
        }

        set
        {
            base.Properties = value;
            if (value.ContainsKey("Bomb Radius"))
            {
                if (value["Bomb Radius"] == "Large")
                    Size = BombSize.Large;
                else
                    Size = BombSize.Small;
            }
        }
    }

    /// <summary>
    /// Sets the trigger time of the spike trap
    /// </summary>
    public override void Awake()
    {
        base.Awake();
        TriggerTime = 1000;
    }
}
