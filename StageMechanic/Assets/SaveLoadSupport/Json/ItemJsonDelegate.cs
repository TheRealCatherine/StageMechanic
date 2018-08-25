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
[DataContract(Name = "Item")]
public class ItemJsonDelegate
{
	public IItem Item { get; set; }

	internal string _name = null;
	internal string _type = null;
	internal Vector3 _pos;
	internal string _palette = null;
	internal Dictionary<string, string> _properties;

	public ItemJsonDelegate(IItem itemToSerialize)
	{
		Item = itemToSerialize;
	}

	public ItemJsonDelegate() { }

	[DataMember(Name = "Name", Order = 10)]
	public string Name
	{
		get
		{
			Debug.Assert(Item != null);
			return Item.Name;
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
			Debug.Assert(Item != null);
			AbstractItem ai = (Item as AbstractItem);
			if (ai != null)
				return ai.Palette;
			return "Item Palette Error";
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
			Debug.Assert(Item != null);
			return Item.TypeName;
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
			Debug.Assert(Item != null);
			return Item.Position;
		}
		set
		{
			_pos = value;
		}
	}

	[DataMember(Name = "Properties", Order = 100)]
	public Dictionary<string, string> Properties
	{
		get
		{
			Debug.Assert(Item != null);
			return Item.Properties;
		}
		set
		{
			_properties = value;
		}
	}

	[OnDeserialized()]
	internal void OnDeserialedMethod(StreamingContext context)
	{
		Debug.Assert(_name != null);
		Debug.Assert(_type != null);
		Debug.Assert(_palette != null);
		IItem newItem = ItemManager.CreateItemAt(_pos, _palette, _type);
		newItem.Name = _name;
		if (_properties != null && _properties.Count > 0)
			newItem.Properties = _properties;
	}
}
