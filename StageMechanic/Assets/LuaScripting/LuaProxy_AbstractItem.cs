using MoonSharp.Interpreter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuaProxy_AbstractItem 
{
	AbstractItem target;

	[MoonSharpHidden]
	public LuaProxy_AbstractItem(AbstractItem p)
	{
		this.target = p;
	}

	public string name
	{
		get
		{
			return target.Name;
		}
		set
		{
			target.Name = value;
		}
	}

	public string type
	{
		get
		{
			return target.TypeName;
		}
	}

	public Vector3 position
	{
		get
		{
			return target.Position;
		}
		set
		{
			target.Position = value;
		}
	}

	public AbstractBlock owningblock
	{
		get
		{
			return (target.OwningBlock as AbstractBlock);
		}
	}

	public AbstractPlayerCharacter owningplayer
	{
		get
		{
			return (target.OwningPlayer as AbstractPlayerCharacter);
		}
	}

	public void set(string property, string value)
	{
		Dictionary<string, string> dic = target.Properties;
		dic[property] = value;
		target.Properties = dic;
	}

	public void set(string property, int value)
	{
		Dictionary<string, string> dic = target.Properties;
		dic[property] = value.ToString();
		target.Properties = dic;
	}

	public void set(string property, float value)
	{
		Dictionary<string, string> dic = target.Properties;
		dic[property] = value.ToString();
		target.Properties = dic;
	}

	public void set(string property, bool value)
	{
		Dictionary<string, string> dic = target.Properties;
		dic[property] = value.ToString();
		target.Properties = dic;
	}

	public void unset(string property)
	{
		set(property, target.DefaultProperties[property].Value);
	}

	public string get(string property)
	{
		return target.Properties[property];
	}

	public Sprite icon
	{
		get
		{
			return target.Icon;
		}
	}

	public bool collectable
	{
		get
		{
			return target.Collectable;
		}
		set
		{
			target.Collectable = value;
		}
	}

	public int uses
	{
		get
		{
			return target.Uses;
		}
		set
		{
			target.Uses = value;
		}
	}

	public bool trigger
	{
		get
		{
			return target.Trigger;
		}
		set
		{
			target.Trigger = value;
		}
	}

	public int score
	{
		get
		{
			return target.Score;
		}
		set
		{
			target.Score = value;
		}
	}

	public void run(string code)
	{
		try
		{
			Script script = LuaScriptingManager.BaseScript;
			DynValue item = UserData.Create(this);
			script.Globals.Set("item", item);
			DynValue function = script.DoString(code);
			target.StartCoroutine(script.CreateCoroutine(function).Coroutine.AsUnityCoroutine());
		}
		catch (SyntaxErrorException ex)
		{
			Debug.Log("Syntax Error! " + ex.DecoratedMessage);
		}
		catch (InternalErrorException ex)
		{
			Debug.Log("An internal error occured! " + ex.DecoratedMessage);
		}
		catch (DynamicExpressionException ex)
		{
			Debug.Log("A dynamic expression error occured! " + ex.DecoratedMessage);
		}
		catch (ScriptRuntimeException ex)
		{
			Debug.Log("An error occured! " + ex.DecoratedMessage);
		}
	}

}
