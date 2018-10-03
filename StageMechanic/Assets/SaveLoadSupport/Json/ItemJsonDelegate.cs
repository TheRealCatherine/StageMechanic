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
	internal Dictionary<string, string> _customProperties;

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
			if (Item.OwningPlayer != null)
				return new Vector3(-255, -255, -255);
			return Item.Position;
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
			Debug.Assert(Item != null);
			List<PropertyJsonDelegate> properties = new List<PropertyJsonDelegate>();
			foreach (KeyValuePair<string, string> item in Item.Properties)
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

	[DataMember(Name = "CustomProperties", Order = 110)]
	public List<PropertyJsonDelegate> CustomProperties
	{
		get
		{
			Debug.Assert(Item != null);
			List<PropertyJsonDelegate> properties = new List<PropertyJsonDelegate>();
			AbstractItem abs = Item as AbstractItem;
			if (abs.CustomProperties != null)
			{
				foreach (KeyValuePair<string, string> item in (Item as AbstractItem).CustomProperties)
					properties.Add(new PropertyJsonDelegate(item));
			}
			return properties;
		}
		set
		{
			if (value != null)
			{
				Dictionary<string, string> properties = new Dictionary<string, string>();
				foreach (PropertyJsonDelegate property in value)
					properties.Add(property.Key, property.Value);
				_customProperties = properties;
			}
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
		(newItem as AbstractItem).CustomProperties = _customProperties;
	}
}
