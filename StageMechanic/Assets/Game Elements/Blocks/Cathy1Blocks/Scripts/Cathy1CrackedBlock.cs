/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cathy1CrackedBlock : Cathy1Block {

    public int StepsRemaining = 2;
    public AudioClip Crack;
    public AudioClip Collapse;

    private enum State
    {
        NoPlayer = 0,
        PlayerEnter,
        PlayerStand,
        PlayerLeave
    }

    private State CurrentState = State.NoPlayer;

    public sealed override BlockType Type
    {
        get
        {
            if (StepsRemaining == 1)
                return Cathy1Block.BlockType.Crack1;
            return Cathy1Block.BlockType.Crack2;
        }
    }

    private IEnumerator HandleStep()
    {
        CurrentState = State.PlayerEnter;
        yield return new WaitForSeconds(0.35f);
        --StepsRemaining;
        CurrentState = State.PlayerStand;

        Renderer rend = GetComponent<Renderer>();
        if(StepsRemaining > 2)
        {
            
            GetComponent<AudioSource>().PlayOneShot(Crack);
            yield return new WaitForSeconds(0.35f);
            rend.material = Material2;
        }
        else if(StepsRemaining == 1)
        {
            
            GetComponent<AudioSource>().PlayOneShot(Crack);
            yield return new WaitForSeconds(0.35f);
            rend.material = Material1;
        }
        else
        {
            GetComponent<AudioSource>().PlayOneShot(Collapse);
            yield return new WaitForSeconds(0.55f);
            Destroy(gameObject);
        }
            
    }

    private void Update()
    {
        if (!BlockManager.PlayMode)
            return;
        Vector3 player = PlayerManager.Player1Location();
        if (player != transform.position + Vector3.up || PlayerManager.Player1StateName() != "Idle") {
            if (CurrentState != State.NoPlayer)
                CurrentState = State.PlayerLeave;
            CurrentState = State.NoPlayer;
            return;
        }

        if (CurrentState == State.PlayerStand)
            return;
        else if (CurrentState == State.NoPlayer)
            StartCoroutine(HandleStep());
    }
}
