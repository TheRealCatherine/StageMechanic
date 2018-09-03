using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StringField : MonoBehaviour, IPropertyField
{
	public Type FieldType
	{
		get
		{
			return typeof(string);
		}
	}

	public string Value {
		get
		{
			return gameObject.GetComponent<InputField>().text;
		}
		set
		{
			gameObject.GetComponent<InputField>().text = value;
		}
	}
	public string Placeholder {
		get
		{
			return gameObject.GetComponent<InputField>().placeholder.GetComponent<Text>().text;
		}
		set
		{
			gameObject.GetComponent<InputField>().placeholder.GetComponent<Text>().text = value;
		}
	}

	public string PropertyName
	{
		get;
		set;
	}

	public string PropertyDefault
	{
		get;
		set;
	}

	public GameObject GameObject
	{
		get
		{
			return gameObject;
		}
	}
}
