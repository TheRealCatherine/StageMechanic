using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{

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

	public static List<AbstractItem> GetBlocksNear(Vector3 position, float radius = 0.01f)
	{
		return Utility.GetGameObjectsNear<AbstractItem>(position, radius);
	}

	public static List<IItem> GetBlocksOfType(string type = null)
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
}
