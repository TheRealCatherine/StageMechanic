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
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Xml;
using UnityEngine;

public class BlockManager : MonoBehaviour {

    #region Serialization
    /// TODO: Consider relocating UNDO related

    /// <summary>
    /// Used internally to record information about an undo state,
    /// this includes all blocks, their states and positions as well
    /// as player states and positions.
    /// </summary>
    public struct UndoState
    {
        public enum DataType
        {
            Unknown = 0,
            Json,
            Binary
        }

        public byte[] BlockState;
        public DataType Type;
        public Vector3 PlayerPosition;
        public Vector3 PlayerFacingDirection;
        public int PlayerStateIndex;
        public float PlatformYPosition;
    }

    public static int MaxUndoLevels = 99;
    private static List<UndoState> _undoStates = new List<UndoState>();
    private static List<UndoState> _redoStates = new List<UndoState>();
    private static string _startState;
    private static string _lastCheckpointState;

    private static bool _undoEnabled = true;
    public static bool UndoEnabled
    {
        get
        {
            return _undoEnabled;
        }
        set
        {
            _undoEnabled = value;
            if(value)
                LogController.Log(MaxUndoLevels + " Undos On");
            else
                LogController.Log("Undo off");
        }
    }

    /// <summary>
    /// Used for auto-saving and saving while creating stages as
    /// well as reloading a level from a file.
    /// </summary>
    public string LastAccessedFileName;

    /// <summary>
    /// When turned on, BlockManager will record BlockManager.MaxUndoLevels worth of states.
    /// Note that this is the same as setting the BlockManager.UndoEnabled property to its
    /// inverse.
    /// </summary>
    /// code to another class
    public static void ToggleUndoEnabled()
    {
        UndoEnabled = !UndoEnabled;
    }

    public static void ClearUndoStates()
    {
        _undoStates.Clear();
    }

    public static void RecordStartState()
    {
        _startState = Instance.BlocksToCondensedJson();
    }

    public static void ReloadStartState()
    {
        if (!string.IsNullOrWhiteSpace(_startState))
        {
            Clear();
            Instance.BlocksFromJson(_startState);
        }
    }

    public static void RecordUndo()
    {
        if (!UndoEnabled)
            return;
        try
        {
            if (_undoStates.Count > MaxUndoLevels)
            {
                _undoStates.RemoveAt(0);
            }
            UndoState state = new UndoState
            {
                //TODO support binary
                BlockState = Instance.BlocksToBinaryStream(),
                Type = UndoState.DataType.Binary,
                PlayerPosition = PlayerManager.Player1Location(),
                PlayerFacingDirection = PlayerManager.Player1FacingDirection(),
                PlayerStateIndex = PlayerManager.PlayerState(),
                PlatformYPosition = ActiveFloor.transform.position.y
            };
            Debug.Assert(state.BlockState != null);
            _undoStates.Add(state);
        }
        catch (Exception e)
        {
            Debug.LogAssertion(e.Message);
        }
    }

    public static int AvailableUndoCount { get { if (!UndoEnabled) return 0; return _undoStates.Count; } }
    public static int AvailableRedoCount { get { if (!UndoEnabled) return 0; return _redoStates.Count; } }

    public static void Undo()
    {
        if (!UndoEnabled)
            return;
        if (_undoStates.Count > 0)
        {
            Instance.ClearForUndo();
            PlayerManager.HideAllPlayers();
            UndoState state = _undoStates[_undoStates.Count - 1];
            ActiveFloor.transform.position = new Vector3(0f, state.PlatformYPosition, 0f);
            if(state.Type == UndoState.DataType.Json)
                Instance.BlocksFromJsonStream(state.BlockState);
            else if (state.Type == UndoState.DataType.Binary)
                Instance.BlocksFromBinaryStream(state.BlockState);

            PlayerManager.SetPlayer1State(state.PlayerStateIndex);
            PlayerManager.SetPlayer1FacingDirection(state.PlayerFacingDirection);
            PlayerManager.SetPlayer1Location(state.PlayerPosition);
            _undoStates.RemoveAt(_undoStates.Count - 1);
            PlayerManager.ShowAllPlayers();
            LogController.Log("Undo");
        }
        else
            LogController.Log("No undos left");
    }

