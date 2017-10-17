using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cathy1PlayerCharacter : MonoBehaviour {

    public GameObject Player1Prefab;

    private GameObject _player;
    private Vector3 _nextMove;

    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;

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
        if (controller.isGrounded)
        {
            moveDirection = new Vector3(_nextMove.x*10, 0, _nextMove.z*10);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            if (_nextMove.y>0)
            {
                moveDirection.y = jumpSpeed;
            }
        }
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
        _nextMove.Set(0, 0, 0);
    }

    internal void Move(Vector3 direction)
    {
        _nextMove = direction;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
       
        // Rigidbody body = hit.collider.attachedRigidbody;
       // if (body == null || body.isKinematic)
       //     return;

        //if (hit.moveDirection.y < -0.3F)
        //    return;

//        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
 //       body.velocity = pushDir * pushPower;
    }
}
