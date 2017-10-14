/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using System;

[System.Serializable]
public class InputManager : MonoBehaviour {

	public GameObject Stage;
	public GameObject Cursor;
	public GameObject Camera;
    public BlockInfoBoxController BlockInfoBox;

	public BlockManager GetBlockManager() {
		return Stage.GetComponent<BlockManager>();
	}

	public GameObject GetActiveFloor() {
		return GetBlockManager ().ActiveFloor;
	}

	public Camera GetCamera() {
		return Camera.GetComponent<Camera>();
	}

	// Use this for initialization
	void Start () {
	}

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

	void Update () {

		float vert = Input.GetAxis ("Vertical");
		float hori = Input.GetAxis ("Horizontal");

		bool altDown = Input.GetKey (KeyCode.LeftAlt) || Input.GetKey (KeyCode.RightAlt) || Input.GetKey(KeyCode.RightApple) || Input.GetKey(KeyCode.LeftApple);
		bool shiftDown = Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift);
		bool ctrlDown = Input.GetKey (KeyCode.LeftControl) || Input.GetKey (KeyCode.RightControl) || Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightCommand);

		float scroll = Input.GetAxis("Mouse ScrollWheel");
		if(scroll >= 0.005f || scroll <= -0.005f)
			Camera.transform.Translate(0, 0, scroll * scrollSpeed, Space.World);

		if (Input.GetMouseButton (1)) {
			rotationX += Input.GetAxis ("Mouse X") * sensX * Time.deltaTime;
			rotationY += Input.GetAxis ("Mouse Y") * sensY * Time.deltaTime;
			rotationY = Mathf.Clamp (rotationY, minY, maxY);
			Camera.transform.localEulerAngles = new Vector3 (-rotationY, rotationX, 0);
		}

		float rotationXOffset = Input.GetAxis ("RightStickH") * (sensX*2) * Time.deltaTime;
		float rotationYOffset = Input.GetAxis ("RightStickV") * (sensY*2) * Time.deltaTime;
		if (rotationXOffset >= 0.05 || rotationYOffset >= 0.05 || rotationXOffset <= -0.05 || rotationYOffset <= -0.05) {
			rotationX += rotationXOffset;
			rotationY += rotationYOffset;
			rotationY = Mathf.Clamp (rotationY, minY, maxY);
			Camera.transform.localEulerAngles = new Vector3 (rotationY, rotationX, 0);
		}

		bool goFurther = Input.GetAxis ("LeftStickV") * 100  * Time.deltaTime >= 1;
		bool goCloser = Input.GetAxis ("LeftStickV") * 100  * Time.deltaTime <= -1;

		// Buttons for creating blocks
		if (Input.GetKeyDown (KeyCode.Alpha1) || Input.GetKeyDown (KeyCode.Joystick1Button3)) {
			GetBlockManager ().Cathy1BlockFactory.CreateBlock(Cursor.transform.position,Cursor.transform.rotation, Cathy1Block.BlockType.Basic, GetActiveFloor());
		} else if (Input.GetKeyDown (KeyCode.Alpha2)) {
			GetBlockManager ().Cathy1BlockFactory.CreateBlock (Cursor.transform.position, Cursor.transform.rotation, Cathy1Block.BlockType.Crack2, GetActiveFloor());
		} else if (Input.GetKeyDown (KeyCode.Alpha3)) {
			GetBlockManager ().Cathy1BlockFactory.CreateBlock (Cursor.transform.position, Cursor.transform.rotation, Cathy1Block.BlockType.Heavy, GetActiveFloor());
		} else if (Input.GetKeyDown (KeyCode.Alpha4)) {
			GetBlockManager ().Cathy1BlockFactory.CreateBlock (Cursor.transform.position, Cursor.transform.rotation, Cathy1Block.BlockType.Ice, GetActiveFloor());
		} else if (Input.GetKeyDown (KeyCode.Alpha5)) {
			GetBlockManager ().Cathy1BlockFactory.CreateBlock (Cursor.transform.position, Cursor.transform.rotation, Cathy1Block.BlockType.Bomb1, GetActiveFloor());
		}
		// Save
		else if (Input.GetKeyDown (KeyCode.S)) {
			GetBlockManager ().SaveToJson ();
		}

		else if (Input.GetKeyDown (KeyCode.L)) {
			GetBlockManager ().LoadFromJson ();
		}
		// Toggle block info display
		else if (Input.GetKeyDown (KeyCode.I) || Input.GetKeyDown (KeyCode.Joystick1Button6)) {
            BlockInfoBox.ToggleVisibility();
        }
        else if(Input.GetKeyDown(KeyCode.P))
        {
            GetBlockManager().PlayMode = !GetBlockManager().PlayMode;
        }
		//Quit
		else if (Input.GetKeyDown (KeyCode.Escape) || Input.GetKeyDown (KeyCode.Q)) {
            #if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
            #else
				Application.Quit ();
            #endif
		}
			
		// Block type cycling
		else if (Input.GetKeyDown (KeyCode.Space) || Input.GetKeyDown (KeyCode.Joystick1Button0)) {
			GetBlockManager ().CreateBlockAtCursor ( GetBlockManager ().BlockCycleType);
		} else if (Input.GetKeyDown (KeyCode.LeftBracket) || Input.GetKeyDown (KeyCode.Joystick1Button4)) {
            //TODO generic
            Cathy1Block.BlockType type = GetBlockManager ().PrevBlockType ();
			if(GetBlockManager().ActiveObject != null)
				GetBlockManager ().CreateBlockAtCursor (type);
		} else if (Input.GetKeyDown (KeyCode.RightBracket)  || Input.GetKeyDown (KeyCode.Joystick1Button5)) {
            //TODO generic
            Cathy1Block.BlockType type = GetBlockManager ().NextBlockType ();
			if(GetBlockManager().ActiveObject != null)
				GetBlockManager ().CreateBlockAtCursor (type);
		}

		// Destorying/modifying blocks
		else if (Input.GetKeyDown (KeyCode.Delete) || Input.GetKeyDown (KeyCode.Joystick1Button1)) {
			if (GetBlockManager ().ActiveObject != null)
				//TODO use a method of BlockManager to do this
				Destroy (GetBlockManager ().ActiveObject);
		} 

		// Buttons for setting items
		else if (Input.GetKeyDown (KeyCode.Home)  || Input.GetKeyDown(KeyCode.Joystick1Button2)) {
			if (GetBlockManager().ActiveObject != null) {
				//TODO use a method of BlockManager to do this
				Cathy1Block block = GetBlockManager().ActiveObject.GetComponent<Cathy1Block>();
				block.FirstItem = Instantiate (GetBlockManager().StartLocationIndicator, Cursor.transform.position + new Vector3 (0, 0.5F, 0), Quaternion.Euler (0, 180, 0)) as GameObject;
			}
		} else if (Input.GetKeyDown (KeyCode.End)) {
			if (GetBlockManager().ActiveObject != null) {
				//TODO use a method of BlockManager to do this
				Cathy1Block block = GetBlockManager().ActiveObject.GetComponent<Cathy1Block>();
				block.FirstItem = Instantiate (GetBlockManager().GoalLocationIndicator, Cursor.transform.position + new Vector3 (0, 0.5F, 0), Quaternion.Euler (0, 180, 0)) as GameObject;
			}
		}

		// Cursor/stage movement cotrol
		// Keyboard & XBox 360 Input
		// TODO update ActiveObject based on cursor position using colliders
		else if (Input.GetKeyDown (KeyCode.UpArrow) || vert > 0) {
			if (period < joystickThrottleRate) {
				period += Time.deltaTime;
				return;
			}
			period = 0.0f;
			if (altDown) {
				GetBlockManager ().ActiveFloor.transform.Rotate (90, 0, 0, Space.Self);
				Cursor.transform.Rotate (90, 0, 0, Space.Self);
			} else if (shiftDown) {
				GameObject ao = GetBlockManager ().ActiveObject;
				if (ao != null)
					ao.transform.Translate (0, 1, 0);
				Cursor.transform.position += new Vector3 (0, 1, 0);
			} else if (ctrlDown) {
				GetCamera().offset += new Vector3 (0, 1, 0);
			} else {
				Cursor.transform.position += new Vector3 (0, 1, 0);
			}
			Input.ResetInputAxes ();
		} else if (Input.GetKeyDown (KeyCode.DownArrow) || vert < 0) {
			if (period < joystickThrottleRate) {
				period += Time.deltaTime;
				return;
			}
			period = 0.0f;
			if (altDown) {
				GetBlockManager ().ActiveFloor.transform.Rotate (-90, 0, 0, Space.Self);
				Cursor.transform.Rotate (-90, 0, 0, Space.Self);
			} else if(shiftDown) {
				GameObject ao = GetBlockManager ().ActiveObject;
				if (ao != null)
					ao.transform.Translate (0, -1, 0);
				Cursor.transform.position += new Vector3 (0, -1, 0);
			} else if (ctrlDown) {
				GetCamera().offset += new Vector3 (0, -1, 0);
			} else {
				Cursor.transform.position += new Vector3 (0, -1, 0);
			}
			Input.ResetInputAxes ();
		} else if (Input.GetKeyDown (KeyCode.LeftArrow) || hori < 0) {
			if (period < joystickThrottleRate) {
				period += Time.deltaTime;
				return;
			}
			period = 0.0f;
			if (altDown) {
				GetBlockManager ().ActiveFloor.transform.Rotate (0, 90, 0, Space.Self);
				Cursor.transform.Rotate (0, 90, 0, Space.Self);
			} else if(shiftDown) {
				GameObject ao = GetBlockManager ().ActiveObject;
				if (ao != null)
					ao.transform.Translate (-1, 0, 0);
				Cursor.transform.position += new Vector3 (-1, 0, 0);
			} else if (ctrlDown) {
				GetCamera().offset += new Vector3 (-1, 0, 0);
			} else {
				Cursor.transform.position += new Vector3 (-1, 0, 0);
			}
			Input.ResetInputAxes ();
		} else if (Input.GetKeyDown (KeyCode.RightArrow) || hori > 0) {
			if (period < joystickThrottleRate) {
				period += Time.deltaTime;
				return;
			}
			period = 0.0f;
			if (altDown) {
				GetBlockManager ().ActiveFloor.transform.Rotate (0, -90, 0, Space.Self);
				Cursor.transform.Rotate (0, -90, 0, Space.Self);
			} else if(shiftDown) {
				GameObject ao = GetBlockManager ().ActiveObject;
				if (ao != null)
					ao.transform.Translate (1, 0, 0);
				Cursor.transform.position += new Vector3 (1, 0, 0);
			} else if (ctrlDown) {
				GetCamera().offset += new Vector3 (1, 0, 0);
			} else {
				Cursor.transform.position += new Vector3 (1, 0, 0);
			}
			Input.ResetInputAxes ();
		} else if (goFurther) {
			if (period < joystickThrottleRate) {
				period += Time.deltaTime;
				return;
			}
			period = 0.0f;
			Cursor.transform.position += new Vector3 (0, 0, -1);
			Input.ResetInputAxes ();
		} else if (goCloser) {
			if (period < joystickThrottleRate) {
				period += Time.deltaTime;
				return;
			}
			period = 0.0f;
			Cursor.transform.position += new Vector3 (0, 0, 1);
			Input.ResetInputAxes ();
		}
	}
}
