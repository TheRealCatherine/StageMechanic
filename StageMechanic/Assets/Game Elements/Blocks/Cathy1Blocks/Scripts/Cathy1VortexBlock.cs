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

    public sealed override TrapBlockType TrapType { get; } = TrapBlockType.Vortex;
    public sealed override float TriggerTime { get; set; } = 0f;

    internal override IEnumerator HandleStep()
    {
        IsTriggered = true;
        yield return new WaitForSeconds(TriggerTime);
        GetComponent<AudioSource>().Play();
        foreach (AbstractPlayerCharacter player in PlayerManager.GetPlayersNear(Position + Vector3.up, radius: 0.25f))
        {
            player.TakeDamage(float.PositiveInfinity);
        }
        IsArmed = true;
        IsTriggered = false;
    }

    public override void Awake()
    {
        base.Awake();
        TriggerTime = 0f;
    }

    internal override void Update()
    {
        base.Update();
        if (!BlockManager.PlayMode)
            return;
        IBlock onTop = BlockManager.GetBlockNear(transform.position + Vector3.up);
        if (onTop != null) {
            GetComponent<AudioSource>()?.Play();
            BlockManager.DestroyBlock(onTop);
        }

    }
}
