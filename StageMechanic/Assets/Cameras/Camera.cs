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

    // Use this for initialization
    void Start()
    {
        offset = transform.position - Cursor.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
		transform.position = new Vector3(Cursor.transform.position.x + offset.x, Cursor.transform.position.y + offset.y, transform.position.z);
    }
}
