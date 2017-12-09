/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;
using System;

public class Cathy1Block : AbstractBlock
{
    public Material Material1;
    public Material Material2;
    public Material Material3;
    public Material Material4;

    /// <summary>
    /// Used internally by Cathy1 blocks and game rules to ensure correctness.
    /// </summary>
    public enum BlockType
    {
        Custom = 0,
        Basic,          //Typical block
        SpikeTrap,      //Spike Trap
        Spring,         //Teleport variation, moves character up along edge
        Monster,        //Teleport variation, moves character down from edge
        Ice,            //Teleport variation, moves charcter along top of block
        Vortex,         //Vortex trap
        Bomb1,          //Bomb with short timing
        Bomb2,          //Bomb with long timing
        Crack1,         //Can step on once
        Crack2,         //Can step on twice
        Heavy,          //Similar to Basic but slower to move
        Immobile,       //Basic blocks that cannot normally be moved by the player
        Random,         //Not a fixed type, one of a selectable subset
        Goal            //Level completion zone
    }

    //TODO move to teleport subclass
    public enum TeleportBlockType
    {
        None = 0,
        Custom,
        Side,               //Move player up or down along the side of the blocks, allows edge grabbing
        SideNoGrab,         //Same as Slide, but does not allow edge grabbing
        Platform,           //Move player to the top of a block from the top of this block
        PlatformToSide,     //Move player from top of platform to an edge grab
        SideToPlatform      //Move player from an edge grab to the top of a platform
    }

    //TODO move to Abstract
    internal BlockType _type = BlockType.Custom;
    public virtual BlockType Type { get { return _type; } }

    public BlockManager BlockManager { get; set; } = null;

    /**
	 * Will be true if this is a customized block type
	 * That is, any of its properties have been changed
	 * manually, rather than by setting the BlockType
	 */
    public bool IsCustomType
    {
        get
        {
            return this.Type == BlockType.Custom;
        }
    }

    /// <summary>
    /// Will be true if this block exibits properties of one of the Cathy1 style traps
    /// Bomb, Spike, or Vortex
    /// </summary>
    public bool IsTrap
    {
        get
        {
            Cathy1AbstractTrapBlock trapHouse = GetComponent<Cathy1AbstractTrapBlock>();
            if (trapHouse != null)
                return trapHouse.TrapType != Cathy1AbstractTrapBlock.TrapBlockType.None;
            return false;
        }
    }

    /**
	 * Property describing the the type of movement action to the player
	 * this block exerts. For example spring blocks move the player up
	 * along the side, allowing grabbing. Ice blocks move the player
	 * along the platform to the next block, etc.
	 */
    private TeleportBlockType _teleportType = TeleportBlockType.None;
    public TeleportBlockType TeleportType
    {
        get
        {
            return _teleportType;
        }
        set
        {
            _teleportType = value;

        }
    }

    /**
	 * Describes how far away from this block the user should be moved
	 */
    private Vector3 _teleportDistance;
    public Vector3 TeleportDistance
    {
        get
        {
            return _teleportDistance;
        }
        set
        {
            _teleportDistance = value;

        }
    }

    /**
	 * Will be true if this block is any type of teleport block
	 */
    public bool IsTeleport
    {
        get
        {
            return this.TeleportType != TeleportBlockType.None;
        }
    }

    /**
	 * How many steps on top of the block cause it to destruct
	 * 0 means it collapses on instantiation (perhaps useful for beginning
	 * stage animation) Less than 0 means the block does not collapse.
	 */
    private int _collapseSteps = -1;
    public int CollapseAfterNSteps
    {
        get
        {
            return _collapseSteps;
        }
        set
        {
            _collapseSteps = value;

        }
    }

    /**
	 * Will be true if the block collapses after a certain number of steps
	 */
    public bool IsCollapseOnStep
    {
        get
        {
            return _collapseSteps >= 0;
        }
    }

    /**
	 * How many grabs on the edge of the block cause it to destruct
	 * 0 means it collapses on instantiation (perhaps useful for beginning
	 * stage animation) Less than 0 means the block does not collapse.
	 */
    private int _collapseGrabs = -1;
    public int CollapseAfterNGrabs
    {
        get
        {
            return _collapseGrabs;
        }
        set
        {
            _collapseGrabs = value;

        }
    }


    /**
	 * Will be true if the block collapses after a certain number of edge grabs
	 */
    public bool IsCollapseOnGrab
    {
        get
        {
            return _collapseGrabs >= 0;
        }
    }

    /**
	 * Returns true if the block is heavier than normal
	 */
    public bool IsHeavy
    {
        get
        {
            return WeightFactor > 1.0F;
        }
    }

    /**
	 * Returns true if the block is lighter than normal
	 */
    public bool IsLight
    {
        get
        {
            return WeightFactor < 1.0F;
        }
    }

    public bool IsMovableByPlayer
    {
        get
        {
            return WeightFactor != 0;
        }
    }

    public bool IsFixedPosition
    {
        get
        {
            return GravityFactor == 0;
        }
    }


    /**
	 * An Item associated with this Block, for example powerups
	 * coins, as well as start/end markers. Note that setting
	 * an item does not cause the block type to change to custom
	 * and the block will take ownership of the item (ie the
	 * item will be destoryed when the block is.
	 */
    public GameObject FirstItem
    {
        get
        {
            if (Items != null && Items.Count > 0)
                return Items[0];
            return null;
        }
        set
        {
            if (value == null && Items != null && Items.Count == 1)
            {
                Items = null;
                return;
            }
            if (Items == null)
                Items = new List<GameObject>();
            if (Items.Count < 1)
                Items.Add(value);
            else
                Items[0] = value;
            if (Items[0] != null)
                Items[0].transform.parent = transform;
        }
    }

    /// <summary>
    /// The first event associated with this block. In Cathy1 style
    /// there is only ever one event on any given block, so this is
    /// a convenience method for Events[0].
    /// </summary>
    public Cathy1AbstractEvent FirstEvent
    {
        get
        {
            if (Events != null && Events.Count > 0)
                return Events[0] as Cathy1AbstractEvent;
            return null;
        }
        set
        {
            if (value == null && Items != null && Events.Count == 1)
            {
                Events = null;
                return;
            }
            if (Events == null)
                Events = new List<IEvent>();
            if (Events.Count < 1)
                Events.Add(value);
            else
                Events[0] = value;
            if (Events[0] != null)
            {
                Cathy1AbstractEvent ev = Events[0] as Cathy1AbstractEvent;
                if(ev)
                {
                    ev.transform.parent = transform;
                }
            }
        }
    }

    public override string TypeName
    {
        get
        {
            return Cathy1BlockFactory.NameForType(Type);
        }
        set
        {
            //TODO    Cathy1Block.BlockType type = (Cathy1Block.BlockType)Enum.Parse(typeof(Cathy1Block.BlockType), value);
            //    Debug.Assert(Enum.IsDefined(typeof(Cathy1Block.BlockType), type));
            //    Type = type;
        }
    }

    /// <summary>
    /// The complete list of properties associated with this block.
    /// Includes bass-class properties.
    /// </summary>
    public override Dictionary<string, string> Properties
    {
        get
        {
            Dictionary<string, string> ret = base.Properties;
            //TODO (do we add anything here?)
            return ret;
        }
        set
        {
            base.Properties = value;
            //TODO
        }
    }

    /**
	 * Called once per frame
	 */
    void Update()
    {
        //TODO bounds checking for gravity/edge mechanics.
    }
}

