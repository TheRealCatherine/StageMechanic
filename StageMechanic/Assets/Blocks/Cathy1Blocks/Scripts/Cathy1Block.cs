using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;
using System;

public class Cathy1Block : MonoBehaviour, IBlock
{

    // Conenience enum for setting multiple properties for common types
    public enum BlockType
    {
        Custom = 0,
        Basic,          //Typical block
        Trap1,          //Spike Trap
        Trap2,          //??? Trap
        Spring,         //Teleport variation, moves character up along edge
        Monster,        //Teleport variation, moves character down from edge
        Ice,            //Teleport variation, moves charcter along top of block
        Vortex,         //Vortex trap
        Bomb1,          //Bomb with short timing
        Bomb2,          //Bomb with long timing
        Crack1,         //Can step on once
        Crack2,         //Can step on twice
        Teleport,       //Moves character from one block to another
        Heavy1,         //Similar to Basic but slower to move
        Heavy2,         //Even slower to move than Heavy1
        Immobile,       //Basic blocks that cannot normally be moved by the player
        Fixed,          //Basic blocks that are fixed in space, cannot be moved no matter what
        Random,         //Not a fixed type, one of a selectable subset
        Goal            //Level completion zone
    }

    public enum TrapBlockType
    {
        None = 0,
        Custom,
        Spike,
        Vortex
    }

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

    //TODO: refactor into different classes

    public GameObject GameObject
    {
        get
        {
            return gameObject;
        }
    }

