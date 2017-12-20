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

[RequireComponent(typeof(GridWalkBehavior))]
public class Cathy1PlayerCharacter : MonoBehaviour {

    public GameObject Player1Prefab;
    public RuntimeAnimatorController Player1AnimationController;
    public Avatar Player1Avatar;

    private GameObject _player;
   /* private Vector3 _nextMove;
	private Vector3 _lastSidleMove;
	private Vector3 _lastSidleRequest;*/
	private Vector3 _facingDirection = Vector3.back;
    /*
	public bool IsGrounded { get; set; } = false;
	public bool IsSidled { get; set; } = false;
	public bool HasLetGo { get; set; } = false;
	public bool IsGrabbingBlock { get; set; } = false;

    public float speed = 10.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;*/

	private const float HEIGHT_ADJUST = 1.0f;
    /*
    private Vector3 moveDirection;*/

    // Use this for initialization
    void Start () {
		_player=Instantiate(Player1Prefab, transform.position-new Vector3(0f,HEIGHT_ADJUST,0f), transform.rotation, gameObject.transform);
        _player.transform.RotateAround(transform.position, transform.up, 180f);
        _player.GetComponent<Animator>().runtimeAnimatorController = Player1AnimationController;
        _player.GetComponent<Animator>().avatar = Player1Avatar;

        GridWalkBehavior walker = GetComponent<GridWalkBehavior>();
        walker.Character = _player;
        //GridClimbBehavior climber = GetComponent<GridClimbBehavior>();
        //climber.Character = _player;
    }

