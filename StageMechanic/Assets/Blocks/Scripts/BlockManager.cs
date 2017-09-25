/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour {

	// Unity Inspector variables

	public GameObject BlockPrefab;
	public GameObject CursorPrefab;
	public GameObject StartLocationIndicator;
	public Material IceBlockMaterial;
	public Material Bomb1Material;
	public Material HeavyBlockMaterial;



	// Properties

	// The obect (block/item/etc) currently under the cursor
	public GameObject ActiveObject {
		get {
			CursorCollider col = (CursorCollider)Cursor.GetComponent (typeof(CursorCollider));
			return col.ObjectUnderCursor;
		}
		set {
			CursorCollider col = (CursorCollider)Cursor.GetComponent (typeof(CursorCollider));
			col.ObjectUnderCursor=value;
		}
	}

	// The cursor object
	private GameObject _cursor;
	public GameObject Cursor {
		get {
			return _cursor;
		}
		set {
			_cursor = value;
		}
	}

	// Called when the BlockManager is intantiated, when the Level Editor is loaded
	void Start() {
        // Create the cursor

        //because you told me to
        //Cursor = Instantiate (CursorPrefab, transform.position, transform.rotation) as GameObject;
        Cursor = CursorPrefab;

		Cursor.transform.SetParent (transform, false);
	}

	// Called once every frame
	void Update() {

		float vert = Input.GetAxis ("Vertical");
		float hori = Input.GetAxis ("Horizontal");

		// Buttons for creating blocks
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			CreateBlockAtCursor (Block.BlockType.Basic);
		} else if (Input.GetKeyDown (KeyCode.Alpha2)) {
			GameObject newBlock = CreateBlockAtCursor ();
			SetMaterial (newBlock, IceBlockMaterial);
		} else if (Input.GetKeyDown (KeyCode.Alpha3)) {
			GameObject newBlock = CreateBlockAtCursor ();
			SetMaterial (newBlock, HeavyBlockMaterial);
		} else if (Input.GetKeyDown (KeyCode.Alpha4)) {
			GameObject newBlock = CreateBlockAtCursor ();
			SetMaterial (newBlock, Bomb1Material);
		} else if (Input.GetKeyDown (KeyCode.Delete)) {
			if(ActiveObject != null)
				Destroy (ActiveObject);
		} else if (Input.GetKeyDown (KeyCode.Home)) {
			if (ActiveObject != null) {
				Block block = (Block)ActiveObject.GetComponent (typeof(Block));
				block.Item = Instantiate (StartLocationIndicator, Cursor.transform.position + new Vector3 (0, 0.5F, 0), Quaternion.Euler (0, 180, 0)) as GameObject;
			}
		}

		// Cursor movement cotrol
		// Keyboard & XBox 360 Input
		// TODO update ActiveObject based on cursor position using colliders
		else if (Input.GetKeyDown (KeyCode.UpArrow) || vert > 0) {
			Cursor.transform.position += new Vector3 (0, 1, 0);
			Input.ResetInputAxes ();
		} else if (Input.GetKeyDown (KeyCode.DownArrow) || vert < 0) {
			Cursor.transform.position += new Vector3 (0, -1, 0);
			Input.ResetInputAxes ();
		} else if (Input.GetKeyDown (KeyCode.LeftArrow) || hori < 0) {
			Cursor.transform.position += new Vector3 (-1, 0, 0);
			Input.ResetInputAxes ();
		} else if (Input.GetKeyDown (KeyCode.RightArrow) || hori > 0) {
			Cursor.transform.position += new Vector3 (1, 0, 0);
			Input.ResetInputAxes ();
		} else if (Input.GetKeyDown (KeyCode.Comma) || Input.GetKeyDown(KeyCode.Joystick1Button4)) {
			Input.ResetInputAxes ();
			Cursor.transform.position += new Vector3 (0, 0, -1);
		} else if (Input.GetKeyDown (KeyCode.Period) || Input.GetKeyDown(KeyCode.Joystick8Button5)) {
			Input.ResetInputAxes ();
			Cursor.transform.position += new Vector3 (0, 0, 1);
		}
	}

	// Retrieve a List of all child game objects from a given parent
	static List<GameObject> GetChildren(GameObject obj) {
		List<GameObject> list = new List<GameObject>();
		foreach (Transform child in obj.transform)
			list.Add (child.gameObject);
		return list;
	}

	// Create a basic block at the current cursor position
	GameObject CreateBlockAtCursor( Block.BlockType type = Block.BlockType.Custom ) {
		// If there is a block currently under the cursor destroy it.
		if (ActiveObject != null)
			Destroy (ActiveObject);

		GameObject newBlock = null;

		switch (type) {
		case Block.BlockType.Basic:
			newBlock = Instantiate (BlockPrefab, Cursor.transform.position, Cursor.transform.rotation) as GameObject;
			break;
		default:
			//Create a new block at the cursor position and set it as the active game block
			newBlock = Instantiate (BlockPrefab, Cursor.transform.position, Cursor.transform.rotation) as GameObject;
			break;
		}

		Debug.Assert (newBlock != null);

        //line giving an issue creating a block AROUND the cursor instead of in the world
		newBlock.transform.SetParent (transform, false);

		ActiveObject = newBlock;
		return newBlock;
	}

	// Sets the material for a block
	void SetMaterial( GameObject block, Material material ) {
		Renderer rend = block.GetComponent<Renderer> ();
		rend.material = material;
	}
}
