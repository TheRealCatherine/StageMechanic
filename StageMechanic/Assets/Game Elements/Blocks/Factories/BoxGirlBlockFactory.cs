using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoxGirlBlockFactory : AbstractBlockFactory
{
	public override string Name
	{
		get
		{
			return "BoxGirl Internal";
		}
	}

	public override string DisplayName
	{
		get
		{
			return "Boxgril";
		}
	}

	public AbstractBlock[] Blocks;
	private Dictionary<string, AbstractBlock> _prefabs = new Dictionary<string, AbstractBlock>();
	public BoxGirlBlockTheme CurrentTheme;

	private void Awake()
	{
		foreach (AbstractBlock block in Blocks)
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

		AbstractBlock prefab = _prefabs[type];
		Debug.Assert(prefab != null);
		newBlock = Instantiate(prefab, globalPosition, globalRotation, parent.transform).gameObject;

		Debug.Assert(newBlock != null);
		AbstractBlock block = newBlock.GetComponent<AbstractBlock>();
		Debug.Assert(block != null);
		block.transform.parent = parent.transform;
		if (!string.IsNullOrWhiteSpace(oldName))
			block.Name = oldName;
		if (CurrentTheme != null)
			(block as BoxGirlDoorBlock).ApplyTheme(CurrentTheme);
		return block;
	}

	public override Sprite IconForType(string name)
	{
		return _prefabs[name].Icon;
	}
}
