/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cathy1EdgeMechanic : MonoBehaviour {

    public bool IsGrounded { get; set; } = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Cathy1Block thisBlock = gameObject.GetComponent<Cathy1Block>();
		if (thisBlock == null)
			return;

		BlockManager bm = thisBlock.BlockManager;
		Debug.Assert(bm != null);
		if (bm.PlayMode) {

			IsGrounded = false;

			Vector3 down = transform.TransformDirection (Vector3.down);

			if (Physics.Raycast (transform.position, down, 0.5f) && (transform.position.y % 1) == 0) {
				IsGrounded = true;
			}

			foreach (Collider col in Physics.OverlapBox(transform.position - new Vector3(0f,0.7f,0f),new Vector3(0.75f,0.01f,0.75f))) {
				if (col.gameObject == gameObject)
					continue;
				Cathy1EdgeMechanic otherBlock = col.gameObject.GetComponent<Cathy1EdgeMechanic> ();
				if (otherBlock == null)
					continue;
				if (!otherBlock.IsGrounded)
					continue;
				if ((transform.position.y % 1) == 0) {
					IsGrounded = true;
					break;
				}
			}

		}
        ApplyGravity();
	}

    public void ApplyGravity()
    {
        if (IsGrounded)
            return;

        Cathy1Block thisBlock = gameObject.GetComponent<Cathy1Block>();
        if (thisBlock == null)
            return;

        BlockManager bm = thisBlock.BlockManager;
        Debug.Assert(bm != null);
        if(bm.PlayMode)
            thisBlock.Position -= new Vector3(0, 0.25f, 0);
    }

    //TODO Make this work properly
    void TestForSupport(Cathy1Block thisBlock, Cathy1Block otherBlock)
    {
        BlockManager bm = thisBlock.BlockManager;
        Debug.Assert(bm != null);
        if (!bm.PlayMode)
            return;

        if (otherBlock != null && thisBlock != null)
        {
            //Check if this block is above the other one
            if (Utility.AlmostEquals(otherBlock.Position.y, thisBlock.Position.y - 1, 0.1))
            {
                //TODO check for diagnol
               // IsFalling = false;
                //Check if this block is at a diagnol
                if (otherBlock.Position.x != thisBlock.Position.x || otherBlock.Position.z != thisBlock.Position.z)
                {
                    //We have an EDGE connection  
                    //TODO check if there is a block underneath to see if this is
                    //an EDGE only connection

                }
            }
        }
    }

    void TestForSupport(Cathy1Block thisBlock, Platform platform)
    {
        BlockManager bm = thisBlock.BlockManager;
        Debug.Assert(bm != null);
        if (!bm.PlayMode)
            return;


        if (platform != null && thisBlock != null)
        {
            //Check if this block is above the other one
            if (Utility.AlmostEquals(platform.gameObject.transform.position.y, thisBlock.Position.y-0.5f, 0.1))
            {
                //IsFalling = false;
                //Check if this block is at a diagnol
                if (platform.gameObject.transform.position.x != thisBlock.Position.x || platform.gameObject.transform.position.z != thisBlock.Position.z)
                {
                    //We have an EDGE connection  
                    //TODO check if there is a block underneath to see if this is
                    //an EDGE only connection

                }
            }
        }
    }
}
