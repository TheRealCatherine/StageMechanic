/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractBlock : MonoBehaviour, IBlock
{


    /// <summary>
    /// Synonym/passthrough for GameObject.name
    /// See <see cref="IBlock.Name"/>
    /// See also <seealso cref="UnityEngine.GameObject"/>
    /// </summary>
    public string Name
    {
        get
        {
            return name;
        }

        set
        {
            name = value;
        }
    }

    /// <summary>
    /// See <see cref="IBlock.TypeName"/>
    /// </summary>
    public abstract string TypeName
    {
        get;
        set;
    }

    /// <summary>
    /// Synonym/passthrough for GameObject.transform.position
    /// See <see cref="IBlock.Position"/>
    /// See also <seealso cref="UnityEngine.Transform.position"/>
    /// See also <seealso cref="Vector3"/>
    /// </summary>
    public Vector3 Position
    {
        get
        {
            return transform.position;
        }
        set
        {
            transform.position = value;
        }
    }

    /// <summary>
    /// Synonym/passthrough for GameObject.transform.rotation
    /// See <see cref="IBlock.Rotation"/>
    /// See also <seealso cref="UnityEngine.Transform.rotation"/>
    /// See also <seealso cref="Quaternion"/>
    /// </summary>
    public Quaternion Rotation
    {
        get
        {
            return transform.rotation;
        }
        set
        {
            transform.rotation = value;
        }
    }

    /// <summary>
    /// Synonym/passthrough for GameObject.gameObject
    /// See <see cref="IBlock.GameObject"/>
    /// See also <seealso cref="UnityEngine.GameObject.gameObject"/>
    /// </summary>
    public GameObject GameObject
    {
        get
        {
            return gameObject;
        }
    }

    /// <summary>
    /// List of items associated with this block. These will moved to be child elements of the block.
    /// This property may be null, so check this when calling.
    /// </summary>
    public virtual List<GameObject> Items { get; set; }

    public virtual List<IEvent> Events { get; set; }

    /// <summary>
    /// Uses GameObject.transform.parent internally. This method is
    /// virtual because some blocks may want to report no parent
    /// if their actual parent is a certain type, or some other
    /// condition in which there actually is a parent.
    /// See <see cref="IBlock.Parent"/>
    /// See also <seealso cref="UnityEngine.GameObject"/>
    /// See also <seealso cref="Transform.parent"/>
    /// </summary>
    public virtual GameObject Parent
    {
        get
        {
            Debug.Assert(transform != null);
            if (transform.parent == null)
                return null;
            return transform.parent.gameObject;
        }

        set
        {
            if (value == null)
                transform.SetParent(null);
            else
                transform.SetParent(value.transform, true);
        }
    }

    /// <summary>
    /// This implementation returns all AbstractBlock-derived
    /// children of an instance. It is marked virtual because it
    /// is expected that other implementations may include other
    /// types of children. Note that Items, while technically children
    /// of blocks in most implementations, have their own properties.
    /// </summary>
    public virtual List<GameObject> Children
    {
        get
        {
            List<GameObject> chillins = new List<GameObject>();
            foreach (GameObject kiddo in transform)
            {
                AbstractBlock jennyFromTheBlock = kiddo.GetComponent<AbstractBlock>();
                if (jennyFromTheBlock != null)
                    chillins.Add(kiddo);
            }
            return chillins;
        }
        //TODO remove items not in list or clear list first
        set
        {
            foreach (GameObject rugrat in value)
            {
                //Cathy1 blocks can currently only have Cathy1 blocks as children
                //Items are also children technically, but should be accessd via Item or Items
                AbstractBlock blockPartyLikeIts1999 = rugrat.GetComponent<AbstractBlock>();
                if (blockPartyLikeIts1999 != null)
                    blockPartyLikeIts1999.Parent = GameObject;
            }
        }
    }

    public virtual Dictionary<string, KeyValuePair<string, string>> DefaultProperties
    {
        get
        {
            Dictionary<string, KeyValuePair<string, string>> ret = new Dictionary<string, KeyValuePair<string, string>>();
            ret.Add("Rotation", new KeyValuePair<string, string>("Quaternion", Quaternion.identity.ToString()));
            ret.Add("Fixed Rotation", new KeyValuePair<string, string>("bool", "false"));
            ret.Add("Weight Factor", new KeyValuePair<string, string>("float", "1.0"));
            ret.Add("Gravity Factor", new KeyValuePair<string, string>("float", "1.0"));
            return ret;
        }
    }

    public virtual Dictionary<string, string> Properties
    {
        get
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            if(Rotation != Quaternion.identity)
                ret.Add("Rotation", Rotation.ToString());
            if (IsFixedRotation)
                ret.Add("Fixed Rotation", IsFixedRotation.ToString());
            if(WeightFactor != 1.0f)
                ret.Add("Weight Factor", WeightFactor.ToString());
            if(GravityFactor != 1.0f)
                ret.Add("Gravity Factor", GravityFactor.ToString());
            return ret;
        }
        set
        {
            //    Rotation = Utility.StringToQuaternion(value["Rotation"]);*/
            if (value.ContainsKey("Fixed Rotation"))
                IsFixedRotation = Convert.ToBoolean(value["Fixed Rotation"]);
            if (value.ContainsKey("Weight Factor"))
                WeightFactor = (float)Convert.ToDouble(value["Weight Factor"]);
            if (value.ContainsKey("Gravity Factor"))
                GravityFactor = (float)Convert.ToDouble(value["Gravity Factor"]);
        }
    }

    /// <summary>
    /// See <see cref="IBlock.IsFixedRotation"/>
    /// </summary>
    public bool IsFixedRotation { get; set; } = false;

    /// <summary>
    /// See <see cref="IBlock.WeightFactor"/>
    /// </summary>
    public float WeightFactor { get; set; } = 1.0f;

    /// <summary>
    /// See <see cref="IBlock.GravityFactor"/>
    /// </summary>
    public float GravityFactor { get; set; } = 1.0f;

    virtual public BlockJsonDelegate GetJsonDelegate()
    {
        return new BlockJsonDelegate(this);
    }

	public virtual bool CanBeMoved (Vector3 direction, int distance = 1)
	{
		if (WeightFactor == 0)
			return false;

		IBlock neighbor = BlockManager.GetBlockAt (Position + direction);
		if(neighbor != null)
			return neighbor.CanBeMoved (direction, distance);
		return true;
	}

    public virtual float MoveWeight( Vector3 direction, int distance = 1)
    {
        if (!CanBeMoved(direction, distance))
            return 0;
        IBlock neighbor = BlockManager.GetBlockAt(Position + direction);
        float weight = WeightFactor;
        if(neighbor != null)
        {
            if (!neighbor.CanBeMoved(direction, distance))
                return 0;
            weight = Math.Max(weight, neighbor.WeightFactor);
        }
        return weight;
    }

	public virtual bool Move(Vector3 direction, int distance = 1)
	{
		if(!CanBeMoved(direction,distance))
			return false;
		IBlock neighbor = BlockManager.GetBlockAt (Position + direction);
		if (neighbor != null)
			neighbor.Move (direction, distance);
        StartCoroutine(AnimateMove(Position, Position + direction, 0.2f*MoveWeight(direction,distance)));
		//Position += direction;
		return true;
	}

    IEnumerator AnimateMove(Vector3 origin, Vector3 target, float duration)
    {
        float journey = 0f;
        while (journey <= duration)
        {
            journey = journey + Time.deltaTime;
            float percent = Mathf.Clamp01(journey / duration);

            transform.position = Vector3.Lerp(origin, target, percent);

            yield return null;
        }
    }

    /// <summary>
    /// Sets the name to a random GUID
    /// Called when this object is created in the scene. If overriding
    /// you may wish to call this base class in order to have the name
    /// set to a random GUID.
    /// </summary>
    public virtual void Awake()
    {
        name = System.Guid.NewGuid().ToString();
    }
}
