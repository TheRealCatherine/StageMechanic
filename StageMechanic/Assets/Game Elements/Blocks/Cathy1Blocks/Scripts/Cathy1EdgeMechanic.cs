/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Cathy1Block))]
[RequireComponent(typeof(BoxCollider))]
public class Cathy1EdgeMechanic : MonoBehaviour
{
    private Cathy1Block thisBlock;

    public void OnCollisionEnter(Collision collision)
    {
        IBlock otherBlock = collision.gameObject.GetComponent<IBlock>();
        if (otherBlock == null)
        {
            if (collision.gameObject == BlockManager.ActiveFloor && thisBlock.IsGrounded)
            {
                thisBlock.PhysicsEnabled = false;
                thisBlock.MotionState = BlockMotionState.Grounded;
            }
            return;
        }
        else
        {
            if ((otherBlock.Position.y > thisBlock.Position.y + 0.1) || (otherBlock.Position.y < thisBlock.Position.y - 0.1))
            {
                thisBlock.PhysicsEnabled = false;
                otherBlock.PhysicsEnabled = false;
            }
            thisBlock.UpdateNeighborsCache();
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        thisBlock.UpdateNeighborsCache();
    }

    private void SetStateBySupport()
    {
        if ((thisBlock.Position.x % 1) != 0 || (thisBlock.Position.z % 1) != 0)
            return;

        if ((thisBlock.Position.y % 1) != 0)
            thisBlock.MotionState = BlockMotionState.Falling;

        BlockMotionState oldState = thisBlock.MotionState;

        if (thisBlock.MotionState != BlockMotionState.Falling)
        {
            if (thisBlock.Position.y == 1)
            {
                thisBlock.PhysicsEnabled = false;
                thisBlock.MotionState = BlockMotionState.Grounded;
                return;
            }
            else
            {
                thisBlock.MotionState = BlockMotionState.Hovering;
            }
        }
        else
            thisBlock.PhysicsEnabled = true;

        if (thisBlock.blocksBelow[AbstractBlock.DOWN] != null && thisBlock.blocksBelow[AbstractBlock.DOWN].IsGrounded)
            thisBlock.MotionState = BlockMotionState.Grounded;
        else
        {
            foreach (IBlock edge in thisBlock.blocksBelow)
            {
                if (edge != null && edge.IsGrounded)
                {
                    thisBlock.MotionState = BlockMotionState.Edged;
                    break;
                }
            }
        }

        if (oldState != thisBlock.MotionState)
        {
            thisBlock.UpdateNeighborsCache();
            thisBlock.PhysicsEnabled = !thisBlock.IsGrounded;
            if (thisBlock.MotionState == BlockMotionState.Hovering)
                StartCoroutine(DoHoverAnimation());
        }
    }


    private void Awake()
    {
        thisBlock = GetComponent<Cathy1Block>();
        
    }

    private void Start()
    {
        //thisBlock.Position = Utility.Round(thisBlock.Position, 0);
        //GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        if (thisBlock.Position.y == 1f)
        {
            thisBlock.MotionState = BlockMotionState.Grounded;
            thisBlock.PhysicsEnabled = false;
        }

    }

    private void Update()
    {
        SetStateBySupport();
#if UNITY_EDITOR
        thisBlock.CurrentMoveState = thisBlock.MotionStateName;
#endif
        /* if (CurrentState == State.Falling)
        {
            //StartCoroutine(DoFall());
            thisBlock.Position = Utility.Round(thisBlock.Position - new Vector3(0f, 1f, 0f), 0);//new Vector3(0, DEFAULT_GRAVITY_BASE * thisBlock.GravityFactor, 0), 1);
        }
        else if (CurrentState == State.Hovering)
            StartCoroutine(DoHoverAnimation());*/
    }

    public IEnumerator DoHoverAnimation()
    {
        thisBlock.GameObject.transform.Rotate(0f, 0f, .5f);
        yield return new WaitForSeconds(0.1f);
        thisBlock.GameObject.transform.Rotate(0f, 0f, -.10f);
        yield return new WaitForSeconds(0.1f);
        thisBlock.GameObject.transform.Rotate(0f, 0f, .10f);
        yield return new WaitForSeconds(0.1f);
        thisBlock.GameObject.transform.Rotate(0f, 0f, -.5f);
        yield return new WaitForSeconds(0.1f);
        thisBlock.GameObject.transform.rotation = Quaternion.identity;
        yield return new WaitForSeconds(0.6f);
        thisBlock.MotionState = BlockMotionState.Falling;
    }

   // public IEnumerator DoFall()
   // {
        /*float journey = 0f;
        float duration = 0.25f;
        Vector3 origin = transform.position;
        Vector3 target = transform.position + Vector3.down;
        while (journey <= duration)
        {
            journey = journey + Time.deltaTime;
            float percent = Mathf.Clamp01(journey / duration);

            transform.position = Utility.Round(Vector3.Lerp(origin, target, percent),1);

            yield return null;
        }*/

       /* thisBlock.Position = Utility.Round(thisBlock.Position - new Vector3(0f, 0.25f, 0f), 2);//new Vector3(0, DEFAULT_GRAVITY_BASE * thisBlock.GravityFactor, 0), 1);
        if (BlockManager.GetBlockAt(thisBlock.Position + Vector3.down) != null)
            CurrentState = State.Grounded;
        else if (BlockManager.GetBlockAt(thisBlock.Position + Vector3.down + Vector3.back) != null
            || BlockManager.GetBlockAt(thisBlock.Position + Vector3.down + Vector3.forward) != null
            || BlockManager.GetBlockAt(thisBlock.Position + Vector3.down + Vector3.left) != null
            || BlockManager.GetBlockAt(thisBlock.Position + Vector3.down + Vector3.right) != null)
            CurrentState = State.Edged;
        yield return null;
        */
   // }
}
