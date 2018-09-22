/*  
 * Copyright (C) 2018 You're Perfect Studio. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using MoonSharp.Interpreter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public abstract class AbstractItem : MonoBehaviour, IItem
{
	public string Palette;
	public GameManager.GameMode CurrentMode = GameManager.GameMode.Initialize;

	public string ScriptOnCreate;
	public string ScriptOnDestroy;
	public string ScriptOnBlockDestroy;
	public string ScriptOnPlayerActivate;
	public string ScriptOnPlayerContact;
	public string ScriptOnEnemyContact;
	public string ScriptOnGameModeChange;



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
			if (value != null && GameObject?.transform.parent != null && value.Name == GameObject?.transform.parent.name)
				return;
			if (value == null)
				GameObject.transform.SetParent(BlockManager.ActiveFloor?.transform, true);
			else
				GameObject.transform.SetParent(value?.GameObject?.transform, true);
		}
	}

	/// <summary>
	/// Sets the given player as the parent of this item
	/// </summary>
	public IPlayerCharacter OwningPlayer
	{
		get
		{
			return GameObject?.transform.parent?.GetComponent<IPlayerCharacter>();
		}
		set
		{
			if (value == null)
			{
				//Player drops item
				if (OwningPlayer != null)
				{
					GameObject?.transform.SetParent(BlockManager.ActiveFloor?.transform, true);
					GameObject?.SetActive(true);
					for (int i = 0; i < PlayerManager.PlayerCount; ++i)
					{
						if (PlayerManager.Player(i)?.Item?.Name == Name)
							PlayerManager.Player(i).Item = null;
					}
				}
				return;
			}
			GameObject.transform.SetParent(value?.GameObject?.transform, true);
			GameObject.transform.localPosition = Vector3.zero;
			GameObject.SetActive(false);
			value.Item = this;
		}
	}

	public virtual IHierarchical Parent
	{
		get
		{
			if (OwningBlock != null)
				return OwningBlock as IHierarchical;
			else if (OwningPlayer != null)
				return OwningPlayer as IHierarchical;
			else
				return null;
		}

		set
		{
			IBlock block = value as IBlock;
			if (block == null)
			{
				OwningBlock = null;
				IPlayerCharacter player = value as IPlayerCharacter;
				if (player == null)
					OwningPlayer = null;
				else
					OwningPlayer = value as IPlayerCharacter;
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
			ret.Add("OnCreate Script", new DefaultValue { TypeInfo = typeof(MultilinePlaintext), Value = "" });
			ret.Add("OnDestroy Script", new DefaultValue { TypeInfo = typeof(MultilinePlaintext), Value = "" });
			ret.Add("OnBlockDestroy Script", new DefaultValue { TypeInfo = typeof(MultilinePlaintext), Value = "" });
			ret.Add("OnPlayerActivate Script", new DefaultValue { TypeInfo = typeof(MultilinePlaintext), Value = "" });
			ret.Add("OnPlayerContact Script", new DefaultValue { TypeInfo = typeof(MultilinePlaintext), Value = "" });
			ret.Add("OnEnemyContact Script", new DefaultValue { TypeInfo = typeof(MultilinePlaintext), Value = "" });
			ret.Add("OnGameModeChange Script", new DefaultValue { TypeInfo = typeof(MultilinePlaintext), Value = "" });
			ret.Add("Owning Block", new DefaultValue { TypeInfo = typeof(string), Value = "" });
			ret.Add("Owning Player", new DefaultValue { TypeInfo = typeof(string), Value = "" });
			ret.Add("Collectable", new DefaultValue { TypeInfo = typeof(bool), Value = "True" });
			ret.Add("Uses", new DefaultValue { TypeInfo = typeof(int), Value = "1" });
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
			if (!string.IsNullOrWhiteSpace(ScriptOnCreate))
				ret.Add("OnCreate Script", ScriptOnCreate);
			if (!string.IsNullOrWhiteSpace(ScriptOnDestroy))
				ret.Add("OnDestroy Script", ScriptOnDestroy);
			if (!string.IsNullOrWhiteSpace(ScriptOnBlockDestroy))
				ret.Add("OnBlockDestroy Script", ScriptOnBlockDestroy);
			if (!string.IsNullOrWhiteSpace(ScriptOnPlayerActivate))
				ret.Add("OnPlayerActivate Script", ScriptOnPlayerActivate);
			if (!string.IsNullOrWhiteSpace(ScriptOnPlayerContact))
				ret.Add("OnPlayerContact Script", ScriptOnPlayerContact);
			if (!string.IsNullOrWhiteSpace(ScriptOnEnemyContact))
				ret.Add("OnEnemyContact Script", ScriptOnEnemyContact);
			if (!string.IsNullOrWhiteSpace(ScriptOnGameModeChange))
				ret.Add("OnGameModeChange Script", ScriptOnGameModeChange);
			if (OwningBlock != null)
				ret.Add("Owning Block", OwningBlock.Name);
			if (OwningPlayer != null)
				ret.Add("Owning Player", OwningPlayer.Name);
			if (Collectable != true)
				ret.Add("Collectable", "False");
			if (Uses != 1)
				ret.Add("Uses", Uses.ToString());
			if (Trigger != false)
				ret.Add("Trigger", "True");
			if (Score != 0)
				ret.Add("Score", Score.ToString());
			return ret;
		}
		set
		{
			//    Rotation = Utility.StringToQuaternion(value["Rotation"]);*/
			if (value.ContainsKey("OnCreate Script"))
				ScriptOnCreate = value["OnCreate Script"];
			if (value.ContainsKey("OnDestroy Script"))
				ScriptOnDestroy = value["OnDestroy Script"];
			if (value.ContainsKey("OnBlockDestroy Script"))
				ScriptOnBlockDestroy = value["OnBlockDestory Script"];
			if (value.ContainsKey("OnPlayerActivate Script"))
				ScriptOnPlayerActivate = value["OnPlayerActivate Script"];
			if (value.ContainsKey("OnPlayerContact Script"))
				ScriptOnPlayerContact = value["OnPlayerContact Script"];
			if (value.ContainsKey("OnEnemyContact Script"))
				ScriptOnEnemyContact = value["OnEnemyContact Script"];
			if (value.ContainsKey("OnGameModeChange Script"))
				ScriptOnGameModeChange = value["OnGameModeChange Script"];
			if (value.ContainsKey("Owning Block"))
				StartCoroutine(UnserializeHelper(value["Owning Block"]));
			if (value.ContainsKey("Owning Player"))
				StartCoroutine(UnserializeHelper(value["Owning Player"]));
			if (value.ContainsKey("Collectable"))
				Collectable = bool.Parse(value["Collectable"]);
			if (value.ContainsKey("Uses"))
				Uses = int.Parse(value["Uses"]);
			if (value.ContainsKey("Trigger"))
				Trigger = bool.Parse(value["Trigger"]);
			if (value.ContainsKey("Score"))
				Score = int.Parse(value["Score"]);
		}
	}

	/// <summary>
	/// Ensure owning block/player has been created when deserializing.
	/// </summary>
	/// <param name="owningBlockName"></param>
	/// <returns></returns>
	private IEnumerator UnserializeHelper(string owningObjectName)
	{
		if (string.IsNullOrWhiteSpace(owningObjectName))
			yield break;
		GameObject owningObject = GameObject.Find(owningObjectName);
		IBlock block = owningObject?.GetComponent<IBlock>();
		IPlayerCharacter player = owningObject?.GetComponent<IPlayerCharacter>();
		int frameCount = 420;
		while (block == null && player == null && --frameCount > 0)
		{
			yield return new WaitForEndOfFrame();
			owningObject = GameObject.Find(owningObjectName);
			block = owningObject?.GetComponent<IBlock>();
			player = owningObject?.GetComponent<IPlayerCharacter>();
		}
		if (block == null && player == null)
			LogController.Log("Can't find item owniner: " + owningObjectName);
		OwningBlock = block;
		OwningPlayer = player;
		yield break;
	}

	public Sprite Icon
	{
		get
		{
			return ItemManager.Instance.ItemFactories[ItemManager.ItemFactoryIndex(Palette)].IconForType(TypeName);
		}
	}

	public virtual bool Collectable
	{
		get;
		set;
	} = true;

	public virtual int Uses
	{
		get;
		set;
	} = 1;

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

	protected virtual void Start()
	{
		if (!string.IsNullOrWhiteSpace(ScriptOnCreate))
		{
			Script script = LuaScriptingManager.BaseScript;
			DynValue item = UserData.Create(this);
			script.Globals.Set("item", item);
			LuaScriptingManager.RunScript(script, ScriptOnCreate);
		}
	}

	private void OnDestroy()
	{
		if (!string.IsNullOrWhiteSpace(ScriptOnDestroy))
		{
			Script script = LuaScriptingManager.BaseScript;
			DynValue item = UserData.Create(this);
			script.Globals.Set("item", item);
			LuaScriptingManager.RunScript(script, ScriptOnDestroy);
		}
		ItemManager.ItemCache.Remove(this);
	}
	#endregion

	internal virtual void Update()
	{
		GameManager.GameMode newMode = (BlockManager.PlayMode ? GameManager.GameMode.Play : GameManager.GameMode.StageEdit);
		if (newMode != CurrentMode)
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


	public void OnCollisionExit(Collision collision)
	{
	}

	public void OnTriggerEnter(Collider other)
	{
		if (!Trigger)
		{
			IBlock asBlock = other.GetComponent<IBlock>();
			if (asBlock != null && Serializer.CurrentState != Serializer.State.Deserializing)
			{
				ItemManager.DestroyItem(this);
				return;
			}
		}

		IPlayerCharacter asPlayer = other.GetComponent<IPlayerCharacter>();
		if (asPlayer != null)
		{
			OnPlayerContact(asPlayer);
			//TODO support non-collectable usable items
			if (Collectable)
			{
				if (Uses > 0)
				{
					OwningBlock = null;
					GameObject.transform.SetParent(asPlayer.GameObject.transform);
					asPlayer.Item = this;
					GameObject.SetActive(false);
				}
				else
				{
					ItemManager.DestroyItem(this);
				}
			}
			(asPlayer as AbstractPlayerCharacter).Score += Score;
			return;
		}
	}

	public virtual void OnPlayerActivate(IPlayerCharacter player) {	if (!string.IsNullOrWhiteSpace(ScriptOnPlayerActivate)) RunScriptOnPlayerActivate(player); }
	public virtual void OnBlockDestroyed() { if (!string.IsNullOrWhiteSpace(ScriptOnBlockDestroy)) RunScriptOnBlockDestroyed(); }
	public virtual void OnEnemyContact(INonPlayerCharacter enemy) { if (!string.IsNullOrWhiteSpace(ScriptOnEnemyContact)) RunScriptOnEnemyContact(enemy); }
	public virtual void OnPlayerContact(IPlayerCharacter player) { if (!string.IsNullOrWhiteSpace(ScriptOnPlayerContact)) RunScriptOnPlayerContact(player); }
	public virtual void OnGameModeChanged(GameManager.GameMode newMode, GameManager.GameMode oldMode) { if (!string.IsNullOrWhiteSpace(ScriptOnGameModeChange)) RunScriptOnGameModeChange(newMode,oldMode); }

	protected virtual void RunScriptOnPlayerActivate(IPlayerCharacter player)
	{
		Script script = LuaScriptingManager.BaseScript;
		DynValue p = UserData.Create(player as AbstractPlayerCharacter);
		DynValue item = UserData.Create(this);
		script.Globals.Set("player", p);
		script.Globals.Set("item", item);
		LuaScriptingManager.RunScript(script, ScriptOnPlayerActivate);
	}

	protected virtual void RunScriptOnBlockDestroyed()
	{
		Script script = LuaScriptingManager.BaseScript;
		DynValue block = UserData.Create(OwningBlock);
		DynValue item = UserData.Create(this);
		//DynValue location = UserData.Create(ev.Location);
		script.Globals.Set("block", block);
		script.Globals.Set("item", item);
		//script.Globals.Set("loc", location);
		LuaScriptingManager.RunScript(script, ScriptOnBlockDestroy);
	}

	protected virtual void RunScriptOnPlayerContact(IPlayerCharacter player)
	{
		Script script = LuaScriptingManager.BaseScript;
		DynValue p = UserData.Create(player as AbstractPlayerCharacter);
		DynValue item = UserData.Create(this);
		script.Globals.Set("player", p);
		script.Globals.Set("item", item);
		LuaScriptingManager.RunScript(script, ScriptOnPlayerContact);
	}

	protected virtual void RunScriptOnEnemyContact(INonPlayerCharacter enemy)
	{
		Script script = LuaScriptingManager.BaseScript;
		DynValue p = UserData.Create(enemy); //TODO abstract
		DynValue item = UserData.Create(this);
		script.Globals.Set("enemy", p);
		script.Globals.Set("item", item);
		LuaScriptingManager.RunScript(script, ScriptOnEnemyContact);
	}

	protected virtual void RunScriptOnGameModeChange(GameManager.GameMode newMode, GameManager.GameMode oldMode)
	{
		Script script = LuaScriptingManager.BaseScript;
		DynValue item = UserData.Create(this);
		script.Globals.Set("item", item);
		//TODO game mode
		LuaScriptingManager.RunScript(script, ScriptOnEnemyContact);
	}

	#endregion
}