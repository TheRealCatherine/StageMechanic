/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public float FallTime = 0.25f;

    private static State CurrentEdgeState( IBlock block )
    {
        Cathy1EdgeMechanic otherBlock = block.GameObject.GetComponent<Cathy1EdgeMechanic>();
        Debug.Assert(otherBlock != null);
        return otherBlock.CurrentState;
    }

    private bool SetStateBySupport()
    {
        Cathy1Block thisBlock = gameObject.GetComponent<Cathy1Block>();
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
        Cathy1Block thisBlock = gameObject.GetComponent<Cathy1Block>();
        if (thisBlock == null)
            yield break;

        ApplyGravity();
        thisBlock.transform.rotation = Quaternion.identity;

        if ((thisBlock.Position.y % 1) != 0)
        {
            CurrentState = State.Falling;
            yield break;
        }

        if(!SetStateBySupport() && CurrentState != State.Falling)
        {
            CurrentState = State.Hovering;
            thisBlock.transform.Rotate(0f, 0f, .3f);
            yield return new WaitForSeconds(0.1f);
            thisBlock.transform.Rotate(0f, 0f, -.6f);
            yield return new WaitForSeconds(0.1f);
            thisBlock.transform.Rotate(0f, 0f, .6f);
            yield return new WaitForSeconds(0.1f);
            thisBlock.transform.Rotate(0f, 0f, -.3f);
            yield return new WaitForSeconds(0.1f);
            thisBlock.transform.rotation = Quaternion.identity;
            yield return new WaitForSeconds(0.6f);
        }

        if(CurrentState == State.Hovering)
        {
            thisBlock.transform.rotation = Quaternion.identity;
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

        Cathy1Block thisBlock = gameObject.GetComponent<Cathy1Block>();
        if (thisBlock == null)
            return;

        if (BlockManager.PlayMode)
            thisBlock.Position -= new Vector3(0, 0.25f * thisBlock.GravityFactor, 0);

    }

	public bool TestForSupportedBlock (int height)
	{
		foreach (Collider col in Physics.OverlapBox(transform.position + new Vector3(0f,0.7f*height,0f),new Vector3(0.01f,0.01f,0.01f))) {
			if (col.gameObject == gameObject)
				continue;
			Cathy1EdgeMechanic otherBlock = col.gameObject.GetComponent<Cathy1EdgeMechanic> ();
			if (otherBlock == null)
				continue;
			return true;
		}
		return false;
	}
}
