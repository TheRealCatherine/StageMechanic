﻿/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System;
using System.Collections.Generic;
using UnityEngine;

public class Cathy1Block : AbstractBlock
{
	public Material Material1;
	public Material Material2;
	public Material Material3;
	public Material Material4;


	/**
	 * An Item associated with this Block, for example powerups
	 * coins, as well as start/end markers. Note that setting
	 * an item does not cause the block type to change to custom
	 * and the block will take ownership of the item (ie the
	 * item will be destoryed when the block is.
	 */
	public GameObject FirstItem
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
				Items = new List<GameObject>();
			if (Items.Count < 1)
				Items.Add(value);
			else
				Items[0] = value;
			if (Items[0] != null)
				Items[0].transform.parent = transform;
		}
	}

	/// <summary>
	/// The first event associated with this block. In Cathy1 style
	/// there is only ever one event on any given block, so this is
	/// a convenience method for Events[0].
	/// </summary>
	public Cathy1AbstractEvent FirstEvent
	{
		get
		{
			if (Events != null && Events.Count > 0)
				return Events[0] as Cathy1AbstractEvent;
			return null;
		}
		set
		{
			if (value == null && Items != null && Events.Count == 1)
			{
				Events = null;
				return;
			}
			if (Events == null)
				Events = new List<IEvent>();
			if (Events.Count < 1)
				Events.Add(value);
			else
				Events[0] = value;
			if (Events[0] != null)
			{
				Cathy1AbstractEvent ev = Events[0] as Cathy1AbstractEvent;
				if(ev)
				{
					ev.transform.parent = transform;
				}
			}
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
			//TODO material number
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
			//TODO the material number
			return ret;
		}
		set
		{
			base.Properties = value;
			//TODO
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

