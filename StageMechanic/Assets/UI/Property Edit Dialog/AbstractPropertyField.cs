using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractPropertyField : MonoBehaviour, IPropertyField  {

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


	public abstract Type FieldType
	{
		get;
	}


	public abstract string Value
	{
		get;
		set;
	}

	public virtual string Placeholder
	{
		get;
		set;
	}
}
