using MoonSharp.Interpreter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuaProxy_AbstractBlock
{
	AbstractBlock target;

	[MoonSharpHidden]
	public LuaProxy_AbstractBlock(AbstractBlock p)
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

	public Vector3 pos
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

	public float weight
	{
		get
		{
			return target.WeightFactor;
		}
		set
		{
			target.WeightFactor = value;
		}
	}

	public float gravity
	{
		get
		{
			return target.GravityFactor;
		}
		set
		{
			target.GravityFactor = value;
		}
	}

	public string motionstate
	{
		get
		{
			return target.MotionStateName;
		}
		set
		{
			target.MotionStateName = value;
		}
	}

	public bool grounded
	{
		get
		{
			return target.IsGrounded;
		}
	}

	public int group
	{
		get
		{
			return BlockManager.BlockGroupNumber(target);
		}
		set
		{
			BlockManager.AddBlockToGroup(target, value);
		}
	}

	public bool canbepulled(Vector3 direction, int distance = 1)
	{
		return target.CanBePulled(direction, distance);
	}

	public bool canbepulled(int x, int y, int z, int distance = 1)
	{
		return target.CanBePulled(new Vector3(x,y,z), distance);
	}

	public float pullweight(Vector3 direction, int distance = 1)
	{
		return target.PullWeight(direction, distance);
	}

	public float pullweight(int x, int y, int z, int distance = 1)
	{
		return target.PullWeight(new Vector3(x, y, z), distance);
	}

	public bool pull(Vector3 direction, int distance = 1)
	{
		return target.Pull(direction, distance);
	}

	public bool pull(int x, int y, int z, int distance = 1)
	{
		return target.Pull(new Vector3(x, y, z), distance);
	}

	public bool canbepushed(Vector3 direction, int distance = 1)
	{
		return target.CanBePushed(direction, distance);
	}

	public bool canbepushed(int x, int y, int z, int distance = 1)
	{
		return target.CanBePushed(new Vector3(x, y, z), distance);
	}

	public float pushweight(Vector3 direction, int distance = 1)
	{
		return target.PushWeight(direction, distance);
	}

	public float pushweight(int x, int y, int z, int distance = 1)
	{
		return target.PushWeight(new Vector3(x, y, z), distance);
	}

	public bool push(Vector3 direction, int distance = 1)
	{
		return target.Push(direction, distance);
	}

	public bool push(int x, int y, int z, int distance = 1)
	{
		return target.Push(new Vector3(x, y, z), distance);
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
		set(property,target.DefaultProperties[property].Value);
	}

	public string get(string property)
	{
		Dictionary<string, string> props = target.Properties;
		if (props.ContainsKey(property))
			return props[property];
		else if (target.DefaultProperties.ContainsKey(property))
			return target.DefaultProperties[property].Value;
		else
			return "";
		//TODO user properties and maybe throw error
	}

	public void run(string code)
	{
		try
		{
			Script script = LuaScriptingManager.BaseScript;
			DynValue block = UserData.Create(this);
			script.Globals.Set("block", block);
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
