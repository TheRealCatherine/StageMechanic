using System.Collections.Generic;
using UnityEngine;

public class Pusher1PlayerCharacter : Cathy1PlayerCharacter {

	public override void Sidle(Vector3 direction)
	{
		
	}

	public override Dictionary<string, string[]> SuggestedInputs
	{
		get {
			Dictionary<string, string[]> ret = base.SuggestedInputs;
			ret.Add("Jump", ret["Item"]);
			ret.Remove("Item");
			return ret;
		}
	}

	public override float ApplyInput(List<string> inputNames, Dictionary<string, string> parameters = null)
	{
		if (CurrentMoveState != State.Idle && CurrentMoveState != State.Sidle)
			return 0f;
		if (Position.y % 1 != 0)
			return 0f;

		float expectedTime = 0f;
		bool pushpull = false;
		if (inputNames.Contains("Grab"))
		{
			pushpull = true;
		}
		if (inputNames.Contains("Up"))
		{
			return QueueMove(Vector3.forward, pushpull);
		}
		else if (inputNames.Contains("Down"))
		{
			return QueueMove(Vector3.back, pushpull);
		}
		else if (inputNames.Contains("Left"))
		{
			return QueueMove(Vector3.left, pushpull);
		}
		else if (inputNames.Contains("Right"))
		{
			return QueueMove(Vector3.right, pushpull);
		}
		else if(inputNames.Contains("Jump"))
		{
			Boingy(CurrentLocation + FacingDirection + FacingDirection + Vector3.up);
			return 1f;
		}

		return expectedTime;
	}

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

		if (FacingDirection != direction)
		{
			IBlock nextFloor = BlockManager.GetBlockNear(transform.position + direction + Vector3.down);
			if (nextFloor == null)
				return 0f;
		}

			IBlock blockInQuestion = BlockManager.GetBlockAt(transform.position + FacingDirection);
		if (blockInQuestion == null)
			return 0f;
		Serializer.RecordUndo();
		//TODO make this one movement
		bool moved = false;
		if (direction == FacingDirection)
		{
			if (BlockManager.BlockGroupNumber(blockInQuestion) > -1)
				moved = BlockManager.PushGroup(BlockManager.BlockGroupNumber(blockInQuestion), direction, 1);
			else
				moved = blockInQuestion.Push(direction, 1);
		}
		else
		{

			if (BlockManager.BlockGroupNumber(blockInQuestion) > -1)
				moved = BlockManager.PullGroup(BlockManager.BlockGroupNumber(blockInQuestion), direction, 1);
			else
				moved = blockInQuestion.Pull(direction, 1);
		}
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
