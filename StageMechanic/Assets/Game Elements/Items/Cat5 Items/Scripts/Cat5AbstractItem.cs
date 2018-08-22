/*  
 * Copyright (C) 2018 You're Perfect Studio. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System.Collections.Generic;
using UnityEngine;

public abstract class Cat5AbstractItem : AbstractItem
{

	public GameObject Model1;
	public GameObject Model2;
	public GameObject Model3;
	public GameObject Model4;

	public GameObject CurrentModel;
	public int CurrentModelNumber = 0;

	public void ShowRandomModel()
	{
		ShowModel(Random.Range(1, 4));
	}

	public void ShowModel(int number)
	{
		if (CurrentModelNumber == number && CurrentModel != null)
			return;
		Destroy(CurrentModel);
		switch (number)
		{
			default:
			case 1:
				CurrentModel = Instantiate(Model1, gameObject.transform);
				CurrentModelNumber = 1;
				break;
			case 2:
				if (Model2 != null)
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
	}

	public virtual void ApplyTheme(Cathy1BlockTheme theme)
	{
		Debug.Assert(theme.BasicBlock1 != null);
		Model1 = theme.BasicBlock1;
		Model2 = theme.BasicBlock2;
		Model3 = theme.BasicBlock3;
		Model4 = theme.BasicBlock4;
	}

	internal virtual void Start()
	{
		if (CurrentModelNumber == 0)
			ShowModel(1);
	}

	public string TypeOfItem = "Unknown";
	public override string TypeName
	{
		get
		{
			return TypeOfItem;
		}
		set
		{

		}
	}

	/// <summary>
	/// The recognized properties for this item, includes base class properties
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
	/// The list of properties associated with this item.
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
				Debug.Log(int.Parse(value["Model Variant"]));
			}
		}
	}
}