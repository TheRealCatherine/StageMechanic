/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{

	// Unity Inspector variables
	public GameObject Stage;                            //Keep this one
	public GameObject CursorPrefab;                     //Get 
	public GameObject BasicPlatformPrefab;              //Rid
	public GameObject StartLocationIndicator;           //Of
	public GameObject GoalLocationIndicator;            //These
	public AbstractBlockFactory[] BlockFactories;
	private List<KeyValuePair<string, string>> BlockTypeCache;



	/// <summary>
	/// There should only ever be one BlockManager in the scene - it manages all blocks for all platforms and loaded stages. As such it
	/// can be used statically via this property when accessing methods that are not already marked static.
	/// </summary>
	/// TODO Singleton flamewar, is there a better pattern in Unity for doing this?
	public static BlockManager Instance { get; private set; }


	/// <summary>
	/// Used by many classes to determine current game state, eventually this will be moved into
	/// a seperate GameManager class or something but for now this is pretty much THE way to
	/// determine if the application is currently in PlayMode or EditMode. Note that this property
	/// is read-pnly. Use BlockManager.TogglePlayMode() to change the application state to/from
	/// PlayMode and EditMode. This too will change in an upcoming revision.
	/// </summary>
	/// TODO: Move this out to a separate GameManager class
	public static bool PlayMode { get; private set; } = false;

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
			Serializer.RecordStartState();
			VisualEffectsManager.EnableFog(PlayerPrefs.GetInt("Fog", 1) == 1);
		}
		else
		{
			//Reset blocks to their pre-PlayMode state
			if (Serializer.HasStartState())
				Serializer.ReloadStartState();
			//UIManager.Instance.BlockInfoBox.gameObject.SetActive(true);
			VisualEffectsManager.EnableFog(false);
		}
		UIManager.RefreshButtonMappingDialog();
		PlayerManager.Instance.PlayMode = PlayMode;
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
	/// the block the player is standing on or sidled on (if sidling). Setting this property
	/// while in PlayMode has no effect.
	/// </summary>
	public static AbstractBlock ActiveBlock
	{
		get
		{
			if (PlayMode)
				return PlayerManager.Player(0)?.GameObject?.GetComponent<Cathy1PlayerCharacter>()?.CurrentBlock?.GameObject?.GetComponent<AbstractBlock>();
			else
				return GetBlockNear(Cursor.transform.position,0.01f,0.0f)?.GameObject?.GetComponent<AbstractBlock>();
		}
		set
		{
			if (!PlayMode)
				Cursor.transform.position = value.Position;
		}
	}

	/// <summary>
	/// Clears not only BlockManager but also PlayerManager, EventManager, the Serializer
	/// and everything else. Eventually most of this will be moved out to like a GameManager
	/// or similar class.
	/// </summary>
	public static void Clear()
	{
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
		Serializer.ClearUndoStates();
		Serializer.LastAccessedFileName = null;

		ActiveFloor.transform.position = Vector3.zero;
		Cursor.transform.position = new Vector3(0f, 1f, 0f);
	}

	/// <summary>
	/// Clears only the internal block cache (and destroys all the blocks in it)
	/// and clears the EventManager only.
	/// </summary>
	public static void ClearForUndo()
	{
		foreach (IBlock block in BlockCache)
		{
			Destroy(block.GameObject);
		}
		BlockCache.Clear();
		EventManager.Clear();
	}

	/// <summary>
	/// Convenience method for CreateBlockAt() that uses the current location of the cursor as the position.
	/// </summary>
	/// <param name="palette"></param>
	/// <param name="type"></param>
	/// <returns></returns>
	public static IBlock CreateBlockAtCursor(string palette, string type)
	{
		return CreateBlockAt(Cursor.transform.position, palette, type);
	}

	/// <summary>
	/// Convenience method for CreateBlockAt() that uses the current location of the cursor as the position.
	/// </summary>
	/// <param name="palette"></param>
	/// <param name="type"></param>
	/// <returns></returns>
	public static IBlock CreateBlockAtCursor(KeyValuePair<string, string> type)
	{
		return CreateBlockAt(Cursor.transform.position, type.Key, type.Value);
	}

	/// <summary>
	/// Attempts to create a block of a given type from the given block palette at the specified position. Currently
	/// only the "Cathy1 Internal" block palette is supported. If the palette is uknown or the block type
	/// requested is not part of the specified palette this will return null.
	/// </summary>
	/// <param name="position"></param>
	/// <param name="palette"></param>
	/// <param name="type"></param>
	/// <returns></returns>
	public static IBlock CreateBlockAt(Vector3 position, string palette, string type)
	{
		Debug.Assert(Instance != null);
		Debug.Assert(Cursor != null);
		if (palette == "Cathy1 Internal")
		{
			Cathy1Block block = Instance.BlockFactories[0].CreateBlock(position, Cursor.transform.rotation, type, ActiveFloor) as Cathy1Block;
			block.Palette = palette;
			block.gameObject.layer = Instance.Stage.gameObject.layer;
			BlockCache.Add(block);
			Serializer.AutoSave();
			return block;
		}
		else if(palette == "Bloxels Internal")
		{
			AbstractBloxelsBlock block = Instance.BlockFactories[2].CreateBlock(position, Cursor.transform.rotation, type, ActiveFloor) as AbstractBloxelsBlock;
			block.Palette = palette;
			block.gameObject.layer = Instance.Stage.gameObject.layer;
			BlockCache.Add(block);
			Serializer.AutoSave();
			return block;
		}
		else if(palette =="PushPull Internal")
		{
			AbstractPushPullBlock block = Instance.BlockFactories[1].CreateBlock(position, Cursor.transform.rotation, type, ActiveFloor) as AbstractPushPullBlock;
			block.Palette = palette;
			block.gameObject.layer = Instance.Stage.gameObject.layer;
			BlockCache.Add(block);
			Serializer.AutoSave();
			return block;
		}
		return null;
	}

	/// <summary>
	/// We probably want to do this another way - but right now this is used to deal with Block Groups
	/// and other edge cases (see what I did there).
	/// </summary>
	/// <param name="block"></param>
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
	/// The Cursor used in Edit Mode.
	/// </summary>
	/// TODO Right now this gets moved from the top level scene, do we want this behavior?
	private static GameObject _cursor;
	public static GameObject Cursor
	{
		get
		{
			return _cursor;
		}
		set
		{
			_cursor = value;
		}
	}

	private static int _blockCycleType = 0;
	public static KeyValuePair<string, string> BlockCycleType
	{
		get
		{
			return Instance.BlockTypeCache[_blockCycleType];
		}
		set
		{
			_blockCycleType = Instance.BlockTypeCache.IndexOf(value);
		}
	}

	private List<KeyValuePair<string, string>> GetAllBlockTypes()
	{
		if (BlockTypeCache != null)
			return BlockTypeCache;
		List<KeyValuePair<string, string>> ret = new List<KeyValuePair<string, string>>();
		foreach (AbstractBlockFactory factory in BlockFactories)
		{
			string[] blockNames = factory.BlockTypeNames;
			foreach (string name in blockNames)
			{
				ret.Add(new KeyValuePair<string, string>("Cathy1 Internal", name));
			}
		}
		BlockTypeCache = ret;
		return ret;
	}


	/// <summary>
	/// Used for cycling the default block type while in EditMode. This will be moved to
	/// Cathy1BlockFactory or similar class later.
	/// </summary>
	public static KeyValuePair<string, string> NextBlockType()
	{
		if ((++_blockCycleType) >= Instance.BlockTypeCache.Count)
		{
			_blockCycleType = 0;
			return BlockCycleType;
		}
		return BlockCycleType;
	}

	/// <summary>
	/// Used for cycling the default block type while in EditMode. This will be moved to
	/// Cathy1BlockFactory or similar class later.
	/// </summary>
	public static KeyValuePair<string, string> PrevBlockType()
	{
		if ((--_blockCycleType) < 0)
		{
			_blockCycleType = Instance.BlockTypeCache.Count - 1;
			return BlockCycleType;
		}
		return BlockCycleType;
	}

	#region Monobehavior implementations

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		//TODO pretty much none of this should be here. This is some of the very earliest code in the
		//whole project. BlockManager shouldn't be reponsible for managing platforms or the cursor.
		ActiveFloor = Instantiate(BasicPlatformPrefab, new Vector3(0, 0f, 3f), new Quaternion(0, 0, 0, 0));
		ActiveFloor.name = "Platform";
		ActiveFloor.transform.SetParent(Stage.transform, false);
		RotatableFloors.Add(ActiveFloor);
		Cursor = CursorPrefab;
		Cursor.transform.SetParent(Stage.transform, false);
		Cursor.SetActive(true);
		GetAllBlockTypes(); //Cache all palettes and block types
	}
	#endregion

	public void DestroyActiveObject()
	{
		if (ActiveBlock != null)
		{
			if (ActiveBlock.GetComponent<IBlock>() != null)
				DestroyBlock(ActiveBlock.GetComponent<IBlock>());
			else
				Destroy(ActiveBlock.gameObject);
			Serializer.AutoSave();
		}
	}

	public static AbstractBlock GetBlockAt(Vector3 position, float radius = 0.01f, float minDensity = 1f)
	{
		AbstractBlock ret = Utility.GetGameObjectAt<AbstractBlock>(position, radius);
		if (ret) {
			if (ret.DensityFactor >= minDensity)
				return ret;
			foreach(AbstractBlock block in GetBlocksNear(position,radius))
			{
				if (block.DensityFactor >= minDensity)
					return block;
			}
		}
		return null;
	}

	public static AbstractBlock GetBlockNear(Vector3 position, float radius = 0.01f, float minDensity = 1f)
	{
		AbstractBlock ret = Utility.GetGameObjectNear<AbstractBlock>(position, radius);
		if (ret)
		{
			if (ret.DensityFactor >= minDensity)
				return ret;
			foreach (AbstractBlock block in GetBlocksNear(position, radius))
			{
				if (block.DensityFactor >= minDensity)
					return block;
			}
		}
		return null;
	}

	public static List<AbstractBlock> GetBlocksNear(Vector3 position, float radius = 0.01f)
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
		if (groupNumber < 0)
		{
			if (blockToGroupMapping.ContainsKey(block))
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
		else if (!blockToGroupMapping.ContainsKey(block))
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

	public static bool CanPushGroup(int groupNumber, Vector3 direction, int distance = 1)
	{
		Debug.Assert(groupNumber >= 0);
		Debug.Assert(blockGroups.ContainsKey(groupNumber));
		foreach (IBlock block in BlockGroup(groupNumber))
		{
			if (!block.CanBePushed(direction, distance))
				return false;
		}
		return true;
	}

	public static bool PushGroup(int groupNumber, Vector3 direction, int distance = 1)
	{
		if (!CanPushGroup(groupNumber, direction, distance))
			return false;

		foreach (IBlock block in BlockGroup(groupNumber))
		{

			IBlock neighbor = GetBlockAt(block.Position + direction);
			if (neighbor != null)
			{
				if (BlockGroupNumber(neighbor) < 0)
					neighbor.Push(direction, distance);
				else if (BlockGroupNumber(neighbor) != groupNumber)
					PushGroup(BlockGroupNumber(neighbor), direction, distance);
			}
			AbstractBlock ab = block.GameObject?.GetComponent<AbstractBlock>();
			if (ab == null)
				block.Position += direction;
			else
				ab.StartCoroutine(ab.AnimateMove(ab.Position, ab.Position + direction, 0.2f * ab.PushWeight(direction, distance), true));

		}
		return true;
	}

	public static bool CanPullGroup(int groupNumber, Vector3 direction, int distance = 1)
	{
		Debug.Assert(groupNumber >= 0);
		Debug.Assert(blockGroups.ContainsKey(groupNumber));
		foreach (IBlock block in BlockGroup(groupNumber))
		{
			if (!block.CanBePulled(direction, distance))
				return false;
		}
		return true;
	}

	public static bool PullGroup(int groupNumber, Vector3 direction, int distance = 1)
	{
		if (!CanPullGroup(groupNumber, direction, distance))
			return false;

		foreach (IBlock block in BlockGroup(groupNumber))
		{

			IBlock neighbor = GetBlockAt(block.Position + direction);
			if (neighbor != null)
			{
				if (BlockGroupNumber(neighbor) < 0)
					neighbor.Pull(direction, distance);
				else if (BlockGroupNumber(neighbor) != groupNumber)
					PullGroup(BlockGroupNumber(neighbor), direction, distance);
			}
			AbstractBlock ab = block.GameObject?.GetComponent<AbstractBlock>();
			if (ab == null)
				block.Position += direction;
			else
				ab.StartCoroutine(ab.AnimateMove(ab.Position, ab.Position + direction, 0.2f * ab.PullWeight(direction, distance), false));

		}
		return true;
	}

	#endregion

	//TODO Move these to some kind of Platform Manager
	#region Platform management

	public static PlatformJsonDelegate GetPlatformJsonDelegate()
	{
		return new PlatformJsonDelegate(ActiveFloor);
	}
	public static PlatformBinaryDelegate GetPlatformBinaryDelegate()
	{
		return new PlatformBinaryDelegate(ActiveFloor);
	}

	/// <summary>
	/// Used to support Cathy-2 style rotatable floors and other multi-platform implementations
	/// This is to be implemented in the future
	/// Right now it only contains the BlockManager.ActiveFloor
	/// </summary>
	private List<GameObject> _rotatableFloors = new List<GameObject>();
	public List<GameObject> RotatableFloors
	{
		get
		{
			return _rotatableFloors;
		}
		set
		{
			_rotatableFloors = value;
		}
	}

	/// <summary>
	/// Currently this will always be the platform on which the stage rests. When BlockManager.RotatableFloors
	/// is implemented later this will be set to the platform currently selected by the cursor or occupied by
	/// the player.
	/// </summary>
	private static GameObject _activeFloor;
	public static GameObject ActiveFloor
	{
		get
		{
			return _activeFloor;
		}
		set
		{
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
	#endregion
}
