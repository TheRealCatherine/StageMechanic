using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cathy1EdgeMechanic : MonoBehaviour {

    public bool IsFalling { get; set; } = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        ApplyGravity();
	}

    public void ApplyGravity()
    {
        if (!IsFalling)
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
                IsFalling = false;
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
                IsFalling = false;
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

   /* void OnCollisionEnter(Collision collision)
    {
        Cathy1Block otherBlock = collision.collider.gameObject.GetComponent<Cathy1Block>();
        Cathy1Block thisBlock = gameObject.GetComponent<Cathy1Block>();
        Platform platform = collision.collider.gameObject.GetComponent<Platform>();
        TestForSupport(thisBlock, otherBlock);
        TestForSupport(thisBlock, platform);
    }

    void OnCollisionStay(Collision collisionInfo)
    {
        Cathy1Block otherBlock = collisionInfo.collider.gameObject.GetComponent<Cathy1Block>();
        Cathy1Block thisBlock = gameObject.GetComponent<Cathy1Block>();
        Platform platform = collisionInfo.collider.gameObject.GetComponent<Platform>();
        TestForSupport(thisBlock, otherBlock);
        TestForSupport(thisBlock, platform);
    }

    void OnTriggerEnter(Collider other)
    {
        Cathy1Block otherBlock = other.gameObject.GetComponent<Cathy1Block>();
        Cathy1Block thisBlock = gameObject.GetComponent<Cathy1Block>();
        Platform platform = other.gameObject.GetComponent<Platform>();
        TestForSupport(thisBlock, otherBlock);
        TestForSupport(thisBlock, platform);
    }

    void OnTriggerStay(Collider collisionInfo)
    {
        Cathy1Block otherBlock = collisionInfo.gameObject.GetComponent<Cathy1Block>();
        Cathy1Block thisBlock = gameObject.GetComponent<Cathy1Block>();
        Platform platform = collisionInfo.gameObject.GetComponent<Platform>();
        TestForSupport(thisBlock, otherBlock);
        TestForSupport(thisBlock, platform);
    }*/
}
