/*  
 * Copyright (C) 2018 You're Perfect Studio. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
[DataContract(Name="Event")]
public class EventJsonDelegate {
	public IItem Event { get; set; }

	internal string _name = null;
	internal string _type = null;
	internal Vector3 _pos;
	internal string _palette = null;
	internal Dictionary<string, string> _properties;

	public EventJsonDelegate( IItem eventToSerialize )
	{
		Event = eventToSerialize;
	}

	public EventJsonDelegate() { }

	[DataMember(Name = "Name", Order = 10)]
	public string Name
	{
		get
		{
			Debug.Assert(Event != null);
			return Event.Name;
		}
		set
		{
			_name = value;
		}
	}

	//TODO
	[DataMember(Name = "Palette", Order = 20)]
	public string Palette
	{
		get
		{
			Debug.Assert(Event != null);
			//TODO(ItemManager)
			//Cathy1PlayerStartLocation loc = Event as Cathy1PlayerStartLocation;
			//if (loc != null)
			//	return loc.Palette;
			return "Cathy1 Internal";
		}
		set
		{
			_palette = value;
		}
	}

	[DataMember(Name = "Type", Order = 30)]
	public string Type
	{
		get
		{
			Debug.Assert(Event != null);
			return Event.TypeName;
		}
		set
		{
			_type = value;
		}
	}

	[DataMember(Name = "Position", Order = 40)]
	public Vector3 Position
	{
		get
		{
			Debug.Assert(Event != null);
			return Event.Position;
		}
		set
		{
			_pos = value;
		}
	}

	[DataMember(Name = "Properties", Order = 100)]
	public List<PropertyJsonDelegate> Properties
	{
		get
		{
			Debug.Assert(Event != null);
			List<PropertyJsonDelegate> properties = new List<PropertyJsonDelegate>();
			foreach (KeyValuePair<string, string> item in Event.Properties)
				properties.Add(new PropertyJsonDelegate(item));
			return properties;
		}
		set
		{
			Dictionary<string, string> properties = new Dictionary<string, string>();
			foreach (PropertyJsonDelegate property in value)
				properties.Add(property.Key, property.Value);
			_properties = properties;
		}
	}

	[OnDeserialized()]
	internal void OnDeserialedMethod(StreamingContext context)
	{
		Debug.Assert(_name != null);
		Debug.Assert(_type != null);
		
		int playerNumber = 0;
		if (_properties.ContainsKey("PlayerNumber"))
			playerNumber = int.Parse(_properties["PlayerNumber"]);
		IItem newItem = ItemManager.CreateItemAt(_pos+new Vector3(0,0.5f,0), "Cat5 Internal", "Player Start");
		newItem.Name = _name;
		(newItem as Cat5PlayerStart).PlayerNumber = playerNumber;
	}
}
