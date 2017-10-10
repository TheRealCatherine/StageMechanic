using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cathy1EdgeMechanic : MonoBehaviour {

    public bool IsFalling { get; set; } = true;

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

        thisBlock.Position -= new Vector3(0, 0.25f, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        Cathy1Block otherBlock = other.gameObject.GetComponent<Cathy1Block>();
        Cathy1Block thisBlock = gameObject.GetComponent<Cathy1Block>();
        if(otherBlock != null && thisBlock != null)
        {
            //Check if this block is above the other one
            if(Utility.AlmostEquals(otherBlock.Position.y,thisBlock.Position.y-1,0.0001))
            {
                IsFalling = false;
                //Check if this block is at a diagnol
                if (otherBlock.Position.x != thisBlock.Position.x || otherBlock.Position.z != thisBlock.Position.z)
                {
                    //We have an EDGE connection  
                    //TODO check if there is a block underneath to see if this is
                    //an EDGE only connection

                }
            }
            else if(Utility.AlmostEquals(otherBlock.Position.y, thisBlock.Position.y + 1, 0.0001))
            {
                Cathy1EdgeMechanic EDGE = otherBlock.GetComponent<Cathy1EdgeMechanic>();
                if(EDGE != null)
                {
                    EDGE.IsFalling = IsFalling;
                }
            }
        }
    }

    void OnCollisionStay(Collision collisionInfo)
    {
        Cathy1Block otherBlock = collisionInfo.collider.gameObject.GetComponent<Cathy1Block>();
        Cathy1Block thisBlock = gameObject.GetComponent<Cathy1Block>();
        if (otherBlock != null && thisBlock != null)
        {
            //Check if this block is above the other one
            if (Utility.AlmostEquals(otherBlock.Position.y, thisBlock.Position.y - 1, 0.0001))
            {
                IsFalling = false;
                //Check if this block is at a diagnol
                if (otherBlock.Position.x != thisBlock.Position.x || otherBlock.Position.z != thisBlock.Position.z)
                {
                    //We have an EDGE connection
                    //TODO check if there is a block underneath to see if this is
                    //an EDGE only connection

                }
            }
            else if (Utility.AlmostEquals(otherBlock.Position.y, thisBlock.Position.y + 1, 0.0001))
            {
                Cathy1EdgeMechanic EDGE = otherBlock.GetComponent<Cathy1EdgeMechanic>();
                if (EDGE != null)
                {
                    EDGE.IsFalling = IsFalling;
                }
            }
        }
    }
}
