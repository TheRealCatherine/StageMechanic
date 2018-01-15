/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System.Collections.Generic;
using UnityEngine;
using System;
using CnControls;

[System.Serializable]
public class InputManager : MonoBehaviour {

	public GameObject Cursor;
	public CameraController Camera;

	internal const float scrollSpeed = 2.0f;

	public float minX = -360.0f;
	public float maxX = 360.0f;

	public float minY = -45.0f;
	public float maxY = 45.0f;

	public float sensX = 100.0f;
	public float sensY = 100.0f;

	float rotationY = 0.0f;
	float rotationX = 0.0f;

	float period = 0.0f;
	const float joystickThrottleRate = 0.1f;

    /// <summary>
    /// Check to see if any current user input matches block creation commands for edit mode
    /// </summary>
    /// <returns>true if user input was consumed</returns>
    /// TODO: don't hardcode block types or buttons
    bool TryCreateBlockAtCursor()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Joystick1Button3))
        {
            BlockManager.CreateBlockAtCursor(Cathy1Block.BlockType.Basic);
            return true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            BlockManager.CreateBlockAtCursor(Cathy1Block.BlockType.Immobile);
            return true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            BlockManager.CreateBlockAtCursor(Cathy1Block.BlockType.Crack2);
            return true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            BlockManager.CreateBlockAtCursor(Cathy1Block.BlockType.Heavy);
            return true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            BlockManager.CreateBlockAtCursor(Cathy1Block.BlockType.SpikeTrap);
            return true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            BlockManager.CreateBlockAtCursor(Cathy1Block.BlockType.Ice);
            return true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            BlockManager.CreateBlockAtCursor(Cathy1Block.BlockType.Bomb1);
            return true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            BlockManager.CreateBlockAtCursor(Cathy1Block.BlockType.Random);
            return true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            BlockManager.CreateBlockAtCursor(Cathy1Block.BlockType.Monster);
            return true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            BlockManager.CreateBlockAtCursor(Cathy1Block.BlockType.Vortex);
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
                    BlockManager.Instance.SaveToJson();
                else
                    BlockManager.Instance.QuickSave();

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
            BlockManager.Instance.LoadFromJson();
            return true;
        }

        // Reload current level
        else if (Input.GetKeyDown(KeyCode.F5))
        {
            BlockManager.Instance.ReloadCurrentLevel();
            return true;
        }

        // Clear all blocks
        else if (Input.GetKeyDown(KeyCode.Delete) && ctrlDown)
        {
            BlockManager.Instance.Clear();
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
        else if (Input.GetKeyDown(KeyCode.Joystick1Button6))
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
        bool altDown = Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt) || Input.GetKey(KeyCode.RightApple) || Input.GetKey(KeyCode.LeftApple);

        // Block type cycling
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button0) || CnInputManager.GetButtonDown("Grab"))
        {
            if (!BlockManager.PlayMode)
            {
                BlockManager.CreateBlockAtCursor(BlockManager.Instance.BlockCycleType);
                return true;
            }
            
        }
        else if (Input.GetKeyDown(KeyCode.LeftBracket) || Input.GetKeyDown(KeyCode.Joystick1Button4) || CnInputManager.GetButtonDown("Previous"))
        {
            //TODO generic
            Cathy1Block.BlockType type = BlockManager.Instance.PrevBlockType();
            if (BlockManager.Instance.ActiveObject != null)
                BlockManager.CreateBlockAtCursor(type);
            return true;
        }
        else if (Input.GetKeyDown(KeyCode.RightBracket) || Input.GetKeyDown(KeyCode.Joystick1Button5) || CnInputManager.GetButtonDown("Next"))
        {
            //TODO generic
            Cathy1Block.BlockType type = BlockManager.Instance.NextBlockType();
            if (BlockManager.Instance.ActiveObject != null)
                BlockManager.CreateBlockAtCursor(type);
            return true;
        }

        // Randomize gravity
        else if (Input.GetKeyDown(KeyCode.G) && altDown)
        {
            BlockManager.Instance.RandomizeGravity();
            return true;
        }

        // Destorying/modifying blocks
        else if (Input.GetKeyDown(KeyCode.Delete) || Input.GetKeyDown(KeyCode.Joystick1Button1) || CnInputManager.GetButtonDown("Delete"))
        {
            BlockManager.Instance.DestroyActiveObject();
        }
        else if (Input.GetKeyDown(KeyCode.B) || CnInputManager.GetButtonDown("StartPosition"))
        {
            GetComponent<EventManager>().CreatePlayerStartLocation(0, Cursor.transform.position, Cursor.transform.rotation);
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            GetComponent<EventManager>().CreatePlayerStartLocation(1, Cursor.transform.position, Cursor.transform.rotation);
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            GetComponent<EventManager>().CreatePlayerStartLocation(2, Cursor.transform.position, Cursor.transform.rotation);
        }


        return false;
    }

    bool TryPlayerInput()
    {
        if (period < joystickThrottleRate)
        {
            period += Time.deltaTime;
            return false;
        }

        List<string> axees = new List<string>();
        if (CnInputManager.GetAxis("joystick 1 7th axis") > 0f)
            axees.Add("joystick 1 7th axis +");
        else if (CnInputManager.GetAxis("joystick 1 7th axis") < 0f)
            axees.Add("joystick 1 7th axis -");
        if (CnInputManager.GetAxis("joystick 1 6th axis") > 0f)
            axees.Add("joystick 1 6th axis +");
        else if (CnInputManager.GetAxis("joystick 1 6th axis") < 0f)
            axees.Add("joystick 1 6th axis -");
        if (CnInputManager.GetAxis("joystick 1 Y axis") > 0)
            axees.Add("joystick 1 Y axis -");
        else if (CnInputManager.GetAxis("joystick 1 Y axis") < 0)
            axees.Add("joystick 1 Y axis +");
        if (CnInputManager.GetAxis("joystick 1 X axis") > 0)
            axees.Add("joystick 1 X axis +");
        else if (CnInputManager.GetAxis("joystick 1 X axis") < 0)
            axees.Add("joystick 1 X axis -");

        bool usedAtLeastOne = false;
        for (int playerNumber = 0; playerNumber < PlayerManager.PlayerCount(); ++playerNumber)
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
                                    Debug.Log(item.Key + " " + key);
                                }
                            }
                            else if (((CnInputManager.ButtonExists(key) && CnInputManager.GetButton(key)) || (!CnInputManager.ButtonExists(key) && (Input.GetKey(key)))) && !inputs.Contains(item.Key))
                            {
                                inputs.Add(item.Key);
                                Debug.Log(item.Key + " " + key);
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
                    period = joystickThrottleRate - time;
                    usedAtLeastOne = true;
                }
            }
        }
        return usedAtLeastOne;
    }

    void Update() {

        if (UIManager.IsAnyInputDialogOpen)
            return;

        float vert = CnInputManager.GetAxis("joystick 1 7th axis");
        float hori = CnInputManager.GetAxis("joystick 1 6th axis");

        bool altDown = Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt) || Input.GetKey(KeyCode.RightApple) || Input.GetKey(KeyCode.LeftApple);
        bool shiftDown = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) || (BlockManager.PlayMode && Input.GetKey(KeyCode.JoystickButton0));
        bool ctrlDown = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightCommand);

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll >= 0.005f || scroll <= -0.005f)
            Camera.zoom += scroll * scrollSpeed;
        //Camera.transform.Translate(0, 0, scroll * scrollSpeed, Space.World);

        if (Input.GetMouseButton(1)) {
            rotationX += Input.GetAxis("Mouse X") * sensX * Time.deltaTime;
            rotationY += Input.GetAxis("Mouse Y") * sensY * Time.deltaTime;
            rotationY = Mathf.Clamp(rotationY, minY, maxY);
            Camera.transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        }

        float rotationXOffset = Input.GetAxis("joystick 1 4th axis") * (sensX * 2) * Time.deltaTime;
        float rotationYOffset = Input.GetAxis("joystick 1 5th axis") * (sensY * 2) * Time.deltaTime;
        if (rotationXOffset >= 0.05 || rotationYOffset >= 0.05 || rotationXOffset <= -0.05 || rotationYOffset <= -0.05) {
            rotationX += rotationXOffset;
            rotationY += rotationYOffset;
            rotationY = Mathf.Clamp(rotationY, minY, maxY);
            //Camera.transform.localEulerAngles = new Vector3 (rotationY, rotationX, 0);
        }

        bool goFurther = CnInputManager.GetAxis("joystick 1 Y axis") * 100 * Time.deltaTime >= 1;
        bool goCloser = CnInputManager.GetAxis("joystick 1 Y axis") * 100 * Time.deltaTime <= -1;

        if (Input.GetKeyDown(KeyCode.Comma) && !BlockManager.PlayMode)
            goFurther = true;
        else if (Input.GetKeyDown(KeyCode.Period) && !BlockManager.PlayMode)
            goCloser = true;

        //Play Mode
        if (BlockManager.PlayMode)
        {
            if (TryPlayerInput())
                return;
            else if (Input.GetKeyDown(KeyCode.U))
            {
                BlockManager.ToggleUndoOn();
                return;
            }
            else if (Input.GetKeyDown(KeyCode.Backspace) && BlockManager.PlayMode)
            {
                BlockManager.Undo();
                return;
            }

        }

        //Edit Mode
        else
        {
            if (TryCreateBlockAtCursor())
                return;
            else if (TryBlockCommands())
                return;

            
        }

        if (TryLevelOperationCommands())
            return;
        else if (TryUICommands())
            return;




        //Play mode
        else if (Input.GetKeyDown(KeyCode.P) || CnInputManager.GetButtonDown("Play"))
        {
            BlockManager.Instance.TogglePlayMode();
        }
        //Quit
        

        //Manually move the platform
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
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
        }


        // Buttons for setting items
        else if (Input.GetKeyDown(KeyCode.Home) || Input.GetKeyDown(KeyCode.Joystick1Button2))
        {
            if (BlockManager.Instance.ActiveObject != null)
            {
                //TODO use a method of BlockManager to do this
                Cathy1Block block = BlockManager.Instance.ActiveObject.GetComponent<Cathy1Block>();
                block.FirstItem = Instantiate(BlockManager.Instance.StartLocationIndicator, Cursor.transform.position + new Vector3(0, 0.5F, 0), Quaternion.Euler(0, 180, 0)) as GameObject;
            }
        }
        else if (Input.GetKeyDown(KeyCode.End))
        {
            if (BlockManager.Instance.ActiveObject != null)
            {
                //TODO use a method of BlockManager to do this
                Cathy1Block block = BlockManager.Instance.ActiveObject.GetComponent<Cathy1Block>();
                block.FirstItem = Instantiate(BlockManager.Instance.GoalLocationIndicator, Cursor.transform.position + new Vector3(0, 0.5F, 0), Quaternion.Euler(0, 180, 0)) as GameObject;
            }
        }

        //Music controlls
        else if (Input.GetKeyDown(KeyCode.F11))
        {
            MusicManager.TogglePause();
        }
        else if (Input.GetKeyDown(KeyCode.F10))
        {
            MusicManager.PlayPreviousTrack();
        }
        else if (Input.GetKeyDown(KeyCode.F12))
        {
            MusicManager.PlayNextTrack();
        }
        else if (Input.GetKeyDown(KeyCode.Minus))
        {
            MusicManager.VolumeDown();
        }
        else if (Input.GetKeyDown(KeyCode.Equals))
        {
            MusicManager.VolumeUp();
        }

        //Skybox controlls
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            SkyboxManager.NextSkybox();
        }

        //Fast cursor movement
        else if (Input.GetKeyDown(KeyCode.PageUp) && !BlockManager.PlayMode)
        {
            Cursor.transform.position += new Vector3(0, 10, 0);
        }
        else if (Input.GetKeyDown(KeyCode.PageDown) && !BlockManager.PlayMode)
        {
            Cursor.transform.position += new Vector3(0, -10, 0);
        }

        //Cursor/Camera reset
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            Camera.ResetZoom();
            if (!BlockManager.PlayMode)
                Cursor.transform.position = Vector3.zero;
        }

        

        // Cursor/stage movement cotrol
        // Keyboard & XBox 360 Input
        // TODO update ActiveObject based on cursor position using colliders
        else if (Input.GetKeyDown(KeyCode.UpArrow) || vert > 0)
        {
            List<string> player1Inputs = new List<string>();
            if (shiftDown)
                player1Inputs.Add("Grab");

            if (vert > 0 && period < joystickThrottleRate)
            {
                period += Time.deltaTime;
                return;
            }
            period = 0.0f;
            if (altDown && !BlockManager.PlayMode)
            {
                BlockManager.ActiveFloor.transform.Rotate(90, 0, 0, Space.Self);
                Cursor.transform.Rotate(90, 0, 0, Space.Self);
            }
            else if (shiftDown && !BlockManager.PlayMode)
            {
                GameObject ao = BlockManager.Instance.ActiveObject;
                if (ao != null)
                    ao.transform.Translate(0, 1, 0);
                Cursor.transform.position += new Vector3(0, 1, 0);
            }
            else if (ctrlDown && !BlockManager.PlayMode)
            {
                Camera.offset += new Vector3(0, 1, 0);
            }
            else
            {
                if (!BlockManager.PlayMode)
                    Cursor.transform.position += new Vector3(0, 1, 0);
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || vert < 0)
        {
            if (vert < 0 && period < joystickThrottleRate)
            {
                period += Time.deltaTime;
                return;
            }
            period = 0.0f;
            if (altDown && !BlockManager.PlayMode)
            {
                BlockManager.ActiveFloor.transform.Rotate(-90, 0, 0, Space.Self);
                Cursor.transform.Rotate(-90, 0, 0, Space.Self);
            }
            else if (shiftDown && !BlockManager.PlayMode)
            {
                GameObject ao = BlockManager.Instance.ActiveObject;
                if (ao != null)
                    ao.transform.Translate(0, -1, 0);
                Cursor.transform.position += new Vector3(0, -1, 0);
            }
            else if (ctrlDown && !BlockManager.PlayMode)
            {
                Camera.offset += new Vector3(0, -1, 0);
            }
            else
            {
                if (!BlockManager.PlayMode)
                    Cursor.transform.position += new Vector3(0, -1, 0);
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || hori < 0)
        {
            if (hori < 0 && period < joystickThrottleRate)
            {
                period += Time.deltaTime;
                return;
            }
            period = 0.0f;
            if (altDown && !BlockManager.PlayMode)
            {
                BlockManager.ActiveFloor.transform.Rotate(0, 90, 0, Space.Self);
                Cursor.transform.Rotate(0, 90, 0, Space.Self);
            }
            else if (shiftDown && !BlockManager.PlayMode)
            {
                GameObject ao = BlockManager.Instance.ActiveObject;
                if (ao != null)
                    ao.transform.Translate(-1, 0, 0);
                Cursor.transform.position += new Vector3(-1, 0, 0);
            }
            else if (ctrlDown && !BlockManager.PlayMode)
            {
                Camera.offset += new Vector3(-1, 0, 0);
            }
            else
            {
                if (!BlockManager.PlayMode)
                    Cursor.transform.position += new Vector3(-1, 0, 0);
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || hori > 0)
        {
            if (hori > 0 && period < joystickThrottleRate)
            {
                period += Time.deltaTime;
                return;
            }
            period = 0.0f;
            if (altDown && !BlockManager.PlayMode)
            {
                BlockManager.ActiveFloor.transform.Rotate(0, -90, 0, Space.Self);
                Cursor.transform.Rotate(0, -90, 0, Space.Self);
            }
            else if (shiftDown && !BlockManager.PlayMode)
            {
                GameObject ao = BlockManager.Instance.ActiveObject;
                if (ao != null)
                    ao.transform.Translate(1, 0, 0);
                Cursor.transform.position += new Vector3(1, 0, 0);
            }
            else if (ctrlDown && !BlockManager.PlayMode)
            {
                Camera.offset += new Vector3(1, 0, 0);
            }
            else
            {
                if (!BlockManager.PlayMode)
                    Cursor.transform.position += new Vector3(1, 0, 0);
            }
        }
        else if (goFurther)
        {
            if (CnInputManager.GetAxis("joystick 1 Y axis") > 0 && period < joystickThrottleRate)
            {
                period += Time.deltaTime;
                return;
            }
            period = 0.0f;
            Cursor.transform.position += new Vector3(0, 0, -1);
        }
        else if (goCloser)
        {
            if (CnInputManager.GetAxis("joystick 1 Y axis") < 0 && period < joystickThrottleRate)
            {
                period += Time.deltaTime;
                return;
            }
            period = 0.0f;
            Cursor.transform.position += new Vector3(0, 0, 1);
        }
	}
}
