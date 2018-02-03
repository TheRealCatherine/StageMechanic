/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject Cursor;

    public Vector3 offset;
    public bool LazyScroll = false;
    private float _cursorZoom = 0f;
    private float _playerZoom = 0f;
    public float zoom
    {
        get
        {
            if (BlockManager.PlayMode)
                return _playerZoom;
            return _cursorZoom;
        }
        set
        {
            if (BlockManager.PlayMode)
                _playerZoom = value;
            else
                _cursorZoom = value;
        }
    }

    public void RotateAroundPlatform(Vector3 direction)
    {
        if(direction == Vector3.left)
        {
            offset = new Vector3(10, Cursor.transform.position.y, 5);
        }
        else if (direction == Vector3.left)
        {
            offset = new Vector3(5, Cursor.transform.position.y, 10);
        }
        if (BlockManager.ActiveBlock != null)
            transform.LookAt(BlockManager.ActiveBlock.Position);
        else
            transform.LookAt(new Vector3(BlockManager.ActiveFloor.transform.position.x,0, BlockManager.ActiveFloor.transform.position.z));
    }

    public void ResetZoom()
    {
        zoom = 0;
    }

    // Use this for initialization
    void Start()
    {
        offset = transform.position - Cursor.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(BlockManager.PlayMode)
        {
            Vector3 player1pos = PlayerManager.Player1Location();
            if(player1pos != new Vector3(-255,-255,-255))
            {
                float xPos = 0f;
                if (player1pos.x > 6f)
                    xPos = 8f;
                else if (player1pos.x < -6f)
                    xPos = -8f;

                if(LazyScroll && (transform.position.y > player1pos.y +4f || transform.position.y < player1pos.y))
                    StartCoroutine(AnimateMove(transform.position, new Vector3(xPos, player1pos.y + 3f, player1pos.z - 7f + zoom),0.2f));
                else
                    StartCoroutine(AnimateMove(transform.position, new Vector3(xPos, player1pos.y + 3f, player1pos.z - 7f + zoom), 0.2f));
            }
        }
        else {
            if(LazyScroll && (transform.position.y > Cursor.transform.position.y + 4f || transform.position.y < Cursor.transform.position.y))
                StartCoroutine(AnimateMove(transform.position, new Vector3(Cursor.transform.position.x + offset.x, Cursor.transform.position.y + offset.y, Cursor.transform.position.z - 7f + zoom), 0.2f));
            else
                StartCoroutine(AnimateMove(transform.position, new Vector3(Cursor.transform.position.x + offset.x, Cursor.transform.position.y + offset.y, Cursor.transform.position.z -7f + zoom), 0.2f));
        }
    }

    IEnumerator AnimateMove(Vector3 origin, Vector3 target, float duration)
    {
        float journey = 0f;
        while (journey <= duration)
        {
            journey = journey + Time.deltaTime;
            float percent = Mathf.Clamp01(journey / duration);

            transform.position = Vector3.Lerp(origin, target, percent);

            yield return null;
        }
    }
}
