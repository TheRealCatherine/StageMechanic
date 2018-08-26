using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{

	public AbstractItemFactory[] ItemFactories;
	private List<KeyValuePair<string, string>> ItemTypeCache;

	/// <summary>
	/// There should only ever be one ItemManager in the scene - it manages all items for all platforms and loaded stages. As such it
	/// can be used statically via this property when accessing methods that are not already marked static.
	/// </summary>
	/// TODO Singleton flamewar, is there a better pattern in Unity for doing this?
	public static ItemManager Instance { get; private set; }

	public static AbstractItem GetItemAt(Vector3 position, float radius = 0.01f)
	{
		return  Utility.GetGameObjectAt<AbstractItem>(position, radius);
	}

	public static AbstractItem GetItemNear(Vector3 position, float radius = 0.01f)
	{
		return Utility.GetGameObjectNear<AbstractItem>(position, radius);
	}

	public static List<AbstractItem> GetItemsNear(Vector3 position, float radius = 0.01f)
	{
		return Utility.GetGameObjectsNear<AbstractItem>(position, radius);
	}

	public static List<IItem> GetItemsOfType(string type = null)
	{
		List<IItem> ret = new List<IItem>();
		foreach (Transform child in BlockManager.ActiveFloor.transform)
		{
			IItem item = child.gameObject.GetComponent<IItem>();
			if (item != null && (type == null || item.TypeName == type))
				ret.Add(item);
		}
		return ret;
	}

	#region ItemAccounting
	internal static List<IItem> ItemCache = new List<IItem>();

	/// <summary>
	/// Read-only property that technically returns the number of items
	/// in the internal cache.
	/// </summary>
	public static int ItemCount
	{
		get
		{
			return ItemCache.Count;
		}
	}

	/// <summary>
	/// When in EditMode, returns the item, if any, that is currently under the cursor,
	/// or null if the cursor is not on an item. Setting this property in EditMode will
	/// move the cursor to the position of the item.
	/// 
	/// When in PlayMode, returns the item associated with player 1.
	/// Setting this property while in PlayMode has no effect.
	/// </summary>
	public static AbstractItem ActiveItem
	{
		get
		{
			if (BlockManager.PlayMode)
				return PlayerManager.Player(0)?.GameObject?.GetComponent<Cathy1PlayerCharacter>()?.CurrentBlock?.GameObject?.GetComponent<AbstractItem>();
			else
				return GetItemNear(BlockManager.Cursor.transform.position, 0.01f)?.GameObject?.GetComponent<AbstractItem>();
		}
		set
		{
			if (!BlockManager.PlayMode)
				BlockManager.Cursor.transform.position = value.Position;
		}
	}

	/// <summary>
	/// Clears the item cache and destroys all items in the stage
	/// </summary>
	public static void Clear()
	{
		//Clear all cached data
		foreach (IItem item in ItemCache)
		{
			Destroy(item.GameObject);
		}
		ItemCache.Clear();
	}

	/// <summary>
	/// Currently the same as Clear();
	/// </summary>
	public static void ClearForUndo()
	{
		Clear();
	}

	/// <summary>
	/// Convenience method for CreateItemAt() that uses the current location of the cursor as the position.
	/// </summary>
	/// <param name="palette"></param>
	/// <param name="type"></param>
	/// <returns></returns>
	public static IItem CreateItemAtCursor(string palette, string type)
	{
		return CreateItemAt(BlockManager.Cursor.transform.position, palette, type);
	}

	/// <summary>
	/// Convenience method for CreateItemAt() that uses the current location of the cursor as the position.
	/// </summary>
	/// <param name="palette"></param>
	/// <param name="type"></param>
	/// <returns></returns>
	public static IItem CreateBlockAtCursor(KeyValuePair<string, string> type)
	{
		return CreateItemAt(BlockManager.Cursor.transform.position, type.Key, type.Value);
	}

	/// <summary>
	/// Attempts to create a item of a given type from the given block palette at the specified position. 
	/// If the palette is uknown or the block type
	/// requested is not part of the specified palette this will return null.
	/// </summary>
	/// <param name="position"></param>
	/// <param name="palette"></param>
	/// <param name="type"></param>
	/// <returns></returns>
	/// TODO don't hardcode item factories here
	public static IItem CreateItemAt(Vector3 position, string palette, string type)
	{
		Debug.Assert(Instance != null);
		Debug.Assert(BlockManager.Cursor != null);
		if (palette == "Cathy1 Internal" || palette == "Cathy Internal" || palette == "Cat5 Internal")
		{
			GameObject parent = BlockManager.ActiveFloor;
			AbstractBlock blockBelow = BlockManager.GetBlockNear(position + Vector3.down);
			if (blockBelow != null)
				parent = blockBelow.GameObject;

			AbstractItem item = Instance.ItemFactories[0].CreateItem(position, BlockManager.Cursor.transform.rotation, type, parent) as AbstractItem;
			if (blockBelow != null)
				item.OwningBlock = blockBelow;
			item.Palette = palette;
			item.gameObject.layer = BlockManager.Instance.Stage.gameObject.layer;
			ItemCache.Add(item);
			Serializer.AutoSave();
			return item;
		}
		return null;
	}

	//TODO don't hardcode these
	public static int ItemFactoryIndex(string palette)
	{
		switch (palette)
		{
			case "Cathy1 Internal":
			case "Cathy Internal":
			case "Cat5 Internal":
			default:
				return 0;
		}
	}

	/// <summary>
	/// </summary>
	/// <param name="block"></param>
	public static void DestroyItem(IItem item)
	{
		ItemCache.Remove(item);
		item.GameObject.SetActive(false);
		Destroy(item.GameObject);
	}

	#endregion


	#region Monobehavior implementations

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		//GetAllItemTypes(); //Cache all palettes and block types
	}
	#endregion

}