    /**
	 * Please note that modifying any block properties directly (rather than setting a common type)
	 * will set Type to BlockType.Custom even if the properties you set directly
	 * match ane of the common types. So rather than testing if a block is an ice block it is
	 * usually better to check the Slide property.
	 */
    private BlockType _type = BlockType.Custom;
    public BlockType Type
    {
        get
        {
            return _type;
        }
        set
        {
            _type = value;
            Debug.Log("Setting block to type: " + _type.ToString());
            switch (_type)
            {
                case BlockType.Basic:
                    _trapType = TrapBlockType.None;
                    _isBomb = false;
                    _bombTimeMS = 0;
                    _bombRadius = 0;
                    _teleportType = TeleportBlockType.None;
                    _teleportDistance = new Vector3(0, 0, 0);
                    _collapseSteps = -1;
                    _collapseGrabs = -1;
                    _weightFactor = 1.0F;
                    _isMovableByPlayer = true;
                    _isFixedPosition = false;
                    _gravityFactor = 1.0F;
                    break;
                case BlockType.Trap1:
                    _trapType = TrapBlockType.Spike;
                    _isBomb = false;
                    _bombTimeMS = 0;
                    _bombRadius = 0;
                    _teleportType = TeleportBlockType.None;
                    _teleportDistance = new Vector3(0, 0, 0);
                    _collapseSteps = -1;
                    _collapseGrabs = -1;
                    _weightFactor = 1.0F;
                    _isMovableByPlayer = true;
                    _isFixedPosition = false;
                    _gravityFactor = 1.0F;
                    break;
                case BlockType.Trap2:
                    _trapType = TrapBlockType.Custom;
                    _isBomb = false;
                    _bombTimeMS = 0;
                    _bombRadius = 0;
                    _teleportType = TeleportBlockType.None;
                    _teleportDistance = new Vector3(0, 0, 0);
                    _collapseSteps = -1;
                    _collapseGrabs = -1;
                    _weightFactor = 1.0F;
                    _isMovableByPlayer = true;
                    _isFixedPosition = false;
                    _gravityFactor = 1.0F;
                    break;
                case BlockType.Spring:
                    _trapType = TrapBlockType.None;
                    _isBomb = false;
                    _bombTimeMS = 0;
                    _bombRadius = 0;
                    _teleportType = TeleportBlockType.Side;
                    _teleportDistance = new Vector3(0, 5, 0);
                    _collapseSteps = -1;
                    _collapseGrabs = -1;
                    _weightFactor = 1.0F;
                    _isMovableByPlayer = true;
                    _isFixedPosition = false;
                    _gravityFactor = 1.0F;
                    break;
                case BlockType.Monster:
                    _trapType = TrapBlockType.None;
                    _isBomb = false;
                    _bombTimeMS = 0;
                    _bombRadius = 0;
                    _teleportType = TeleportBlockType.SideNoGrab;
                    _teleportDistance = new Vector3(0, 1, 0);
                    _collapseSteps = -1;
                    _collapseGrabs = -1;
                    _weightFactor = 1.0F;
                    _isMovableByPlayer = true;
                    _isFixedPosition = false;
                    _gravityFactor = 1.0F;
                    break;
                case BlockType.Ice:
                    _trapType = TrapBlockType.None;
                    _isBomb = false;
                    _bombTimeMS = 0;
                    _bombRadius = 0;
                    _teleportType = TeleportBlockType.Platform;
                    _teleportDistance = new Vector3(1, 0, 0);
                    _collapseSteps = -1;
                    _collapseGrabs = -1;
                    _weightFactor = 1.0F;
                    _isMovableByPlayer = true;
                    _isFixedPosition = false;
                    _gravityFactor = 1.0F;
                    break;
                case BlockType.Vortex:
                    _trapType = TrapBlockType.Vortex;
                    _isBomb = false;
                    _bombTimeMS = 0;
                    _bombRadius = 0;
                    _teleportType = TeleportBlockType.None;
                    _teleportDistance = new Vector3(0, 0, 0);
                    _collapseSteps = -1;
                    _collapseGrabs = -1;
                    _weightFactor = 1.0F;
                    _isMovableByPlayer = true;
                    _isFixedPosition = false;
                    _gravityFactor = 1.0F;
                    break;
                case BlockType.Bomb1:
                    _trapType = TrapBlockType.None;
                    _isBomb = true;
                    _bombTimeMS = 5000;
                    _bombRadius = 3;
                    _teleportType = TeleportBlockType.None;
                    _teleportDistance = new Vector3(0, 0, 0);
                    _collapseSteps = -1;
                    _collapseGrabs = -1;
                    _weightFactor = 1.0F;
                    _isMovableByPlayer = true;
                    _isFixedPosition = false;
                    _gravityFactor = 1.0F;
                    break;
                case BlockType.Bomb2:
                    _trapType = TrapBlockType.None;
                    _isBomb = true;
                    _bombTimeMS = 3000;
                    _bombRadius = 3;
                    _teleportType = TeleportBlockType.None;
                    _teleportDistance = new Vector3(0, 0, 0);
                    _collapseSteps = -1;
                    _collapseGrabs = -1;
                    _weightFactor = 1.0F;
                    _isMovableByPlayer = true;
                    _isFixedPosition = false;
                    _gravityFactor = 1.0F;
                    break;
                case BlockType.Crack1:
                    _trapType = TrapBlockType.None;
                    _isBomb = false;
                    _bombTimeMS = 0;
                    _bombRadius = 0;
                    _teleportType = TeleportBlockType.None;
                    _teleportDistance = new Vector3(0, 0, 0);
                    _collapseSteps = 1;
                    _collapseGrabs = -1;
                    _weightFactor = 1.0F;
                    _isMovableByPlayer = true;
                    _isFixedPosition = false;
                    _gravityFactor = 1.0F;
                    break;
                case BlockType.Crack2:
                    _trapType = TrapBlockType.None;
                    _isBomb = false;
                    _bombTimeMS = 0;
                    _bombRadius = 0;
                    _teleportType = TeleportBlockType.None;
                    _teleportDistance = new Vector3(0, 0, 0);
                    _collapseSteps = 2;
                    _collapseGrabs = -1;
                    _weightFactor = 1.0F;
                    _isMovableByPlayer = true;
                    _isFixedPosition = false;
                    _gravityFactor = 1.0F;
                    break;
                case BlockType.Teleport:
                    _trapType = TrapBlockType.None;
                    _isBomb = false;
                    _bombTimeMS = 0;
                    _bombRadius = 0;
                    _teleportType = TeleportBlockType.Platform;
                    _teleportDistance = new Vector3(1, 1, 1);
                    _collapseSteps = -1;
                    _collapseGrabs = -1;
                    _weightFactor = 1.0F;
                    _isMovableByPlayer = true;
                    _isFixedPosition = false;
                    _gravityFactor = 1.0F;
                    break;
                case BlockType.Heavy1:
                    _trapType = TrapBlockType.None;
                    _isBomb = false;
                    _bombTimeMS = 0;
                    _bombRadius = 0;
                    _teleportType = TeleportBlockType.None;
                    _teleportDistance = new Vector3(0, 0, 0);
                    _collapseSteps = -1;
                    _collapseGrabs = -1;
                    _weightFactor = 2.0F;
                    _isMovableByPlayer = true;
                    _isFixedPosition = false;
                    _gravityFactor = 1.0F;
                    break;
                case BlockType.Heavy2:
                    _trapType = TrapBlockType.None;
                    _isBomb = false;
                    _bombTimeMS = 0;
                    _bombRadius = 0;
                    _teleportType = TeleportBlockType.None;
                    _teleportDistance = new Vector3(0, 0, 0);
                    _collapseSteps = -1;
                    _collapseGrabs = -1;
                    _weightFactor = 4.0F;
                    _isMovableByPlayer = true;
                    _isFixedPosition = false;
                    _gravityFactor = 1.0F;
                    break;
                case BlockType.Immobile:
                    _trapType = TrapBlockType.None;
                    _isBomb = false;
                    _bombTimeMS = 0;
                    _bombRadius = 0;
                    _teleportType = TeleportBlockType.None;
                    _teleportDistance = new Vector3(0, 0, 0);
                    _collapseSteps = -1;
                    _collapseGrabs = -1;
                    _weightFactor = 1.0F;
                    _isMovableByPlayer = true;
                    _isFixedPosition = false;
                    _gravityFactor = 1.0F;
                    break;
                case BlockType.Fixed:
                    _trapType = TrapBlockType.None;
                    _isBomb = false;
                    _bombTimeMS = 0;
                    _bombRadius = 0;
                    _teleportType = TeleportBlockType.None;
                    _teleportDistance = new Vector3(0, 0, 0);
                    _collapseSteps = -1;
                    _collapseGrabs = -1;
                    _weightFactor = 1.0F;
                    _isMovableByPlayer = true;
                    _isFixedPosition = false;
                    _gravityFactor = 1.0F;
                    break;
                case BlockType.Goal:
                    _trapType = TrapBlockType.None;
                    _isBomb = false;
                    _bombTimeMS = 0;
                    _bombRadius = 0;
                    _teleportType = TeleportBlockType.None;
                    _teleportDistance = new Vector3(0, 0, 0);
                    _collapseSteps = -1;
                    _collapseGrabs = -1;
                    _weightFactor = 1.0F;
                    _isMovableByPlayer = true;
                    _isFixedPosition = false;
                    _gravityFactor = 1.0F;
                    break;
                case BlockType.Custom:
                default:
                    _trapType = TrapBlockType.None;
                    _isBomb = false;
                    _bombTimeMS = 0;
                    _bombRadius = 0;
                    _teleportType = TeleportBlockType.None;
                    _teleportDistance = new Vector3(0, 0, 0);
                    _collapseSteps = -1;
                    _collapseGrabs = -1;
                    _weightFactor = 1.0F;
                    _isMovableByPlayer = true;
                    _isFixedPosition = false;
                    _gravityFactor = 1.0F;
                    break;
            }
        }
    }

