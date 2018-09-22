using MoonSharp.Interpreter;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LuaProxy_ItemManager
{
	public int count
	{
		get
		{
			return ItemManager.ItemCount;
		}
	}

	public List<KeyValuePair<string, string>> types
	{
		get
		{
			//TODO Item Manager doesnt seem to have an analogue to ItemManager.Instance.GetAllBlockTypes();
			return null;
		}
	}

	public AbstractItem at(float x, float y, float z)
	{
		return ItemManager.GetItemNear(new Vector3(x, y, z), 0.1f);
	}

	public AbstractItem at(Vector3 position)
	{
		return ItemManager.GetItemNear(position, 0.1f);
	}

	public void clear()
	{
		ItemManager.Clear();
	}

	public AbstractItem create(float x, float y, float z, string type = "Coin", string palette = "Cat5 Internal")
	{
		return ItemManager.CreateItemAt(new Vector3(x, y, z), palette, type) as AbstractItem;
	}

	public AbstractItem create(Vector3 position, string type = "Coin", string palette = "Cat5 Internal")
	{
		return ItemManager.CreateItemAt(position, palette, type) as AbstractItem;
	}

	public void destroy(AbstractItem item)
	{
		ItemManager.DestroyItem(item);
	}

	public bool destroy(string itemName)
	{
		if (GameObject.Find(itemName).GetComponent<AbstractItem>() is AbstractItem item) {
			destroy(item);
			return true;
		}
		return false;	
	}

	public List<AbstractItem> getall(string type = null)
	{
		return ItemManager.GetItemsOfType(type).Cast<AbstractItem>().ToList();
	}
}
