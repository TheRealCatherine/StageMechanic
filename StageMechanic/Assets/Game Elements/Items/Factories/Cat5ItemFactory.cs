/*  
 * Copyright (C) 2018 You're Perfect Studio. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cat5ItemFactory : AbstractItemFactory {

	public Cat5AbstractItem[] Items;
	private Dictionary<string, Cat5AbstractItem> _prefabs = new Dictionary<string, Cat5AbstractItem>();

	public Cat5ItemTheme[] Themes;
	public Cat5ItemTheme CurrentTheme;

	private void Awake()
	{
		foreach (Cat5AbstractItem item in Items)
		{
			_prefabs.Add(item.TypeName, item);
		}
	}

	public override string[] ItemTypeNames
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
			return "Cat5 Internal";
		}
	}

	public override string DisplayName
	{
		get
		{
			return "Cat5";
		}
	}
	public override Sprite IconForType(string name)
	{
		if (CurrentTheme == null)
			return _prefabs[name].Icon;
		switch (name)
		{
			case "Player Start":
				if (CurrentTheme.PlayerStartIcon != null)
					return CurrentTheme.PlayerStartIcon
				break;
			case "Goal":
				if (CurrentTheme.GoalIcon != null)
					return CurrentTheme.GoalIcon
				break;
			case "Checkpoint":
				if (CurrentTheme.CheckpointIcon != null)
					return CurrentTheme.CheckpointIcon
				break;
			case "Enemy Spawn":
				if (CurrentTheme.EnemySpawnIcon != null)
					return CurrentTheme.EnemySpawnIcon
				break;
			case "Stage Section Spawn Trigger":
				if (CurrentTheme.SectionSpawnIcon != null)
					return CurrentTheme.SectionSpawnIcon
				break;
			case "Platform Move Trigger":
				if (CurrentTheme.PlatformMoveIcon != null)
					return CurrentTheme.PlatformMoveIcon;
				break;
			case "Story Trigger":
				if (CurrentTheme.StoryIcon != null)
					return CurrentTheme.StoryIcon
				break;
			case "Boss State Trigger":
				if (CurrentTheme.BossStateIcon != null)
					return CurrentTheme.BossStateIcon;
				break;
			case "Coin":
				if (CurrentTheme.CoinIcon != null)
					return CurrentTheme.CoinIcon;
				break;
			case "Special Collectable":
				if (CurrentTheme.SpecialCollectableIcon != null)
					return CurrentTheme.SpecialCollectableIcon
				break;
			case "Create Blocks":
				if (CurrentTheme.CreateBlocksIcon != null)
					return CurrentTheme.CreateBlocksIcon;
				break;
			case "Remove Enemies":
				if (CurrentTheme.EnemyRemovalIcon != null)
					return CurrentTheme.EnemyRemovalIcon;
				break;
			case "1-Up":
				if (CurrentTheme.OneUpIcon != null)
					return CurrentTheme.OneUpIcon;
				break;
			case "Remove Special Blocks":
				if (CurrentTheme.SpecialBlockRemoverIcon != null)
					return CurrentTheme.SpecialBlockRemoverIcon;
				break;
			case "X-Factor":
				if (CurrentTheme.XFactorIcon != null)
					return CurrentTheme.XFactorIcon;
				break;
			case "Stopwatch":
				if (CurrentTheme.StopwatchIcon != null)
					return CurrentTheme.StopwatchIcon;
				break;
			case "Item Stealer":
				if (CurrentTheme.ItemStealIcon != null)
					return CurrentTheme.ItemStealIcon;
				break;
			case "Item Randomizer":
				if (CurrentTheme.ItemRandomizerIcon != null)
					return CurrentTheme.ItemRandomizerIcon;
				break;
			case "Frisbee":
				if (CurrentTheme.FrisbeeIcon != null)
					return CurrentTheme.FrisbeeIcon;
				break;
		}
		return _prefabs[name].Icon;
	}

	public override IItem CreateItem(Vector3 globalPosition, Quaternion globalRotation, string type, GameObject parent)
	{
		string oldName = null;

		IBlock oldBlock = BlockManager.GetBlockNear(globalPosition, 0.01f, 0.0f);
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

}
