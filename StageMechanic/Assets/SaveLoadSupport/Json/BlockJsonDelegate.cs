/*  
 * Copyright (C) 2018 You're Perfect Studio. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

/// <summary>
/// Like the other *JsonDelegate classes, this one is responsible for being a
/// DataContract type that is used directly by C# to serialize and deserialize
/// to JSON. This class is not intended for usage outside of this context.
/// </summary>
[DataContract(Name = "Block")]
public class BlockJsonDelegate
{

	internal IBlock _block;
	public IBlock Block
	{
		get
		{
			return _block;
		}
		set
		{
			_block = value;
		}
	}

	internal string _name = null;
	internal string _type = null;
	internal string _palette = null;
	internal Vector3 _pos;
	internal Dictionary<string, string> _properties;
	internal Dictionary<string, string> _customProperties;

	public BlockJsonDelegate(IBlock block)
	{
		_block = block;
	}

	public BlockJsonDelegate()
	{
	}

	/// <summary>
	/// See <see cref="IBlock.Name"/> for information about this property
	/// </summary>
	[DataMember(Name = "Name", Order = 10)]
	public string Name
	{
		get
		{
			Debug.Assert(_block != null);
			return _block.Name;
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
			AbstractBlock abfab = _block as AbstractBlock;
			if (abfab != null)
				return abfab.Palette;
			else
				return "Unknown";
		}
		set
		{
			_palette = value;
		}
	}

	/// <summary>
	/// See <see cref="IBlock.TypeName"/> for information about this property
	/// </summary>
	[DataMember(Name = "Type", Order = 30)]
	public string Type
	{
		get
		{
			Debug.Assert(_block != null);
			return _block.TypeName;
		}
		set
		{
			_type = value;
		}
	}

	/// <summary>
	/// See <see cref="IBlock.Position"/> for information about this property
	/// </summary>
	[DataMember(Name = "Position", Order = 40)]
	public Vector3 Position
	{
		get
		{
			Debug.Assert(_block != null);
			return _block.Position;
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
			Debug.Assert(_block != null);
			List<PropertyJsonDelegate> properties = new List<PropertyJsonDelegate>();
			foreach (KeyValuePair<string, string> item in _block.Properties)
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
			Debug.Assert(_block != null);
			List<PropertyJsonDelegate> properties = new List<PropertyJsonDelegate>();
			foreach (KeyValuePair<string, string> item in (_block as AbstractBlock).CustomProperties)
				properties.Add(new PropertyJsonDelegate(item));
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

	/// <summary>
	/// While loading from JSON the different properties are stored in temporary
	/// variables and then this method is called automatically on completion and
	/// the block is created. Applying values directly to the block as it loads
	/// causes more overhead as its position/type/etc change.
	/// </summary>
	/// <param name="context"></param>
	[OnDeserialized()]
	internal void OnDeserialedMethod(StreamingContext context)
	{
		Debug.Assert(_name != null);
		Debug.Assert(_type != null);

		//if (_properties.ContainsKey("Rotation"))
		//Quaternion rotation = Utility.StringToQuaternion(value["Rotation"]);
		//else
		Quaternion rotation = Quaternion.identity;
		//TODO support different block factories
		IBlock newBlock = BlockManager.CreateBlockAt(_pos, _palette, _type);
		newBlock.Name = _name;
		newBlock.Properties = _properties;
		(newBlock as AbstractBlock).CustomProperties = _customProperties;
	}
}
