using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cathy1VortexBlock : Cathy1AbstractTrapBlock
{
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
