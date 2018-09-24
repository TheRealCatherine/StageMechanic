using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorMath 
{

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
