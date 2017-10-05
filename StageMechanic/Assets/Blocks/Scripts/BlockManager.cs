/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using System.Threading;
using System.Globalization;
using System;
using System.Net;
using System.Linq;

[System.Serializable]
public class BlockManager : MonoBehaviour {

	// Unity Inspector variables

	public GameObject CursorPrefab;
	public GameObject BasicPlatformPrefab;
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
	public GameObject ActiveFloor {
		get {
			return _activeFloor;
		}
		set {
			_activeFloor = value;
		}
	}

	private bool _showBlockInfo = true;
	public bool ShowBlockInfo {
		get {
			return _showBlockInfo;
		}
		set {
			_showBlockInfo = value;
		}
	}

	public void ToggleBlockInfo() {
		this.ShowBlockInfo = !this.ShowBlockInfo;
	}

	// Called when the BlockManager is intantiated, when the Level Editor is loaded
	void Start() {
        // Create the cursor
		ActiveFloor = Instantiate (BasicPlatformPrefab, new Vector3(0,0,0), new Quaternion(0,0,0,0)) as GameObject;
		//ActiveFloor = GameObject.CreatePrimitive (PrimitiveType.Plane);
		//ActiveFloor.transform.position = transform.position;
		//ActiveFloor.transform.rotation = transform.rotation;
		ActiveFloor.name = "Platform1";
		ActiveFloor.transform.SetParent (transform, false);
		//MeshCollider colider = ActiveFloor.GetComponent<MeshCollider> ();
		//colider.isTrigger = false;

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
	// TODO return Block
	public Block CreateBlockAtCursor( Block.BlockType type = Block.BlockType.Basic ) {
		Debug.Assert (Cursor != null);
		return CreateBlock (Cursor.transform.position, type);
	}

	public Block CreateBlock( Vector3 pos, Block.BlockType type = Block.BlockType.Basic ) {
		string oldName = null;
		GameObject oldItem = null;

		GameObject[] collidedGameObjects = 
			Physics.OverlapSphere(pos, 0.1f)
				.Except(new [] {GetComponent<Collider>()})
				.Select(c=>c.gameObject)
				.ToArray();

		Debug.Log ("Collided with " + collidedGameObjects.Length + " objects");

		foreach( GameObject obj in collidedGameObjects) {
			Block bl = obj.GetComponent (typeof(Block)) as Block;
			if (bl != null) {
				oldName = bl.Name;
				Debug.Log ("Collided with: " + bl.Name);
				if (bl.Item != null) {
					bl.Item.transform.parent = null;
					oldItem = bl.Item;
				}
				if (bl.Type != type) {
					//TODO make this not shitty hacky type styff
					bl.Type = type;
					Block tmp = CreateBlock (new Vector3 (-20, -20, -20), type);
					bl.GetComponent<Renderer> ().material = tmp.GetComponent<Renderer> ().material;
					bl.transform.localScale = tmp.transform.localScale;
					Destroy (tmp);
				}
				return bl;
			}
		}

		GameObject newBlock = null;

		switch (type) {
		case Block.BlockType.Basic:
			newBlock = Instantiate (BlockPrefab, pos, ActiveFloor.transform.rotation) as GameObject;
			break;
		case Block.BlockType.Ice:
			newBlock = Instantiate (IceBlockPrefab, pos,  ActiveFloor.transform.rotation) as GameObject;
			break;
		case Block.BlockType.Heavy1:
			newBlock = Instantiate (Heavy1BlockPrefab, pos,  ActiveFloor.transform.rotation) as GameObject;
			break;
		case Block.BlockType.Heavy2:
			newBlock = Instantiate (Heavy2BlockPrefab, pos,  ActiveFloor.transform.rotation) as GameObject;
			break;
		case Block.BlockType.Bomb1:
			newBlock = Instantiate (Bomb1BlockPrefab, pos,  ActiveFloor.transform.rotation) as GameObject;
			break;
		case Block.BlockType.Bomb2:
			newBlock = Instantiate (Bomb2BlockPrefab, pos,  ActiveFloor.transform.rotation) as GameObject;
			break;
		case Block.BlockType.Immobile:
			newBlock = Instantiate (ImmobleBlockPrefab, pos,  ActiveFloor.transform.rotation) as GameObject;
			break;
		case Block.BlockType.Fixed:
			newBlock = Instantiate (FixedBlockPrefab, pos,  ActiveFloor.transform.rotation) as GameObject;
			break;
		case Block.BlockType.Spring:
			newBlock = Instantiate (SpringBlockPrefab, pos, ActiveFloor.transform.rotation) as GameObject;
			break;
		case Block.BlockType.Crack1:
			newBlock = Instantiate (Crack1BlockPrefab, pos,  ActiveFloor.transform.rotation) as GameObject;
			break;
		case Block.BlockType.Crack2:
			newBlock = Instantiate (Crack2BlockPrefab, pos,  ActiveFloor.transform.rotation) as GameObject;
			break;
		case Block.BlockType.Trap1:
			newBlock = Instantiate (Trap1BlockPrefab, pos,  ActiveFloor.transform.rotation) as GameObject;
			break;
		case Block.BlockType.Trap2:
			newBlock = Instantiate (Trap2BlockPrefab, pos,  ActiveFloor.transform.rotation) as GameObject;
			break;
		case Block.BlockType.Vortex:
			newBlock = Instantiate (VortexBlockPrefab, pos,  ActiveFloor.transform.rotation) as GameObject;
			break;
		case Block.BlockType.Monster:
			newBlock = Instantiate (MonsterBlockPrefab, pos,  ActiveFloor.transform.rotation) as GameObject;
			break;
		case Block.BlockType.Teleport:
			newBlock = Instantiate (TeleportBlockPrefab, pos,  ActiveFloor.transform.rotation) as GameObject;
			break;
		case Block.BlockType.Random:
			newBlock = Instantiate (RandomBlockPrefab, pos, ActiveFloor.transform.rotation) as GameObject;
			break;
		case Block.BlockType.Goal:
			newBlock = Instantiate (GoalBlockPrefab, pos,  ActiveFloor.transform.rotation) as GameObject;
			break;
		default:
			//Create a new block at the cursor position and set it as the active game block
			newBlock = Instantiate (BlockPrefab, pos,  ActiveFloor.transform.rotation) as GameObject;
			break;
		}

		Debug.Assert (newBlock != null);
	
		newBlock.transform.SetParent (ActiveFloor.transform, true);

		Block block = newBlock.GetComponent (typeof(Block)) as Block;
		Debug.Assert (block != null);
		block.Type = type;
		return block;
	}

	// Sets the material for a block
	void SetMaterial( GameObject block, Material material ) {
		Renderer rend = block.GetComponent<Renderer> ();
		rend.material = material;
	}

	public string BlocksToJson() {
		Debug.Assert (ActiveFloor != null);
		string output = "";
		StageJsonDelegate stage = new StageJsonDelegate (this);
		StageCollection collection = new StageCollection (stage);
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

		try	{
			MemoryStream ms = new MemoryStream();
			DataContractJsonSerializer serializer = new DataContractJsonSerializer (typeof(StageCollection));
			XmlDictionaryWriter writer = JsonReaderWriterFactory.CreateJsonWriter (ms, Encoding.UTF8, true, true, "    ");
			serializer.WriteObject (writer, collection);
			writer.Flush ();
			output += Encoding.UTF8.GetString(ms.ToArray());
		}
		catch (System.Exception exception)
		{
			Debug.Log(exception.ToString());
		}
		finally
		{
			Thread.CurrentThread.CurrentCulture = currentCulture;
		}

		return output;
	}

	public void DestroyBlock( Block block ) {
		Destroy (block);
	}

	public void BlocksFromJson( Uri path ) {
		Debug.Log ("Loading from " + path.ToString ());
		StageCollection deserializedCollection = new StageCollection(this);
		WebClient webClient = new WebClient();
		Stream fs = webClient.OpenRead(path);  
		DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedCollection.GetType());  
		deserializedCollection = ser.ReadObject(fs) as StageCollection;  
		fs.Close();
		Debug.Log ("Loaded " + deserializedCollection.Stages.Count + "stage(s)");
	}

