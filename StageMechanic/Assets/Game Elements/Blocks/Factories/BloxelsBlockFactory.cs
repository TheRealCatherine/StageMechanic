using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BloxelsBlockFactory : AbstractBlockFactory
{
	public override string Name
	{
		get
		{
			return "Bloxels Internal";
		}
	}

	public AbstractBloxelsBlock[] Blocks;
	private Dictionary<string, AbstractBloxelsBlock> _prefabs = new Dictionary<string, AbstractBloxelsBlock>();
	public AbstractBlockTheme CurrentTheme;

	private void Awake()
	{
		foreach (AbstractBloxelsBlock block in Blocks)
		{
			_prefabs.Add(block.TypeName, block);
		}
	}

	public override string[] BlockTypeNames
	{
		get
		{
			return _prefabs.Keys.ToArray();
		}
	}

	public override IBlock CreateBlock(Vector3 globalPosition, Quaternion globalRotation, string type, GameObject parent)
	{
		string oldName = null;

		IBlock oldBlock = BlockManager.GetBlockNear(globalPosition,0.01f,0.0f);
		if (oldBlock != null)
		{
			oldName = oldBlock.Name;
			BlockManager.DestroyBlock(oldBlock);
		}

		if (parent == null)
			parent = BlockManager.Instance.Stage;

		GameObject newBlock = null;

		AbstractBloxelsBlock prefab = _prefabs[type];
		Debug.Assert(prefab != null);
		newBlock = Instantiate(prefab, globalPosition, globalRotation, parent.transform).gameObject;

		Debug.Assert(newBlock != null);
		AbstractBloxelsBlock block = newBlock.GetComponent<AbstractBloxelsBlock>();
		Debug.Assert(block != null);
		block.transform.parent = parent.transform;
		if (!string.IsNullOrWhiteSpace(oldName))
			block.Name = oldName;
		if (CurrentTheme != null)
			block.ApplyTheme(CurrentTheme);
		return block;
	}

	public override Sprite IconForType(string name)
	{
		return _prefabs[name].Icon;
	}
}
