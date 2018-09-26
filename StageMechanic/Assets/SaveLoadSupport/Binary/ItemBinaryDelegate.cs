/*  
 * Copyright (C) 2018 You're Perfect Studio. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
public class ItemBinaryDelegate
{

	public float PositionX;
	public float PositionY;
	public float PositionZ;
	public string[] PropertyKeys;
	public string[] PropertyValues;
	public string[] CustomPropertyKeys;
	public string[] CustomPropertyValues;
	public string Palette;
	public string TypeName;
	public string Name;

	public ItemBinaryDelegate(AbstractItem ev) {
		if (ev.OwningPlayer != null)
		{
			PositionX = -255;
			PositionY = -255;
			PositionZ = -255;
		}
		else
		{
			PositionX = ev.Position.x;
			PositionY = ev.Position.y;
			PositionZ = ev.Position.z;
		}
		Dictionary<string, string> properties = ev.Properties;
		PropertyKeys = properties.Keys.ToArray();
		PropertyValues = properties.Values.ToArray();
		Palette = ev.Palette;
		TypeName = ev.TypeName;
		Name = ev.Name;
		if (ev.CustomProperties != null)
		{
			CustomPropertyKeys = ev.CustomProperties.Keys.ToArray();
			CustomPropertyValues = ev.CustomProperties.Values.ToArray();
		}
	}

	[OnDeserialized]
	private void OnDeserialedMethod(StreamingContext context)
	{
		Dictionary<string, string> properties = new Dictionary<string, string>();
		for (int i = 0; i < PropertyKeys.Length; ++i)
		{
			properties.Add(PropertyKeys[i], PropertyValues[i]);
		}
		IItem newItem = ItemManager.CreateItemAt(new Vector3(PositionX, PositionY, PositionZ), Palette, TypeName);
		newItem.Name = Name;
		if (properties != null && properties.Count > 0)
			newItem.Properties = properties;

		if (CustomPropertyKeys != null)
		{
			Dictionary<string, string> customProperties = new Dictionary<string, string>();
			for (int i = 0; i < CustomPropertyKeys.Length; ++i)
			{
				customProperties.Add(CustomPropertyKeys[i], CustomPropertyValues[i]);
			}
			(newItem as AbstractItem).CustomProperties = customProperties;
		}
	}
}

