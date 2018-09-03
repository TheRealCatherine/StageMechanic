using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatField : AbstractPropertyField
{
	public override Type FieldType
	{
		get
		{
			return typeof(float);
		}
	}

	public override string Value {
		get
		{
			return gameObject.GetComponent<InputField>().text;
		}
		set
		{
			gameObject.GetComponent<InputField>().text = value;
		}
	}
	public override string Placeholder {
		get
		{
			return gameObject.GetComponent<InputField>().placeholder.GetComponent<Text>().text;
		}
		set
		{
			gameObject.GetComponent<InputField>().placeholder.GetComponent<Text>().text = value;
		}
	}
}
