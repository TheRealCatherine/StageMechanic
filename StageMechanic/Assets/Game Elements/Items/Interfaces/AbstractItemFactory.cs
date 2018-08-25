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

	/// <summary>
	/// Convenience method that adds an item to a block
	/// </summary>
	/// <param name="itemTypeName"></param>
	/// <param name="parent"></param>
	/// <returns></returns>
	public IItem CreateItem(string itemTypeName, IBlock parent)
	{
		return CreateItem(parent.Position + Vector3.up, parent.Rotation, itemTypeName, parent.GameObject);
	}
}
