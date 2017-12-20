/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cathy1PlayerCharacter : MonoBehaviour {

    public GameObject Player1Prefab;
    public RuntimeAnimatorController Player1AnimationController;
    public Avatar Player1Avatar;

    public IBlock CurrentBlock;

    private GameObject _player;
	private Vector3 _facingDirection = Vector3.back;

	private const float HEIGHT_ADJUST = 0.5f;
    public float WalkSpeed { get; set; } = 0.05f;
    public float Granularity { get; set; } = 1.0f;
    public float ClimbSpeed { get; set; } = 0.05f;
    public enum State
    {
        None = 0,
        Aproach,
        Climb,
        Center
    }
    public State CurrentMoveState { get; set; } = State.None;
    public GameObject Character { get; set; }
    public bool IsWalking { get; set; } = false;

    public Vector3 CurrentLocation
    {
        get
        {
            return transform.localPosition;
        }
        set
        {
            transform.localPosition = value;
        }
    }

    public Vector3 DesiredLocation { get; set; }

    public void ForceMove(Vector3 offset)
    {
        if (offset != Vector3.zero)
            CurrentLocation += offset;
    }

    public void Teleport(Vector3 location)
    {
        CurrentLocation = location;
    }

    public void Walk(Vector3 offset)
    {
        DesiredLocation = CurrentLocation + offset;
        IsWalking = true;
    }

    public void MoveUp()
    {
        Walk(new Vector3(0f, Granularity, 0f));
    }

    public void MoveDown()
    {
        Walk(new Vector3(0f, -Granularity, 0f));
    }

    public void MoveLeft()
    {
        Walk(new Vector3(-Granularity, 0f, 0f));
    }

    public void MoveRight()
    {
        Walk(new Vector3(Granularity, 0f, 0f));
    }

    public void MoveForward()
    {
        Walk(new Vector3(0f, 0f, -Granularity));
    }

    public void MoveBack()
    {
        Walk(new Vector3(0f, 0f, Granularity));
    }

    /// <summary>
    /// Apply the movement offset taking into account ClimbSpeed
    /// </summary>
    /// <param name="offset"></param>
    public void Climb(Vector3 offset)
    {
        //if (CurrentMoveState == State.None)
        //{
        DesiredLocation = CurrentLocation + offset;
        //CurrentMoveState = State.Climb;
        // }
    }

    public void ClimbUp()
    {
        Climb(new Vector3(0f, Granularity, 0f));
    }

    public void ClimbDown()
    {
        Climb(new Vector3(0f, -Granularity, 0f));
    }

    public void ClimbLeft()
    {
        Climb(new Vector3(-Granularity, 0f, 0f));
    }

    public void ClimbRight()
    {
        Climb(new Vector3(Granularity, 0f, 0f));
    }

    public void ClimbForward()
    {
        Climb(new Vector3(0f, 0f, -Granularity));
    }

    public void ClimbBack()
    {
        Climb(new Vector3(0f, 0f, Granularity));
    }

    internal IEnumerator MoveToLocation()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            Teleport(DesiredLocation);
        }
    }

    // Use this for initialization
    void Start () {
		_player=Instantiate(Player1Prefab, transform.position-new Vector3(0f,HEIGHT_ADJUST,0f), transform.rotation, gameObject.transform);
        _player.transform.RotateAround(transform.position, transform.up, 180f);
        _player.GetComponent<Animator>().runtimeAnimatorController = Player1AnimationController;
        _player.GetComponent<Animator>().avatar = Player1Avatar;

        DesiredLocation = CurrentLocation;
        StartCoroutine(MoveToLocation());
    }

    private void Update()
    {
        if(BlockManager.GetBlockAt(transform.position + Vector3.down) == null)
        {
            MoveDown();
        }
    }

  

    public void ApplyGravity()
	{
		
	}

	public void Face(Vector3 direction) {
		float degrees = 0f;
		if (_facingDirection == Vector3.back) {
			if (direction == Vector3.left)
				degrees = 90f;
			else if (direction == Vector3.right)
				degrees = -90f;
			else if (direction == Vector3.forward)
				degrees = 180f;
		}
		else if (_facingDirection == Vector3.forward) {
			if (direction == Vector3.left)
				degrees = -90f;
			else if (direction == Vector3.right)
				degrees = 90f;
			else if (direction == Vector3.back)
				degrees = 180f;
		}
		else if (_facingDirection == Vector3.left) {
			if (direction == Vector3.forward)
				degrees = 90f;
			else if (direction == Vector3.right)
				degrees = -180f;
			else if (direction == Vector3.back)
				degrees = -90f;
		}
		else if (_facingDirection == Vector3.right) {
			if (direction == Vector3.left)
				degrees = 180f;
			else if (direction == Vector3.back)
				degrees = 90f;
			else if (direction == Vector3.forward)
				degrees = -90f;
		}
		_player.transform.RotateAround(transform.position, transform.up, degrees);
		_facingDirection = direction;
	}

	public void TurnAround() {
		Face(-_facingDirection);
	}

	public void Turn( Vector3 direction ) {
		if (direction == Vector3.right)
			TurnRight ();
		else if (direction == Vector3.left)
			TurnLeft ();
		else if (direction == Vector3.zero)
			Debug.Log ("Daddy! Quit skipping my turn!");
		//TODO other turns?
	}

	public void TurnRight() {
		if (_facingDirection == Vector3.forward)
			Face (Vector3.right);
		else if (_facingDirection == Vector3.right)
			Face (Vector3.back);
		else if (_facingDirection == Vector3.left)
			Face (Vector3.forward);
		else
			Face (Vector3.left);
	}

	public void TurnLeft() {
		if (_facingDirection == Vector3.forward)
			Face (Vector3.left);
		else if (_facingDirection == Vector3.right)
			Face (Vector3.forward);
		else if (_facingDirection == Vector3.back)
			Face (Vector3.right);
		else
			Face (Vector3.back);
	}

    public void Move(Vector3 direction)
    {
        if (_facingDirection == direction || direction == Vector3.up || direction == Vector3.down)
        {
            IBlock blockInWay = BlockManager.GetBlockAt(transform.position + direction);
            if (blockInWay != null)
            {
                IBlock oneBlockUp = BlockManager.GetBlockAt(transform.position + direction + Vector3.up);
                if (oneBlockUp == null)
                {
                    Climb(direction + Vector3.up);
                }
            }
            else
            {
                IBlock nextFloor = BlockManager.GetBlockAt(transform.position + direction + Vector3.down);
                if (nextFloor != null) {
                   Walk(direction);
                }
                else
                {
                    IBlock stepDown = BlockManager.GetBlockAt(transform.position + direction + Vector3.down + Vector3.down);
                   // if(stepDown != null)
                    {
                        Climb(direction + Vector3.down);
                    }
                }
            }
        }
        else
        {
            Face(direction);
        }
        /*if (IsSidled || _facingDirection == direction || direction == Vector3.up || direction == Vector3.down)
			_nextMove = direction;
		else {
			Face (direction);
		}*/
    }

	public void PushPull(Vector3 direction)
	{
		if (direction == Vector3.zero)
			return;
        //TODO no sideways movement
		IBlock blockInQuestion = BlockManager.GetBlockAt (transform.position+_facingDirection);
		if (blockInQuestion == null)
			return;
		bool moved = blockInQuestion.Move(direction);
        if (moved)
        {
            IBlock nextFloor = BlockManager.GetBlockAt(transform.position + direction + Vector3.down);
            if (nextFloor != null || direction != _facingDirection)
            {
                Walk(direction);
            }
        }
	}
}
