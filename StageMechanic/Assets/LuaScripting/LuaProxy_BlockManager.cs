using MoonSharp.Interpreter;
using System.Collections;
using System.Collections.Generic;
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

	public IBlock At(float x, float y, float z)
	{
		return BlockManager.GetBlockNear(new Vector3(x, y, z), 0.1f, 0f);
	}

	public IBlock At(Vector3 position)
	{
		return BlockManager.GetBlockNear(position, 0.1f, 0f);
	}

	public void Clear()
	{
		BlockManager.Clear();
	}

	public IBlock Create(float x, float y, float z, string type = "Basic", string palette = "Cat5 Internal")
	{
		return BlockManager.CreateBlockAt(x, y, z, palette, type);
	}

	public IBlock Create(Vector3 position, string type = "Basic", string palette = "Cat5 Internal")
	{
		return BlockManager.CreateBlockAt(position, palette, type);
	}

	public void Destroy(IBlock block)
	{
		BlockManager.DestroyBlock(block);
	}

	public bool Destroy(string blockName)
	{
		if (GameObject.Find(blockName).GetComponent<IBlock>() is IBlock block) {
			Destroy(block);
			return true;
		}
		return false;	
	}

	public List<IBlock> GetAll(string type = null)
	{
		return BlockManager.GetBlocksOfType(type);
	}
}
