/*  
 * Copyright (C) 2018 You're Perfect Studio. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System.Collections.Generic;
using UnityEngine;

public class Cathy1Block : AbstractBlock
{
	public GameObject Model1;
	public GameObject Model2;
	public GameObject Model3;
	public GameObject Model4;

	public GameObject CurrentModel;
	public int CurrentModelNumber=0;

	public void ShowRandomModel()
	{
		ShowModel(Random.Range(1, 4));
	}

	public void ShowModel(int number)
	{
		if (CurrentModelNumber == number && CurrentModel != null)
			return;
		Destroy(CurrentModel);
		switch (number) {
			default:
			case 1:
				CurrentModel = Instantiate(Model1,gameObject.transform);
				CurrentModelNumber = 1;
				break;
			case 2:
				if(Model2 != null)
					CurrentModel = Instantiate(Model2, gameObject.transform);
				else
					CurrentModel = Instantiate(Model1, gameObject.transform);
				CurrentModelNumber = 2;
				break;
			case 3:
				if (Model3 != null)
					CurrentModel = Instantiate(Model3, gameObject.transform);
				else
					CurrentModel = Instantiate(Model1, gameObject.transform);
				CurrentModelNumber = 3;
				break;
			case 4:
				if (Model4 != null)
					CurrentModel = Instantiate(Model4, gameObject.transform);
				else
					CurrentModel = Instantiate(Model1, gameObject.transform);
				CurrentModelNumber = 4;
				break;
		}
		int blockGroup = BlockManager.BlockGroupNumber(this);
		if (blockGroup != -1)
			OnBlockGroupChanged(blockGroup);
	}

	internal override void OnBlockGroupChanged(int newGroup) {
		Outline outline = CurrentModel.GetComponent<Outline>();
		if (outline is null)
			outline = CurrentModel.AddComponent<Outline>();
		outline.OutlineMode = Outline.Mode.OutlineVisible;
		outline.OutlineWidth = 5f;

		switch(newGroup)
		{
			case 0:
				outline.OutlineMode = Outline.Mode.OutlineHidden;
				break;
			case 1:
				outline.OutlineColor = Color.red;
				break;
			case 2:
				outline.OutlineColor = Color.blue;
				break;
			case 3:
				outline.OutlineColor = Color.green;
				break;
			case 4:
				outline.OutlineColor = Color.white;
				break;
			case 5:
				outline.OutlineColor = Color.yellow;
				break;
			case 6:
				outline.OutlineColor = Color.magenta;
				break;
			case 7:
				outline.OutlineColor = Color.cyan;
				break;
			case 8:
				outline.OutlineColor = Color.grey;
				break;
			case 9:
			default:
				outline.OutlineColor = Color.red;
				break;
		}
	}

	public virtual void ApplyTheme( Cathy1BlockTheme theme )
	{
		Debug.Assert(theme.BasicBlock1 != null);
		Model1 = theme.BasicBlock1;
		Model2 = theme.BasicBlock2;
		Model3 = theme.BasicBlock3;
		Model4 = theme.BasicBlock4;
	}

	internal override void Start()
	{
		base.Start();
		if(CurrentModelNumber == 0)
			ShowModel(1);
	}

	/**
	 * An Item associated with this Block, for example powerups
	 * coins, as well as start/end markers. The block will take ownership of the item
	 * (ie the item will be destoryed when the block is. and will move with the block)
	 */
	public IItem FirstItem
	{
		get
		{
			if (Items != null && Items.Count > 0)
				return Items[0];
			return null;
		}
		set
		{
			if (value == null && Items != null && Items.Count == 1)
			{
				Items = null;
				return;
			}
			if (Items == null)
				Items = new List<IItem>();
			if (Items.Count < 1)
				Items.Add(value);
			else
				Items[0] = value;
			if (Items[0] != null)
				Items[0].OwningBlock = this;
		}
	}

	public string TypeOfBlock="Basic";
	public override string TypeName
	{
		get
		{
			return TypeOfBlock;
		}
		set
		{
			
		}
	}

	/// <summary>
	/// The recognized properties for this block, includes base class properties
	/// </summary>
	public override Dictionary<string, DefaultValue> DefaultProperties
	{
		get
		{
			Dictionary<string, DefaultValue> ret = base.DefaultProperties;
			ret.Add("Model Variant", new DefaultValue { TypeInfo = typeof(int), Value = "1" });
			return ret;
		}
	}

	/// <summary>
	/// The list of properties associated with this block.
	/// Includes bass-class properties.
	/// </summary>
	public override Dictionary<string, string> Properties
	{
		get
		{
			Dictionary<string, string> ret = base.Properties;
			if (CurrentModelNumber > 1)
				ret.Add("Model Variant", CurrentModelNumber.ToString());
			return ret;
		}
		set
		{
			base.Properties = value;
			if (value.ContainsKey("Model Variant"))
			{
				ShowModel(int.Parse(value["Model Variant"]));
			}
		}
	}

	private void CrushPlayer(IPlayerCharacter player)
	{
		player.TakeDamage(float.PositiveInfinity, "Crush");
	}

	protected override void OnPlayerEnter(PlayerMovementEvent ev) {
		if (ev.Location == PlayerMovementEvent.EventLocation.Bottom && MotionState == BlockMotionState.Falling)
			CrushPlayer(ev.Player);
	}
	protected override void OnPlayerStay(PlayerMovementEvent ev) {
		if (ev.Location == PlayerMovementEvent.EventLocation.Bottom && MotionState == BlockMotionState.Falling)
			CrushPlayer(ev.Player);

	}
}

