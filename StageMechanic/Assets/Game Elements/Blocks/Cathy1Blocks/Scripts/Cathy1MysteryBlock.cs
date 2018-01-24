/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cathy1MysteryBlock : Cathy1Block
{
    public sealed override BlockType Type { get; } = BlockType.Random;
    public readonly string[] PossibleTypes = {
        "Basic",
        "Immobile",
        "Cracked (2 Steps)",
        "Cracked (1 Step)",
        "Heavy",
        "Spike Trap",
        "Ice",
        "Small Bomb",
        "Large Bomb",
        "Spring",
        "Mystery",
        "Monster",
        "Vortex"};

    bool hasPlayer()
    {
        Vector3 player = PlayerManager.Player1Location();
        return (player == transform.position + Vector3.up && (PlayerManager.PlayerStateName() == "Idle" || PlayerManager.PlayerStateName() == "Walk" || PlayerManager.PlayerStateName() == "Center"));
    }

    internal override void Update()
    {
        base.Update();
        if (!BlockManager.PlayMode)
            return;
        if (!hasPlayer())
            return;
        StartCoroutine(HandleStep());
    }

    private IEnumerator HandleStep()
    {
        GetComponent<AudioSource>()?.Play();
        yield return new WaitForSeconds(0.05f);
        System.Random rnd = new System.Random();
        int index = rnd.Next(PossibleTypes.Length);
        BlockManager.CreateBlockAt(Position, "Cathy1 Internal", PossibleTypes[index]);
    }
}
