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

    private GameObject _player;
    private Vector3 _nextMove;
	private Vector3 _facingDirection = Vector3.back;

	public bool IsGrounded { get; set; } = false;
	public bool IsSidled { get; set; } = false;
	public bool IsGrabbingBlock { get; set; } = false;

    public float speed = 10.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    private Vector3 moveDirection;

    // Use this for initialization
    void Start () {
		_player=Instantiate(Player1Prefab, transform.position-new Vector3(0f,0.3f,0f), transform.rotation, gameObject.transform);
        _player.transform.RotateAround(transform.position, transform.up, 180f);
        _player.GetComponent<Animator>().runtimeAnimatorController = GetComponent<Animator>().runtimeAnimatorController;
    }

        // Update is called once per frame
    void Update()
    {


		if (_nextMove != Vector3.zero) {
			IBlock blockInWay = BlockManager.GetBlockAt (transform.position+_nextMove);

			if (blockInWay != null && (transform.position.x % 1) == 0 && (transform.position.z % 1) == 0) {
				IBlock oneBlockUp = BlockManager.GetBlockAt (transform.position + _nextMove + Vector3.up);
				if (oneBlockUp == null) {
					transform.position += (_nextMove + Vector3.up);
					_nextMove = Vector3.zero;
				} else {
					_nextMove = Vector3.zero;
				}
			} else if (_nextMove == Vector3.right) {
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

		}

		bool wasGrounded = IsGrounded;
		bool wasSidled = IsSidled;
		IsSidled = false;
		IsGrounded = false;

		Vector3 down = transform.TransformDirection (Vector3.down);

		if (Physics.Raycast (transform.position, down, 0.5f) && (transform.position.y % 1) == 0) {
			IsGrounded = true;
		}

		if (!IsGrounded) {
			//TODO grab edge
			List<Collider> crossColiders = new List<Collider> (Physics.OverlapBox (transform.position + new Vector3 (0f, 0.5f, 0f), new Vector3 (0.1f, 0.1f, 0.75f)));
			crossColiders.AddRange (Physics.OverlapBox (transform.position + new Vector3 (0f, 0.5f, 0f), new Vector3 (0.75f, 0.1f, 0.1f)));

			foreach (Collider col in crossColiders) {
				if (col.gameObject == gameObject)
					continue;
				Cathy1EdgeMechanic otherBlock = col.gameObject.GetComponent<Cathy1EdgeMechanic> ();
				if (otherBlock == null)
					continue;
				if (!otherBlock.IsGrounded)
					continue;
				if (!wasSidled)
					TurnAround ();
				IsSidled = true;
				break;
			}
		}

		ApplyGravity ();
    }

	public void ApplyGravity()
	{
		if (IsGrounded || IsSidled)
			return;
		
		transform.position -= new Vector3(0, 0.25f, 0);
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

    public void Move(Vector3 direction)
    {
		if (IsSidled || _facingDirection == direction || direction == Vector3.up || direction == Vector3.down)
			_nextMove = direction;
		else {
			Face (direction);
		}
    }

	public void PushPull(Vector3 direction)
	{
		IBlock blockInQuestion = BlockManager.GetBlockAt (transform.position+_facingDirection);
		if (blockInQuestion == null)
			return;
		Debug.Log (blockInQuestion.Name);
		bool moved = blockInQuestion.Move(direction);
		if(moved && direction != _facingDirection)
			_nextMove = direction;
	}
}
