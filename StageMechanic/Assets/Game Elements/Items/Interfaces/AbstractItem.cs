﻿/*  
 * Copyright (C) 2018 You're Perfect Studio. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public abstract class AbstractItem : MonoBehaviour, IItem
{
	public Sprite Icon;
	public string Palette;


	#region Interface property implementations
	/// <summary>
	/// Synonym/passthrough for GameObject.name
	/// See <see cref="IItem.Name"/>
	/// See also <seealso cref="UnityEngine.GameObject"/>
	/// </summary>
	public string Name
	{
		get
		{
			return name;
		}

		set
		{
			name = value;
		}
	}

	/// <summary>
	/// See <see cref="IItem.TypeName"/>
	/// </summary>
	public abstract string TypeName
	{
		get;
		set;
	}

	/// <summary>
	/// Synonym/passthrough for GameObject.transform.position
	/// If there is no GameObject, the coordinates will be float.NaN
	/// See <see cref="IItem.Position"/>
	/// See also <seealso cref="UnityEngine.Transform.position"/>
	/// See also <seealso cref="Vector3"/>
	/// </summary>
	public Vector3 Position
	{
		get
		{
			if (gameObject != null)
				return transform.position;
			return new Vector3(float.NaN, float.NaN, float.NaN);
		}
		set
		{
			transform.position = value;
		}
	}

	/// <summary>
	/// Synonym/passthrough for GameObject.transform.rotation
	/// See <see cref="IItem.Rotation"/>
	/// See also <seealso cref="UnityEngine.Transform.rotation"/>
	/// See also <seealso cref="Quaternion"/>
	/// </summary>
	public Quaternion Rotation
	{
		get
		{
			return transform.rotation;
		}
		set
		{
			transform.rotation = value;
		}
	}

	/// <summary>
	/// Synonym/passthrough for GameObject.gameObject
	/// See <see cref="IItem.GameObject"/>
	/// See also <seealso cref="UnityEngine.GameObject.gameObject"/>
	/// </summary>
	public GameObject GameObject
	{
		get
		{
			return gameObject;
		}
	}

	/// <summary>
	/// Sets the given block as the parent of this item
	/// </summary>
	public IBlock OwningBlock
	{
		get
		{
			return GameObject?.transform.parent?.GetComponent<IBlock>();
		}
		set
		{
			//TODO if null should parent to the stage
			GameObject.transform.SetParent(value?.GameObject?.transform,true);
		}
	}

	public virtual IHierarchical Parent
	{
		get
		{
			return OwningBlock as IHierarchical;
		}

		set
		{
			IBlock block = value as IBlock;
			if (block == null)
				throw new HierarchyException("Item parents must be blocks");
			else
				OwningBlock = block;
		}
	}


	public virtual IHierarchical[] Children
	{
		get
		{
			return null;
		}
		set
		{
			throw new HierarchyException("Items do not yet support children");
		}
	}

	public virtual Dictionary<string, DefaultValue> DefaultProperties
	{
		get
		{
			Dictionary<string, DefaultValue> ret = new Dictionary<string, DefaultValue>();
			ret.Add("Owning Block", new DefaultValue { TypeInfo = typeof(string), Value = "" });
			ret.Add("Collectable", new DefaultValue { TypeInfo = typeof(bool), Value = "True" });
			ret.Add("Score", new DefaultValue { TypeInfo = typeof(int), Value = "0" });
			return ret;
		}
	}

	public virtual Dictionary<string, string> Properties
	{
		get
		{
			Dictionary<string, string> ret = new Dictionary<string, string>();
			if (OwningBlock != null)
				ret.Add("Owning Block", OwningBlock.Name);
			if (Collectable != true)
				ret.Add("Collectable", "False");
			if (Score != 0)
				ret.Add("Score", Score.ToString());
			return ret;
		}
		set
		{
			//    Rotation = Utility.StringToQuaternion(value["Rotation"]);*/
			if (value.ContainsKey("Owning Block"))
				OwningBlock = GameObject.Find(value["Owning Block"]).GetComponent<IBlock>();
			if (value.ContainsKey("Collectable"))
				Collectable = bool.Parse(value["Collectable"]);
			if (value.ContainsKey("Score"))
				Score = int.Parse(value["Score"]);
		}
	}
	#endregion

	public string TypeName
	{
		get
		{
			throw new NotImplementedException();
		}
	}



	public bool Collectable { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
	public int Score { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
	public Vector3 Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
	public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

	public Dictionary<string, DefaultValue> DefaultProperties => throw new NotImplementedException();

	public Dictionary<string, string> Properties { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

	public ItemJsonDelegate GetJsonDelegate()
	{
		throw new NotImplementedException();
	}

	public void OnBlockDestroyed()
	{
		throw new NotImplementedException();
	}

	public void OnEnemyContact(INonPlayerCharacter enemy)
	{
		throw new NotImplementedException();
	}

	public void OnPlayerContact(IPlayerCharacter player)
	{
		throw new NotImplementedException();
	}
}