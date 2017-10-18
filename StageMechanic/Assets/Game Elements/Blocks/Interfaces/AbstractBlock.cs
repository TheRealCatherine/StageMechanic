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

    public virtual Dictionary<string, string> Properties
    {
        get
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            ret.Add("Name", Name);
            ret.Add("Parent", Parent.name);
            ret.Add("Type", TypeName);
            ret.Add("Postition", Position.ToString());
            ret.Add("Rotation", Rotation.ToString());
            ret.Add("IsFixedRotation", IsFixedRotation.ToString());
            ret.Add("Weight", WeightFactor.ToString());
            ret.Add("Gravity", GravityFactor.ToString());
            return ret;
        }
        set
        {
            if (value.ContainsKey("Name"))
                Name = value["Name"];
            //TODO find parent in object tree
            //if (value.ContainsKey("Parent"))
            //    Parent = value["Parent"];
            if (value.ContainsKey("Type"))
                TypeName = value["Type"];
            if (value.ContainsKey("Position"))
                Position = Utility.StringToVector3(value["Position"]);
            //TODO Quaternion
            //if (value.ContainsKey("Rotation"))
            //    Rotation = Utility.StringToQuaternion(value["Rotation"]);
            if (value.ContainsKey("IsFixedRotation"))
                IsFixedRotation = Convert.ToBoolean(value["IsFixedRotation"]);
            if (value.ContainsKey("Weight"))
                WeightFactor = (float)Convert.ToDouble(value["Weight"]);
            if (value.ContainsKey("Gravity"))
                GravityFactor = (float)Convert.ToDouble(value["Gravity"]);
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

    public BlockJsonDelegate GetJsonDelegate()
    {
        return new BlockJsonDelegate(this);
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
