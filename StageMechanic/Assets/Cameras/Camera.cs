/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{

    public GameObject Cursor;

    public Vector3 offset;
    public bool LazyScroll = false;

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
                if(LazyScroll && (transform.position.y > player1pos.y +4f || transform.position.y < player1pos.y))
                    StartCoroutine(AnimateMove(transform.position, new Vector3(0f, player1pos.y + 3f, player1pos.z - 7f),0.2f));
                else
                    StartCoroutine(AnimateMove(transform.position, new Vector3(0f, player1pos.y + 3f, player1pos.z - 7f), 0.2f));
            }
        }
        else {
            if(LazyScroll && (transform.position.y > Cursor.transform.position.y + 4f || transform.position.y < Cursor.transform.position.y))
                StartCoroutine(AnimateMove(transform.position, new Vector3(Cursor.transform.position.x + offset.x, Cursor.transform.position.y + offset.y, transform.position.z), 0.2f));
            else
                StartCoroutine(AnimateMove(transform.position, new Vector3(Cursor.transform.position.x + offset.x, Cursor.transform.position.y + offset.y, transform.position.z), 0.2f));
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
