/*  
 * Copyright (C) 2018 You're Perfect Studio. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public abstract class AbstractItem : MonoBehaviour, IItem
{
	public Sprite Icon;
	public string Palette;
	public GameManager.GameMode CurrentMode = GameManager.GameMode.Initialize;


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
			if(value == null)
				GameObject.transform.SetParent(BlockManager.ActiveFloor?.transform, true); 
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
			{
				OwningBlock = null;
			}
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

	public void RemoveChild(IHierarchical child)
	{
		throw new HierarchyException("Items do not yet support children");
	}

	public void AddChild(IHierarchical child)
	{
		throw new HierarchyException("Items do not yet support children");
	}

	public virtual Dictionary<string, DefaultValue> DefaultProperties
	{
		get
		{
			Dictionary<string, DefaultValue> ret = new Dictionary<string, DefaultValue>();
			ret.Add("Owning Block", new DefaultValue { TypeInfo = typeof(string), Value = "" });
			ret.Add("Collectable", new DefaultValue { TypeInfo = typeof(bool), Value = "True" });
			ret.Add("Trigger", new DefaultValue { TypeInfo = typeof(bool), Value = "False" });
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
			if (Trigger != false)
				ret.Add("Trigger", "True");
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
			if (value.ContainsKey("Trigger"))
				Trigger = bool.Parse(value["Trigger"]);
			if (value.ContainsKey("Score"))
				Score = int.Parse(value["Score"]);
		}
	}

	public virtual bool Collectable
	{
		get;
		set;
	} = true;

	public virtual bool Trigger
	{
		get;
		set;
	} = false;

	public virtual int Score
	{
		get;
		set;
	} = 0;
	#endregion

	#region constructors/destructors
	/// <summary>
	/// Sets the name to a random GUID
	/// Called when this object is created in the scene. If overriding
	/// you may wish to call this base class in order to have the name
	/// set to a random GUID.
	/// </summary>
	protected virtual void Awake()
	{
		name = System.Guid.NewGuid().ToString();
	}
	#endregion

	internal virtual void Update()
	{
		GameManager.GameMode newMode = (BlockManager.PlayMode ? GameManager.GameMode.Play : GameManager.GameMode.StageEdit);
		if(newMode != CurrentMode)
		{
			GameManager.GameMode oldMode = CurrentMode;
			CurrentMode = newMode;
			OnGameModeChanged(newMode, oldMode);
		}
	}

	public ItemJsonDelegate GetJsonDelegate()
	{
		return new ItemJsonDelegate(this);
	}

	public ItemBinaryDelegate GetBinaryDelegate()
	{
		return new ItemBinaryDelegate(this);
	}


	#region colliders and triggers
	public void OnCollisionEnter(Collision collision)
	{
		//TODO destory item if a block is in the same location
		//TODO call OnPlayeContact or OnEnemyContact
	}

	public void OnDestroy()
	{
		ItemManager.ItemCache.Remove(this);
	}

	public void OnCollisionExit(Collision collision)
	{
	}

	public void OnTriggerEnter(Collider other)
	{
		if (!Trigger)
		{
			IBlock asBlock = other.GetComponent<IBlock>();
			if (asBlock != null)
			{
				ItemManager.DestroyItem(this);
				return;
			}
		}

		IPlayerCharacter asPlayer = other.GetComponent<IPlayerCharacter>();
		if(asPlayer != null)
		{
			OnPlayerContact(asPlayer);
			if (Collectable)
				ItemManager.DestroyItem(this);
			return;
		}
	}

	public virtual void OnBlockDestroyed()
	{
	}

	public virtual void OnEnemyContact(INonPlayerCharacter enemy)
	{
	}

	public virtual void OnPlayerContact(IPlayerCharacter player)
	{
	}

	public virtual void OnGameModeChanged(GameManager.GameMode newMode, GameManager.GameMode oldMode)
	{
	}
	#endregion
}