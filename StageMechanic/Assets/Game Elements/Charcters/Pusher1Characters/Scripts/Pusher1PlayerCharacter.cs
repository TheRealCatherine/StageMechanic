using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pusher1PlayerCharacter : Cathy1PlayerCharacter {

	public override float PushPull(Vector3 direction)
	{
		if (direction == Vector3.zero)
			return 0f;

		if (CurrentMoveState == State.Sidle)
		{
			if (BlockManager.GetBlockAt(transform.position + Vector3.down) == null)
				return 0f;
			CurrentMoveState = State.Fall;
		}

		//Don't allow pull if there is a block in your way
		if (direction == ReverseDirection(FacingDirection))
		{
			IBlock blockInWay = BlockManager.GetBlockAt(transform.position + ReverseDirection(FacingDirection));
			if (blockInWay != null)
				return 0f;
		}

		//TODO no sideways movement
		IBlock blockInQuestion = BlockManager.GetBlockAt(transform.position + FacingDirection);
		if (blockInQuestion == null)
			return 0f;
		Serializer.RecordUndo();
		//TODO make this one movement
		bool moved = false;
		if (direction == FacingDirection)
			moved = blockInQuestion.Push(direction, 1);
		else
			moved = blockInQuestion.Pull(direction, 1);
		if (moved)
		{
			if (EffortSounds.Length > 0)
			{
				//int gruntDex = rng.Next(EffortSounds.Length);
				//GetComponent<AudioSource>().PlayOneShot(EffortSounds[gruntDex]);
			}
			if (FacingDirection != direction)
			{
				IBlock nextFloor = BlockManager.GetBlockNear(transform.position + direction + Vector3.down);
				if (nextFloor != null)
				{
					DoPushPull(direction);
				}
				else
				{
					CurrentMoveState = State.Idle;
					Sidle(direction);
				}
			}
			return 0.35f;
		}
		return 0f;
	}
}
