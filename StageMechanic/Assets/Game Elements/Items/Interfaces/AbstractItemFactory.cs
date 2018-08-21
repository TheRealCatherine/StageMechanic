using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractItemFactory : MonoBehaviour, IItemFactory
{

	public abstract string Name { get; }
	public abstract string DisplayName { get; }
	public abstract string[] ItemTypeNames { get; }

	public abstract Sprite IconForType(string name);

	public abstract IItem CreateItem(Vector3 globalPosition, Quaternion globalRotation, string itemTypeName, GameObject parent = null);
	public abstract IItem CreateItem(string eventTypeName, IBlock parent);
}
