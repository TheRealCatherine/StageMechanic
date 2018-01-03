/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cathy1SpringBlock : Cathy1Block
{
    public sealed override BlockType Type { get; } = BlockType.Spring;
    public Vector3 Distance = new Vector3(0f,5f,0f);
    public float Delay = 0.55f;

    IEnumerator DoBoigy()
    {
        if (!hasPlayer())
            yield break;
        yield return new WaitForSeconds(Delay);
        if (!hasPlayer())
            yield break;
        PlayerManager.Player1BoingyTo(transform.position + Vector3.up + Distance);
    }

    bool hasPlayer()
    {
        Vector3 player = PlayerManager.Player1Location();
        return (player == transform.position + Vector3.up);
    }

    private void Update()
    {
        if (!BlockManager.PlayMode)
            return;
        if (!hasPlayer())
            return;
        StartCoroutine(DoBoigy());
        
    }

}
