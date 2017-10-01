using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[System.Serializable]
public class InputManager : MonoBehaviour {

	public GameObject Stage;
	public GameObject Cursor;
	public GameObject Camera;

	public BlockManager GetBlockManager() {
		return (BlockManager)Stage.GetComponent (typeof(BlockManager));
	}

	public GameObject GetActiveFloor() {
		return GetBlockManager ().ActiveFloor;
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

	void Update () {

		float vert = Input.GetAxis ("Vertical");
		float hori = Input.GetAxis ("Horizontal");

		bool altDown = Input.GetKey (KeyCode.LeftAlt) || Input.GetKey (KeyCode.RightAlt);

		float scroll = Input.GetAxis("Mouse ScrollWheel");
		Camera.transform.Translate(0, 0, scroll * scrollSpeed, Space.World);

		if (Input.GetMouseButton (0)) {
			rotationX += Input.GetAxis ("Mouse X") * sensX * Time.deltaTime;
			rotationY += Input.GetAxis ("Mouse Y") * sensY * Time.deltaTime;
			rotationY = Mathf.Clamp (rotationY, minY, maxY);
			Camera.transform.localEulerAngles = new Vector3 (-rotationY, rotationX, 0);
		}

		// Buttons for creating blocks
		if (Input.GetKeyDown (KeyCode.Alpha1) || Input.GetKeyDown (KeyCode.Joystick1Button0)) {
			GetBlockManager ().CreateBlockAtCursor (Cursor, Block.BlockType.Basic);
		} else if (Input.GetKeyDown (KeyCode.Alpha2) || Input.GetKeyDown (KeyCode.Joystick1Button1)) {
			GetBlockManager ().CreateBlockAtCursor (Cursor, Block.BlockType.Ice);
		} else if (Input.GetKeyDown (KeyCode.Alpha3) || Input.GetKeyDown (KeyCode.Joystick1Button2)) {
			GetBlockManager ().CreateBlockAtCursor (Cursor, Block.BlockType.Heavy1);
		} else if (Input.GetKeyDown (KeyCode.Alpha4) || Input.GetKeyDown (KeyCode.Joystick1Button3)) {
			GetBlockManager ().CreateBlockAtCursor (Cursor, Block.BlockType.Bomb1);
		} else if (Input.GetKeyDown (KeyCode.Alpha5)) {
			GetBlockManager ().CreateBlockAtCursor (Cursor, Block.BlockType.Bomb2);
		}
		// Save
		else if (Input.GetKeyDown (KeyCode.S)) {
//TODO WTF UNITY?!
#if UNITY_EDITOR
			string path = UnityEditor.EditorUtility.SaveFilePanel (
				              "Save level as JSON",
				              "",
				              "level.json",
				              ".json");
			if (path.Length != 0) {
				string json = GetBlockManager ().BlocksToJSON ();
				if (json.Length != 0)
					System.IO.File.WriteAllText (path, json);
			}
#endif
		}
		// Toggle block info display
		else if (Input.GetKeyDown (KeyCode.I)) {
			GetBlockManager ().ToggleBlockInfo ();
		}
		//Quit
		else if (Input.GetKeyDown (KeyCode.Escape) || Input.GetKeyDown (KeyCode.Q)) {
#if UNITY_EDITOR
			if (UnityEditor.EditorApplication.isPlaying)
				UnityEditor.EditorApplication.isPlaying = false;
			else
#endif
				Application.Quit ();
		}
			
		// Block type cycling
		else if (Input.GetKeyDown (KeyCode.Space) || Input.GetKeyDown (KeyCode.Joystick1Button0)) {
			GetBlockManager ().CreateBlockAtCursor (Cursor, GetBlockManager ().BlockCycleType);
		} else if (Input.GetKeyDown (KeyCode.LeftBracket)) {
			Block.BlockType type = GetBlockManager ().PrevBlockType ();
			if(GetBlockManager().ActiveObject != null)
				GetBlockManager ().CreateBlockAtCursor (Cursor, type);
		} else if (Input.GetKeyDown (KeyCode.RightBracket)) {
			Block.BlockType type = GetBlockManager ().NextBlockType ();
			if(GetBlockManager().ActiveObject != null)
				GetBlockManager ().CreateBlockAtCursor (Cursor, type);
		}

		// Destorying/modifying blocks
		else if (Input.GetKeyDown (KeyCode.Delete) || Input.GetKeyDown (KeyCode.Joystick1Button7)) {
			if (GetBlockManager ().ActiveObject != null)
				//TODO use a method of BlockManager to do this
				Destroy (GetBlockManager ().ActiveObject);
		} 

		// Buttons for setting items
		else if (Input.GetKeyDown (KeyCode.Home)  || Input.GetKeyDown(KeyCode.Joystick1Button6)) {
			if (GetBlockManager().ActiveObject != null) {
				//TODO use a method of BlockManager to do this
				Block block = (Block)GetBlockManager().ActiveObject.GetComponent (typeof(Block));
				block.Item = Instantiate (GetBlockManager().StartLocationIndicator, Cursor.transform.position + new Vector3 (0, 0.5F, 0), Quaternion.Euler (0, 180, 0)) as GameObject;
			}
		} else if (Input.GetKeyDown (KeyCode.End)) {
			if (GetBlockManager().ActiveObject != null) {
				//TODO use a method of BlockManager to do this
				Block block = (Block)GetBlockManager().ActiveObject.GetComponent (typeof(Block));
				block.Item = Instantiate (GetBlockManager().GoalLocationIndicator, Cursor.transform.position + new Vector3 (0, 0.5F, 0), Quaternion.Euler (0, 180, 0)) as GameObject;
			}
		}

		// Cursor/stage movement cotrol
		// Keyboard & XBox 360 Input
		// TODO update ActiveObject based on cursor position using colliders
		else if (Input.GetKeyDown (KeyCode.UpArrow) || vert > 0) {
			if (altDown) {
				GetBlockManager ().ActiveFloor.transform.Rotate (90, 0, 0);
				Cursor.transform.Rotate (90, 0, 0);
				Input.ResetInputAxes ();
			} else {
				Cursor.transform.position += new Vector3 (0, 1, 0);
				Input.ResetInputAxes ();
			}
		} else if (Input.GetKeyDown (KeyCode.DownArrow) || vert < 0) {
			if (altDown) {
				GetBlockManager ().ActiveFloor.transform.Rotate (-90, 0, 0);
				Cursor.transform.Rotate (90, 0, 0);
				Input.ResetInputAxes ();
			} else {
				Cursor.transform.position += new Vector3 (0, -1, 0);
				Input.ResetInputAxes ();
			}
		} else if (Input.GetKeyDown (KeyCode.LeftArrow) || hori < 0) {
			if (altDown) {
				GetBlockManager ().ActiveFloor.transform.Rotate (0, 90, 0);
				Cursor.transform.Rotate (90, 0, 0);
				Input.ResetInputAxes ();
			} else {
				Cursor.transform.position += new Vector3 (-1, 0, 0);
				Input.ResetInputAxes ();
			}
		} else if (Input.GetKeyDown (KeyCode.RightArrow) || hori > 0) {
			if (altDown) {
				GetBlockManager ().ActiveFloor.transform.Rotate (0, -90, 0);
				Cursor.transform.Rotate (90, 0, 0);
				Input.ResetInputAxes ();
			} else {
				Cursor.transform.position += new Vector3 (1, 0, 0);
				Input.ResetInputAxes ();
			}
		} else if (Input.GetKeyDown (KeyCode.Comma) || Input.GetKeyDown(KeyCode.Joystick1Button4)) {
			Input.ResetInputAxes ();
			Cursor.transform.position += new Vector3 (0, 0, -1);
		} else if (Input.GetKeyDown (KeyCode.Period) || Input.GetKeyDown(KeyCode.Joystick8Button5)) {
			Input.ResetInputAxes ();
			Cursor.transform.position += new Vector3 (0, 0, 1);
		}
	}
}