	public PlatformJsonDelegate GetPlatformJsonDelegate() {
		return new PlatformJsonDelegate (ActiveFloor);
	}
		
	void OnGUI(){
		if (ShowBlockInfo) {

			GUIStyle style = new GUIStyle ();
			style.normal.textColor = Color.black;
			int YPos = 1;

			if (ActiveObject != null) {

				Block block = null;
				try {
					block = (Block)ActiveObject.GetComponent (typeof(Block));
				} catch (System.InvalidCastException) {
					block = null;
				}
					
				if (block == null)
					return;



				GUI.Label (new Rect (10, (YPos * 25), 50, 25), "Name: ", style);
				block.name = GUI.TextField (new Rect (55, (YPos++ * 25), 250, 25), block.name, 36);

				GUI.Label (new Rect (10, (YPos++ * 25), 350, 25), "Type: " + block.Type.ToString (), style);
				GUI.Label (new Rect (10, (YPos++ * 25), 350, 25), "Trap Type: " + block.TrapType.ToString (), style);

				if (block.TrapType != Block.TrapBlockType.None) {
					GUI.Label (new Rect (10, (YPos++ * 25), 350, 25), "    Trigger time (ms): (??TODO??)", style);
				}

				GUI.Label (new Rect (10, (YPos++ * 25), 350, 25), "Teleport Type: " + block.TeleportType.ToString (), style);
				if (block.TeleportType != Block.TeleportBlockType.None) {
					GUI.Label (new Rect (10, (YPos++ * 25), 350, 25), "    Distance: " + block.TeleportDistance.ToString (), style);
				}

				GUI.Label (new Rect (10, (YPos++ * 25), 350, 25), "Collapse: " + ((block.CollapseAfterNSteps > -1 || block.CollapseAfterNSteps > -1) ? "Yes" : "No"), style);
				if (block.CollapseAfterNSteps > -1 || block.CollapseAfterNSteps > -1) {
					GUI.Label (new Rect (10, (YPos++ * 25), 350, 25), "    Steps: " + (block.CollapseAfterNSteps > -1 ? block.CollapseAfterNSteps.ToString () : "N/A"), style);
					GUI.Label (new Rect (10, (YPos++ * 25), 350, 25), "    Grabs: " + (block.CollapseAfterNGrabs > -1 ? block.CollapseAfterNGrabs.ToString () : "N/A"), style);
				}

				GUI.Label (new Rect (10, (YPos * 25), 50, 25), "Bomb: ", style);
				GUI.Toggle (new Rect (55, (YPos++ * 25), 250, 25), block.IsBomb, (block.IsBomb ? "Yes" : "No"), style);
				if (block.IsBomb) {
					GUI.Label (new Rect (10, (YPos++ * 25), 350, 25), "   Fuse time (ms): " + block.BombTimeMS, style);
					GUI.Label (new Rect (10, (YPos++ * 25), 350, 25), "  Radius: " + block.BombRadius, style);
				}
			}
			YPos = 9;

			GUI.Label (new Rect (10, (YPos++ * 25), 350, 25), "   Arrow keys: Move cursor up/down/left/right", style);
			GUI.Label (new Rect (10, (YPos++ * 25), 350, 25), "   Alt+Arrow keys: Rotate tower (broken)", style);
			GUI.Label (new Rect (10, (YPos++ * 25), 350, 25), "   Shift+Arrow keys: Move block", style);
			GUI.Label (new Rect (10, (YPos++ * 25), 350, 25), "   Ctrl+Arrow keys: Move camera", style);
			GUI.Label (new Rect (10, (YPos++ * 25), 350, 25), "   Mouse wheel: zoom in/out", style);
			GUI.Label (new Rect (10, (YPos++ * 25), 350, 25), "   [,] and [.]: Move cursor closer/further", style);
			GUI.Label (new Rect (10, (YPos++ * 25), 350, 25), "   [space]: Place block", style);
			GUI.Label (new Rect (10, (YPos++ * 25), 350, 25), "   [[] and []] (brackets): Change block type", style);
			GUI.Label (new Rect (10, (YPos++ * 25), 350, 25), "   [1]-[5]: Place blocks of different types", style);
			GUI.Label (new Rect (10, (YPos++ * 25), 350, 25), "   [Delete]: Remove block under cursor", style);
			GUI.Label (new Rect (10, (YPos++ * 25), 350, 25), "   [Home] and [End]: Place players", style);
			GUI.Label (new Rect (10, (YPos++ * 25), 350, 25), "   [I]: Toggle info display", style);
			GUI.Label (new Rect (10, (YPos++ * 25), 350, 25), "   [S]: Save (broken)", style);
			GUI.Label (new Rect (10, (YPos++ * 25), 350, 25), "   [Esc]/[Q]: Quit", style);
		}
	}
}
