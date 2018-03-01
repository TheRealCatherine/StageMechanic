using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxGirlDoorBlock : Cathy1GoalBlock {

	public Vector3 DoorFrameOffset;
	public AudioClip DoorOpenSound;
	public AudioClip DoorCloseSound;

	public virtual void ApplyTheme(BoxGirlBlockTheme theme)
	{
		Debug.Assert(theme.DoorBlock != null);
		Model1 = theme.DoorBlock;
		Model2 = theme.Door;
		Model3 = theme.DoorOpenAnimation;
		Model4 = theme.DoorCloseAnimation;
		DoorFrameOffset = theme.DoorFrameOffset;
		DoorOpenSound = theme.DoorOpenSound;
		DoorCloseSound = theme.DoorCloseSound;
	}

	internal override void HandlePlayer(PlayerMovementEvent ev)
	{
		if (ev.Location != PlayerMovementEvent.EventLocation.Top)
			return;
		string statename = ev.Player.StateNames[ev.Player.CurrentStateIndex];
		if (statename == "Idle" || statename == "Walk" || statename == "Center")
		{
			if (Input.GetKey(KeyCode.UpArrow))
			{
				if (DoorOpenSound != null)
					AudioEffectsManager.PlaySound(this, DoorOpenSound);

				if (!string.IsNullOrWhiteSpace(NextStageFilename) && PlayerPrefs.HasKey("LastLoadDir"))
				{
					Uri location = new Uri(PlayerPrefs.GetString("LastLoadDir") + "/" + NextStageFilename);
					Debug.Log("loading " + location.ToString());
					BlockManager.Instance.TogglePlayMode();
					string[] startPos = null;
					if (!string.IsNullOrWhiteSpace(NextStageStartPos))
					{
						startPos = new string[1];
						startPos[0] = NextStageStartPos;
					}
					Serializer.BlocksFromJson(location, startPlayMode: true, startPositionOverrides: startPos);
				}
				else if (string.IsNullOrWhiteSpace(NextStageFilename))
				{
					Debug.Log("No next level specified");
				}
				else if (!PlayerPrefs.HasKey("LastLoadDir"))
				{
					Debug.Log("Unknown file location");
				}
			}
		}
	}
}
