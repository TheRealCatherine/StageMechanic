using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPropertyField  {

	GameObject GameObject
	{
		get;
	}

	Type FieldType
	{
		get;
	}

	string PropertyName
	{
		get;
		set;
	}

	string PropertyDefault
	{
		get;
		set;
	}

	string Value
	{
		get;
		set;
	}

	string Placeholder
	{
		get;
		set;
	}
}
