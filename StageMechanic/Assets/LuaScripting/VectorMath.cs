using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuaVector3 
{

	public static readonly Vector3 up = Vector3.up;
	public static readonly Vector3 down = Vector3.down;
	public static readonly Vector3 left = Vector3.left;
	public static readonly Vector3 right = Vector3.right;
	public static readonly Vector3 zero = Vector3.zero;

	public Vector3 add(Vector3 first, Vector3 second)
	{
		return first + second;
	}

	public Vector3 add(Vector3 first, Vector3 second, Vector3 third)
	{
		return first + second + third;
	}

	public Vector3 add(Vector3 first, Vector3 second, Vector3 third, Vector3 fourth)
	{
		return first + second + third + fourth;
	}

	public Vector3 sub(Vector3 first, Vector3 second)
	{
		return first - second;
	}

	public Vector3 sub(Vector3 first, Vector3 second, Vector3 third)
	{
		return first - second - third;
	}

	public Vector3 sub(Vector3 first, Vector3 second, Vector3 third, Vector3 fourth)
	{
		return first - second - third - fourth;
	}
}
