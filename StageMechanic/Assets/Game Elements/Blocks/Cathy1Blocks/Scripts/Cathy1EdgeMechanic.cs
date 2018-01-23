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
        Initial,
        Grounded,
        Edged,
        Hovering,
        Falling
    }

    public State CurrentState = State.Grounded;
    public int BelowCount;
    public int AboveCount;
    private const float DEFAULT_GRAVITY_BASE = 0.25f;
    private IBlock thisBlock;
    public bool Calculating = true;
    public List<IBlock> blocksBelow = new List<IBlock>(5);
    public List<IBlock> blocksAbove = new List<IBlock>(5);
    private const int DOWN = 0;
    private const int UP = DOWN;
    private const int FORWARD = 1;
    private const int BACK = 2;
    private const int LEFT = 3;
    private const int RIGHT = 4;

    public bool IsGrounded
    {
        get
        {
            return CurrentState == State.Edged || CurrentState == State.Grounded;
        }
    }
    /*
        private void OnTriggerStay(Collider other)
        {
            Cathy1EdgeMechanic em = other.gameObject.GetComponent<Cathy1EdgeMechanic>();
            if (em != null)
            {
                if (em.CurrentState == State.Grounded || em.CurrentState == State.Edged)
                {
                    if (!IsGrounded)
                    {
                        IBlock otherBlock = em.GetComponent<IBlock>();
                        Vector3 downBase = thisBlock.Position + Vector3.down;
                        if (otherBlock.Position == downBase)
                            CurrentState = State.Grounded;
                        else if (otherBlock.Position == downBase + Vector3.back
                            || otherBlock.Position == downBase + Vector3.forward
                            || otherBlock.Position == downBase + Vector3.left
                            || otherBlock.Position == downBase + Vector3.right)
                            CurrentState = State.Edged;
                    }
                }
                else if(em.CurrentState == State.Hovering || em.CurrentState == State.Falling)
                {
                    if (BlockManager.GetBlockAt(thisBlock.Position + Vector3.down) != null)
                        CurrentState = State.Grounded;
                    else if (BlockManager.GetBlockAt(thisBlock.Position + Vector3.down + Vector3.back) != null
                        || BlockManager.GetBlockAt(thisBlock.Position + Vector3.down + Vector3.forward) != null
                        || BlockManager.GetBlockAt(thisBlock.Position + Vector3.down + Vector3.left) != null
                        || BlockManager.GetBlockAt(thisBlock.Position + Vector3.down + Vector3.right) != null)
                        CurrentState = State.Edged;
                    else if (BlockManager.ActiveFloor.transform.position.y == (thisBlock.Position + Vector3.down).y)
                    {
                        if (CurrentState != State.Falling && CurrentState != State.Hovering)
                        {
                            CurrentState = State.Grounded;
                        }
                    }
                    else if (CurrentState != State.Falling)
                        CurrentState = State.Hovering;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            Cathy1EdgeMechanic em = other.gameObject.GetComponent<Cathy1EdgeMechanic>();
            if (em != null)
            {
                Debug.Log(thisBlock.Position.y);
                IBlock otherBlock = em.GetComponent<IBlock>();
                if (em.CurrentState == State.Grounded || em.CurrentState == State.Edged)
                {
                    Vector3 downBase = Utility.Round(thisBlock.Position + Vector3.down,0);
                    if (otherBlock.Position == downBase)
                        CurrentState = State.Grounded;
                    else if (otherBlock.Position == downBase + Vector3.back
                        || otherBlock.Position == downBase + Vector3.forward
                        || otherBlock.Position == downBase + Vector3.left
                        || otherBlock.Position == downBase + Vector3.right)
                        CurrentState = State.Edged;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (BlockManager.GetBlockAt(thisBlock.Position + Vector3.down) != null)
                CurrentState = State.Grounded;
            else if (BlockManager.GetBlockAt(thisBlock.Position + Vector3.down + Vector3.back) != null
                || BlockManager.GetBlockAt(thisBlock.Position + Vector3.down + Vector3.forward) != null
                || BlockManager.GetBlockAt(thisBlock.Position + Vector3.down + Vector3.left) != null
                || BlockManager.GetBlockAt(thisBlock.Position + Vector3.down + Vector3.right) != null)
                CurrentState = State.Edged;
            else if (BlockManager.ActiveFloor.transform.position.y == (thisBlock.Position + Vector3.down).y)
            {
                if (CurrentState != State.Falling && CurrentState != State.Hovering)
                {
                    CurrentState = State.Grounded;
                }
            }

            else if (CurrentState != State.Falling)
                CurrentState = State.Hovering;

        }
        */

    public void HardUpdateAbove()
    {
        blocksAbove[UP] = BlockManager.GetBlockAt(thisBlock.Position + Vector3.up);
        blocksAbove[FORWARD] = BlockManager.GetBlockAt(thisBlock.Position + Vector3.up + Vector3.forward);
        blocksAbove[BACK] = BlockManager.GetBlockAt(thisBlock.Position + Vector3.up + Vector3.back);
        blocksAbove[LEFT] = BlockManager.GetBlockAt(thisBlock.Position + Vector3.up + Vector3.left);
        blocksAbove[RIGHT] = BlockManager.GetBlockAt(thisBlock.Position + Vector3.up + Vector3.right);
        UpdateCounts();
    }

    public void HardUpdateBelow()
    {
        blocksBelow[DOWN] = BlockManager.GetBlockAt(thisBlock.Position + Vector3.down);
        blocksBelow[FORWARD] = BlockManager.GetBlockAt(thisBlock.Position + Vector3.down + Vector3.forward);
        blocksBelow[BACK] = BlockManager.GetBlockAt(thisBlock.Position + Vector3.down + Vector3.back);
        blocksBelow[LEFT] = BlockManager.GetBlockAt(thisBlock.Position + Vector3.down + Vector3.left);
        blocksBelow[RIGHT] = BlockManager.GetBlockAt(thisBlock.Position + Vector3.down + Vector3.right);
        UpdateCounts();
    }

    public bool IsFrozen = false;
    private void FreezeBlock(bool freeze)
    {
        if(freeze)
        {
            transform.position = Utility.Round(thisBlock.Position, 0);
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            GetComponent<Rigidbody>().useGravity = false;
            IsFrozen = true;
        }
        else
        {
            GetComponent<Rigidbody>().constraints = (RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ);
            GetComponent<Rigidbody>().useGravity = true;
            IsFrozen = false;
        }
    }

    /* private void OnCollisionEnter(Collision collision)
     {
         IBlock otherBlock = collision.gameObject.GetComponent<IBlock>();
         if (otherBlock == null)
         {
             if (collision.gameObject == BlockManager.ActiveFloor && (IsGrounded || CurrentState == State.Initial))
             {
                 FreezeBlock(true);
                 CurrentState = State.Grounded;
             }
             return;
         }
         else if (otherBlock.Position.y < thisBlock.Position.y)
         {
             FreezeBlock(true);
         }
     }*/

    /*private void OnTriggerEnter(Collider other)
    {
        IBlock otherBlock = other.gameObject.GetComponent<IBlock>();
        if (otherBlock == null)
        {
            if (other.gameObject == BlockManager.ActiveFloor && (IsGrounded || CurrentState == State.Initial))
            {
                FreezeBlock(true);
                CurrentState = State.Grounded;
            }
            return;
        }
        else if (otherBlock.Position.y < thisBlock.Position.y)
        {
            FreezeBlock(true);
        }
        if (!otherBlock.GameObject.GetComponent<Cathy1EdgeMechanic>().IsFrozen)
            return;
        if (otherBlock.Position.y < thisBlock.Position.y)
        {
            AddBelow(otherBlock);
        }
        else if (otherBlock.Position.y > thisBlock.Position.y)
        {
            AddAbove(otherBlock);
        }
    }

    /*private void OnCollisionExit(Collision collision)
    {
        IBlock otherBlock = collision.gameObject.GetComponent<IBlock>();
        if (otherBlock == null)
            return;
    }

    private void OnTriggerExit(Collider other)
    {
        IBlock otherBlock =other.gameObject.GetComponent<IBlock>();
        if (otherBlock == null)
            return;
        if (otherBlock.Position.y < thisBlock.Position.y)
            RemoveBelow(otherBlock);
        else
            RemoveAbove(otherBlock);
    }*/

    public void OnCollisionEnter(Collision collision)
    {
        IBlock otherBlock = collision.gameObject.GetComponent<IBlock>();
        if (otherBlock == null)
        {
            if (collision.gameObject == BlockManager.ActiveFloor && (IsGrounded || CurrentState == State.Initial))
            {
                FreezeBlock(true);
                CurrentState = State.Grounded;
            }
            return;
        }
        else
        {
            if((otherBlock.Position.y > thisBlock.Position.y+0.5) ||( otherBlock.Position.y < thisBlock.Position.y-0.5)) {
                FreezeBlock(true);
                otherBlock.GameObject.GetComponent<Cathy1EdgeMechanic>().FreezeBlock(true);
                HardUpdateBelow();
                HardUpdateAbove();
            }
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<IBlock>() == null)
            return;
        HardUpdateAbove();
        HardUpdateBelow();
    }

    public void UpdateCounts()
    {
        BelowCount = 0;
        AboveCount = 0;
        foreach (IBlock support in blocksBelow)
        {
            if (support != null)
                ++BelowCount;
        }
        foreach (IBlock support in blocksAbove)
        {
            if (support != null)
                ++AboveCount;
        }
    }

    public void AddAbove(IBlock block)
    {
        if (block.Position.x == transform.position.x && block.Position.z == transform.position.z)
            blocksAbove[DOWN] = block;
        else if (block.Position.x > transform.position.x)
            blocksAbove[RIGHT] = block;
        else if (block.Position.x < transform.position.x)
            blocksAbove[LEFT] = block;
        else if (block.Position.z > transform.position.z)
            blocksAbove[FORWARD] = block;
        else if (block.Position.z < transform.position.z)
            blocksAbove[BACK] = block;
        UpdateCounts();
    }

    public void AddBelow(IBlock block)
    {
        if (block.Position.x == transform.position.x && block.Position.z == transform.position.z)
            blocksBelow[DOWN] = block;
        else if (block.Position.x > transform.position.x)
            blocksBelow[RIGHT] = block;
        else if (block.Position.x < transform.position.x)
            blocksBelow[LEFT] = block;
        else if (block.Position.z > transform.position.z)
            blocksBelow[FORWARD] = block;
        else if (block.Position.z < transform.position.z)
            blocksBelow[BACK] = block;
        UpdateCounts();
    }

    public void RemoveBelow(IBlock block)
    {
        if (blocksBelow[DOWN] == block)
            blocksBelow[DOWN] = null;
        else if (blocksBelow[RIGHT] == block)
            blocksBelow[RIGHT] = null;
        else if (blocksBelow[LEFT] == block)
            blocksBelow[LEFT] = null;
        else if (blocksBelow[FORWARD] == block)
            blocksBelow[FORWARD] = null;
        else if (blocksBelow[BACK] == block)
            blocksBelow[BACK] = null;
        UpdateCounts();
    }

    public void RemoveAbove(IBlock block)
    {
        if (blocksAbove[DOWN] == block)
            blocksAbove[DOWN] = null;
        else if (blocksAbove[RIGHT] == block)
            blocksAbove[RIGHT] = null;
        else if (blocksAbove[LEFT] == block)
            blocksAbove[LEFT] = null;
        else if (blocksAbove[FORWARD] == block)
            blocksAbove[FORWARD] = null;
        else if (blocksAbove[BACK] == block)
            blocksAbove[BACK] = null;
        block.GameObject.GetComponent<Cathy1EdgeMechanic>().RemoveBelow(thisBlock);
        UpdateCounts();
    }

    private void SetStateBySupport()
    {
        if ((thisBlock.Position.x % 1) != 0 || (thisBlock.Position.z % 1) != 0)
            return;

        if ((thisBlock.Position.y % 1) != 0)
            CurrentState = State.Falling;

        State oldState = CurrentState;

        if (CurrentState != State.Falling)
        {
            if (thisBlock.Position.y == 1)
            {
                CurrentState = State.Grounded;
                return;
            }
            else
            {
                CurrentState = State.Hovering;
            }
        }

        if (blocksBelow[DOWN] != null && blocksBelow[DOWN].GameObject.GetComponent<Cathy1EdgeMechanic>().IsGrounded)
            CurrentState = State.Grounded;
        else
        {
            foreach (IBlock edge in blocksBelow)
            {
                if (edge != null && edge.GameObject.GetComponent<Cathy1EdgeMechanic>().IsGrounded)
                {
                    CurrentState = State.Edged;
                    break;
                }
            }
        }

        if (oldState != CurrentState)
        {
            HardUpdateAbove();
            HardUpdateBelow();
            FreezeBlock(IsGrounded);
        }
    }

    private void OnDisable()
    {
        foreach(IBlock block in blocksAbove)
        {
            if(block != null && block.GameObject != null)
            {
                block.GameObject.GetComponent<Cathy1EdgeMechanic>().RemoveBelow(this.GetComponent<IBlock>());
            }
        }
    }

    private void Awake()
    {
        thisBlock = GetComponent<IBlock>();
        while (blocksAbove.Count < 5)
            blocksAbove.Add(null);
        while (blocksBelow.Count < 5)
            blocksBelow.Add(null);
    }

    private void Start()
    {
        //thisBlock.Position = Utility.Round(thisBlock.Position, 0);
        //GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        if (thisBlock.Position.y == 1f)
            CurrentState = State.Grounded;
    }

    private void Update()
    {
        SetStateBySupport();
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
        CurrentState = State.Falling;
    }

    public IEnumerator DoFall()
    {
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

        thisBlock.Position = Utility.Round(thisBlock.Position - new Vector3(0f, 0.25f, 0f),2);//new Vector3(0, DEFAULT_GRAVITY_BASE * thisBlock.GravityFactor, 0), 1);
        if (BlockManager.GetBlockAt(thisBlock.Position + Vector3.down) != null)
            CurrentState = State.Grounded;
        else if (BlockManager.GetBlockAt(thisBlock.Position + Vector3.down + Vector3.back) != null
            || BlockManager.GetBlockAt(thisBlock.Position + Vector3.down + Vector3.forward) != null
            || BlockManager.GetBlockAt(thisBlock.Position + Vector3.down + Vector3.left) != null
            || BlockManager.GetBlockAt(thisBlock.Position + Vector3.down + Vector3.right) != null)
            CurrentState = State.Edged;
        yield return null;

    }
}
