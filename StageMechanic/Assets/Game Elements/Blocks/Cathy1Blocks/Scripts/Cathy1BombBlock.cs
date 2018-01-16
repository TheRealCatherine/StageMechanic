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

    public AudioClip FuseSound;
    public AudioClip ExplosionSound;

    public enum BombSize
    {
        Small,
        Large
    }

    public BombSize Size = BombSize.Small;

    private enum State
    {
        NoPlayer = 0,
        PlayerEnter,
        PlayerStand,
        PlayerLeave,
        Disarmed
    }

    private State CurrentState = State.NoPlayer;

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
    /// Sets the trigger time of the spike trap
    /// </summary>
    public override void Awake()
    {
        base.Awake();
        TriggerTime = 1000;
    }

    bool hasPlayer()
    {
        Vector3 player = PlayerManager.Player1Location();
        return (player == transform.position + Vector3.up && (PlayerManager.PlayerStateName() == "Idle" || PlayerManager.PlayerStateName() == "Walk" || PlayerManager.PlayerStateName() == "Center"));
    }

    private IEnumerator HandleStep()
    {
        CurrentState = State.PlayerEnter;
        GetComponent<AudioSource>()?.PlayOneShot(FuseSound);
        yield return new WaitForSeconds(1.2f);
        GetComponent<AudioSource>()?.PlayOneShot(ExplosionSound);
        if (hasPlayer())
        {
            CurrentState = State.PlayerStand;
            PlayerManager.Player(0).TakeDamage(float.PositiveInfinity);

        }
        List<IBlock> localBlocks = BlockManager.GetBlocskAt(Position, Size==BombSize.Small?1f:3f);
        foreach(IBlock block in localBlocks)
        {
            BlockManager.CreateBlockAt(block.Position, "Cathy1 Internal", "Cracked (1 Step)");
        }
        CurrentState = State.Disarmed;
        Destroy(GameObject);
    }

    private void Update()
    {
        if (!BlockManager.PlayMode)
            return;
        if (!hasPlayer())
            return;
        if (CurrentState == State.Disarmed)
            return;
        StartCoroutine(HandleStep());
    }
}
