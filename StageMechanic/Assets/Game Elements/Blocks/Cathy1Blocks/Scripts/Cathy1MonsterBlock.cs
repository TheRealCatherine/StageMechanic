﻿/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cathy1MonsterBlock : Cathy1Block
{
    public sealed override BlockType Type { get; } = BlockType.Monster;
    public float MoveProbability = 0.0005f;
    private System.Random randomNumberGenerator = new System.Random();


    private enum State
    {
        NoPlayer = 0,
        PlayerEnter,
        PlayerStand,
        PlayerLeave,
        PlayerSidle,
        Disarmed
    }

    private State CurrentState = State.NoPlayer;

    bool hasPlayerTop()
    {
        Vector3 player = PlayerManager.Player1Location();
        return (player == transform.position + Vector3.up && (PlayerManager.PlayerStateName() == "Idle" || PlayerManager.PlayerStateName() == "Walk" || PlayerManager.PlayerStateName() == "Center"));
    }

    bool hasPlayerSidle()
    {
        Vector3 player = PlayerManager.Player1Location();
        return (PlayerManager.PlayerStateName() == "Sidle" &&
            (player == transform.position + Vector3.forward
            || player == transform.position + Vector3.back
            || player == transform.position + Vector3.left
            || player == transform.position + Vector3.right));
    }

    private IEnumerator HandleStep()
    {
        CurrentState = State.PlayerEnter;
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(0.15f);
        if (hasPlayerTop())
        {
            CurrentState = State.PlayerStand;
        }
//        Renderer rend = GetComponent<Renderer>();
//        rend.material = DisarmedStateMaterial;
        CurrentState = State.Disarmed;
        //TODO disarm
        BlockManager.CreateBlockAt(Position, "Cathy1 Internal", "Basic");
    }

    private IEnumerator HandleSidle()
    {
        CurrentState = State.PlayerEnter;
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(0.01f);
        if (hasPlayerSidle())
        {
            CurrentState = State.PlayerSidle;
            PlayerManager.Player(0).CurrentStateIndex = PlayerManager.Player(0).StateNames.IndexOf("Fall");
        }
    }

    bool WouldHaveSupport(Vector3 position)
    {
        List<IBlock> supportingBlocks = new List<IBlock>();
        supportingBlocks.Add(BlockManager.GetBlockAt(position + Vector3.down));
        supportingBlocks.Add(BlockManager.GetBlockAt(position + Vector3.down + Vector3.back));
        supportingBlocks.Add(BlockManager.GetBlockAt(position + Vector3.down + Vector3.forward));
        supportingBlocks.Add(BlockManager.GetBlockAt(position + Vector3.down + Vector3.left));
        supportingBlocks.Add(BlockManager.GetBlockAt(position + Vector3.down + Vector3.right));
        foreach(IBlock block in supportingBlocks)
        {
            if (block != null && block.GameObject != GameObject)
                return true;
        }
        return false;
    }

    private IEnumerator MoveBlock()
    {
        yield return new WaitForSeconds(0.1f);
        int direction = randomNumberGenerator.Next(7);
        switch(direction)
        {
            case 1:
                if (BlockManager.GetBlockAt(Position + Vector3.up) == null && WouldHaveSupport(Position+Vector3.up))
                    Position += Vector3.up;
                break;
            case 2:
                if (BlockManager.GetBlockAt(Position + Vector3.down) == null && WouldHaveSupport(Position + Vector3.down))
                    Position += Vector3.down;
                break;
            case 3:
                if (BlockManager.GetBlockAt(Position + Vector3.left) == null && WouldHaveSupport(Position + Vector3.left))
                    Position += Vector3.left;
                break;
            case 4:
                if (BlockManager.GetBlockAt(Position + Vector3.right) == null && WouldHaveSupport(Position + Vector3.right))
                    Position += Vector3.right;
                break;
            case 5:
                if (BlockManager.GetBlockAt(Position + Vector3.back) == null && WouldHaveSupport(Position + Vector3.back))
                    Position += Vector3.back;
                break;
            case 6:
                if (BlockManager.GetBlockAt(Position + Vector3.forward) == null && WouldHaveSupport(Position + Vector3.forward))
                    Position += Vector3.forward;
                break;
        }
    }

    private void Update()
    {
        if (!BlockManager.PlayMode)
            return;
        if (CurrentState == State.Disarmed)
            return;
        if (hasPlayerTop())
            StartCoroutine(HandleStep());
        else if (hasPlayerSidle())
            StartCoroutine(HandleSidle());
        else
        {
            if (randomNumberGenerator.NextDouble() < MoveProbability)
                StartCoroutine(MoveBlock());
        }

    }
}