        // Update is called once per frame
   /* void Update()
    {
		if (_nextMove != Vector3.zero) {
			if (IsSidled && (transform.position.x % 1) == 0 && (transform.position.z % 1) == 0) {

				if (_nextMove == Vector3.forward) {
					IBlock oneBlockUp = BlockManager.GetBlockAt (transform.position + _facingDirection + Vector3.up);
					if (oneBlockUp == null) {
						_player.transform.localPosition = new Vector3(0f,-HEIGHT_ADJUST,0f);
						transform.position += (_facingDirection + Vector3.up);
						_nextMove = Vector3.zero;
						IsSidled = false;
						HasLetGo = false;
						IsGrounded = true;
						IsGrabbingBlock = false;
					} else {
						_nextMove = Vector3.zero;
					}
				}
				else if (_nextMove == Vector3.back) {
					_nextMove = Vector3.zero;
				}
				else if (_nextMove == Vector3.left || _nextMove == Vector3.right) {
					//TODO this can be done in 1 if block.
					if (_facingDirection == Vector3.forward) {
						IBlock blockInWay = BlockManager.GetBlockAt (transform.position + _nextMove);
						IBlock sameEdgeBlock = BlockManager.GetBlockAt (transform.position + _nextMove + _facingDirection);
						if (blockInWay != null) {
							Turn( _nextMove );
							_lastSidleRequest = _nextMove;
							_lastSidleMove = Vector3.zero;
							_nextMove = Vector3.zero;
						} else if (sameEdgeBlock != null) {
							transform.position += _nextMove;
							_lastSidleRequest = _nextMove;
							_lastSidleMove = _nextMove;
							_nextMove = Vector3.zero;
						} else {
							transform.position += _nextMove;
							Turn(-_nextMove);
							transform.position += Vector3.forward;
							_lastSidleRequest = _nextMove;
							_lastSidleMove = (_nextMove + Vector3.forward);
							_nextMove = Vector3.zero;
					 	}
					} else if (_facingDirection == Vector3.back) {
						IBlock blockInWay = BlockManager.GetBlockAt (transform.position - _nextMove);
						IBlock sameEdgeBlock = BlockManager.GetBlockAt (transform.position - _nextMove + _facingDirection);
						if (blockInWay != null) {
							Turn( _nextMove );
							_lastSidleRequest = _nextMove;
							_lastSidleMove = Vector3.zero;
							_nextMove = Vector3.zero;
						} else if (sameEdgeBlock != null) {
							transform.position -= _nextMove;
							_lastSidleRequest = _nextMove;
							_lastSidleMove = -_nextMove;
							_nextMove = Vector3.zero;
						} else {
							transform.position -= _nextMove;
							Turn(-_nextMove);
							transform.position += Vector3.back;
							_lastSidleRequest = _nextMove;
							_lastSidleMove = (-_nextMove + Vector3.back);
							_nextMove = Vector3.zero;
						}
					  } else if (_facingDirection == Vector3.left) {
						Vector3 move = (_nextMove == Vector3.right ? Vector3.forward : Vector3.back);
						IBlock blockInWay = BlockManager.GetBlockAt (transform.position + move);
						IBlock sameEdgeBlock = BlockManager.GetBlockAt (transform.position + move + _facingDirection);
						if (blockInWay != null) {
							Turn( _nextMove );
							_lastSidleRequest = _nextMove;
							_lastSidleMove = Vector3.zero;
							_nextMove = Vector3.zero;
						} else if (sameEdgeBlock != null) {
							transform.position += move;
							_lastSidleRequest = _nextMove;
							_lastSidleMove = move;
							_nextMove = Vector3.zero;
						} else {
							transform.position += move;
							Turn(-_nextMove);
							transform.position += Vector3.left;
							_lastSidleRequest = _nextMove;
							_lastSidleMove = (move + Vector3.left);
							_nextMove = Vector3.zero;
						}
					}  else if (_facingDirection == Vector3.right) {
						Vector3 move = (_nextMove == Vector3.right ? Vector3.back : Vector3.forward);
						IBlock blockInWay = BlockManager.GetBlockAt (transform.position + move);
						IBlock sameEdgeBlock = BlockManager.GetBlockAt (transform.position + move + _facingDirection);
						if (blockInWay != null) {
							Turn( _nextMove );
							_lastSidleRequest = _nextMove;
							_lastSidleMove = Vector3.zero;
							_nextMove = Vector3.zero;
						} else if (sameEdgeBlock != null) {
							transform.position += move;
							_lastSidleRequest = _nextMove;
							_lastSidleMove = move;
							_nextMove = Vector3.zero;
						} else {
							transform.position += move;
							Turn(-_nextMove);
							transform.position += Vector3.right;
							_lastSidleRequest = _nextMove;
							_lastSidleMove = (move + Vector3.right);
							_nextMove = Vector3.zero;
						}
					}
				} 

				//Not Sidling
			} else {
				_lastSidleMove = Vector3.zero;
				_lastSidleRequest = Vector3.zero;
				IBlock blockInWay = BlockManager.GetBlockAt (transform.position + _nextMove);
				if (blockInWay != null && (transform.position.x % 1) == 0 && (transform.position.z % 1) == 0) {
					IBlock oneBlockUp = BlockManager.GetBlockAt (transform.position + _nextMove + Vector3.up);
					if (oneBlockUp == null) {
						transform.position += (_nextMove + Vector3.up);
						_nextMove = Vector3.zero;
					} else {
						_nextMove = Vector3.zero;
					}
				} else {
					MoveNextDirection();
				}
			}
		}

		bool wasGrounded = IsGrounded;
		bool wasSidled = IsSidled;
		IsSidled = false;
		IsGrounded = false;

		Vector3 down = transform.TransformDirection (Vector3.down);

		if ((!wasSidled || IsGrabbingBlock) && Physics.Raycast (transform.position, down, 0.5f) && (transform.position.y % 1) == 0) {
			IsGrounded = true;
		}

		if (!IsGrounded && !HasLetGo) {
			List<Collider> crossColiders;
			if(_facingDirection == Vector3.forward || _facingDirection == Vector3.back)
			 	crossColiders = new List<Collider> (Physics.OverlapBox (transform.position + new Vector3 (0f, 0.5f, 0f), new Vector3 (0.1f, 0.1f, 0.5f)));
			else
				crossColiders = new List<Collider> (Physics.OverlapBox (transform.position + new Vector3 (0f, 0.5f, 0f), new Vector3 (0.5f, 0.1f, 0.1f)));

			foreach (Collider col in crossColiders) {
				if (col.gameObject == gameObject)
					continue;
				Cathy1EdgeMechanic otherBlock = col.gameObject.GetComponent<Cathy1EdgeMechanic> ();
				if (otherBlock == null)
					continue;
				if (!otherBlock.IsGrounded)
					continue;
				//if (otherBlock.TestForSupportedBlock (1))
				//	continue;
				if (!wasSidled && !IsGrabbingBlock) {
					TurnAround ();
				}
				if (!IsGrabbingBlock) {
					IsSidled = true;
					_player.transform.localPosition = new Vector3 (_facingDirection.x * 0.4f, 0f, _facingDirection.z * 0.4f);
					break;
				} else {
					IsSidled = false;
					break;
				}
			}
		}
		if(!IsSidled)
			_player.transform.localPosition = new Vector3(0f,-HEIGHT_ADJUST,0f);
		ApplyGravity ();
		if ((transform.position.y % 1) == 0) {
			HasLetGo = false;
			IsGrabbingBlock = false;
		}
    }*/

