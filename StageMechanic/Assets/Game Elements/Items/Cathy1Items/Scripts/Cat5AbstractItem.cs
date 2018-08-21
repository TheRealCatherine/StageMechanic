/*  
 * Copyright (C) 2018 You're Perfect Studio. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System.Collections.Generic;
using UnityEngine;

public class Cat5AbstractItem : MonoBehaviour, IItem
{

    public enum ItemType
    {
        PlayerStart,
        Goal,
        Checkpoint,
        EnemySpawn,
		StageSectionSpawnTrigger,
		StageSectionDropTrigger,
		StoryTrigger,
		BossStateTrigger,
		Coins,
		SpecialCollectable,
		CreateBlocks,
		EnemyRemoval,
		OneUp,
		RemoveEnemies,
		RemoveSpecialBlocks,
		XFactor,
		Stopwatch,
		ItemSteal,
		ItemRandomizer,
		Frisbee
    }

    public virtual ItemType Type { get; set; }

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

    public virtual string TypeName
    {
        get
        {
            switch (Type)
            {
                case ItemType.PlayerStart:
                    return "Player Start";
                case ItemType.Goal:
                    return "Goal";
                case ItemType.Checkpoint:
                    return "Checkpoint";
                case ItemType.EnemySpawn:
                    return "Enemy Spawn";
                case ItemType.Custom:
                default:
                    return "Custom";
            }
        }
    }

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

    public virtual Dictionary<string, string> Properties
    {
        get
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            return ret;
        }
        set
        {
        
            //base.Properties = value;
            //TODO
        }
    }

    public EventLocationAffinity LocationAffinity { get; set; }

	public bool Collectable
	{
		get
		{
			throw new System.NotImplementedException();
		}

		set
		{
			throw new System.NotImplementedException();
		}
	}

	public long Score { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

	public Dictionary<string, DefaultValue> DefaultProperties => throw new System.NotImplementedException();

	virtual public EventJsonDelegate GetJsonDelegate()
    {
        return new EventJsonDelegate(this);
    }

    public EventBinaryDelegate GetBinaryDelegate()
    {
        return new EventBinaryDelegate(this);
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
