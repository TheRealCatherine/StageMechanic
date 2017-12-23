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
            yield return new WaitForSeconds(0.35f);
            Destroy(gameObject);
        }
            
    }

    private void Update()
    {
        List<Collider> crossColiders = new List<Collider>(Physics.OverlapBox(transform.position + new Vector3(0f, 0.75f, 0f), new Vector3(0.1f, 0.1f, 0.75f)));
        foreach (Collider col in crossColiders)
        {
            if (col.gameObject == gameObject)
                continue;
            Cathy1EdgeMechanic otherBlock = col.gameObject.GetComponent<Cathy1EdgeMechanic>();
            if (otherBlock != null)
                continue;
            Cathy1PlayerCharacter player = col.gameObject.GetComponent<Cathy1PlayerCharacter>();
            if (player != null)
            {
                if (CurrentState == State.PlayerStand)
                    continue;
                else if (CurrentState == State.NoPlayer)
                    StartCoroutine(HandleStep());

            }
            else
            {
               CurrentState = State.NoPlayer;
            }
            
        }
        if(crossColiders.Count == 0)
            CurrentState = State.NoPlayer;
    }
}