	public void ApplyGravity()
	{
		/*if (IsGrounded || IsSidled)
			return;
		
		transform.position -= new Vector3(0, 0.25f, 0);*/
	}

	/*protected void MoveNextDirection() {

		if (_nextMove == Vector3.right) {
			transform.position += new Vector3 (0.25f, 0, 0);
			if ((transform.position.x % 1) == 0) {
				_nextMove = Vector3.zero;
			}
		} else if (_nextMove == Vector3.left) {
			transform.position += new Vector3 (-0.25f, 0, 0);
			if ((transform.position.x % 1) == 0) {
				_nextMove = Vector3.zero;
			}
		} else if (_nextMove == Vector3.forward) {
			transform.position += new Vector3 (0, 0, 0.25f);
			if ((transform.position.z % 1) == 0) {
				_nextMove = Vector3.zero;
			}
		} else if (_nextMove == Vector3.back) {
			transform.position += new Vector3 (0, 0, -0.25f);
			if ((transform.position.z % 1) == 0) {
				_nextMove = Vector3.zero;
			}
		}
	}*/

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
                Debug.Log("Block: " + blockInWay.Name);
                IBlock oneBlockUp = BlockManager.GetBlockAt(transform.position + direction + Vector3.up);
                if (oneBlockUp == null)
                {
                    GridWalkBehavior climber = GetComponent<GridWalkBehavior>();
                    climber.Climb(direction + Vector3.up);
                }
            }
            else
            {
                IBlock nextFloor = BlockManager.GetBlockAt(transform.position + direction + Vector3.down);
                if (nextFloor != null) {
                    GridWalkBehavior walker = GetComponent<GridWalkBehavior>();
                    walker.Move(direction);
                }
                else
                {
                    IBlock stepDown = BlockManager.GetBlockAt(transform.position + direction + Vector3.down + Vector3.down);
                    if(stepDown != null)
                    {
                        GridWalkBehavior climber = GetComponent<GridWalkBehavior>();
                        climber.Climb(direction + Vector3.down);
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
		/*if (IsSidled) {
			IsSidled = false;
			IsGrounded = false;
			HasLetGo = true;
			return;
		}
		if (direction == Vector3.zero)
			return;
		IBlock blockInQuestion = BlockManager.GetBlockAt (transform.position+_facingDirection);
		if (blockInQuestion == null)
			return;
		IsGrabbingBlock = true;
		Debug.Log ("Grabbing block: " + blockInQuestion.Name);
		bool moved = blockInQuestion.Move(direction);
		IsGrounded = false;
		IsSidled = false;
		if(moved && direction != _facingDirection)
			_nextMove = direction;
	*/	
	}
}
