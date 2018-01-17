/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IBlock))]
public class Cathy1EdgeMechanic : MonoBehaviour
{
    public enum State
    {
        Grounded,
        Edged,
        Hovering,
        Falling
    }


    public State CurrentState = State.Grounded;
    private const float DEFAULT_GRAVITY_BASE = 0.1f;

    private static State CurrentEdgeState( IBlock block )
    {
        Cathy1EdgeMechanic otherBlock = block.GameObject.GetComponent<Cathy1EdgeMechanic>();
        Debug.Assert(otherBlock != null);
        return otherBlock.CurrentState;
    }

    private bool SetStateBySupport()
    {
        IBlock thisBlock = gameObject.GetComponent<IBlock>();
        Debug.Assert(thisBlock != null);

        IBlock down = BlockManager.GetBlockAt(thisBlock.Position + Vector3.down);
        if (down != null)
        {
            State obs = CurrentEdgeState(down);
            if (obs != State.Hovering && obs != State.Falling)
            {
                CurrentState = State.Grounded;
                return true;
            }
        }

        IBlock back = BlockManager.GetBlockAt(thisBlock.Position + Vector3.down + Vector3.back);
        if (back != null)
        {
            State obs = CurrentEdgeState(back);
            if (obs != State.Hovering && obs != State.Falling)
            {
                CurrentState = State.Edged;
                return true;
            }
        }

        IBlock left = BlockManager.GetBlockAt(thisBlock.Position + Vector3.down + Vector3.left);
        if (left != null)
        {
            State obs = CurrentEdgeState(left);
            if (obs != State.Hovering && obs != State.Falling)
            {
                CurrentState = State.Edged;
                return true;
            }
        }

        IBlock right = BlockManager.GetBlockAt(thisBlock.Position + Vector3.down + Vector3.right);
        if (right != null)
        {
            State obs = CurrentEdgeState(right);
            if (obs != State.Hovering && obs != State.Falling)
            {
                CurrentState = State.Edged;
                return true;
            }
        }

        IBlock front = BlockManager.GetBlockAt(thisBlock.Position + Vector3.down + Vector3.forward);
        if (front != null)
        {
            State obs = CurrentEdgeState(front);
            if (obs != State.Hovering && obs != State.Falling)
            {
                CurrentState = State.Edged;
                return true;
            }
        }

        if (BlockManager.ActiveFloor.transform.position.y == (thisBlock.Position + Vector3.down).y)
        {
            if (CurrentState != State.Falling && CurrentState != State.Hovering)
            {
                CurrentState = State.Grounded;
                return true;
            }
        }

        return false;
    }

    public IEnumerator UpdateState()
    {
       IBlock thisBlock = gameObject.GetComponent<Cathy1Block>();

        ApplyGravity();
        thisBlock.Rotation = Quaternion.identity;

        if ((thisBlock.Position.y % 1) != 0)
        {
            CurrentState = State.Falling;
            yield break;
        }

        if(!SetStateBySupport() && CurrentState != State.Falling)
        {
            CurrentState = State.Hovering;
            thisBlock.GameObject.transform.Rotate(0f, 0f, .3f);
            yield return new WaitForSeconds(0.1f);
            thisBlock.GameObject.transform.Rotate(0f, 0f, -.6f);
            yield return new WaitForSeconds(0.1f);
            thisBlock.GameObject.transform.Rotate(0f, 0f, .6f);
            yield return new WaitForSeconds(0.1f);
            thisBlock.GameObject.transform.Rotate(0f, 0f, -.3f);
            yield return new WaitForSeconds(0.1f);
            thisBlock.GameObject.transform.rotation = Quaternion.identity;
            yield return new WaitForSeconds(0.6f);
        }

        if(CurrentState == State.Hovering)
        {
            thisBlock.Rotation = Quaternion.identity;
            if (!SetStateBySupport())
                CurrentState = State.Falling;
        }
    }

    void Update ()
	{
        if(BlockManager.PlayMode)
            StartCoroutine(UpdateState());
            
	}

	public void ApplyGravity ()
	{
        if (CurrentState != State.Falling)
            return;

        IBlock thisBlock = gameObject.GetComponent<IBlock>();
        if (thisBlock == null)
            return;

        if (BlockManager.PlayMode)
            thisBlock.Position = Utility.Round(thisBlock.Position - new Vector3(0, DEFAULT_GRAVITY_BASE * thisBlock.GravityFactor, 0),2);

    }
}
