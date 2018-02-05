using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class AbstractBlockFactory : MonoBehaviour, IBlockFactory
{

    public virtual string[] BlockTypeNames { get { throw new System.NotImplementedException(); } }

    public abstract Sprite IconForType(string name);

    public abstract IBlock CreateBlock(Vector3 globalPosition, Quaternion globalRotation, string blockTypeName, GameObject parent = null);

}
