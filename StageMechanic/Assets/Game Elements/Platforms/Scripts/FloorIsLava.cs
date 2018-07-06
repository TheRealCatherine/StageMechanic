/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using UnityEngine;

public class FloorIsLava : Platform
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
	if (BlockManager.PlayMode) {
		int count = PlayerManager.PlayerCount();
		for(int i=0;i<count;++i)
		{
			IPlayerCharacter player = PlayerManager.Player(i);
			Debug.Assert(player != null);
			if (player.Position.y < transform.position.y)
				player.TakeDamage(float.PositiveInfinity, "Bottomless Pit");

		}
	}
    }

    void OnCollisionEnter(Collision collision)
    {
        if (BlockManager.PlayMode)
        {
            if (collision.gameObject.GetComponent<IBlock>() != null)
                BlockManager.DestroyBlock(collision.gameObject.GetComponent<IBlock>());
            else if(collision.gameObject.GetComponent<IPlayerCharacter>() != null)
                collision.gameObject.GetComponent<IPlayerCharacter>().TakeDamage(float.PositiveInfinity);
            else
                Destroy(collision.gameObject);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (BlockManager.PlayMode)
        {
            if (collision.gameObject.GetComponent<IBlock>() != null)
                BlockManager.DestroyBlock(collision.gameObject.GetComponent<IBlock>());
            else if (collision.gameObject.GetComponent<IPlayerCharacter>() != null)
                collision.gameObject.GetComponent<IPlayerCharacter>().TakeDamage(float.PositiveInfinity);
            else
                Destroy(collision.gameObject);
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (BlockManager.PlayMode)
        {
            if (collision.gameObject.GetComponent<IBlock>() != null)
                BlockManager.DestroyBlock(collision.gameObject.GetComponent<IBlock>());
            else if (collision.gameObject.GetComponent<IPlayerCharacter>() != null)
                collision.gameObject.GetComponent<IPlayerCharacter>().TakeDamage(float.PositiveInfinity);
            else
                Destroy(collision.gameObject);
        }
    }

    void OnTriggerStay(Collider collision)
    {
        if (BlockManager.PlayMode)
        {
            if (collision.gameObject.GetComponent<IBlock>() != null)
                BlockManager.DestroyBlock(collision.gameObject.GetComponent<IBlock>());
            else if (collision.gameObject.GetComponent<IPlayerCharacter>() != null)
                collision.gameObject.GetComponent<IPlayerCharacter>().TakeDamage(float.PositiveInfinity);
            else
                Destroy(collision.gameObject);
        }
    }
}
