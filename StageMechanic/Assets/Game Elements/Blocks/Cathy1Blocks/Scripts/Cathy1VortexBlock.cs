/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cathy1VortexBlock : Cathy1AbstractTrapBlock
{

    public sealed override BlockType Type { get; } = BlockType.Vortex;

    public override TrapBlockType TrapType
    {
        get
        {
            return TrapBlockType.Vortex;
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
    /// Returns the capsule collider associated with the block
    /// </summary>
    public override sealed Collider ItemTriggerCollider
    {
        get
        {
            return GetComponent<CapsuleCollider>();
        }
        set { }
    }

    /// <summary>
    /// Returns the capsule collider associated with the block
    /// </summary>
    public override sealed Collider BlockTriggerCollider
    {
        get
        {
            return GetComponent<CapsuleCollider>();
        }
        set { }
    }
}
