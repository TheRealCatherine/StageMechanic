using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoolField : AbstractPropertyField
{
	public override Type FieldType
	{
		get
		{
			return typeof(bool);
		}
	}

	public override string Value {
		get
		{
			return gameObject.GetComponent<Toggle>().isOn.ToString();
		}
		set
		{
			gameObject.GetComponent<Toggle>().isOn = bool.Parse(value);
		}
	}

	private string _default;
	public override string Placeholder
	{
		get
		{
			return _default;
		}
		set
		{
			gameObject.GetComponent<Toggle>().isOn = bool.Parse(value);
			_default = value;
		}
	}

}
