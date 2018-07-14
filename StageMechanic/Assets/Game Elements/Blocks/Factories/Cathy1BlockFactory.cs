/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cathy1BlockFactory : AbstractBlockFactory
{
	public Cathy1Block[] Blocks;
	private Dictionary<string, Cathy1Block> _prefabs = new Dictionary<string, Cathy1Block>();

	public Cathy1BlockTheme[] Themes;

	public Cathy1BlockTheme CurrentTheme;

	private void Awake()
	{
		foreach(Cathy1Block block in Blocks)
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

	public override string Name
	{
		get
		{
			return "Cathy1 Internal";
		}
	}

	public override string DisplayName
	{
		get
		{
			return "RoboCathy";
		}
	}

	public override Sprite IconForType(string name)
	{
		if(CurrentTheme == null)
			return _prefabs[name].Icon;
		switch (name)
		{
			case "Basic":
				if (CurrentTheme.BasicBlockIcon != null)
					return CurrentTheme.BasicBlockIcon;
				break;
			case "Small Bomb":
				if (CurrentTheme.SmallBombIcon != null)
					return CurrentTheme.SmallBombIcon;
				break;
			case "Large Bomb":
				if (CurrentTheme.LargeBombIcon != null)
					return CurrentTheme.LargeBombIcon;
				break;
			case "Cracked (1 Step)":
				if (CurrentTheme.HeavyCracksIcon != null)
					return CurrentTheme.HeavyCracksIcon;
				break;
			case "Cracked (2 Steps)":
				if (CurrentTheme.LightCracksIcon != null)
					return CurrentTheme.LightCracksIcon;
				break;
			case "Goal":
				if (CurrentTheme.GoalIcon != null)
					return CurrentTheme.GoalIcon;
				break;
			case "Heavy":
				if (CurrentTheme.HeavyIcon != null)
					return CurrentTheme.HeavyIcon;
				break;
			case "Ice":
				if (CurrentTheme.IceIcon != null)
					return CurrentTheme.IceIcon;
				break;
			case "Immobile":
				if (CurrentTheme.Immobile != null)
					return CurrentTheme.ImmobileIcon;
				break;
			case "Monster":
				if (CurrentTheme.MonsterIcon != null)
					return CurrentTheme.MonsterIcon;
				break;
			case "Mystery":
				if (CurrentTheme.MysteryIcon != null)
					return CurrentTheme.MysteryIcon;
				break;
			case "Spike Trap":
				if (CurrentTheme.TrapIcon != null)
					return CurrentTheme.TrapIcon;
				break;
			case "Spring":
				if (CurrentTheme.SpringIcon != null)
					return CurrentTheme.SpringIcon;
				break;
			case "Vortex":
				if (CurrentTheme.VortexIcon != null)
					return CurrentTheme.VortexIcon;
				break;
		}

		return _prefabs[name].Icon;
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

		Cathy1Block prefab = _prefabs[type];
		Debug.Assert(prefab != null);
		newBlock = Instantiate(prefab, globalPosition, globalRotation, parent.transform).gameObject;

		Debug.Assert(newBlock != null);
		Cathy1Block block = newBlock.GetComponent<Cathy1Block>();
		Debug.Assert(block != null);
		block.transform.parent = parent.transform;
		if (!string.IsNullOrWhiteSpace(oldName))
			block.Name = oldName;
		if (CurrentTheme != null)
			block.ApplyTheme(CurrentTheme);
		// Randomize which model is shown for Basic blocks
		// This adds an extra step for each basic block
		// when loading/undoing so we might need to add a 
		// state to serializer to let us skip this step
		if (type == "Basic")
			block.ShowRandomModel();
		return block;
	}

	public void ApplyTheme(Cathy1BlockTheme theme)
	{
		CurrentTheme = theme;
		foreach(IBlock block in BlockManager.BlockCache)
		{
			Cathy1Block cb = block as Cathy1Block;
			if (cb != null)
			{
				int modelNumber = cb.CurrentModelNumber;
				Destroy(cb.CurrentModel);
				cb.CurrentModel = null;
				cb.ApplyTheme(theme);
				cb.ShowModel(modelNumber);
			}
		}
	}
}
