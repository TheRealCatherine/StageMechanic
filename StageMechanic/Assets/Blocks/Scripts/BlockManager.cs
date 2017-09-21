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

	// Properties

	private GameObject _activeObject;
	public GameObject ActiveObject {
		get {
			return _activeObject;
		}
		set {
			_activeObject = value;
		}
	}

	private GameObject _cursor;
	public GameObject Cursor {
		get {
			return _cursor;
		}
		set {
			_cursor = value;
		}
	}


	void Start() {
		Cursor = Instantiate (CursorPrefab, transform.position, transform.rotation) as GameObject;
		Cursor.transform.SetParent (transform, false);
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.C)) {
			//Create a new block at the cursor position and set it as the active game block
			GameObject newBlock = Instantiate (BlockPrefab, Cursor.transform.position, Cursor.transform.rotation) as GameObject;
			newBlock.transform.SetParent (transform, false);
			ActiveObject = newBlock;
		}

		//Cursor movement cotrol
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
	}

	static List<GameObject> GetChildren(GameObject obj) {
		List<GameObject> list = new List<GameObject>();
		foreach (Transform child in obj.transform)
			list.Add (child.gameObject);
		return list;
	}
}
