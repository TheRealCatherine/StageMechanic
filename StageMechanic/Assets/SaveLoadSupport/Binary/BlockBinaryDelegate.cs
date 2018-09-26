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

[Serializable]
public class BlockBinaryDelegate
{
	public string Name;
	public string Type;
	public string Palette;
	public float PositionX;
	public float PositionY;
	public float PositionZ;
	public string[] PropertyKeys;
	public string[] PropertyValues;
	public string[] CustomPropertyKeys;
	public string[] CustomPropertyValues;

	public BlockBinaryDelegate(IBlock block)
	{
		Name = block.Name;
		Type = block.TypeName;
		AbstractBlock abfab = block as AbstractBlock;
		if (abfab)
			Palette = abfab.Palette;
		else
			Palette = "Unknown";
		PositionX = block.Position.x;
		PositionY = block.Position.y;
		PositionZ = block.Position.z;
		Dictionary<string, string> properties = block.Properties;
		PropertyKeys = properties.Keys.ToArray();
		PropertyValues = properties.Values.ToArray();
		if(abfab.CustomProperties != null)
		{
			CustomPropertyKeys = abfab.CustomProperties.Keys.ToArray();
			CustomPropertyValues = abfab.CustomProperties.Values.ToArray();
		}
	}

	[OnDeserialized]
	private void OnDeserialedMethod(StreamingContext context)
	{
		//TODO support different block factories
		IBlock newBlock = BlockManager.CreateBlockAt(new UnityEngine.Vector3(PositionX,PositionY,PositionZ), Palette, Type);
		newBlock.Name = Name;
		Dictionary<string,string> properties = new Dictionary<string, string>();
		for(int i=0;i<PropertyKeys.Length;++i)
		{
			properties.Add(PropertyKeys[i], PropertyValues[i]);
		}
		newBlock.Properties = properties;

		if(CustomPropertyKeys != null)
		{
			Dictionary<string, string> customProperties = new Dictionary<string, string>();
			for (int i = 0; i < CustomPropertyKeys.Length; ++i)
			{
				customProperties.Add(CustomPropertyKeys[i], CustomPropertyValues[i]);
			}
			(newBlock as AbstractBlock).CustomProperties = customProperties;
		}
	}
}
