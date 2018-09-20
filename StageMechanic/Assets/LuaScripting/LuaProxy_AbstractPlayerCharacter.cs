using MoonSharp.Interpreter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuaProxy_AbstractPlayerCharacter
{
	AbstractPlayerCharacter target;

	[MoonSharpHidden]
	public LuaProxy_AbstractPlayerCharacter(AbstractPlayerCharacter p)
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

	public Vector3 facing
	{
		get
		{
			return target.FacingDirection;
		}
	}

	public List<string> allstates
	{
		get
		{
			return target.StateNames;
		}
	}

	public string state
	{
		get
		{
			return allstates[target.CurrentStateIndex];
		}
		set
		{
			int index = allstates.IndexOf(value);
			if(index >= 0)
				target.CurrentStateIndex = index;
			//TODO throw
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

	public AbstractItem item
	{
		get
		{
			return (target.Item as AbstractItem);
		}
		set
		{
			target.Item = value;
		}
	}

	public void set(string property, string value)
	{
		target.Properties[property] = value;
	}

	public void unset(string property)
	{
		//TODO player default properties?
		//target.Properties[property] = target.DefaultProperties[property].Value;
	}

	public string get(string property)
	{
		return target.Properties[property];
	}

	public void applygravity()
	{
		target.ApplyGravity();
	}

	public bool turnaround()
	{
		return target.TurnAround();
	}

	public bool turn(Vector3 direction)
	{
		return target.Turn(direction);
	}

	public bool turnright()
	{
		return target.TurnRight();
	}

	public bool turnleft()
	{
		return target.TurnLeft();
	}

	public bool takedamage(float amount = float.PositiveInfinity, string type = null)
	{
		return target.TakeDamage(amount, type);
	}

	public bool useitem()
	{
		return target.UseItem();
	}
}
