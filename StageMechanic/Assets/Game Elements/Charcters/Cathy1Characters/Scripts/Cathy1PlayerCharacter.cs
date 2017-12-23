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

    public AudioClip WalkSound;
    public AudioClip JumpSound;
    public AudioClip LandSound;

    public IBlock CurrentBlock;

    private GameObject _player;
	private Vector3 _facingDirection = Vector3.back;

	private const float HEIGHT_ADJUST = 0.5f;
    public float WalkTime { get; set; } = 0.15f;
    public float Granularity { get; set; } = 1.0f;
    public float ClimbSpeed { get; set; } = 0.05f;
    public enum State
    {
        Idle = 0,
        Walk,
        Aproach,
        Climb,
        Center,
        Sidle,
        SidleMove,
        Fall
    }
    private State _currentState = State.Idle;
    public State CurrentMoveState
    {
        get
        {
            return _currentState;
        }
        set
        {
            _currentState = value;
            if(value == State.Sidle || value == State.SidleMove || value == State.Fall)
            {
                _player.transform.position = transform.position - new Vector3(0f, HEIGHT_ADJUST, 0f) + new Vector3(_facingDirection.x / 3f, 0.1f, _facingDirection.z / 3f);
            }
            else
            {
                _player.transform.position = transform.position - new Vector3(0f, HEIGHT_ADJUST, 0f);
            }
        }
    }

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

    public void ForceMove(Vector3 offset)
    {
        if (offset != Vector3.zero)
            CurrentLocation += offset;
    }

    public void Teleport(Vector3 location)
    {
        if(CurrentLocation != location)
            CurrentLocation = location;
    }

    public IEnumerator WalkTo(Vector3 location)
    {
        CurrentMoveState = State.Walk;
        _player.GetComponent<Animator>().SetBool("walking", true);
        yield return new WaitForEndOfFrame();
        GetComponent<AudioSource>().PlayOneShot(WalkSound);
        float journey = 0f;
        Vector3 origin = CurrentLocation;
        while (journey <= WalkTime)
        {
            journey = journey + Time.deltaTime;
            float percent = Mathf.Clamp01(journey / WalkTime);
            Teleport(Vector3.Lerp(origin, location, percent));

            yield return null;
        }
        
        yield return new WaitForEndOfFrame();
        _player.GetComponent<Animator>().SetBool("walking", false);
        CurrentMoveState = State.Idle;
        yield return null;
    }

    public void Walk(Vector3 direction)
    {
        StartCoroutine(WalkTo(CurrentLocation + direction));
    }

    public IEnumerator ClimbTo(Vector3 location)
    {
        CurrentMoveState = State.Aproach;
        _player.GetComponent<Animator>().SetBool("walking", true);
        GetComponent<AudioSource>().PlayOneShot(JumpSound);
        float journey = 0f;
        Vector3 origin = CurrentLocation;
        Vector3 offset = (location-CurrentLocation);
        Vector3 firstPart = origin+ new Vector3(offset.x / 4, 0f, offset.z / 4);
        float firstPartTime = WalkTime * 0.25f;
        while (journey <= firstPartTime)
        {
            journey = journey + Time.deltaTime;
            float percent = Mathf.Clamp01(journey / firstPartTime);

            Teleport(Vector3.Lerp(origin, firstPart, percent));

            yield return null;
        }
        _player.GetComponent<Animator>().SetBool("walking", false);
        CurrentMoveState = State.Climb;
        yield return new WaitForEndOfFrame();
        _player.GetComponent<Animator>().SetBool("climbing", true);
        
        yield return new WaitForEndOfFrame();
        _player.GetComponent<Animator>().SetBool("climbing", false);
        
        yield return new WaitForEndOfFrame();
        _player.GetComponent<Animator>().SetBool("walking", true);
        journey = 0f;
        origin = CurrentLocation;
        float secondPartTime = WalkTime * 0.75f;
        while (journey <= secondPartTime)
        {
            journey = journey + Time.deltaTime;
            float percent = Mathf.Clamp01(journey / secondPartTime);

            Teleport(Vector3.Lerp(origin, location, percent));

            yield return null;
        }
        GetComponent<AudioSource>().PlayOneShot(LandSound);
        _player.GetComponent<Animator>().SetBool("walking", false);
        yield return new WaitForEndOfFrame();
        CurrentMoveState = State.Idle;
        yield return null;
    }

    public void Climb(Vector3 direction)
    {
        StartCoroutine(ClimbTo(CurrentLocation + direction));
    }


    public IEnumerator SidleTo(Vector3 location)
    {
        if (CurrentMoveState == State.Idle)
        {
            CurrentMoveState = State.Aproach;
            yield return new WaitForEndOfFrame();
            CurrentMoveState = State.Climb;
            yield return new WaitForEndOfFrame();
            Teleport(location + Vector3.down);
            yield return new WaitForEndOfFrame();
            CurrentMoveState = State.Sidle;
            yield return null;
        }
        else
        {
            CurrentMoveState = State.SidleMove;
            yield return new WaitForEndOfFrame();
            float journey = 0f;
            Vector3 origin = CurrentLocation;
            while (journey <= WalkTime)
            {
                journey = journey + Time.deltaTime;
                float percent = Mathf.Clamp01(journey / WalkTime);

                Teleport(Vector3.Lerp(origin, location, percent));

                yield return null;
            }
            yield return new WaitForEndOfFrame();
            CurrentMoveState = State.Sidle;
            yield return null;
        }
    }

    public void Sidle(Vector3 direction)
    {
        StartCoroutine(SidleTo(CurrentLocation + direction));
    }

    internal IEnumerator MoveToLocation()
    {
        while (true)
        {
            //Teleport(DesiredLocation);
            //yield return new WaitForSeconds(0.05f);
            //ApplyGravity();
        }
    }

    // Use this for initialization
    void Start () {
		_player=Instantiate(Player1Prefab, transform.position-new Vector3(0f,HEIGHT_ADJUST,0f), transform.rotation, gameObject.transform);
        _player.transform.RotateAround(transform.position, transform.up, 180f);
        _player.GetComponent<Animator>().runtimeAnimatorController = Player1AnimationController;
        _player.GetComponent<Animator>().avatar = Player1Avatar;
    }

    private void Update()
    {
       ApplyGravity();
    }

  

    public void ApplyGravity()
	{
        if (CurrentMoveState == State.Idle || CurrentMoveState == State.Fall)
        {
            if (BlockManager.GetBlockAt(transform.position + Vector3.down) == null)
            {
                CurrentMoveState = State.Fall;
                Teleport(CurrentLocation + Vector3.down);
            }
            else
                CurrentMoveState = State.Idle;
        }
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
			LogController.Log ("Daddy! Quit skipping my turn!");
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

    public void QueueMove(Vector3 direction)
    {
        if (CurrentMoveState == State.Idle)
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
                    if (nextFloor != null)
                    {
                        Walk(direction);
                    }
                    else
                    {
                        IBlock stepDown = BlockManager.GetBlockAt(transform.position + direction + Vector3.down + Vector3.down);
                        if (stepDown != null)
                        {
                            Climb(direction + Vector3.down);
                        }
                        else
                        {
                            if (direction == _facingDirection)
                                TurnAround();
                            Sidle(direction);
                        }
                    }
                }
            }
            else
            {
                Face(direction);
            }
        }
        else if(CurrentMoveState == State.Sidle)
        {
            IBlock attemptedGrab = null;
            if(direction == Vector3.right || direction == Vector3.left)
            {
                Vector3 originalDirection = direction;
                if (_facingDirection == Vector3.forward) { }
                else if (_facingDirection == Vector3.back)
                {
                    if (direction == Vector3.left)
                        direction = Vector3.right;
                    else
                        direction = Vector3.left;
                }
                else if(_facingDirection == Vector3.right)
                {
                    if (direction == Vector3.left)
                        direction = Vector3.forward;
                    else
                        direction = Vector3.back;
                }
                else if(_facingDirection == Vector3.left)
                {
                    if (direction == Vector3.left)
                        direction = Vector3.back;
                    else
                        direction = Vector3.forward;
                }

                attemptedGrab = BlockManager.GetBlockAt(transform.position + direction);
                if (attemptedGrab == null)
                {
                    attemptedGrab = BlockManager.GetBlockAt(transform.position + direction + _facingDirection);
                    if (attemptedGrab != null)
                    {
                        Sidle(direction);
                    }
                    else
                    {
                        Sidle(_facingDirection + direction);
                        if (originalDirection == Vector3.left)
                            TurnRight();
                        else
                            TurnLeft();
                    }
                }
                else
                {
                    Turn(originalDirection);
                }
            }
            else if(direction == Vector3.forward)
            {
                attemptedGrab = BlockManager.GetBlockAt(transform.position + Vector3.up + _facingDirection);
                if(attemptedGrab == null)
                {
                    Climb(_facingDirection + Vector3.up);
                }
            }
            else if(direction == Vector3.back)
            {
                CurrentMoveState = State.Fall;
            }
        }
    }

	public void PushPull(Vector3 direction)
	{
		if (direction == Vector3.zero)
			return;
        //TODO no sideways movement
		IBlock blockInQuestion = BlockManager.GetBlockAt (transform.position+_facingDirection);
		if (blockInQuestion == null)
			return;
        //TODO make this one movement
		bool moved = blockInQuestion.Move(direction);
        if (moved)
        {
            if (_facingDirection != direction)
            {
                IBlock nextFloor = BlockManager.GetBlockAt(transform.position + direction + Vector3.down);
                if (nextFloor != null)
                {
                    Walk(direction);
                }
                else
                {
                    Sidle(direction);
                }
            }
        }
	}
}
