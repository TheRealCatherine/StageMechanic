using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class AbstractBlockFactory : MonoBehaviour, IBlockFactory
{
	public abstract string Name { get; }
	public abstract string[] BlockTypeNames { get; }

	public abstract Sprite IconForType(string name);

	public abstract IBlock CreateBlock(Vector3 globalPosition, Quaternion globalRotation, string blockTypeName, GameObject parent = null);

}
