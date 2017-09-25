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

	public GameObject CursorPrefab;
	public GameObject StartLocationIndicator;
	public GameObject GoalLocationIndicator;

	public GameObject BlockPrefab;
	public GameObject IceBlockPrefab;
	public GameObject HeavyBlockPrefab;
	public GameObject Bomb1BlockPrefab;
	public GameObject Bomb2BlockPrefab;



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

	private Block.BlockType _blockCycleType = Block.BlockType.Basic;
	public Block.BlockType BlockCycleType {
		get {
			return _blockCycleType;
		}
		set {
			_blockCycleType = value;
		}
	}

	public Block.BlockType NextBlockType() {
		if (BlockCycleType >= Block.BlockType.Goal) {
			BlockCycleType = Block.BlockType.Basic;
			return BlockCycleType;
		}
		return ++BlockCycleType;
	}

	public Block.BlockType PrevBlockType() {
		if (BlockCycleType <= Block.BlockType.Basic) {
			BlockCycleType = Block.BlockType.Goal;
			return BlockCycleType;
		}
		return --BlockCycleType;
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
		
	}

	// Retrieve a List of all child game objects from a given parent
	static List<GameObject> GetChildren(GameObject obj) {
		List<GameObject> list = new List<GameObject>();
		foreach (Transform child in obj.transform)
			list.Add (child.gameObject);
		return list;
	}

	// Create a basic block at the current cursor position
	public GameObject CreateBlockAtCursor( GameObject cursor, Block.BlockType type = Block.BlockType.Custom ) {
		// If there is a block currently under the cursor destroy it.
		if (ActiveObject != null)
			Destroy (ActiveObject);

		GameObject newBlock = null;

		switch (type) {
		case Block.BlockType.Basic:
			newBlock = Instantiate (BlockPrefab, cursor.transform.position, Cursor.transform.rotation) as GameObject;
			break;
		case Block.BlockType.Ice:
			newBlock = Instantiate (IceBlockPrefab, cursor.transform.position, Cursor.transform.rotation) as GameObject;
			break;
		case Block.BlockType.Heavy1:
			newBlock = Instantiate (HeavyBlockPrefab, cursor.transform.position, Cursor.transform.rotation) as GameObject;
			break;
		case Block.BlockType.Bomb1:
			newBlock = Instantiate (Bomb1BlockPrefab, cursor.transform.position, Cursor.transform.rotation) as GameObject;
			break;
		case Block.BlockType.Bomb2:
			newBlock = Instantiate (Bomb2BlockPrefab, cursor.transform.position, Cursor.transform.rotation) as GameObject;
			break;
		default:
			//Create a new block at the cursor position and set it as the active game block
			newBlock = Instantiate (BlockPrefab, cursor.transform.position, Cursor.transform.rotation) as GameObject;
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
