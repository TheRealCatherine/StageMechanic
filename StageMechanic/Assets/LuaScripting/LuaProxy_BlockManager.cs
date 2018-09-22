using MoonSharp.Interpreter;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LuaProxy_BlockManager
{
	public int count
	{
		get
		{
			return BlockManager.BlockCount;
		}
	}

	public List<KeyValuePair<string, string>> types
	{
		get
		{
			 return BlockManager.Instance.GetAllBlockTypes();
		}
	}

	public AbstractBlock at(float x, float y, float z)
	{
		return BlockManager.GetBlockNear(new Vector3(x, y, z), 0.1f, 0f);
	}

	public AbstractBlock at(Vector3 position)
	{
		return BlockManager.GetBlockNear(position, 0.1f, 0f);
	}

	public void clear()
	{
		BlockManager.Clear();
	}

	public AbstractBlock create(float x, float y, float z, string type = "Basic", string palette = "Cat5 Internal")
	{
		return BlockManager.CreateBlockAt(x, y, z, palette, type) as AbstractBlock;
	}

	public AbstractBlock create(Vector3 position, string type = "Basic", string palette = "Cat5 Internal")
	{
		return BlockManager.CreateBlockAt(position, palette, type) as AbstractBlock;
	}

	public void destroy(AbstractBlock block)
	{
		BlockManager.DestroyBlock(block);
	}

	public bool destroy(string blockName)
	{
		if (GameObject.Find(blockName).GetComponent<AbstractBlock>() is AbstractBlock block) {
			destroy(block);
			return true;
		}
		return false;	
	}

	public List<AbstractBlock> getall(string type = null)
	{
		return BlockManager.GetBlocksOfType(type).Cast<AbstractBlock>().ToList();
	}
}
