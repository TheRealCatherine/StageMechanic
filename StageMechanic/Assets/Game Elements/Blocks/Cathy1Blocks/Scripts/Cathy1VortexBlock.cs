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

    bool hasPlayer()
    {
        Vector3 player = PlayerManager.Player1Location();
        return (player == transform.position + Vector3.up && (player != transform.position + Vector3.up || (PlayerManager.PlayerStateName() != "Idle" || PlayerManager.PlayerStateName() != "Walk" || PlayerManager.PlayerStateName() != "Center")));
    }

    private void Update()
    {
        if (!BlockManager.PlayMode)
            return;
        if (hasPlayer())
        {
            GetComponent<AudioSource>()?.Play();
            PlayerManager.Player(0).TakeDamage(float.PositiveInfinity);
        }
        IBlock onTop = BlockManager.GetBlockAt(transform.position + Vector3.up);
        if (onTop != null) {
            GetComponent<AudioSource>()?.Play();
            Destroy(onTop.GameObject);
        }

    }
}
