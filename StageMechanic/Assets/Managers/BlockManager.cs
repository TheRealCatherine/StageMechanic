/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */

using GracesGames;
using GracesGames.SimpleFileBrowser.Scripts;
using System;
using System.Collections;
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

    public int MaxUndoLevels = 99;
    public bool UndoEnabled = true;

    /// <summary>
    /// There should only ever be one BlockManager in the scene - it manages all blocks for all platforms and loaded stages. As such it
    /// can be used statically via this property when accessing methods that are not already marked static.
    /// </summary>
    /// TODO Singleton flamewar
    public static BlockManager Instance { get; private set; }

    /// <summary>
    /// Used for auto-saving and saving while creating stages as
    /// well as reloading a level from a file.
    /// </summary>
    public string LastAccessedFileName;

    /// <summary>
    /// Used by many classes to determine current game state, eventually this will be moved into
    /// a seperate GameManager class or something but for now this is pretty much THE way to
    /// determine if the application is currently in PlayMode or EditMode. Note that this property
    /// is read-pnly. Use BlockManager.TogglePlayMode() to change the application state to/from
    /// PlayMode and EditMode. This too will change in an upcoming revision.
    /// </summary>
    /// TODO: Move this out to a separate GameManager class
    public static bool PlayMode { get; protected set; } = false;

    /// <summary>
    /// Toggles the application between PlayMode and EditMode. This will show/hide the cursor,
    /// inform PlayerManager of the new mode, and if entering play mode record the starting state
    /// of the blocks to facilitate player death and test-playing while creating (restore on exiting
    /// PlayMode)
    /// </summary>
    /// TODO Combine this with the PlayMode property and move it to GameManager class
    /// TODO change the way the button mapping box behaves
    public void TogglePlayMode()
    {
        PlayMode = !PlayMode;
        if (PlayMode)
        {
            LogController.Log("Start!");
            UIManager.Instance.BlockInfoBox.gameObject.SetActive(false);
            RecordStartState();
        }
        else
        {
            //Reset blocks to their pre-PlayMode state
            if (_startState != null && _startState.Length != 0)
                ReloadStartState();
            UIManager.Instance.BlockInfoBox.gameObject.SetActive(true);
        }
        UIManager.RefreshButtonMappingDialog();
        GetComponent<PlayerManager>().PlayMode = PlayMode;
        Cursor.SetActive(!PlayMode);
    }

    /// <summary>
    /// When turned on, BlockManager will record BlockManager.MaxUndoLevels worth of states.
    /// Note that currently this is the same as creating a save file but storing it in memory
    /// rather than to disk, as such it can cause a performance impact on some systems. We
    /// will need to address this in a future revision.
    /// </summary>
    /// TODO: Consider moving this to being just a property and perhaps relocating UNDO related
    /// code to another class
    public static void ToggleUndoOn()
    {
        Instance.UndoEnabled = !Instance.UndoEnabled;
        if (Instance.UndoEnabled)
            LogController.Log(Instance.MaxUndoLevels + " Undos On");
        else
            LogController.Log("Undo off");
    }

    /// <summary>
    /// This is a hacky method from early on in the implementation. In theory it should return
    /// the GameObject of whatever is under the cursor. In actuality it usually only does this
    /// correctly if its a block.
    /// </summary>
    /// TODO: Move cursor when set
    public GameObject ActiveObject {
        get {
            return GetBlockAt(Cursor.transform.position)?.GameObject;
        }
        set {
        }
    }

    /// <summary>
    /// When in EditMode, returns the block, if any, that is currently under the cursor,
    /// or null if the cursor is not on a block.
    /// 
    /// When in PlayMode, returns the block underneath Player 1
    /// </summary>
    /// TODO Support IBlock interface
    /// TODO Get rid this Goal Block hack and move it to GoalBlock
    /// TODO Get sidled-on block when player is sidling
    /// TODO Query PlayerManager for active block when in PlayMode
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
                return GetBlockAt(Cursor.transform.position)?.GameObject?.GetComponent<Cathy1Block>();
            }
        }
        set
        {
        }
    }

    /// <summary>
    /// The Cursor used in Edit Mode.
    /// </summary>
    /// TODO Right now this gets moved from the top level scene, do we want this behavior?
    private static GameObject _cursor;
    public static GameObject Cursor {
        get {
            return _cursor;
        }
        set {
            _cursor = value;
        }
    }

    /// <summary>
    /// Used for cycling the default block type while in EditMode. This will be moved to
    /// Cathy1BlockFactory or similar class later.
    /// </summary>
    private Cathy1Block.BlockType _blockCycleType = Cathy1Block.BlockType.Basic;
    public Cathy1Block.BlockType BlockCycleType {
        get {
            return _blockCycleType;
        }
        set {
            _blockCycleType = value;
        }
    }

    /// <summary>
    /// Used for cycling the default block type while in EditMode. This will be moved to
    /// Cathy1BlockFactory or similar class later.
    /// </summary>
    public Cathy1Block.BlockType NextBlockType() {
        if (BlockCycleType >= Cathy1Block.BlockType.Goal) {
            BlockCycleType = Cathy1Block.BlockType.Basic;
            return BlockCycleType;
        }
        return ++BlockCycleType;
    }

    /// <summary>
    /// Used for cycling the default block type while in EditMode. This will be moved to
    /// Cathy1BlockFactory or similar class later.
    /// </summary>
    public Cathy1Block.BlockType PrevBlockType() {
        if (BlockCycleType <= Cathy1Block.BlockType.Basic) {
            BlockCycleType = Cathy1Block.BlockType.Goal;
            return BlockCycleType;
        }
        return --BlockCycleType;
    }

    /// <summary>
    /// Used to support Cathy-2 style rotatable floors and other multi-platform implementations
    /// This is to be implemented in the future
    /// Right now it only contains the BlockManager.ActiveFloor
    /// </summary>
    private List<GameObject> _rotatableFloors = new List<GameObject>();
    public List<GameObject> RotatableFloors {
        get {
            return _rotatableFloors;
        }
        set {
            _rotatableFloors = value;
        }
    }

    /// <summary>
    /// Currently this will always be the platform on which the stage rests. When BlockManager.RotatableFloors
    /// is implemented later this will be set to the platform currently selected by the cursor or occupied by
    /// the player.
    /// </summary>
    private static GameObject _activeFloor;
    public static GameObject ActiveFloor {
        get {
            return _activeFloor;
        }
        set {
            _activeFloor = value;
        }
    }


    private static string _startState;
    private static string _lastCheckpointState;
    private static List<string> _undos = new List<string>();
    private static List<Vector3> _undoPlayerPos = new List<Vector3>();
    private static List<Vector3> _undoPlayerFacing = new List<Vector3>();
    private static List<int> _undoPlayerState = new List<int>();
    private static List<float> _undoPlatformPosition = new List<float>();

    public static void ClearUndoStates()
    {
        _undos.Clear();
        _undoPlayerPos.Clear();
        _undoPlayerFacing.Clear();
        _undoPlayerState.Clear();
        _undoPlatformPosition.Clear();
    }

    public static void RecordStartState()
    {
        _startState = Instance.BlocksToCondensedJson();
    }

    public static void ReloadStartState()
    {
        if (_startState != null && _startState.Length != 0)
        {
            Instance.Clear();
            Instance.BlocksFromJson(_startState);
        }
    }

    public static void RecordUndo()
    {
        if (!Instance.UndoEnabled)
            return;
        Debug.Assert(_undos.Count == _undoPlayerPos.Count && _undoPlayerPos.Count == _undoPlayerState.Count);
        if (_undos.Count > Instance.MaxUndoLevels)
        {
            _undos.RemoveAt(0);
            _undoPlayerPos.RemoveAt(0);
            _undoPlayerFacing.RemoveAt(0);
            _undoPlayerState.RemoveAt(0);
            _undoPlatformPosition.RemoveAt(0);
        }
        _undos.Add(Instance.BlocksToPrettyJson());
        _undoPlayerPos.Add(PlayerManager.Player1Location());
        _undoPlayerFacing.Add(PlayerManager.Player1FacingDirection());
        _undoPlayerState.Add(PlayerManager.PlayerState());
        _undoPlatformPosition.Add(ActiveFloor.transform.position.y);
    }

    public static int AvailableUndoCount { get { if (!Instance.UndoEnabled) return 0; return _undos.Count; } }

    public static void Undo()
    {
        if (!Instance.UndoEnabled)
            return;
        Debug.Assert(_undos.Count == _undoPlayerPos.Count && _undoPlayerPos.Count == _undoPlayerState.Count);
        if (_undos.Count > 0)
        {
            Instance.ClearForUndo();
            ActiveFloor.transform.position = new Vector3(0f, _undoPlatformPosition[_undoPlatformPosition.Count - 1], 0f);
            Instance.BlocksFromJson(_undos[_undos.Count - 1]);
            PlayerManager.SetPlayer1State(_undoPlayerState[_undoPlayerState.Count - 1]);
            PlayerManager.SetPlayer1FacingDirection(_undoPlayerFacing[_undoPlayerFacing.Count - 1]);
            PlayerManager.SetPlayer1Location(_undoPlayerPos[_undoPlayerPos.Count - 1]);

            _undos.RemoveAt(_undos.Count - 1);
            _undoPlayerPos.RemoveAt(_undoPlayerPos.Count - 1);
            _undoPlayerFacing.RemoveAt(_undoPlayerFacing.Count - 1);
            _undoPlayerState.RemoveAt(_undoPlayerState.Count - 1);
            _undoPlatformPosition.RemoveAt(_undoPlatformPosition.Count - 1);
            LogController.Log("Undo");
        }
        else
            LogController.Log("No undos left");
    }


    private void Awake()
    {
        Instance = this;
    }

    // Called when the BlockManager is intantiated, when the Level Editor is loaded
    void Start() {
        // Create the cursor
        ActiveFloor = Instantiate(BasicPlatformPrefab, new Vector3(0, 0f, 3f), new Quaternion(0, 0, 0, 0)) as GameObject;
        ActiveFloor.name = "Platform";
        ActiveFloor.transform.SetParent(transform, false);
        RotatableFloors.Add(ActiveFloor);
        Cursor = CursorPrefab;
        Cursor.transform.SetParent(transform, false);
    }

    // Called once every frame
    void Update() {
        //SortChildren(this.gameObject);
    }

    public static void SortChildren(GameObject gameObject)
    {
        Transform[] children = gameObject.GetComponentsInChildren<Transform>(true);

         var sorted = from child in children
                                orderby child.localPosition.y ascending
                                where child != gameObject.transform
                                select child;
        for (int i = 0; i < sorted.Count(); i++)
        {
            if(sorted.ElementAt(i).GetSiblingIndex() != i)
                sorted.ElementAt(i).SetSiblingIndex(i);
        }
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
        {
            if(!child.GetComponent<Platform>())
                Destroy(child.gameObject);
        }
        ActiveFloor.transform.position = new Vector3(0, 0f, 3f);
        
        PlayerManager.Clear();
        EventManager.Clear();
        LogController.Log("Stage Data Cleared");
    }

    public void ClearForUndo()
    {
        foreach (Transform child in ActiveFloor.transform)
            if (!child.GetComponent<Platform>())
                Destroy(child.gameObject);
        EventManager.Clear();
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

    public static IBlock CreateBlockAtCursor(Cathy1Block.BlockType type = Cathy1Block.BlockType.Basic) {
        Debug.Assert(Instance != null);
        Debug.Assert(Cursor != null);
        Cathy1Block block = Instance.GetComponent<Cathy1BlockFactory>().CreateBlock(Cursor.transform.position, Cursor.transform.rotation, type, ActiveFloor) as Cathy1Block;
        Instance.AutoSave();
        return block;
    }

    public static IBlock CreateBlockAtCursor(string palette, string type)
    {
        return CreateBlockAt(Cursor.transform.position, palette, type);
    }

    public static IBlock CreateBlockAt(Vector3 position, string palette, string type)
    {
        Debug.Assert(Instance != null);
        Debug.Assert(Cursor != null);
        if (palette == "Cathy1 Internal")
        {
            Cathy1Block block = Instance.GetComponent<Cathy1BlockFactory>().CreateBlock(position, Cursor.transform.rotation, type, ActiveFloor) as Cathy1Block;
            Instance.AutoSave();
            return block;
        }
        return null;
    }

    /// <summary>
    /// Returns a Cathy1BlockFactory that can be used to create Cathy1-style blocks
    /// </summary>
    /// TODO create the factory, don't be the factory.
    public Cathy1BlockFactory Cathy1BlockFactory
    {
        get
        {
            return GetComponent<Cathy1BlockFactory>();
        }
    }

	/// <summary>
    /// Sets the material on a block.
    /// </summary>
    /// <param name="block"></param>
    /// <param name="material"></param>
    /// TODO: Move this to AbstractBlock?
	public static void SetMaterial( IBlock block, Material material ) {
		Renderer rend = block.GameObject.GetComponent<Renderer> ();
		rend.material = material;
	}

	public string BlocksToPrettyJson() {
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

    public string BlocksToCondensedJson()
    {
        Debug.Assert(ActiveFloor != null);
        string output = "";
        StageJsonDelegate stage = new StageJsonDelegate(this);
        StageCollection collection = new StageCollection(stage);
        CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

        try
        {
            MemoryStream ms = new MemoryStream();
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(StageCollection));
            XmlDictionaryWriter writer = JsonReaderWriterFactory.CreateJsonWriter(ms, Encoding.UTF8, true, false);
            serializer.WriteObject(writer, collection);
            writer.Flush();
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
        fileBrowserScript.SetupFileBrowser(ViewMode.Landscape, PlayerPrefs.GetString("LastSaveDir"));
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
        fileBrowserScript.SetupFileBrowser(ViewMode.Landscape,PlayerPrefs.GetString("LastLoadDir"));

        fileBrowserScript.OpenFilePanel(this, "LoadFileUsingPath", "json");
	}

	public void BlocksFromJson( Uri path ) {
        Clear();
        ClearUndoStates();
		LogController.Log ("Loading from " + path.ToString ());
		StageCollection deserializedCollection = new StageCollection(this);
		WebClient webClient = new WebClient();
		Stream fs = webClient.OpenRead(path);  
		DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedCollection.GetType());  
		deserializedCollection = ser.ReadObject(fs) as StageCollection;  
		fs.Close();
        LogController.Log("Loaded " + deserializedCollection.Stages.Count + " stage(s)");
        if(PlayerPrefs.GetInt("AutoPlayOnLoad",0) == 1)
        {
            if (!PlayMode)
                TogglePlayMode();
        }
        SortChildren(this.gameObject);
    }

    public void BlocksFromJson( string json )
    {
        //note: this one doesn't clear like the other one... maybe fix this
        MemoryStream stream = new MemoryStream();
        StreamWriter writer = new StreamWriter(stream);
        writer.Write(json);
        writer.Flush();
        stream.Position = 0;
        StageCollection deserializedCollection = new StageCollection(this);
        DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedCollection.GetType());
        deserializedCollection = ser.ReadObject(stream) as StageCollection;
        stream.Close();
    }

	// Saves a file with the textToSave using a path
	private void SaveFileUsingPath(string path) {
		if (path.Length != 0) {
            Uri location = new Uri("file:///" + path);
            string directory = System.IO.Path.GetDirectoryName(location.AbsolutePath);
            PlayerPrefs.SetString("LastSaveDir", directory);
            string json = BlocksToPrettyJson ();
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
            Uri location = new Uri("file:///" + path);
            string directory = System.IO.Path.GetDirectoryName(location.AbsolutePath);
            PlayerPrefs.SetString("LastLoadDir", directory);
            BlocksFromJson(location);
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

    public bool TryReloadCurrentLevel()
    {
        if(LastAccessedFileName.Length != 0)
        {
            ReloadCurrentLevel();
            return true;
        }
        return false;
    }

	public PlatformJsonDelegate GetPlatformJsonDelegate() {
		return new PlatformJsonDelegate (ActiveFloor);
	}

	public static IBlock GetBlockAt( Vector3 position) {
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

    public static List<IBlock> GetBlocskAt(Vector3 position, float radius = 0.1f)
    {
        GameObject[] collidedGameObjects =
            Physics.OverlapSphere(position, radius)
                //.Except (new[] { GetComponent<BoxCollider> () })
                .Select(c => c.gameObject)
                .ToArray();

        List<IBlock> ret = new List<IBlock>();
        foreach (GameObject go in collidedGameObjects)
        {
            IBlock block = go.GetComponent<IBlock>();
            if (block != null)
                ret.Add(block);
        }
        return ret;
    }

    public static List<IBlock> GetBlocksOfType(string type = null)
    {
        List<IBlock> ret = new List<IBlock>();
        foreach (Transform child in ActiveFloor.transform)
        {
            IBlock block = child.gameObject.GetComponent<IBlock>();
            if (block != null && (type == null || block.TypeName == type))
                ret.Add(block);
        }
        return ret;
    }
}
