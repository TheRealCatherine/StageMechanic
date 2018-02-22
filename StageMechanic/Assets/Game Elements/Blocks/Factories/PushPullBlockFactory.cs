using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PushPullBlockFactory : AbstractBlockFactory
{
	public override string Name
	{
		get
		{
			return "PushPull Internal";
		}
	}

	public AbstractPushPullBlock[] Blocks;
	private Dictionary<string, AbstractPushPullBlock> _prefabs = new Dictionary<string, AbstractPushPullBlock>();
	public PushPullBlockTheme CurrentTheme;

	private void Awake()
	{
		foreach (AbstractPushPullBlock block in Blocks)
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

		AbstractPushPullBlock prefab = _prefabs[type];
		Debug.Assert(prefab != null);
		newBlock = Instantiate(prefab, globalPosition, globalRotation, parent.transform).gameObject;

		Debug.Assert(newBlock != null);
		AbstractPushPullBlock block = newBlock.GetComponent<AbstractPushPullBlock>();
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