    /**
	 * Currently a synonym of GameObject.name
	 */
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

    /**
	 * If this block should act as a trap block this property
	 * should be set to a value other than TrapBlockType.None
	 */
    private TrapBlockType _trapType = TrapBlockType.None;
    public TrapBlockType TrapType
    {
        get
        {
            return _trapType;
        }
        set
        {
            _trapType = value;
            this.Type = BlockType.Custom;
        }
    }

    /**
	 * Will be true if this block is any type of trap block
	 */
    public bool IsTrap
    {
        get
        {
            return this.TrapType != TrapBlockType.None;
        }
    }

    /**
	 * If true, this block will be destoryed BombTimeMS milliseconds
	 * after TriggerBomb() is called.
	 */
    private bool _isBomb = false;
    public bool IsBomb
    {
        get
        {
            return _isBomb;
        }
        set
        {
            _isBomb = value;
            this.Type = BlockType.Custom;
        }
    }


    /**
	 * Number of miliseconds after TriggerBomb() is called to wait
	 * before destorying the block.
	 */
    private int _bombTimeMS = 0;
    public int BombTimeMS
    {
        get
        {
            return _bombTimeMS;
        }
        set
        {
            _bombTimeMS = value;
            this.Type = BlockType.Custom;
        }
    }

    /**
	 * How large of an area should be affected by this blocks destruction
	 * via the TriggerBomb() method is called. Note that one standard
	 * block is 10x10x10 so to affect 3 normal blocks set this to 30.
	 */
    private int _bombRadius = 0;
    public int BombRadius
    {
        get
        {
            return _bombRadius;
        }
        set
        {
            _bombRadius = value;
            this.Type = BlockType.Custom;
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
            this.Type = BlockType.Custom;
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
            this.Type = BlockType.Custom;
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
            this.Type = BlockType.Custom;
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
            this.Type = BlockType.Custom;
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
	 * Used to make blocks heavier or lighter (faster/slower to move)
	 * 1 means normal movement speed, 2 takes twice as long, 0.5 takes half as long
	 */
    private float _weightFactor = 1.0F;
    public float WeightFactor
    {
        get
        {
            return _weightFactor;
        }
        set
        {
            _weightFactor = value;
            this.Type = BlockType.Custom;
        }
    }

    /**
	 * Returns true if the block is heavier than normal
	 */
    public bool IsHeavy
    {
        get
        {
            return _weightFactor > 1.0F;
        }
    }

    /**
	 * Returns true if the block is lighter than normal
	 */
    public bool IsLight
    {
        get
        {
            return _weightFactor < 1.0F;
        }
    }

    /**
	 * Set to true if the block cannot be moved by the player
	 */
    private bool _isMovableByPlayer = true;
    public bool IsMovableByPlayer
    {
        get
        {
            return _isMovableByPlayer;
        }
        set
        {
            _isMovableByPlayer = value;
            this.Type = BlockType.Custom;
        }
    }

    /**
	 * Set to true if this block cannot be moved by any means even gravity/enemies/etc
	 */
    private bool _isFixedPosition = false;
    public bool IsFixedPosition
    {
        get
        {
            return _isFixedPosition;
        }
        set
        {
            _isFixedPosition = value;
            this.Type = BlockType.Custom;
        }
    }

    /**
	 * How this block should react to gravity. 1.0 means it falls at the standard speed
	 * 2.0 means it falls twice as quickly.
	 * -1.0 falls upward at normal speed
	 */
    private float _gravityFactor = 1.0F;
    public float GravityFactor
    {
        get
        {
            return _gravityFactor;
        }
        set
        {
            _gravityFactor = value;
            this.Type = BlockType.Custom;
        }
    }

    /**
	 * An Item associated with this Block, for example powerups
	 * coins, as well as start/end markers. Note that setting
	 * an item does not cause the block type to change to custom
	 * and the block will take ownership of the item (ie the
	 * item will be destoryed when the block is.
	 */
    private GameObject _item;
    public GameObject Item
    {
        get
        {
            return _item;
        }
        set
        {
            _item = value;
            if (_item != null)
                _item.transform.parent = transform;
        }
    }

    public string TypeName
    {
        get
        {
            return Type.ToString();
        }
        set
        {
            Cathy1Block.BlockType type = (Cathy1Block.BlockType)Enum.Parse(typeof(Cathy1Block.BlockType), value);
            Debug.Assert(Enum.IsDefined(typeof(Cathy1Block.BlockType), type));
            Type = type;
        }
    }

    public bool IsFixedRotation
    {
        get
        {
            return false;
        }
        set
        {
        }
    }

    List<GameObject> IBlock.Items {
        get
        {
            List<GameObject> ret = new List<GameObject>();
            ret.Add(Item);
            return ret;
        }
        set
        {
            if(value.Count>0)
                Item = value[0];
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

    public GameObject Parent
    {
        get
        {
            return transform.parent.gameObject;
        }

        set
        {
            if (value == null)
                transform.SetParent(null);
            else
                transform.SetParent(value.transform,true);
        }
    }

    public List<GameObject> Children
    {
        get
        {
            List<GameObject> chillins = new List<GameObject>();
            foreach(GameObject kiddo in transform)
            {
                Cathy1Block jennyFromTheBlock = kiddo.GetComponent<Cathy1Block>();
                if(jennyFromTheBlock != null)
                    chillins.Add(kiddo);
            }
            return chillins;
        }
        //TODO remove items not in list or clear list first
        set
        {
            foreach(GameObject rugrat in value)
            {
                //Cathy1 blocks can currently only have Cathy1 blocks as children
                //Items are also children technically, but should be accessd via Item or Items
                Cathy1Block blockPartyLikeIts1999 = rugrat.GetComponent<Cathy1Block>();
                if (blockPartyLikeIts1999 != null)
                    blockPartyLikeIts1999.Parent = GameObject;
            }
        }
    }

    /**
	 * Create and return a new JSON delegate for this Block
	 * This is because GameObjects cannot directly be used
	 * [DataContract] classes. This is primarily used to 
	 * serialize the Block information for saving level data
	 */
    public BlockJsonDelegate GetJsonDelegate()
    {
        return new BlockJsonDelegate(this);
    }

    /**
	 * Called when Block objects are created.
	 * Initializes the name property to a random GUID
	 */
    void Start()
    {
        name = System.Guid.NewGuid().ToString();
    }

    /**
	 * Called when this block is destroyed. Note that this
	 * destorys any items or child blocks as well.
	 */
    void OnDestroy()
    {
        //Destroy any items attached to this block
        if (_item != null)
            Destroy(_item);
    }

    /**
	 * Called once per frame
	 */
    void Update()
    {
        //TODO bounds checking for gravity/edge mechanics.
    }
}

