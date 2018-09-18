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

	public string motionState
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

	public bool isGrounded
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

	public bool CanBePulled(Vector3 direction, int distance = 1)
	{
		return target.CanBePulled(direction, distance);
	}

	public bool CanBePulled(int x, int y, int z, int distance = 1)
	{
		return target.CanBePulled(new Vector3(x,y,z), distance);
	}

	public float PullWeight(Vector3 direction, int distance = 1)
	{
		return target.PullWeight(direction, distance);
	}

	public float PullWeight(int x, int y, int z, int distance = 1)
	{
		return target.PullWeight(new Vector3(x, y, z), distance);
	}

	public bool Pull(Vector3 direction, int distance = 1)
	{
		return target.Pull(direction, distance);
	}

	public bool Pull(int x, int y, int z, int distance = 1)
	{
		return target.Pull(new Vector3(x, y, z), distance);
	}

	public bool CanBePushed(Vector3 direction, int distance = 1)
	{
		return target.CanBePushed(direction, distance);
	}

	public bool CanBePushed(int x, int y, int z, int distance = 1)
	{
		return target.CanBePushed(new Vector3(x, y, z), distance);
	}

	public float PushWeight(Vector3 direction, int distance = 1)
	{
		return target.PushWeight(direction, distance);
	}

	public float PushWeight(int x, int y, int z, int distance = 1)
	{
		return target.PushWeight(new Vector3(x, y, z), distance);
	}

	public bool Push(Vector3 direction, int distance = 1)
	{
		return target.Push(direction, distance);
	}

	public bool Push(int x, int y, int z, int distance = 1)
	{
		return target.Push(new Vector3(x, y, z), distance);
	}

	public void Set(string property, string value)
	{
		target.Properties[property] = value;
	}

	public void Unset(string property)
	{
		target.Properties[property] = target.DefaultProperties[property].Value;
	}

	public string Get(string property)
	{
		return target.Properties[property];
	}

	public void Run(string code)
	{
		Script script = LuaScriptingManager.BaseScript;
		DynValue block = UserData.Create(this);
		script.Globals.Set("block", block);
		DynValue function = script.DoString(code);
		target.StartCoroutine(script.CreateCoroutine(function).Coroutine.AsUnityCoroutine());
	}
}
