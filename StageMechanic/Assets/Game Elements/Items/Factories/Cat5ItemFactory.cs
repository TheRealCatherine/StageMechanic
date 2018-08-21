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

	public Cathy1BlockTheme[] Themes;
	public Cathy1BlockTheme CurrentTheme;

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

	public string[] ItemTypeNames => throw new NotImplementedException();

	public Cathy1AbstractEvent CreateEvent(Vector3 globalPosition, Quaternion globalRotation, Cathy1AbstractEvent.EventType type, Cathy1Block parent = null)
	{
		string oldName = String.Empty;

		GameObject[] collidedGameObjects =
			Physics.OverlapSphere(globalPosition, 0.1f)
				.Except(new[] { GetComponent<BoxCollider>() })
				.Select(c => c.gameObject)
				.ToArray();


		foreach (GameObject obj in collidedGameObjects)
		{
			Cathy1Block bl = obj.GetComponent<Cathy1Block>();
			// In this case the event is being created at the same location that
			// a block currently exists. Instead the system will create the event
			// as being located above the block by one unit and will make the event
			// a child of the block.
			if (bl != null)
			{
				if (parent == null)
					parent = bl;
				globalPosition = bl.Position + new Vector3(0, 0.5f, 0);
				globalRotation = bl.Rotation;
			}

			Cathy1AbstractEvent oldEvent = obj.GetComponent<Cathy1AbstractEvent>();
			if(oldEvent != null) {
				oldName = oldEvent.Name;
				Destroy(oldEvent);
			}

		}

		GameObject newEvent = null;

		switch (type)
		{
			case Cathy1AbstractEvent.EventType.PlayerStart:
				newEvent = Instantiate(PlayerStartPrefab, globalPosition, globalRotation, parent.transform);
				break;
			case Cathy1AbstractEvent.EventType.Goal:
				newEvent = Instantiate(PlayerGoalPrefab, globalPosition, globalRotation, parent.transform);
				break;
			//TODO checkpoint, cutom, enemies
			
		}

		Debug.Assert(newEvent != null);
		Cathy1AbstractEvent ev = newEvent.GetComponent<Cathy1AbstractEvent>();
		Debug.Assert(ev != null);
		//TODO some kind of Parent property in Event
		if (parent != null)
		{
			ev.transform.parent = parent.transform;
			Cathy1Block bl = parent as Cathy1Block;
			if(bl != null)
			{
				bl.FirstEvent = ev;
			}
		}
		if (oldName != String.Empty)
			ev.Name = oldName;
		return ev;
	}

	public IEvent CreateEvent(Vector3 globalPosition, Quaternion globalRotation, int eventTypeIndex, GameObject parent = null)
	{
		throw new System.NotImplementedException();
	}

	public IEvent CreateEvent(Vector3 globalPosition, Quaternion globalRotation, string eventTypeName, GameObject parent = null)
	{
		throw new System.NotImplementedException();
	}

	public IEvent CreateEvent(int eventTypeIndex, IBlock parent)
	{
		throw new System.NotImplementedException();
	}

	public IEvent CreateEvent(string eventTypeName, IBlock parent)
	{
		throw new System.NotImplementedException();
	}

	public IItem CreateItem(Vector3 globalPosition, Quaternion globalRotation, string itemTypeName, GameObject parent = null)
	{
		throw new NotImplementedException();
	}

	public IItem CreateItemt(string eventTypeName, IBlock parent)
	{
		throw new NotImplementedException();
	}

	public Sprite IconForType(string name)
	{
		throw new NotImplementedException();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override IItem CreateItem(Vector3 globalPosition, Quaternion globalRotation, string itemTypeName, GameObject parent = null)
	{
		throw new NotImplementedException();
	}

	public override IItem CreateItem(string eventTypeName, IBlock parent)
	{
		throw new NotImplementedException();
	}
}