    //TODO
    public static void Redo()
    {

    }

    public void ClearForUndo()
    {
        BlockManagerState oldState = State;
        State = BlockManagerState.Clearing;
        foreach(IBlock block in BlockCache)
        {
            Destroy(block.GameObject);
        }
        BlockCache.Clear();
        EventManager.Clear();
        State = oldState;
    }

    public string BlocksToPrettyJson()
    {
        BlockManagerState oldState = State;
        State = BlockManagerState.Saving;
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
            XmlDictionaryWriter writer = JsonReaderWriterFactory.CreateJsonWriter(ms, Encoding.UTF8, true, true, "    ");
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
            State = oldState;
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

    public byte[] BlocksToCondensedJsonStream()
    {
        Debug.Assert(ActiveFloor != null);
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
            return ms.ToArray();
        }
        catch (System.Exception exception)
        {
            LogController.Log(exception.ToString());
        }
        finally
        {
            Thread.CurrentThread.CurrentCulture = currentCulture;
        }
        return null;
    }

    public byte[] BlocksToBinaryStream()
    {
        Debug.Assert(ActiveFloor != null);
        StageBinaryDelegate stage = new StageBinaryDelegate();
        StageCollectionBinaryDelegate collection = new StageCollectionBinaryDelegate(stage);

        try
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(ms,collection);
            ms.Close();
            return ms.ToArray();
        }
        catch (System.Exception exception)
        {
            LogController.Log(exception.ToString());
        }
        return null;
    }

    public void SaveToJson()
    {
        GameObject fileBrowserObject = Instantiate(FileBrowserPrefab, this.transform);
        fileBrowserObject.name = "FileBrowser";
        FileBrowser fileBrowserScript = fileBrowserObject.GetComponent<FileBrowser>();
        fileBrowserScript.SetupFileBrowser(ViewMode.Landscape, PlayerPrefs.GetString("LastSaveDir"));
        fileBrowserScript.SaveFilePanel(this, "SaveFileUsingPath", "MyLevels", "json");
    }

    public void QuickSave()
    {
        if (string.IsNullOrWhiteSpace(LastAccessedFileName))
            SaveToJson();
        else
            SaveFileUsingPath(LastAccessedFileName);
    }

    public void AutoSave()
    {

        if (!PlayMode && !string.IsNullOrWhiteSpace(LastAccessedFileName))
        {
            SaveFileUsingPath(LastAccessedFileName.Replace(".json", "_autosave.json"));
            LogController.Log("Autosaved");
        }
    }

    public void LoadFromJson()
    {
        GameObject fileBrowserObject = Instantiate(FileBrowserPrefab, this.transform);

        fileBrowserObject.name = "FileBrowser";
        FileBrowser fileBrowserScript = fileBrowserObject.GetComponent<FileBrowser>();
        fileBrowserScript.SetupFileBrowser(ViewMode.Landscape, PlayerPrefs.GetString("LastLoadDir"));

        fileBrowserScript.OpenFilePanel(this, "LoadFileUsingPath", "json");
    }

    public void BlocksFromJson(Uri path)
    {
        LogController.Log("Loading from " + path.ToString());
        StageCollection deserializedCollection = new StageCollection(this);
        WebClient webClient = new WebClient();
        Stream fs = webClient.OpenRead(path);
        HandleLoad(fs, true);
        RecordStartState();
        if (PlayerPrefs.GetInt("AutoPlayOnLoad", 0) == 1)
        {
            if (!PlayMode)
                TogglePlayMode();
        }
    }

    public void HandleLoad(Stream stream, bool clearFirst = true)
    {
        if (clearFirst)
            Clear();
        BlockManagerState oldState = State;
        State = BlockManagerState.Loading;
        StageCollection deserializedCollection = new StageCollection(this);
        DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedCollection.GetType());
        deserializedCollection = ser.ReadObject(stream) as StageCollection;
        stream.Close();
        LogController.Log("Loaded " + deserializedCollection.Stages.Count + " stage(s)");
        State = oldState;
    }

    public void HandleBinaryLoad(Stream stream, bool clearFirst = true)
    {
        if (clearFirst)
            Clear();
        BlockManagerState oldState = State;
        State = BlockManagerState.Loading;
        BinaryFormatter formatter = new BinaryFormatter();
        StageCollection deserializedCollection = formatter.Deserialize(stream) as StageCollection;
        stream.Close();
        State = oldState;
    }


    public void BlocksFromJson(string json)
    {
        MemoryStream stream = new MemoryStream();
        StreamWriter writer = new StreamWriter(stream);
        writer.Write(json);
        writer.Flush();
        stream.Position = 0;
        HandleLoad(stream, false);
    }

    public void BlocksFromJsonStream(byte[] bytes)
    {
        Debug.Assert(bytes != null);
        MemoryStream stream = new MemoryStream(bytes);
        stream.Position = 0;
        HandleLoad(stream, false);
    }

    public void BlocksFromBinaryStream(byte[] bytes)
    {
        Debug.Assert(bytes != null);
        MemoryStream stream = new MemoryStream(bytes);
        stream.Position = 0;
        HandleBinaryLoad(stream, false);
    }

    // Saves a file with the textToSave using a path
    private void SaveFileUsingPath(string path)
    {
        if (!string.IsNullOrWhiteSpace(path))
        {
            Uri location = new Uri("file:///" + path);
            string directory = System.IO.Path.GetDirectoryName(location.AbsolutePath);
            PlayerPrefs.SetString("LastSaveDir", directory);
            string json = BlocksToPrettyJson();
            //TODO this probably can throw an exception?
            if (!string.IsNullOrWhiteSpace(json))
                System.IO.File.WriteAllText(path, json);
            if (!path.Contains("_autosave."))
            {
                LastAccessedFileName = path;
                File.Delete(path.Replace(".json", "_autosave.json"));
                LogController.Log("Saved & Autosave");
            }
        }
        else
        {
            LogController.Log("Invalid path");
        }
    }

    // Loads a file using a path
    private void LoadFileUsingPath(string path)
    {
        //TODO ensure file is valid
        if (!string.IsNullOrWhiteSpace(path))
        {
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
        }
        else
        {
            LogController.Log("Invalid path");
        }
    }

    public void ReloadCurrentLevel()
    {
        LoadFileUsingPath(LastAccessedFileName);
    }

    public bool TryReloadCurrentLevel()
    {
        if (!string.IsNullOrWhiteSpace(LastAccessedFileName))
        {
            ReloadCurrentLevel();
            return true;
        }
        return false;
    }

    public PlatformJsonDelegate GetPlatformJsonDelegate()
    {
        return new PlatformJsonDelegate(ActiveFloor);
    }
    public PlatformBinaryDelegate GetPlatformBinaryDelegate()
    {
        return new PlatformBinaryDelegate(ActiveFloor);
    }
    #endregion

    public enum BlockManagerState
    {
        Initializing,
        EditMode,
        Clearing,
        Loading,
        Saving,
        PlayMode
    }

    public BlockManagerState State = BlockManagerState.Initializing;

    // Unity Inspector variables
    public GameObject CursorPrefab;
    public GameObject BasicPlatformPrefab;
    public GameObject StartLocationIndicator;
    public GameObject GoalLocationIndicator;
    public GameObject FileBrowserPrefab;
    public ParticleSystem Fog;


    /// <summary>
    /// There should only ever be one BlockManager in the scene - it manages all blocks for all platforms and loaded stages. As such it
    /// can be used statically via this property when accessing methods that are not already marked static.
    /// </summary>
    /// TODO Singleton flamewar
    public static BlockManager Instance { get; private set; }


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
            State = BlockManagerState.PlayMode;
            Fog.gameObject.SetActive(PlayerPrefs.GetInt("Fog", 0) == 1);
        }
        else
        {
            //Reset blocks to their pre-PlayMode state
            if (!string.IsNullOrWhiteSpace(_startState))
                ReloadStartState();
            UIManager.Instance.BlockInfoBox.gameObject.SetActive(true);
            State = BlockManagerState.EditMode;
            Fog.gameObject.SetActive(false);
        }
        UIManager.RefreshButtonMappingDialog();
        GetComponent<PlayerManager>().PlayMode = PlayMode;
        Cursor.SetActive(!PlayMode);

    }

    #region BlockAccounting
    internal static List<IBlock> BlockCache = new List<IBlock>();

    /// <summary>
    /// Read-only property that technically returns the number of blocks
    /// in the internal cache.
    /// </summary>
    public static int BlockCount
    {
        get
        {
            return BlockCache.Count;
        }
    }

    /// <summary>
    /// When in EditMode, returns the block, if any, that is currently under the cursor,
    /// or null if the cursor is not on a block. Setting this property in EditMode will
    /// move the cursor to the position of the block.
    /// 
    /// When in PlayMode, returns the block associated with player 1, this will be either
    /// the block the player is standing on or sidled on (if sidling)
    /// </summary>
    /// TODO Support IBlock interface
    public Cathy1Block ActiveBlock
    {
        get
        {
            if (PlayMode)
                return PlayerManager.Player(0)?.GameObject?.GetComponent<Cathy1PlayerCharacter>()?.CurrentBlock?.GameObject?.GetComponent<Cathy1Block>();
            else
                return GetBlockAt(Cursor.transform.position)?.GameObject?.GetComponent<Cathy1Block>();
        }
        set
        {
            if(!PlayMode)
                Cursor.transform.position = value.Position;
        }
    }

    public static void Clear()
    {
        BlockManagerState oldState = Instance.State;
        Instance.State = BlockManagerState.Clearing;

        //Clear all cached data
        foreach (IBlock block in BlockCache)
        {
            Destroy(block.GameObject);
        }
        BlockCache.Clear();
        blockGroups.Clear();
        blockToGroupMapping.Clear();
        PlayerManager.Clear();
        EventManager.Clear();
        ClearUndoStates();
        Instance.LastAccessedFileName = null;

        ActiveFloor.transform.position = Vector3.zero;
        Cursor.transform.position = new Vector3(0f, 1f, 0f);
        Instance.State = oldState;
    }

    // Create a basic block at the current cursor position

    public static IBlock CreateBlockAtCursor(Cathy1Block.BlockType type = Cathy1Block.BlockType.Basic)
    {
        Debug.Assert(Instance != null);
        Debug.Assert(Cursor != null);
        Cathy1Block block = Instance.GetComponent<Cathy1BlockFactory>().CreateBlock(Cursor.transform.position, Cursor.transform.rotation, type, ActiveFloor) as Cathy1Block;
        BlockCache.Add(block);
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
            BlockCache.Add(block);
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

    public static void DestroyBlock(IBlock block)
    {
        if (blockToGroupMapping.ContainsKey(block))
        {
            Debug.Assert(blockGroups.ContainsKey(blockToGroupMapping[block]));
            blockGroups[blockToGroupMapping[block]].Remove(block);
            blockToGroupMapping.Remove(block);
        }
        BlockCache.Remove(block);
        block.GameObject.SetActive(false);
        Destroy(block.GameObject);
    }

    #endregion

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
    private static Cathy1Block.BlockType _blockCycleType = Cathy1Block.BlockType.Basic;
    public static Cathy1Block.BlockType BlockCycleType {
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
    public static Cathy1Block.BlockType NextBlockType() {
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
    public static Cathy1Block.BlockType PrevBlockType() {
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

    public static void RotatePlatform(int x, int y, int z)
    {
        ActiveFloor.transform.Rotate(x, y, z, Space.Self);
        Instance.StartCoroutine(Instance.rotateCleanup());
    }

    IEnumerator rotateCleanup()
    {
        yield return new WaitForEndOfFrame();
        IBlock[] blocks = ActiveFloor.GetComponentsInChildren<IBlock>();
        yield return new WaitForEndOfFrame();
        Debug.Log("rotating " + blocks.Length + " blocks");
        foreach (IBlock block in blocks)
        {
            block.GameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            block.Rotation = Quaternion.identity;
            (block as AbstractBlock).SetGravityEnabledByMotionState();
        }
        yield return new WaitForEndOfFrame();
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
        if (UIManager.Instance.MainMenu.isActiveAndEnabled)
            Cursor.SetActive(false);
    }


    

    public void RandomizeGravity()
    {
        System.Random randomNumberGenerator = new System.Random(new System.DateTime().Millisecond);

        foreach (Transform child in ActiveFloor.transform)
        {
            IBlock block = child.gameObject.GetComponent<IBlock>();
            if (block != null)
                block.GravityFactor = randomNumberGenerator.Next(-100,100)/100f;
        }
        AutoSave();
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

	


    public void DestroyActiveObject()
    {
        if (ActiveObject != null)
        {
            if (ActiveObject.GetComponent<IBlock>() != null)
                DestroyBlock(ActiveObject.GetComponent<IBlock>());
            else
                Destroy(ActiveObject);
            AutoSave();
        }
    }

	

	public static AbstractBlock GetBlockAt( Vector3 position, float radius = 0.01f) {

        return Utility.GetGameObjectAt<AbstractBlock>(position, radius);
	}

    public static AbstractBlock GetBlockNear(Vector3 position, float radius = 0.01f)
    {
        return Utility.GetGameObjectNear<AbstractBlock>(position, radius);
    }

    public static List<AbstractBlock> GetBlocskNear(Vector3 position, float radius = 0.01f)
    {
        return Utility.GetGameObjectsNear<AbstractBlock>(position, radius);
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

    #region Block groups
    static Dictionary<IBlock, int> blockToGroupMapping = new Dictionary<IBlock, int>();
    static Dictionary<int, List<IBlock>> blockGroups = new Dictionary<int, List<IBlock>>();

    public static List<IBlock> BlockGroup(int groupNumber)
    {
        if (groupNumber < 0)
            return new List<IBlock>();
        Debug.Assert(blockGroups.ContainsKey(groupNumber));
        return blockGroups[groupNumber];
    }

    public static void AddBlockToGroup(IBlock block, int groupNumber)
    {
        Debug.Assert(block != null);
        if(groupNumber < 0)
        {
            if(blockToGroupMapping.ContainsKey(block))
            {
                blockGroups[blockToGroupMapping[block]].Remove(block);
                blockToGroupMapping.Remove(block);
                return;
            }
            return;
        }
        if (blockToGroupMapping.ContainsKey(block) && blockToGroupMapping[block] != groupNumber)
        {
            Debug.Assert(blockGroups.ContainsKey(blockToGroupMapping[block]));
            blockGroups[blockToGroupMapping[block]].Remove(block);
            if (!blockGroups.ContainsKey(groupNumber))
                blockGroups.Add(groupNumber, new List<IBlock>());
            if (blockGroups[groupNumber] == null)
                blockGroups[groupNumber] = new List<IBlock>();
            blockGroups[groupNumber].Add(block);
            blockToGroupMapping[block] = groupNumber;
            cakeslice.Outline outline = block.GameObject.GetComponent<cakeslice.Outline>();
            if (outline == null)
                outline = block.GameObject.GetComponentInChildren<cakeslice.Outline>();
            if (outline != null)
            {
                outline.enabled = true;
                outline.color = groupNumber;
            }
        }
        else if(!blockToGroupMapping.ContainsKey(block))
        {
            blockToGroupMapping.Add(block, groupNumber);
            if (!blockGroups.ContainsKey(groupNumber))
                blockGroups.Add(groupNumber, new List<IBlock>());
            if (blockGroups[groupNumber] == null)
                blockGroups[groupNumber] = new List<IBlock>();
            blockGroups[groupNumber].Add(block);
            cakeslice.Outline outline = block.GameObject.GetComponent<cakeslice.Outline>();
            if (outline == null)
                outline = block.GameObject.GetComponentInChildren<cakeslice.Outline>();
            if (outline != null)
            {
                outline.enabled = true;
                outline.color = groupNumber;
            }
        }
    }

    public static void RemoveBlockFromGroup(IBlock block)
    {
        AddBlockToGroup(block, -1);
    }

    public static int BlockGroupNumber(IBlock block)
    {
        if (blockToGroupMapping.ContainsKey(block))
            return blockToGroupMapping[block];
        return -1;
    }

    public static bool CanMoveGroup(int groupNumber, Vector3 direction, int distance = 1)
    {
        Debug.Assert(groupNumber >= 0);
        Debug.Assert(blockGroups.ContainsKey(groupNumber));
        foreach(IBlock block in BlockGroup(groupNumber))
        {
            if (!block.CanBeMoved(direction, distance))
                return false;
        }
        return true;
    }

    public static bool MoveGroup(int groupNumber, Vector3 direction, int distance = 1)
    {
        if (!CanMoveGroup(groupNumber, direction, distance))
            return false;

        foreach(IBlock block in BlockGroup(groupNumber))
        {

            IBlock neighbor = GetBlockAt(block.Position + direction);
            if (neighbor != null)
            {
                if (BlockGroupNumber(neighbor) < 0)
                    neighbor.Move(direction, distance);
                else if (BlockGroupNumber(neighbor) != groupNumber)
                    MoveGroup(BlockGroupNumber(neighbor), direction, distance);
            }
            AbstractBlock ab = block.GameObject?.GetComponent<AbstractBlock>();
            if(ab == null)
                block.Position += direction;
            else
                ab.StartCoroutine(ab.AnimateMove(ab.Position, ab.Position + direction, 0.2f * ab.MoveWeight(direction, distance)));

        }
        return true;
    }

    public static bool CanBeMoved(IBlock block, Vector3 direction, int distance = 1)
    {
        if (BlockGroupNumber(block) < 0)
            return block.CanBeMoved(direction, distance);
        return CanMoveGroup(BlockGroupNumber(block),direction,distance);
    }

    public static bool Move(IBlock block, Vector3 direction, int distance = 1)
    {
        if (BlockGroupNumber(block) < 0)
            return block.Move(direction, distance);
        return MoveGroup(BlockGroupNumber(block), direction, distance);
    }
    #endregion


    #region SoundsAndEffects
    private IEnumerator _particleAnimationHelper(Vector3 position, ParticleSystem animationPrefab, float scale, float duration, Quaternion rotation)
    {
        ParticleSystem system = Instantiate(animationPrefab, position, rotation, transform);
        ParticleSystem.MainModule module = system.main;
        if (scale != 1.0f)
        {
            module.scalingMode = ParticleSystemScalingMode.Hierarchy;
            system.transform.localScale = new Vector3(scale, scale, scale);
        }
        if(duration > 0)
            module.simulationSpeed = (module.duration / duration);
      
        system.Play();
        yield return new WaitForSeconds(system.main.duration);
        system.Stop();
        Destroy(system.gameObject);
    }

    public static void PlayEffect(IBlock block, ParticleSystem animationPrefab, float scale = 1f, float duration = -1f, Vector3 offset = default(Vector3), Quaternion rotation = default(Quaternion))
    {
        Debug.Assert(animationPrefab != null);
        if (duration == 0 || scale == 0)
            return;
        Quaternion rot = rotation;
        if(rotation != Quaternion.identity)
        {
            Vector3 lookDirection = (block.Position + offset) - block.Position;
            rot = Quaternion.LookRotation(lookDirection);
            rot *= rotation;
        }
        Instance.StartCoroutine(Instance._particleAnimationHelper(block.Position + offset, animationPrefab, scale, duration, rot));
    }

    private IEnumerator _soundHelper(AudioClip clip, Vector3 position, float volume)
    {
        //AudioSource.PlayClipAtPoint(clip, position, volume);
        //TODO figure out why the above is OMFG quiet AF
        GetComponent<AudioSource>().PlayOneShot(clip, volume);
        yield return new WaitForSeconds(clip.length);
    }

    public static void PlaySound(IBlock block, AudioClip sound, float volume = 1f)
    {
        Debug.Assert(block != null);
        Debug.Assert(sound != null);
        Instance.StartCoroutine(Instance._soundHelper(sound, block.Position,volume));
    }
    #endregion
}
