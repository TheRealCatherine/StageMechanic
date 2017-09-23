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
	public Material IceBlockMaterial;
	public Material Bomb1Material;
	public Material HeavyBlockMaterial;

	// Properties

	// The obect (block/item/etc) currently under the cursor
	private GameObject _activeObject;
	public GameObject ActiveObject {
		get {
			return _activeObject;
		}
		set {
			_activeObject = value;
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

		// Buttons for creating blocks
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			GameObject newBlock = CreateBlockAtCursor ();

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
			Destroy (ActiveObject);
		}

		// Cursor movement cotrol
		// Keyboard
		// TODO update ActiveObject based on cursor position using colliders
		else if (Input.GetKeyDown (KeyCode.UpArrow)) {
			Cursor.transform.position += new Vector3 (0, 1, 0);
		} else if (Input.GetKeyDown (KeyCode.DownArrow)) {
			Cursor.transform.position += new Vector3 (0, -1, 0);
		} else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			Cursor.transform.position += new Vector3 (-1, 0, 0);
		} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
			Cursor.transform.position += new Vector3 (1, 0, 0);
		} else if (Input.GetKeyDown (KeyCode.Comma)) {
			Cursor.transform.position += new Vector3 (0, 0, -1);
		} else if (Input.GetKeyDown (KeyCode.Period)) {
			Cursor.transform.position += new Vector3 (0, 0, 1);
		}
		// Gamepad
		// TODO
	}

	// Retrieve a List of all child game objects from a given parent
	static List<GameObject> GetChildren(GameObject obj) {
		List<GameObject> list = new List<GameObject>();
		foreach (Transform child in obj.transform)
			list.Add (child.gameObject);
		return list;
	}

	// Create a basic block at the current cursor position
	GameObject CreateBlockAtCursor() {
		//Create a new block at the cursor position and set it as the active game block
		GameObject newBlock = Instantiate (BlockPrefab, Cursor.transform.position, Cursor.transform.rotation) as GameObject;

        //line giving an issue creating a block AROUND the cursor instead of in the world
		//newBlock.transform.SetParent (transform, false);

		ActiveObject = newBlock;
		return newBlock;
	}

	// Sets the material for a block
	void SetMaterial( GameObject block, Material material ) {
		Renderer rend = block.GetComponent<Renderer> ();
		rend.material = material;
	}
}
