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

    public float speed = 10.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    private Vector3 moveDirection;

    // Use this for initialization
    void Start () {
        _player=Instantiate(Player1Prefab, transform.position, transform.rotation, gameObject.transform);
        _player.transform.RotateAround(transform.position, transform.up, 180f);
        _player.GetComponent<Animator>().runtimeAnimatorController = GetComponent<Animator>().runtimeAnimatorController;
    }

        // Update is called once per frame
    void Update()
    {
        CharacterController controller = GetComponent<CharacterController>();
		Debug.Assert (controller != null);

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
		} if (_nextMove == Vector3.forward) {
			transform.position += new Vector3 (0, 0, 0.25f);
			if ((transform.position.z % 1) == 0) {
				_nextMove = Vector3.zero;
			}
		} if (_nextMove == Vector3.back) {
			transform.position += new Vector3 (0, 0, 0.25f);
			if ((transform.position.z % 1) == 0) {
				_nextMove = Vector3.zero;
			}
		}


		// Jumping stuff TODO made it not horrible
		if (!controller.isGrounded || moveDirection.y > 0f || _nextMove.y > 0f) {
			if (controller.isGrounded) {
				moveDirection.Set (_nextMove.x * speed, 0, _nextMove.z * speed);
				moveDirection = transform.TransformDirection (moveDirection);
				moveDirection *= speed;
				if (_nextMove.y > 0f) {
					moveDirection.y = jumpSpeed;
				}
			} else {
				moveDirection.Set (0, moveDirection.y, 0);
			}
			moveDirection.y -= gravity * Time.deltaTime;
			controller.Move (moveDirection * Time.deltaTime);
			_nextMove.Set (0, 0, 0);
		}
    }

    internal void Move(Vector3 direction)
    {
		if (_facingDirection == direction || direction == Vector3.up || direction == Vector3.down)
			_nextMove = direction;
		else {
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
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
       
		//GameObject other = hit.gameObject;
		//if (other == null)
//			return;


       // if (body == null || body.isKinematic)
       //     return;

        //if (hit.moveDirection.y < -0.3F)
        //    return;

//        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
 //       body.velocity = pushDir * pushPower;
    }
}
