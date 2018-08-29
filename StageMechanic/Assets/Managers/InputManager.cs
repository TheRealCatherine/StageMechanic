/*  
 * Copyright (C) 2018 You're Perfect Studio. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using CnControls;
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InputManager : MonoBehaviour
{
	public CameraController Camera;

	internal const float scrollSpeed = 8.0f;

	public float minX = -360.0f;
	public float maxX = 360.0f;

	public float minY = -45.0f;
	public float maxY = 45.0f;

	public float sensX = 100.0f;
	public float sensY = 100.0f;

	float rotationY = 0.0f;
	float rotationX = 0.0f;

	float period = 0.0f;
	public const float JoystickThrottleRate = 0.1f;

	enum GamepadType
	{
		None,
		Unknown,
		XBox360,
		PS4
	}

	static GamepadType GetGamepadType(int playerNumber)
	{
		string[] gamepadNames = Input.GetJoystickNames();
		if (gamepadNames.Length <= playerNumber)
			return GamepadType.None;
		if (gamepadNames[playerNumber].Contains("360"))
			return GamepadType.XBox360;
		else if (gamepadNames[playerNumber] == "Wireless Controller")
			return GamepadType.PS4;
		return GamepadType.None;
		
	}

	/// <summary>
	/// Check to see if any current user input matches block creation commands for edit mode
	/// </summary>
	/// <returns>true if user input was consumed</returns>
	/// TODO: don't hardcode block types or buttons
	bool TryCreateBlockAtCursor()
	{
		if((GetGamepadType(0) == GamepadType.XBox360 && Input.GetKeyDown(KeyCode.Joystick1Button3))
			|| (GetGamepadType(0) == GamepadType.PS4 && Input.GetKeyDown(KeyCode.Joystick1Button1))
			|| Input.GetKeyDown(KeyCode.Alpha1))
		{
			BlockManager.CreateBlockAtCursor("Cathy1 Internal", "Basic");
			return true;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			BlockManager.CreateBlockAtCursor("Cathy1 Internal", "Immobile");
			return true;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			BlockManager.CreateBlockAtCursor("Cathy1 Internal", "Cracked (2 Steps)");
			return true;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			BlockManager.CreateBlockAtCursor("Cathy1 Internal", "Heavy");
			return true;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			BlockManager.CreateBlockAtCursor("Cathy1 Internal", "Spike Trap");
			return true;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha6))
		{
			BlockManager.CreateBlockAtCursor("Cathy1 Internal", "Ice");
			return true;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha7))
		{
			BlockManager.CreateBlockAtCursor("Cathy1 Internal", "Small Bomb");
			return true;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha8))
		{
			BlockManager.CreateBlockAtCursor("Cathy1 Internal", "Mystery");
			return true;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha9))
		{
			BlockManager.CreateBlockAtCursor("Cathy1 Internal", "Monster");
			return true;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha0))
		{
			BlockManager.CreateBlockAtCursor("Cathy1 Internal", "Vortex");
			return true;
		}
		return false;
	}

	/// <summary>
	/// Check if any current user input is related to stage operations like loading, saving, or clearing
	/// </summary>
	/// <returns>true if the event was consumed</returns>
	/// TODO: don't hardcode commands or the keys
	bool TryLevelOperationCommands()
	{
		bool ctrlDown = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightCommand);

		// Save
		if (Input.GetKeyDown(KeyCode.S))
		{
			if (!BlockManager.PlayMode)
			{
				if (ctrlDown)
					UIManager.SaveToJson();
				else
					Serializer.QuickSave();

				return true;
			}
			//TODO: save play-in-progress level state
		}

		// Load
		else if (Input.GetKeyDown(KeyCode.L))
		{
			if (BlockManager.PlayMode)
			{
				BlockManager.Instance.TogglePlayMode();
			}
			UIManager.LoadFromJson();
			return true;
		}

		// Reload current level
		else if (Input.GetKeyDown(KeyCode.F5))
		{
			Serializer.ReloadCurrentLevel();
			return true;
		}

		// Clear all blocks
		else if (Input.GetKeyDown(KeyCode.Delete) && ctrlDown)
		{
			BlockManager.Clear();
			Serializer.LastAccessedFileName = null;
			return true;
		}

		else if(Input.GetKeyDown(KeyCode.Keypad1) && ctrlDown)
		{
			Serializer.LoadFileUsingHTTP(new Uri("https://gitlab.com/youreperfectstudio/StageMechanic/raw/master/StageMechanic/Stages/Testing/Cathy1/Easy/1-1.json"));
		}
		else if (Input.GetKeyDown(KeyCode.Keypad2) && ctrlDown)
		{
			Serializer.LoadFileUsingHTTP(new Uri("https://gitlab.com/youreperfectstudio/StageMechanic/raw/master/StageMechanic/Stages/Testing/Cathy1/Easy/2-1.json"));
		}
		else if (Input.GetKeyDown(KeyCode.Keypad3) && ctrlDown)
		{
			Serializer.LoadFileUsingHTTP(new Uri("https://gitlab.com/youreperfectstudio/StageMechanic/raw/master/StageMechanic/Stages/Testing/Cathy1/Easy/3-1.json"));
		}
		else if (Input.GetKeyDown(KeyCode.Keypad4) && ctrlDown)
		{
			Serializer.LoadFileUsingHTTP(new Uri("https://gitlab.com/youreperfectstudio/StageMechanic/raw/master/StageMechanic/Stages/Testing/Cathy1/Easy/4-1.json"));
		}
		else if (Input.GetKeyDown(KeyCode.Keypad5) && ctrlDown)
		{
			Serializer.LoadFileUsingHTTP(new Uri("https://gitlab.com/youreperfectstudio/StageMechanic/raw/master/StageMechanic/Stages/Testing/Cathy1/Easy/5-1.json"));
		}
		else if (Input.GetKeyDown(KeyCode.Keypad6) && ctrlDown)
		{
			Serializer.LoadFileUsingHTTP(new Uri("https://gitlab.com/youreperfectstudio/StageMechanic/raw/master/StageMechanic/Stages/Testing/Cathy1/Easy/6-1.json"));
		}
		else if (Input.GetKeyDown(KeyCode.Keypad7) && ctrlDown)
		{
			Serializer.LoadFileUsingHTTP(new Uri("https://gitlab.com/youreperfectstudio/StageMechanic/raw/master/StageMechanic/Stages/Testing/Cathy1/Easy/7-1.json"));
		}
		else if (Input.GetKeyDown(KeyCode.Keypad8) && ctrlDown)
		{
			Serializer.LoadFileUsingHTTP(new Uri("https://gitlab.com/youreperfectstudio/StageMechanic/raw/master/StageMechanic/Stages/Testing/Cathy1/Easy/8-1.json"));
		}
		else if (Input.GetKeyDown(KeyCode.Keypad9) && ctrlDown)
		{
			Serializer.LoadFileUsingHTTP(new Uri("https://gitlab.com/youreperfectstudio/StageMechanic/raw/master/StageMechanic/Stages/Testing/Cathy1/Easy/9-1.json"));
		}

		//Manually move the platform
		//"Be like me. Prepare to fall. --Amy Macdonald"
		else if (Input.GetKeyDown(KeyCode.Tab))
		{
			bool shiftDown = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
			Vector3 blockDirection = Vector3.down;
			Vector3 platformDirection = Vector3.up;
			if (shiftDown)
			{
				blockDirection = Vector3.up;
				platformDirection = Vector3.down;
			}
			foreach (Transform child in BlockManager.ActiveFloor.gameObject.transform)
			{
				if (child.GetComponent<IBlock>() != null)
					child.localPosition += blockDirection;
			}
			BlockManager.ActiveFloor.transform.localPosition += platformDirection;
			return true;
		}

		return false;
	}

	/// <summary>
	/// Check if current user input should open/close/etc a UI dialog
	/// </summary>
	/// <returns>true if the event was consumed</returns>
	/// TODO: don't hardcode commands or keys
	bool TryUICommands()
	{

		if (Input.GetKeyDown(KeyCode.I))
		{
			UIManager.ToggleBlockInfoDialog();
			return true;
		}
		else if (Input.GetKeyDown(KeyCode.F1))
		{
			UIManager.ToggleButtonMappingDialog();
			return true;
		}
		else if ((GetGamepadType(0)==GamepadType.XBox360 && Input.GetKeyDown(KeyCode.Joystick1Button6))
			|| (GetGamepadType(0)==GamepadType.PS4 && Input.GetKeyDown(KeyCode.Joystick1Button8)))
		{
			UIManager.ToggleBlockInfoDialog();
			UIManager.ToggleButtonMappingDialog();
			return true;
		}
		else if (Input.GetKeyDown(KeyCode.Escape))
		{
			//TODO toggle
			UIManager.ShowMainMenu();
			return true;
		}
		return false;
	}

	/// <summary>
	/// Check if user input matches commands for creating/destroying/etc individual blocks
	/// </summary>
	/// <returns>true if the event was consumed</returns>
	/// TODO: don't hardcode commands or buttons
	bool TryBlockCommands()
	{
		// Block type cycling
		if (Input.GetKeyDown(KeyCode.Space)
			|| CnInputManager.GetButtonDown("Grab") 
			|| (GetGamepadType(0)==GamepadType.XBox360 && Input.GetKeyDown(KeyCode.Joystick1Button0))
			|| (GetGamepadType(0)==GamepadType.PS4 && Input.GetKeyDown(KeyCode.Joystick1Button3)))
		{
			if (!BlockManager.PlayMode)
			{
				BlockManager.CreateBlockAtCursor(BlockManager.BlockCycleType);
				return true;
			}

		}

		else if(Input.GetKeyDown(KeyCode.F6)) {
			(BlockManager.ActiveBlock as Cathy1Block)?.ShowModel(1);
			(BlockManager.ActiveBlock as PushPullGameboardBlock)?.ShowVariation(1);
			return true;
		}
		else if (Input.GetKeyDown(KeyCode.F7))
		{
			(BlockManager.ActiveBlock as Cathy1Block)?.ShowModel(2);
			(BlockManager.ActiveBlock as PushPullGameboardBlock)?.ShowVariation(2);
			return true;
		}
		else if (Input.GetKeyDown(KeyCode.F8))
		{
			(BlockManager.ActiveBlock as Cathy1Block)?.ShowModel(3);
			return true;
		}
		else if (Input.GetKeyDown(KeyCode.F9))
		{
			(BlockManager.ActiveBlock as Cathy1Block)?.ShowModel(4);
			return true;
		}

		else if (Input.GetKeyDown(KeyCode.Return))
		{
			IBlock block = BlockManager.ActiveBlock;
			if(block != null)
			{
				UIManager.ShowBlockEditDialog(block);
			}
		}
		else if (Input.GetKeyDown(KeyCode.LeftBracket)
			|| CnInputManager.GetButtonDown("Previous") 
			|| (GetGamepadType(0)==GamepadType.XBox360 && Input.GetKeyDown(KeyCode.Joystick1Button4))
			|| (GetGamepadType(0)==GamepadType.PS4 && Input.GetKeyDown(KeyCode.Joystick1Button4)))
		{
			KeyValuePair<string,string> type = BlockManager.PrevBlockType();
			if (BlockManager.ActiveBlock != null)
				BlockManager.CreateBlockAtCursor(type);
			return true;
		}
		else if (Input.GetKeyDown(KeyCode.RightBracket) 
			|| CnInputManager.GetButtonDown("Next")
			|| (GetGamepadType(0) == GamepadType.XBox360 && Input.GetKeyDown(KeyCode.Joystick1Button5))
			|| (GetGamepadType(0) == GamepadType.PS4 && Input.GetKeyDown(KeyCode.Joystick1Button5)))
		{
			KeyValuePair<string, string> type = BlockManager.NextBlockType();
			if (BlockManager.ActiveBlock != null)
				BlockManager.CreateBlockAtCursor(type);
			return true;
		}

		// Destorying/modifying blocks
		else if (Input.GetKeyDown(KeyCode.Delete) 
			|| CnInputManager.GetButtonDown("Delete")
			|| (GetGamepadType(0) == GamepadType.XBox360 && Input.GetKeyDown(KeyCode.Joystick1Button1))
			|| (GetGamepadType(0) == GamepadType.PS4 && Input.GetKeyDown(KeyCode.Joystick1Button2)))
		{
			BlockManager.Instance.DestroyActiveObject();
		}
		else if (Input.GetKeyDown(KeyCode.B) || CnInputManager.GetButtonDown("StartPosition1"))
		{
			//TODO(ItemManager)
			//EventManager.Instance.CreateCathy1PlayerStartLocation(0, BlockManager.Cursor.transform.position, BlockManager.Cursor.transform.rotation);
		}
		else if (Input.GetKeyDown(KeyCode.O) || CnInputManager.GetButtonDown("StartPosition2"))
		{
			//TODO(ItemManager)
			//EventManager.Instance.CreateCathy1PlayerStartLocation(1, BlockManager.Cursor.transform.position, BlockManager.Cursor.transform.rotation);
		}
		else if (Input.GetKeyDown(KeyCode.T) || CnInputManager.GetButtonDown("StartPosition3"))
		{
			//TODO(ItemManager)
			//EventManager.Instance.CreateCathy1PlayerStartLocation(2, BlockManager.Cursor.transform.position, BlockManager.Cursor.transform.rotation);
		}
		else if(CnInputManager.GetButtonDown("StartPosition4"))
		{
			//TODO(ItemManager)
			//EventManager.Instance.CreatePusherPlayerStartLocation(0, BlockManager.Cursor.transform.position, BlockManager.Cursor.transform.rotation);
		}

		// Buttons for setting items
		else if (Input.GetKeyDown(KeyCode.Home))
		{
			if (BlockManager.ActiveBlock != null)
			{
				//TODO use a method of BlockManager to do this
				Cathy1Block block = BlockManager.ActiveBlock.GetComponent<Cathy1Block>();
				//TODO(ItemManager)
				//block.FirstItem = Instantiate(BlockManager.Instance.StartLocationIndicator, BlockManager.Cursor.transform.position + new Vector3(0, 0.5F, 0), Quaternion.Euler(0, 180, 0)) as GameObject;
				return true;
			}
		}
		else if (Input.GetKeyDown(KeyCode.End))
		{
			if (BlockManager.ActiveBlock != null)
			{
				//TODO use a method of BlockManager to do this
				Cathy1Block block = BlockManager.ActiveBlock.GetComponent<Cathy1Block>();
				//TODO(ItemManager)
				//block.FirstItem = Instantiate(BlockManager.Instance.GoalLocationIndicator, BlockManager.Cursor.transform.position + new Vector3(0, 0.5F, 0), Quaternion.Euler(0, 180, 0)) as GameObject;
				return true;
			}
		}
		else if(Input.GetKeyDown(KeyCode.Keypad0))
		{
			if(BlockManager.ActiveBlock != null)
				BlockManager.AddBlockToGroup(BlockManager.ActiveBlock,0);
		}
		else if (Input.GetKeyDown(KeyCode.Keypad1))
		{
			if (BlockManager.ActiveBlock != null)
				BlockManager.AddBlockToGroup(BlockManager.ActiveBlock, 1);
		}
		else if (Input.GetKeyDown(KeyCode.Keypad2))
		{
			if (BlockManager.ActiveBlock != null)
				BlockManager.AddBlockToGroup(BlockManager.ActiveBlock, 2);
		}
		else if (Input.GetKeyDown(KeyCode.Keypad3))
		{
			if (BlockManager.ActiveBlock != null)
				BlockManager.AddBlockToGroup(BlockManager.ActiveBlock, 3);
		}
		else if (Input.GetKeyDown(KeyCode.Keypad4))
		{
			if (BlockManager.ActiveBlock != null)
				BlockManager.AddBlockToGroup(BlockManager.ActiveBlock, 4);
		}
		else if (Input.GetKeyDown(KeyCode.Keypad5))
		{
			if (BlockManager.ActiveBlock != null)
				BlockManager.AddBlockToGroup(BlockManager.ActiveBlock, 5);
		}
		else if (Input.GetKeyDown(KeyCode.Keypad6))
		{
			if (BlockManager.ActiveBlock != null)
				BlockManager.AddBlockToGroup(BlockManager.ActiveBlock, 6);
		}
		else if (Input.GetKeyDown(KeyCode.Keypad7))
		{
			if (BlockManager.ActiveBlock != null)
				BlockManager.AddBlockToGroup(BlockManager.ActiveBlock, 7);
		}
		else if (Input.GetKeyDown(KeyCode.Keypad8))
		{
			if (BlockManager.ActiveBlock != null)
				BlockManager.AddBlockToGroup(BlockManager.ActiveBlock, 8);
		}
		else if (Input.GetKeyDown(KeyCode.Keypad9))
		{
			if (BlockManager.ActiveBlock != null)
				BlockManager.AddBlockToGroup(BlockManager.ActiveBlock, 9);
		}
		else if (Input.GetKeyDown(KeyCode.KeypadPeriod))
		{
			if (BlockManager.ActiveBlock != null)
				BlockManager.AddBlockToGroup(BlockManager.ActiveBlock, -1);
		}

		return false;
	}

	bool TryPlayerInput()
	{
		if (period < JoystickThrottleRate)
		{
			period += Time.deltaTime;
			return false;
		}

		List<string> axees = new List<string>();
		if (CnInputManager.GetAxis("joystick 1 X axis") > 0)
			axees.Add("joystick 1 X axis +");
		else if (CnInputManager.GetAxis("joystick 1 X axis") < 0)
			axees.Add("joystick 1 X axis -");

		if (CnInputManager.GetAxis("joystick 1 Y axis") > 0)
			axees.Add("joystick 1 Y axis -");
		else if (CnInputManager.GetAxis("joystick 1 Y axis") < 0)
			axees.Add("joystick 1 Y axis +");


		if (CnInputManager.GetAxis("joystick 1 3rd axis") > 0f)
			axees.Add("joystick 1 3rd axis +");
		else if (CnInputManager.GetAxis("joystick 1 3rd axis") < 0f)
			axees.Add("joystick 1 3rd axis -");

		if (CnInputManager.GetAxis("joystick 1 4th axis") > 0f)
			axees.Add("joystick 1 4th axis +");
		else if (CnInputManager.GetAxis("joystick 1 4th axis") < 0f)
			axees.Add("joystick 1 4th axis -");

		if (CnInputManager.GetAxis("joystick 1 5th axis") > 0f)
			axees.Add("joystick 1 5th axis +");
		else if (CnInputManager.GetAxis("joystick 1 5th axis") < 0f)
			axees.Add("joystick 1 5th axis -");

		if (CnInputManager.GetAxis("joystick 1 6th axis") > 0f)
			axees.Add("joystick 1 6th axis +");
		else if (CnInputManager.GetAxis("joystick 1 6th axis") < 0f)
			axees.Add("joystick 1 6th axis -");

		if (CnInputManager.GetAxis("joystick 1 7th axis") > 0f)
			axees.Add("joystick 1 7th axis +");
		else if (CnInputManager.GetAxis("joystick 1 7th axis") < 0f)
			axees.Add("joystick 1 7th axis -");

		if (CnInputManager.GetAxis("joystick 1 8th axis") > 0f)
			axees.Add("joystick 1 8th axis +");
		else if (CnInputManager.GetAxis("joystick 1 8th axis") < 0f)
			axees.Add("joystick 1 8th axis -");

		bool usedAtLeastOne = false;
		for (int playerNumber = 0; playerNumber < PlayerManager.PlayerCount; ++playerNumber)
		{
			Dictionary<string, string[]> possible = PlayerManager.PlayerInputOptions(playerNumber);
			if (possible != null)
			{
				List<string> inputs = new List<string>();
				foreach (KeyValuePair<string, string[]> item in possible)
				{
					foreach (string key in item.Value)
					{
						try
						{
							if (key.Contains("axis"))
							{
								if (axees.Contains(key))
								{
									inputs.Add(item.Key);
								}
							}
							else if (((CnInputManager.ButtonExists(key) && CnInputManager.GetButton(key)) || (!CnInputManager.ButtonExists(key) && (Input.GetKey(key)))) && !inputs.Contains(item.Key))
							{
								inputs.Add(item.Key);
							}
						}
						catch (ArgumentException)
						{
							continue;
						}
					}
				}
				if (inputs.Count > 0)
				{
					float time = PlayerManager.PlayerApplyInput(playerNumber, inputs);
					period = JoystickThrottleRate - time;
					usedAtLeastOne = true;
				}
			}
		}
		return usedAtLeastOne;
	}

	bool TryAtmosphereCommands()
	{
		//Music controlls
		if (Input.GetKeyDown(KeyCode.F11))
		{
			MusicManager.TogglePause();
			return true;
		}
		else if (Input.GetKeyDown(KeyCode.F10))
		{
			MusicManager.PlayPreviousTrack();
			return true;
		}
		else if (Input.GetKeyDown(KeyCode.F12))
		{
			MusicManager.PlayNextTrack();
			return true;
		}
		else if (Input.GetKeyDown(KeyCode.Minus))
		{
			MusicManager.VolumeDown();
			return true;
		}
		else if (Input.GetKeyDown(KeyCode.Equals))
		{
			MusicManager.VolumeUp();
			return true;
		}

		//Skybox controlls
		else if (Input.GetKeyDown(KeyCode.F3))
		{
			SkyboxManager.NextSkybox();
			return true;
		}

		return false;
	}

	bool TryCursorMovement()
	{
		float vert = 0.0f;
		float hori = 0.0f;
		bool goFurther = false;
		bool goCloser = false;

		if (GetGamepadType(0) == GamepadType.XBox360)
		{
			vert = CnInputManager.GetAxis("joystick 1 7th axis");
			hori = CnInputManager.GetAxis("joystick 1 6th axis");
			goFurther = CnInputManager.GetAxis("joystick 1 Y axis") * 100 * Time.deltaTime >= 1;
			goCloser = CnInputManager.GetAxis("joystick 1 Y axis") * 100 * Time.deltaTime <= -1;
		}
		else if (GetGamepadType(0) == GamepadType.PS4)
		{
			vert = CnInputManager.GetAxis("joystick 1 8th axis");
			hori = CnInputManager.GetAxis("joystick 1 7th axis");
			goFurther = Input.GetKeyDown(KeyCode.Joystick1Button7);
			goCloser = Input.GetKeyDown(KeyCode.Joystick1Button6);
		}

		bool altDown = Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt) || Input.GetKey(KeyCode.RightApple) || Input.GetKey(KeyCode.LeftApple);
		bool shiftDown = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
		bool ctrlDown = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightCommand);


		if (Input.GetKeyDown(KeyCode.Comma))
			goFurther = true;
		else if (Input.GetKeyDown(KeyCode.Period))
			goCloser = true;

		//Fast cursor movement
		if (Input.GetKeyDown(KeyCode.PageUp))
		{
			if (altDown)
			{
				Camera.RotateAroundPlatform(Vector3.up);
			}
			else
			{
				if (BlockManager.Cursor.transform.position.y < 60000f)
					BlockManager.Cursor.transform.position += new Vector3(0, 10, 0);
				return true;
			}
			return false;
		}
		else if (Input.GetKeyDown(KeyCode.PageDown))
		{
			if (BlockManager.Cursor.transform.position.y > -10)
				BlockManager.Cursor.transform.position += new Vector3(0, -10, 0);
			return true;
		}

		//Cursor/Camera reset
		else if (Input.GetKeyDown(KeyCode.F2))
		{
			Camera.ResetZoom();
			BlockManager.Cursor.transform.position = Vector3.zero;
			return true;
		}

		else if (Input.GetKeyDown(KeyCode.UpArrow) || vert > 0)
		{

			if (vert > 0 && period < JoystickThrottleRate)
			{
				period += Time.deltaTime;
				return false;
			}
			period = 0.0f;
			if (altDown)
			{
				BlockManager.RotatePlatform(90, 0, 0);
				return true;
			}
			else if (shiftDown)
			{
				GameObject ao = BlockManager.ActiveBlock?.gameObject;
				if (ao != null)
					ao.transform.Translate(0, 1, 0);
				BlockManager.Cursor.transform.position += new Vector3(0, 1, 0);
				return true;
			}
			else if (ctrlDown)
			{
				Camera.offset += new Vector3(0, 1, 0);
				return true;
			}
			else
			{
				if (BlockManager.Cursor.transform.position.y < 60000f)
					BlockManager.Cursor.transform.position += new Vector3(0, 1, 0);
				return true;
			}
		}
		else if (Input.GetKeyDown(KeyCode.DownArrow) || vert < 0)
		{
			if (vert < 0 && period < JoystickThrottleRate)
			{
				period += Time.deltaTime;
				return false;
			}
			period = 0.0f;
			if (altDown)
			{
				BlockManager.RotatePlatform(-90, 0, 0);
				return true;
			}
			else if (shiftDown)
			{
				GameObject ao = BlockManager.ActiveBlock?.gameObject;
				if (ao != null)
					ao.transform.Translate(0, -1, 0);
				BlockManager.Cursor.transform.position += new Vector3(0, -1, 0);
				return true;
			}
			else if (ctrlDown)
			{
				Camera.offset += new Vector3(0, -1, 0);
			}
			else
			{
				if (BlockManager.Cursor.transform.position.y > -10)
					BlockManager.Cursor.transform.position += new Vector3(0, -1, 0);
				return true;
			}
		}
		else if (Input.GetKeyDown(KeyCode.LeftArrow) || hori < 0)
		{
			if (hori < 0 && period < JoystickThrottleRate)
			{
				period += Time.deltaTime;
				return false;
			}
			period = 0.0f;
			if (altDown)
			{
				BlockManager.RotatePlatform(0, 90, 0);
				return true;
			}
			else if (shiftDown)
			{
				GameObject ao = BlockManager.ActiveBlock?.gameObject;
				if (ao != null)
					ao.transform.Translate(-1, 0, 0);
				BlockManager.Cursor.transform.position += new Vector3(-1, 0, 0);
				return true;
			}
			else if (ctrlDown)
			{
				Camera.offset += new Vector3(-1, 0, 0);
				return true;
			}
			else
			{
				BlockManager.Cursor.transform.position += new Vector3(-1, 0, 0);
				return true;
			}
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow) || hori > 0)
		{
			if (hori > 0 && period < JoystickThrottleRate)
			{
				period += Time.deltaTime;
				return false;
			}
			period = 0.0f;
			if (altDown && !BlockManager.PlayMode)
			{
				BlockManager.RotatePlatform(0, -90, 0);
				return true;
			}
			else if (shiftDown)
			{
				GameObject ao = BlockManager.ActiveBlock?.gameObject;
				if (ao != null)
					ao.transform.Translate(1, 0, 0);
				BlockManager.Cursor.transform.position += new Vector3(1, 0, 0);
				return true;
			}
			else if (ctrlDown)
			{
				Camera.offset += new Vector3(1, 0, 0);
				return true;
			}
			else
			{
				BlockManager.Cursor.transform.position += new Vector3(1, 0, 0);
				return true;
			}
		}
		else if (goFurther)
		{
			if (CnInputManager.GetAxis("joystick 1 Y axis") > 0 && period < JoystickThrottleRate)
			{
				period += Time.deltaTime;
				return false;
			}
			period = 0.0f;
			BlockManager.Cursor.transform.position += new Vector3(0, 0, -1);
			return true;
		}
		else if (goCloser)
		{
			if (CnInputManager.GetAxis("joystick 1 Y axis") < 0 && period < JoystickThrottleRate)
			{
				period += Time.deltaTime;
				return false;
			}
			period = 0.0f;
			BlockManager.Cursor.transform.position += new Vector3(0, 0, 1);
			return true;
		}
		return false;
	}

	bool TryCameraCommands()
	{
		float scroll = Input.GetAxis("Mouse ScrollWheel");
		if (scroll >= 0.005f || scroll <= -0.005f)
		{
			Camera.zoom += scroll * scrollSpeed;
			return true;
		}


		//Camera.transform.Translate(0, 0, scroll * scrollSpeed, Space.World);

#if (UNITY_ANDROID == false)
		if (Input.GetMouseButton(1))
		{
			rotationX += Input.GetAxis("Mouse X") * sensX * Time.deltaTime;
			rotationY += Input.GetAxis("Mouse Y") * sensY * Time.deltaTime;
			rotationY = Mathf.Clamp(rotationY, minY, maxY);
			Camera.transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
			return true;
		}

		float rotationXOffset = 0.0f;
		float rotationYOffset = 0.0f;
		string[] gamepadNames = Input.GetJoystickNames();
		if(gamepadNames.Length>0)
		{
			if(gamepadNames[0].Contains("360"))
			{
				rotationXOffset = Input.GetAxis("joystick 1 4th axis") * (sensX * 2) * Time.deltaTime;
				rotationYOffset = Input.GetAxis("joystick 1 5th axis") * (sensY * 2) * Time.deltaTime;
			}
			else if(gamepadNames[0].Contains("Wireless Controller"))
			{
				//rotationXOffset = Input.GetAxis("joystick 1 3rd axis") * (sensX * 2) * Time.deltaTime;
				//rotationYOffset = Input.GetAxis("joystick 1 6th axis") * (sensY * 2) * Time.deltaTime;
			}
		}
		if (rotationXOffset >= 0.05 || rotationYOffset >= 0.05 || rotationXOffset <= -0.05 || rotationYOffset <= -0.05)
		{
			rotationX += rotationXOffset;
			rotationY += rotationYOffset;
			rotationY = Mathf.Clamp(rotationY, minY, maxY);
			Camera.transform.localEulerAngles = new Vector3(rotationY, rotationX, 0);
			return true;
		}
#endif
		return false;
	}

	void Update()
	{
		if (UIManager.IsAnyInputDialogOpen)
			return;

		//Play Mode
		if (BlockManager.PlayMode)
		{
			if (TryPlayerInput())
				return;
			else if (Input.GetKeyDown(KeyCode.U))
			{
				Serializer.ToggleUndoEnabled();
				return;
			}
			else if (BlockManager.PlayMode && (Input.GetKeyDown(KeyCode.Backspace) || (Input.GetKeyDown(KeyCode.Z) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))))
			{
				Serializer.Undo();
				return;
			}
			else if (BlockManager.PlayMode && Input.GetKeyDown(KeyCode.Y) && ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))))                
			{
				Serializer.Redo();
				return;
			}
		}

		//Edit Mode
		else
		{
			if (TryCursorMovement())
				return;
			else if (TryCreateBlockAtCursor())
				return;
			else if (TryBlockCommands())
				return;
		}

		// Misc
		if (Input.GetKeyDown(KeyCode.P) || CnInputManager.GetButtonDown("Play"))
		{
			BlockManager.Instance.TogglePlayMode();
			return;
		}
		else if (TryAtmosphereCommands())
			return;
		else if (TryLevelOperationCommands())
			return;
		else if (TryUICommands())
			return;
		else if (TryCameraCommands())
			return;
	}
}
