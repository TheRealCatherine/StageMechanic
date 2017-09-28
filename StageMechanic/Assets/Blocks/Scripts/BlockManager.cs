/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlockManager : MonoBehaviour {

	// Unity Inspector variables

	public GameObject CursorPrefab;
	public GameObject StartLocationIndicator;
	public GameObject GoalLocationIndicator;

	public GameObject BlockPrefab;
	public GameObject Trap1BlockPrefab;
	public GameObject Trap2BlockPrefab;
	public GameObject SpringBlockPrefab;
	public GameObject MonsterBlockPrefab;
	public GameObject IceBlockPrefab;
	public GameObject VortexBlockPrefab;
	public GameObject Bomb1BlockPrefab;
	public GameObject Bomb2BlockPrefab;
	public GameObject Crack1BlockPrefab;
	public GameObject Crack2BlockPrefab;
	public GameObject TeleportBlockPrefab;
	public GameObject Heavy1BlockPrefab;
	public GameObject Heavy2BlockPrefab;
	public GameObject ImmobleBlockPrefab;
	public GameObject FixedBlockPrefab;
	public GameObject RandomBlockPrefab;
	public GameObject GoalBlockPrefab;



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

	private List<GameObject> _rotatableFloors = new List<GameObject>();
	public List<GameObject> RotatableFloors {
		get {
			return _rotatableFloors;
		}
		set {
			_rotatableFloors = value;
		}
	}

	private GameObject _activeFloor;
	public GameObject ActiveFloor;

	// Called when the BlockManager is intantiated, when the Level Editor is loaded
	void Start() {
        // Create the cursor

		ActiveFloor = GameObject.CreatePrimitive (PrimitiveType.Plane);
		ActiveFloor.transform.position = transform.position;
		ActiveFloor.transform.rotation = transform.rotation;
		ActiveFloor.name = "Platform1";
		MeshCollider colider = ActiveFloor.GetComponent<MeshCollider> ();
		colider.isTrigger = false;

		RotatableFloors.Add (ActiveFloor);

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
			newBlock = Instantiate (Heavy1BlockPrefab, cursor.transform.position, Cursor.transform.rotation) as GameObject;
			break;
		case Block.BlockType.Heavy2:
			newBlock = Instantiate (Heavy2BlockPrefab, cursor.transform.position, Cursor.transform.rotation) as GameObject;
			break;
		case Block.BlockType.Bomb1:
			newBlock = Instantiate (Bomb1BlockPrefab, cursor.transform.position, Cursor.transform.rotation) as GameObject;
			break;
		case Block.BlockType.Bomb2:
			newBlock = Instantiate (Bomb2BlockPrefab, cursor.transform.position, Cursor.transform.rotation) as GameObject;
			break;
		case Block.BlockType.Immobile:
			newBlock = Instantiate (ImmobleBlockPrefab, cursor.transform.position, Cursor.transform.rotation) as GameObject;
			break;
		case Block.BlockType.Fixed:
			newBlock = Instantiate (FixedBlockPrefab, cursor.transform.position, Cursor.transform.rotation) as GameObject;
			break;
		case Block.BlockType.Spring:
			newBlock = Instantiate (SpringBlockPrefab, cursor.transform.position, Cursor.transform.rotation) as GameObject;
			break;
		case Block.BlockType.Crack1:
			newBlock = Instantiate (Crack1BlockPrefab, cursor.transform.position, Cursor.transform.rotation) as GameObject;
			break;
		case Block.BlockType.Crack2:
			newBlock = Instantiate (Crack2BlockPrefab, cursor.transform.position, Cursor.transform.rotation) as GameObject;
			break;
		case Block.BlockType.Trap1:
			newBlock = Instantiate (Trap1BlockPrefab, cursor.transform.position, Cursor.transform.rotation) as GameObject;
			break;
		case Block.BlockType.Trap2:
			newBlock = Instantiate (Trap2BlockPrefab, cursor.transform.position, Cursor.transform.rotation) as GameObject;
			break;
		case Block.BlockType.Vortex:
			newBlock = Instantiate (VortexBlockPrefab, cursor.transform.position, Cursor.transform.rotation) as GameObject;
			break;
		case Block.BlockType.Monster:
			newBlock = Instantiate (MonsterBlockPrefab, cursor.transform.position, Cursor.transform.rotation) as GameObject;
			break;
		case Block.BlockType.Teleport:
			newBlock = Instantiate (TeleportBlockPrefab, cursor.transform.position, Cursor.transform.rotation) as GameObject;
			break;
		case Block.BlockType.Random:
			newBlock = Instantiate (RandomBlockPrefab, cursor.transform.position, Cursor.transform.rotation) as GameObject;
			break;
		case Block.BlockType.Goal:
			newBlock = Instantiate (GoalBlockPrefab, cursor.transform.position, Cursor.transform.rotation) as GameObject;
			break;
		default:
			//Create a new block at the cursor position and set it as the active game block
			newBlock = Instantiate (BlockPrefab, cursor.transform.position, Cursor.transform.rotation) as GameObject;
			break;
		}

		Debug.Assert (newBlock != null);

        //line giving an issue creating a block AROUND the cursor instead of in the world
		newBlock.transform.SetParent (ActiveFloor.transform, false);

		ActiveObject = newBlock;

		Block block = (Block)newBlock.GetComponent (typeof(Block));
		Debug.Assert (block != null);
		block.Type = type;

		return newBlock;
	}

	// Sets the material for a block
	void SetMaterial( GameObject block, Material material ) {
		Renderer rend = block.GetComponent<Renderer> ();
		rend.material = material;
	}

	public string BlocksToJSON() {
		Debug.Assert (ActiveFloor != null);
		string output = "Platform {\n" +
			"\tName: \"" + ActiveFloor.name + "\",\n" +
			"\tBlocks {\n";
		foreach( Transform transform in ActiveFloor.transform ) {
			Block block = null;
			try {
				block = (Block)transform.gameObject.GetComponent (typeof(Block));
			}
			catch(System.InvalidCastException e) {
			}

			if (block == null)
				continue;
			output += block.ToString ();
		}
		output += "\t},\n" +
			"},\n";
		return output;
	}
		
	void OnGUI(){
		if (ActiveObject != null) {


			Block block = null;
			try {
				block = (Block)ActiveObject.GetComponent (typeof(Block));
			}
			catch(System.InvalidCastException e) {
			}

			if (block == null)
				return;

			GUIStyle style = new GUIStyle ();
			style.normal.textColor = Color.black;

			int YPos = 1;

			GUI.Label (new Rect (10, (YPos*25), 50, 25), "Name: ", style);
			block.name = GUI.TextField (new Rect (55, (YPos++*25), 250, 25), block.name, 36);

			GUI.Label (new Rect (10, (YPos++*25), 350, 25), "Type: " + block.Type.ToString(), style);
			GUI.Label (new Rect (10, (YPos++*25), 350, 25), "Trap Type: " + block.TrapType.ToString(), style);

			if (block.TrapType != Block.TrapBlockType.None) {
				GUI.Label (new Rect (10, (YPos++ * 25), 350, 25), "    Trigger time (ms): (??TODO??)", style);
			}

			GUI.Label (new Rect (10, (YPos++*25), 350, 25), "Teleport Type: " + block.TeleportType.ToString(), style);
			if (block.TeleportType != Block.TeleportBlockType.None) {
				GUI.Label (new Rect (10, (YPos++ * 25), 350, 25), "    Distance: " + block.TeleportDistance.ToString(), style);
			}

			GUI.Label (new Rect (10, (YPos++*25), 350, 25), "Collapse: " + ((block.CollapseAfterNSteps>-1 || block.CollapseAfterNSteps>-1)?"Yes":"No") , style);
			if (block.CollapseAfterNSteps>-1 || block.CollapseAfterNSteps>-1) {
				GUI.Label (new Rect (10, (YPos++ * 25), 350, 25), "    Steps: " + (block.CollapseAfterNSteps>-1?block.CollapseAfterNSteps.ToString():"N/A"), style);
				GUI.Label (new Rect (10, (YPos++ * 25), 350, 25), "    Grabs: " + (block.CollapseAfterNGrabs>-1?block.CollapseAfterNGrabs.ToString():"N/A"), style);
			}

			GUI.Label (new Rect (10, (YPos*25), 50, 25), "Bomb: ", style);
			GUI.Toggle (new Rect (55, (YPos++*25), 250, 25), block.IsBomb, (block.IsBomb?"Yes":"No"),style);
			if (block.IsBomb) {
				GUI.Label (new Rect (10, (YPos++ * 25), 350, 25), "   Fuse time (ms): " + block.BombTimeMS, style);
				GUI.Label (new Rect (10, (YPos++ * 25), 350, 25), "  Radius: " + block.BombRadius, style);
			}
		}
	}
}
