/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cathy1IceBlock : Cathy1Block
{
    public sealed override BlockType Type { get; } = BlockType.Ice;

    bool hasPlayer()
    {
        Vector3 player = PlayerManager.Player1Location();
        return (player == transform.position + Vector3.up && PlayerManager.Player1State() == Cathy1PlayerCharacter.State.Walk);
    }

    private void Update()
    {
        if (!BlockManager.PlayMode)
            return;
        if (hasPlayer())
        {
            PlayerManager.Player1SlideForward();
        }
        IBlock onTop = BlockManager.GetBlockAt(transform.position + Vector3.up);
        if (onTop != null)
        {
        }

    }
}
