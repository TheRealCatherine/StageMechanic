﻿/*  
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
using GracesGames;
using System.Linq;

[System.Serializable]
public class BlockManager : MonoBehaviour {

    // Unity Inspector variables

    public GameObject CursorPrefab;
    public GameObject BasicPlatformPrefab;
    public GameObject StartLocationIndicator;
    public GameObject GoalLocationIndicator;

    public GameObject FileBrowserPrefab;

    // Properties

    public bool PlayMode { get; set; } = false;

    // The obect (block/item/etc) currently under the cursor
    public GameObject ActiveObject {
        get {
            CursorCollider col = Cursor.GetComponent<CursorCollider>();
            return col.ObjectUnderCursor;
        }
        set {
            CursorCollider col = Cursor.GetComponent<CursorCollider>();
            col.ObjectUnderCursor = value;
        }
    }

    //TODO support other types via IBlock
    public Cathy1Block ActiveBlock
    {
        get
        {
            CursorCollider col = Cursor.GetComponent<CursorCollider>();
            Debug.Assert(col != null);
            if (col.ObjectUnderCursor == null)
                return null;
            return col.ObjectUnderCursor.GetComponent<Cathy1Block>();
        }
        set
        {
            CursorCollider col = Cursor.GetComponent<CursorCollider>();
            Debug.Assert(col != null);
            col.ObjectUnderCursor = value.GameObject;
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

    private Cathy1Block.BlockType _blockCycleType = Cathy1Block.BlockType.Basic;
    public Cathy1Block.BlockType BlockCycleType {
        get {
            return _blockCycleType;
        }
        set {
            _blockCycleType = value;
        }
    }

    public Cathy1Block.BlockType NextBlockType() {
        if (BlockCycleType >= Cathy1Block.BlockType.Goal) {
            BlockCycleType = Cathy1Block.BlockType.Basic;
            return BlockCycleType;
        }
        return ++BlockCycleType;
    }

    public Cathy1Block.BlockType PrevBlockType() {
        if (BlockCycleType <= Cathy1Block.BlockType.Basic) {
            BlockCycleType = Cathy1Block.BlockType.Goal;
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

    //TODO make this whole class static
    public static BlockManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    // Called when the BlockManager is intantiated, when the Level Editor is loaded
    void Start() {
        // Create the cursor
        ActiveFloor = Instantiate(BasicPlatformPrefab, new Vector3(0, 0f, 0), new Quaternion(0, 0, 0, 0)) as GameObject;
        ActiveFloor.name = "Platform1";
        ActiveFloor.transform.SetParent(transform, false);
        RotatableFloors.Add(ActiveFloor);
        Cursor = CursorPrefab;
        Cursor.transform.SetParent(transform, false);
    }

    // Called once every frame
    void Update() {

    }

    // Retrieve a List of all child game objects from a given parent
    static List<GameObject> GetChildren(GameObject obj) {
        List<GameObject> list = new List<GameObject>();
        foreach (Transform child in obj.transform)
            list.Add(child.gameObject);
        return list;
    }

    // Create a basic block at the current cursor position

    public IBlock CreateBlockAtCursor(Cathy1Block.BlockType type = Cathy1Block.BlockType.Basic) {
        Debug.Assert(Cursor != null);
        Cathy1Block block = GetComponent<Cathy1BlockFactory>().CreateBlock(Cursor.transform.position, Cursor.transform.rotation, type, ActiveFloor) as Cathy1Block;
        return block;
    }

    public Cathy1BlockFactory Cathy1BlockFactory
    {
        get
        {
            return GetComponent<Cathy1BlockFactory>();
        }
    }

	// Sets the material for a block
	void SetMaterial( IBlock block, Material material ) {
		Renderer rend = block.GameObject.GetComponent<Renderer> ();
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

	public void DestroyBlock( IBlock block ) {
        Debug.Assert(block != null);
        Debug.Assert(block.GameObject != null);
		Destroy (block.GameObject);
	}

	public void SaveToJson() {
		GameObject fileBrowserObject = Instantiate(FileBrowserPrefab, this.transform);
		fileBrowserObject.name = "FileBrowser";
		FileBrowser fileBrowserScript = fileBrowserObject.GetComponent<FileBrowser>();
		fileBrowserScript.SaveFilePanel(this, "SaveFileUsingPath", "MyLevels", "json");
	}

	public void LoadFromJson() {
		GameObject fileBrowserObject = Instantiate(FileBrowserPrefab, this.transform);
		fileBrowserObject.name = "FileBrowser";
		FileBrowser fileBrowserScript = fileBrowserObject.GetComponent<FileBrowser>();
		fileBrowserScript.OpenFilePanel(this, "LoadFileUsingPath", "json");
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

	// Saves a file with the textToSave using a path
	private void SaveFileUsingPath(string path) {
		if (path.Length != 0) {
			string json = BlocksToJson ();
			if (json.Length != 0)
				System.IO.File.WriteAllText (path, json);
		} else {
			Debug.Log("Invalid path given");
		}
	}

	// Loads a file using a path
	private void LoadFileUsingPath(string path) {
		if (path.Length != 0) {
			BlocksFromJson (new Uri("file:///"+path));
		} else {
			Debug.Log("Invalid path given");
		}
	}

	public PlatformJsonDelegate GetPlatformJsonDelegate() {
		return new PlatformJsonDelegate (ActiveFloor);
	}

	public static IBlock GetBlockAt( Vector3 position ) {
		GameObject[] collidedGameObjects =
			Physics.OverlapSphere (position, 0.1f)
				//.Except (new[] { GetComponent<BoxCollider> () })
				.Select (c => c.gameObject)
				.ToArray ();

		foreach (GameObject go in collidedGameObjects) {
			IBlock block = go.GetComponent<IBlock> ();
			if (block != null)
				return block;
		}
		return null;
	}
}
