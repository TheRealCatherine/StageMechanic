/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */

using GracesGames;
using GracesGames.SimpleFileBrowser.Scripts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Xml;
using UnityEngine;

[System.Serializable]
public class BlockManager : MonoBehaviour {

    // Unity Inspector variables

    public GameObject CursorPrefab;
    public GameObject BasicPlatformPrefab;
    public GameObject StartLocationIndicator;
    public GameObject GoalLocationIndicator;

    public GameObject FileBrowserPrefab;

    // Properties

    public static bool PlayMode { get; set; } = false;

    public void TogglePlayMode()
    {
        bool pm = !BlockManager.PlayMode;
        BlockManager.PlayMode = pm;
        GetComponent<PlayerManager>().PlayMode = pm;
        Cursor.SetActive(!pm);
        if (pm)
            LogController.Log("Start!");
    }

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
            if (PlayMode)
            {
                //TODO this is bad, very bad.
                Cathy1Block block = GetBlockAt(PlayerManager.Player1Location() + Vector3.down)?.GameObject?.GetComponent<Cathy1Block>();
                if (block != null && block.Type == Cathy1Block.BlockType.Goal)
                {
                    LogController.Log("CONGRATULATION");
                }
                return block;
            }
            else
            {
                CursorCollider col = Cursor.GetComponent<CursorCollider>();
                Debug.Assert(col != null);
                if (col.ObjectUnderCursor == null)
                    return null;
                return col.ObjectUnderCursor.GetComponent<Cathy1Block>();
            }
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

    public string LastAccessedFileName;

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
        ActiveFloor.name = "Platform";
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

    public int BlockCount()
    {
        int blocks = 0;
        foreach (Transform child in ActiveFloor.transform)
        {
            if (child.gameObject.GetComponent<IBlock>() != null)
                ++blocks;
        }
        return blocks;
    }

    public void Clear()
    {
        foreach (Transform child in ActiveFloor.transform)
            Destroy(child.gameObject);
        PlayerManager.Clear();
        LogController.Log("Stage Data Cleared");
    }

    public void RandomizeGravity()
    {
        foreach (Transform child in ActiveFloor.transform)
        {
            IBlock block = child.gameObject.GetComponent<IBlock>();
            if (block != null)
                block.GravityFactor = UnityEngine.Random.value;
        }
        AutoSave();
    }

    // Create a basic block at the current cursor position

    public IBlock CreateBlockAtCursor(Cathy1Block.BlockType type = Cathy1Block.BlockType.Basic) {
        Debug.Assert(Cursor != null);
        Cathy1Block block = GetComponent<Cathy1BlockFactory>().CreateBlock(Cursor.transform.position, Cursor.transform.rotation, type, ActiveFloor) as Cathy1Block;
        ActiveBlock = block;
        AutoSave();
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
            LogController.Log(exception.ToString());
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
        AutoSave();
	}

    public void DestroyActiveObject()
    {
        if (ActiveObject != null)
        {
            Destroy(ActiveObject);
            AutoSave();
        }
    }

	public void SaveToJson() {
		GameObject fileBrowserObject = Instantiate(FileBrowserPrefab, this.transform);
		fileBrowserObject.name = "FileBrowser";
		FileBrowser fileBrowserScript = fileBrowserObject.GetComponent<FileBrowser>();
        fileBrowserScript.SetupFileBrowser(ViewMode.Landscape);
        fileBrowserScript.SaveFilePanel(this, "SaveFileUsingPath", "MyLevels", "json");
	}

    public void QuickSave()
    {
        if (LastAccessedFileName.Length == 0)
            SaveToJson();
        else
            SaveFileUsingPath(LastAccessedFileName);
    }

    public void AutoSave()
    {

        if (!PlayMode && LastAccessedFileName.Length != 0)
        {
            SaveFileUsingPath(LastAccessedFileName.Replace(".json", "_autosave.json"));
            LogController.Log("Autosaved");
        }
    }

	public void LoadFromJson() {
		GameObject fileBrowserObject = Instantiate(FileBrowserPrefab, this.transform);

        fileBrowserObject.name = "FileBrowser";
		FileBrowser fileBrowserScript = fileBrowserObject.GetComponent<FileBrowser>();
        fileBrowserScript.SetupFileBrowser(ViewMode.Landscape);

        fileBrowserScript.OpenFilePanel(this, "LoadFileUsingPath", "json");
	}

	public void BlocksFromJson( Uri path ) {
        Clear();
		LogController.Log ("Loading from " + path.ToString ());
		StageCollection deserializedCollection = new StageCollection(this);
		WebClient webClient = new WebClient();
		Stream fs = webClient.OpenRead(path);  
		DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedCollection.GetType());  
		deserializedCollection = ser.ReadObject(fs) as StageCollection;  
		fs.Close();
        LogController.Log("Loaded " + deserializedCollection.Stages.Count + " stage(s)");
	}

	// Saves a file with the textToSave using a path
	private void SaveFileUsingPath(string path) {
		if (path.Length != 0) {
			string json = BlocksToJson ();
            //TODO this probably can throw an exception?
			if (json.Length != 0)
				System.IO.File.WriteAllText (path, json);
            if (!path.Contains("_autosave.")) {
                LastAccessedFileName = path;
                File.Delete(path.Replace(".json", "_autosave.json"));
                LogController.Log("Saved & Autosave");
            }
		} else {
            LogController.Log("Invalid path");
		}
	}

	// Loads a file using a path
	private void LoadFileUsingPath(string path) {
        //TODO ensure file is valid
        if (path.Length != 0) {
			BlocksFromJson (new Uri("file:///"+path));
            if (path.Contains("_autosave"))
            {
                LastAccessedFileName = string.Empty;
                LogController.Log("AUTOSAVE OFF");
            }
            else
                LastAccessedFileName = path;
		} else {
            LogController.Log("Invalid path");
		}
	}

    public void ReloadCurrentLevel()
    {
        LoadFileUsingPath(LastAccessedFileName);
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
