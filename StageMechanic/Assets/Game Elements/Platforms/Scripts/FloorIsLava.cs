/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorIsLava : Platform
{

    public GameObject Stage;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Assert(Stage != null);
        BlockManager bm = Stage.GetComponent<BlockManager>();
        Debug.Assert(bm != null);
        if (BlockManager.PlayMode)
            Destroy(collision.gameObject);
    }

    void OnCollisionStay(Collision collision)
    {
        Debug.Assert(Stage != null);
        BlockManager bm = Stage.GetComponent<BlockManager>();
        Debug.Assert(bm != null);
        if (BlockManager.PlayMode)
            Destroy(collision.gameObject);
    }

    void OnTriggerEnter(Collider collision)
    {
        Debug.Assert(Stage != null);
        BlockManager bm = Stage.GetComponent<BlockManager>();
        Debug.Assert(bm != null);
        if (BlockManager.PlayMode)
            Destroy(collision.gameObject);
    }

    void OnTriggerStay(Collider collision)
    {
        Debug.Assert(Stage != null);
        BlockManager bm = Stage.GetComponent<BlockManager>();
        Debug.Assert(bm != null);
        if (BlockManager.PlayMode)
            Destroy(collision.gameObject);
    }
}
