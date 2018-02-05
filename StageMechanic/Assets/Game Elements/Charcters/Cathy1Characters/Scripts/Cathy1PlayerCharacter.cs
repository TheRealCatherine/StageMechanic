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

public class Cathy1PlayerCharacter : AbstractPlayerCharacter {

    public GameObject Player1Prefab;
    public RuntimeAnimatorController Player1AnimationController;
    public Avatar Player1Avatar;

    public AudioClip WalkSound;
    public AudioClip JumpSound;
    public AudioClip LandSound;
    public AudioClip DieSound;
    public AudioClip ThudSound;
    public AudioClip GameOverSound;

    public AudioClip[] EffortSounds;

    private GameObject _player;

    private const float HEIGHT_ADJUST = 0.5f;
    public float WalkTime { get; set; } = 0.15f;
    public float Granularity { get; set; } = 1.0f;
    public float ClimbSpeed { get; set; } = 0.05f;
    private static System.Random rng = new System.Random();

    public override Vector3 FacingDirection
    {
        get
        {
            return _facingDirection;
        }
        set
        {
            if (_player == null)
                StartCoroutine(UpdateFacingDirectionLater(value));
            else
                Face(value);
        }
    }

    public enum State
    {
        Idle = 0,
        Walk,
        Aproach,
        Climb,
        Center,
        Sidle,
        SidleMove,
        Slide,
        PushPull,
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
            if (_player == null)
            {
                StartCoroutine(UpdateStateLater(value));
            }
            else
            {
                _currentState = value;
                if (value == State.Sidle || value == State.SidleMove || value == State.Fall)
                {
                    _player.transform.position = transform.position - new Vector3(0f, HEIGHT_ADJUST, 0f) + new Vector3(FacingDirection.x / 3f, 0.2f, FacingDirection.z / 3f);
                }
                else
                {
                    _player.transform.position = transform.position - new Vector3(0f, HEIGHT_ADJUST, 0f);
                }
                (CurrentBlock as AbstractBlock)?.OnPlayerMovement(this, PlayerMovementEvent.EventType.Stay);
            }
        }
    }

    public IBlock CurrentBlock {
        get {
            if (CurrentMoveState == State.Sidle || CurrentMoveState == State.SidleMove)
                return BlockManager.GetBlockNear(Position + FacingDirection);
            return BlockManager.GetBlockNear(Position + Vector3.down);
        }
    }

    public IEnumerator UpdateStateLater(State state)
    {
        while (_player == null)
            yield return new WaitForEndOfFrame();
        CurrentMoveState = state;
    }

    public IEnumerator UpdateFacingDirectionLater(Vector3 direction)
    {
        while (_player == null)
            yield return new WaitForEndOfFrame();
        FacingDirection = direction;
    }

    public void PlayDieSound()
    {
        GetComponent<AudioSource>().PlayOneShot(DieSound);
    }

    public void PlayGameOverSound()
    {
        GetComponent<AudioSource>().PlayOneShot(GameOverSound);
    }

    public void PlayThudSound()
    {
        GetComponent<AudioSource>().PlayOneShot(ThudSound);
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

    public override List<string> StateNames
    {
        get
        {
            List <string> states = new List<string>();
            foreach (State state in Enum.GetValues(typeof(State)))
                states.Add(state.ToString());
            return states;
        }
    }

    public override int CurrentStateIndex
    {
        get
        {
            return StateNames.IndexOf(CurrentMoveState.ToString());
        }
        set
        {
            Debug.Assert(StateNames.Count > value);
            string stateName = StateNames[value];
            //TODO check this
            State state = (State)Enum.Parse(typeof(State), stateName);
            CurrentMoveState = state;
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

    public IEnumerator SlideTo(Vector3 location)
    {
        CurrentMoveState = State.Slide;
        float journey = 0f;
        Vector3 origin = CurrentLocation;
        while (journey <= WalkTime)
        {
            journey = journey + Time.deltaTime;
            float percent = Mathf.Clamp01(journey / WalkTime);
            Teleport(Vector3.Lerp(origin, location, percent));

            yield return null;
        }
        (CurrentBlock as AbstractBlock)?.OnPlayerMovement(this, PlayerMovementEvent.EventType.Enter);
        CurrentMoveState = State.Idle;
        ApplyGravity();
        yield return null;
    }

    public void SlideForward()
    {
        if(BlockManager.GetBlockAt(transform.position + FacingDirection) == null)
            StartCoroutine(SlideTo(transform.position + FacingDirection));
    }

    public IEnumerator WalkTo(Vector3 location)
    {
        CurrentMoveState = State.Walk;
        _player.GetComponent<Animator>().SetBool("walking", true);
        yield return new WaitForEndOfFrame();
        GetComponent<AudioSource>().PlayOneShot(WalkSound);
        float journey = 0f;
        Vector3 origin = CurrentLocation;
        IBlock oldBlock = CurrentBlock;
        while (journey <= WalkTime)
        {
            journey = journey + Time.deltaTime;
            float percent = Mathf.Clamp01(journey / WalkTime);
            Teleport(Vector3.Lerp(origin, location, percent));

            yield return null;
        }
        (oldBlock as AbstractBlock)?.OnPlayerMovement(this, PlayerMovementEvent.EventType.Leave);
        yield return new WaitForEndOfFrame();
        _player.GetComponent<Animator>().SetBool("walking", false);
        (CurrentBlock as AbstractBlock)?.OnPlayerMovement(this, PlayerMovementEvent.EventType.Enter);
        CurrentMoveState = State.Idle;
        yield return null;
    }

    public void Walk(Vector3 direction)
    {
        StartCoroutine(WalkTo(CurrentLocation + direction));
    }

    public IEnumerator PushPullTo(Vector3 location)
    {
        CurrentMoveState = State.PushPull;
        _player.GetComponent<Animator>().SetBool("walking", true);
        yield return new WaitForEndOfFrame();
        GetComponent<AudioSource>().PlayOneShot(WalkSound);
        float journey = 0f;
        Vector3 origin = CurrentLocation;
        IBlock oldBlock = CurrentBlock;
        while (journey <= WalkTime)
        {
            journey = journey + Time.deltaTime;
            float percent = Mathf.Clamp01(journey / WalkTime);
            Teleport(Vector3.Lerp(origin, location, percent));

            yield return null;
        }
       (oldBlock as AbstractBlock)?.OnPlayerMovement(this, PlayerMovementEvent.EventType.Leave);
        yield return new WaitForEndOfFrame();
        _player.GetComponent<Animator>().SetBool("walking", false);
        (CurrentBlock as AbstractBlock)?.OnPlayerMovement(this, PlayerMovementEvent.EventType.Enter);
        CurrentMoveState = State.Idle;
        yield return null;
    }

    public void DoPushPull(Vector3 direction)
    {
        StartCoroutine(PushPullTo(CurrentLocation + direction));
    }

    public IEnumerator ClimbTo(Vector3 location)
    {
        float journey = 0f;
        Vector3 origin = CurrentLocation;
        Vector3 offset = (location - CurrentLocation);
        IBlock oldBlock = CurrentBlock;
        if (CurrentMoveState != State.Sidle && CurrentMoveState != State.SidleMove)
        {
            CurrentMoveState = State.Aproach;
            _player.GetComponent<Animator>().SetBool("walking", true);
            GetComponent<AudioSource>().PlayOneShot(JumpSound);
            Vector3 firstPart = origin + new Vector3(offset.x / 4, 0f, offset.z / 4);
            float firstPartTime = WalkTime * 0.25f;
            while (journey <= firstPartTime)
            {
                journey = journey + Time.deltaTime;
                float percent = Mathf.Clamp01(journey / firstPartTime);

                Teleport(Vector3.Lerp(origin, firstPart, percent));

                yield return null;
            }
            _player.GetComponent<Animator>().SetBool("walking", false);
        }
        AbstractBlock oab = oldBlock as AbstractBlock;
        if(oab != null && oab.gameObject != null)
            oab.OnPlayerMovement(this, PlayerMovementEvent.EventType.Leave);
        CurrentMoveState = State.Climb;
        yield return new WaitForEndOfFrame();
        _player.GetComponent<Animator>().SetBool("climbing", true);
        
        yield return new WaitForEndOfFrame();
        _player.GetComponent<Animator>().SetBool("climbing", false);

        (CurrentBlock as AbstractBlock)?.OnPlayerMovement(this, PlayerMovementEvent.EventType.Enter);
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
            yield return new WaitForSeconds(0.1f);
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

    private IEnumerator BoingyTo(Vector3 location)
    {
        CurrentMoveState = State.Idle;
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
        CurrentMoveState = State.Fall;
        yield return null;
    }

    public void Boingy(Vector3 location)
    {
        StartCoroutine(BoingyTo(location));
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

    public override bool TakeDamage(float unused, string alsoNotUsed )
    {

        if (!UIManager.IsSinglePlayerDeathDialogOpen)
        {
            UIManager.ShowSinglePlayerDeathDialog(DieSound);
        }
        return true;
    }

    public override bool ApplyGravity(float factor = 1f, float acceleration = 0f)
	{
        if (CurrentMoveState == State.Idle || CurrentMoveState == State.Fall)
        {
            if (BlockManager.GetBlockNear(transform.position + Vector3.down) == null)
            {
                CurrentMoveState = State.Fall;
                base.ApplyGravity(factor, acceleration);
                if (BlockManager.GetBlockAt(transform.position + FacingDirection) != null && BlockManager.GetBlockAt(transform.position + FacingDirection + Vector3.up) == null)
                {
                    CurrentMoveState = State.Sidle;
                }
                return true;
            }
            else
            {
                CurrentMoveState = State.Idle;
                return false;
            }
        }
        if (CurrentMoveState == State.Sidle && BlockManager.GetBlockAt(transform.position + FacingDirection) == null)
        {
            CurrentMoveState = State.Fall;
            return false;
        }
        return false;
    }

	public override bool Face(Vector3 direction) {
        if (FacingDirection == direction)
            return false;
		float degrees = 0f;
		if (FacingDirection == Vector3.back) {
			if (direction == Vector3.left)
				degrees = 90f;
			else if (direction == Vector3.right)
				degrees = -90f;
			else if (direction == Vector3.forward)
				degrees = 180f;
		}
		else if (FacingDirection == Vector3.forward) {
			if (direction == Vector3.left)
				degrees = -90f;
			else if (direction == Vector3.right)
				degrees = 90f;
			else if (direction == Vector3.back)
				degrees = 180f;
		}
		else if (FacingDirection == Vector3.left) {
			if (direction == Vector3.forward)
				degrees = 90f;
			else if (direction == Vector3.right)
				degrees = -180f;
			else if (direction == Vector3.back)
				degrees = -90f;
		}
		else if (FacingDirection == Vector3.right) {
			if (direction == Vector3.left)
				degrees = 180f;
			else if (direction == Vector3.back)
				degrees = 90f;
			else if (direction == Vector3.forward)
				degrees = -90f;
		}
		_player.transform.RotateAround(transform.position, transform.up, degrees);
		base.FacingDirection = direction;
        return true;
	}

	public override bool TurnAround() {
		return Face(-FacingDirection);
	}

    public static Vector3 ReverseDirection( Vector3 direction )
    {
        if (direction == Vector3.left)
            return Vector3.right;
        else if (direction == Vector3.right)
            return Vector3.left;
        else if (direction == Vector3.forward)
            return Vector3.back;
        else if (direction == Vector3.back)
            return Vector3.forward;
        else
            return Vector3.zero;
    }

    public float QueueMove(Vector3 direction, bool pushpull = false)
    {
        float expectedTime = 0f;
        if(pushpull)
        {
            return PushPull(direction);
        }
        if (CurrentMoveState == State.Idle)
        {
            if (FacingDirection == direction || direction == Vector3.up || direction == Vector3.down)
            {
                IBlock blockInWay = BlockManager.GetBlockAt(transform.position + direction);
                if (blockInWay != null)
                {
                    IBlock oneBlockUp = BlockManager.GetBlockAt(transform.position + direction + Vector3.up);
                    IBlock blockAbove = BlockManager.GetBlockAt(transform.position + Vector3.up);
                    if (oneBlockUp == null && blockAbove == null)
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
                            if (direction == FacingDirection)
                                TurnAround();
                            Sidle(direction);
                        }
                    }
                }
                expectedTime = 0.35f;
            }
            else
            {
                Face(direction);
                expectedTime = 0.25f;
            }
        }
        else if(CurrentMoveState == State.Sidle)
        {
            IBlock attemptedGrab = null;
            if(direction == Vector3.right || direction == Vector3.left)
            {
                Vector3 originalDirection = direction;
                if (FacingDirection == Vector3.forward) { }
                else if (FacingDirection == Vector3.back)
                {
                    if (direction == Vector3.left)
                        direction = Vector3.right;
                    else
                        direction = Vector3.left;
                }
                else if(FacingDirection == Vector3.right)
                {
                    if (direction == Vector3.left)
                        direction = Vector3.forward;
                    else
                        direction = Vector3.back;
                }
                else if(FacingDirection == Vector3.left)
                {
                    if (direction == Vector3.left)
                        direction = Vector3.back;
                    else
                        direction = Vector3.forward;
                }

                attemptedGrab = BlockManager.GetBlockAt(transform.position + direction);
                if (attemptedGrab == null)
                {
                    attemptedGrab = BlockManager.GetBlockAt(transform.position + direction + FacingDirection);
                    if (attemptedGrab != null)
                    {
                        if(BlockManager.GetBlockAt(transform.position + direction + Vector3.up) == null)
                            Sidle(direction);
                    }
                    else
                    {
                        if (BlockManager.GetBlockAt(transform.position + direction +FacingDirection + Vector3.up) == null)
                        {
                            Sidle(FacingDirection + direction);
                            if (originalDirection == Vector3.left)
                                TurnRight();
                            else
                                TurnLeft();
                        }
                    }
                    expectedTime = 0.35f;
                }
                else
                {
                    Turn(originalDirection);
                    expectedTime = 0.2f;
                }
            }
            else if(direction == Vector3.forward)
            {
                attemptedGrab = BlockManager.GetBlockAt(transform.position + Vector3.up + FacingDirection);
                if(attemptedGrab == null)
                {
                    Climb(FacingDirection + Vector3.up);
                    expectedTime = 0.35f;
                }
            }
            else if(direction == Vector3.back)
            {
                CurrentMoveState = State.Fall;
                expectedTime = 0.1f;
            }
        }
        return expectedTime;
    }

	public float PushPull(Vector3 direction)
	{
		if (direction == Vector3.zero)
			return 0f;
        
        if (CurrentMoveState == State.Sidle)
        {
            if (BlockManager.GetBlockAt(transform.position + Vector3.down) == null)
                return 0f;
            CurrentMoveState = State.Fall;
        }
            

        if (direction != FacingDirection && direction != ReverseDirection(FacingDirection))
            return 0f;

        //Don't allow pull if there is a block in your way
        if(direction == ReverseDirection(FacingDirection))
        {
            IBlock blockInWay = BlockManager.GetBlockAt(transform.position + ReverseDirection(FacingDirection));
            if (blockInWay != null)
                return 0f;
        }

        //TODO no sideways movement
		IBlock blockInQuestion = BlockManager.GetBlockAt (transform.position+FacingDirection);
		if (blockInQuestion == null)
			return 0f;
        Serializer.RecordUndo();
		//TODO make this one movement
		bool moved = false;
		if (direction == FacingDirection)
			moved = blockInQuestion.Push(direction,1);
		else
			moved = blockInQuestion.Pull(direction, 1);
		if (moved)
        {
            if (EffortSounds.Length > 0)
            {
                int gruntDex = rng.Next(EffortSounds.Length);
                GetComponent<AudioSource>().PlayOneShot(EffortSounds[gruntDex]);
            }
            if (FacingDirection != direction)
            {
                IBlock nextFloor = BlockManager.GetBlockNear(transform.position + direction + Vector3.down);
                if (nextFloor != null)
                {
                    DoPushPull(direction);
                }
                else
                {
                    CurrentMoveState = State.Idle;
                    Sidle(direction);
                }
            }
            return 0.35f;
        }
        return 0f;
	}

    public override float ApplyInput(List<string> inputNames, Dictionary<string, string> parameters = null)
    {
        if (CurrentMoveState != State.Idle && CurrentMoveState != State.Sidle)
            return 0f;
        if (Position.y % 1 != 0)
            return 0f;

        float expectedTime = 0f;
        bool pushpull = false;
        if (inputNames.Contains("Grab"))
        {
            pushpull = true;
        }
        if (inputNames.Contains("Up"))
        {
            return QueueMove(Vector3.forward, pushpull);
        }
        else if (inputNames.Contains("Down"))
        {
            return QueueMove(Vector3.back, pushpull);
        }
        else if (inputNames.Contains("Left"))
        {
            return QueueMove(Vector3.left, pushpull);
        }
        else if (inputNames.Contains("Right"))
        {
            return QueueMove(Vector3.right, pushpull);
        }

        return expectedTime;
    }
}
