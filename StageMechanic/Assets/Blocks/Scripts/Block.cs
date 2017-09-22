using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

	// Conenience enum for setting multiple properties for common types
	public enum BlockType {
		Custom = 0,
		Basic,			//Typical block
		Trap1,			//Spike Trap
		Trap2,			//??? Trap
		Spring,			//Teleport variation, moves character up along edge
		Monster,		//Teleport variation, moves character down from edge
		Ice,			//Teleport variation, moves charcter along top of block
		Vortex,			//Vortex trap
		Bomb1,			//Bomb with short timing
		Bomb2,			//Bomb with long timing
		Crack1,			//Can step on once
		Crack2,			//Can step on twice
		Teleport,		//Moves character from one block to another
		Goal,			//Level completion zone
		Heavy1,			//Similar to Basic but slower to move
		Heavy2,			//Even slower to move than Heavy1
		Immobile,		//Basic blocks that cannot normally be moved by the player
		Fixed			//Basic blocks that are fixed in space, cannot be moved no matter what
	}

	public enum TrapBlockType {
		None = 0,
		Custom,
		Spike,
		Vortex
	}

	public enum TeleportBlockType {
		None = 0,
		Custom,
		Side,				//Move player up or down along the side of the blocks, allows edge grabbing
		SideNoGrab,			//Same as Slide, but does not allow edge grabbing
		Platform,			//Move player to the top of a block from the top of this block
		PlatformToSide, 	//Move player from top of platform to an edge grab
		SideToPlatform		//Move player from an edge grab to the top of a platform
	}

	// Properties

	// Please note that modifying any block properties directly (rather than setting a common type
	// via this one) will set Type to BlockType.Custom even if the properties you set directly
	// match ane of the common types. So rather than testing if a block is an ice block it is
	// usually better to check the Slide property.
	private BlockType _type = BlockType.Custom;
	public BlockType Type {
		get {
			return _type;
		}
		set {
			_type = value;
		}
	}

	//Returns true if this is a customized block type
	public bool IsCustomType() {
		return this.Type == BlockType.Custom;
	}

	// If this block should act as a trap block this property
	// should be set to a value other than TrapBlockType.None
	private TrapBlockType _trapType = TrapBlockType.None;
	public TrapBlockType TrapType {
		get {
			return _trapType;
		}
		set {
			_trapType = value;
			this.Type = BlockType.Custom;
		}
	}

	// Returns true if this block is any type of trap block
	public bool IsTrap() {
		return this.TrapType != TrapBlockType.None;
	}

	// If true, this block will be destoryed BombTimeMS milliseconds
	// after TriggerBomb() is called.
	private bool _isBomb = false;
	public bool IsBomb {
		get {
			return _isBomb;
		}
		set {
			_isBomb = value;
			this.Type = BlockType.Custom;
		}
	}


	// Number of miliseconds after TriggerBomb() is called to wait
	// before destorying the block.
	private int _bombTimeMS = 0;
	public int BombTimeMS {
		get {
			return _bombTimeMS;
		}
		set {
			_bombTimeMS = value;
			this.Type = BlockType.Custom;
		}
	}

	// How large of an area should be affected by this blocks destruction
	// via the TriggerBomb() method is called. Note that one standard
	// block is 10x10x10 so to affect 3 normal blocks set this to 30.
	private int _bombRadius = 0;
	public int BombRadius {
		get {
			return _bombRadius;
		}
		set {
			_bombRadius = value;
			this.Type = BlockType.Custom;
		}
	}

	// Property describing the the type of movement action to the player
	// this block exerts. For example spring blocks move the player up
	// along the side, allowing grabbing. Ice blocks move the player
	// along the platform to the next block, etc.
	private TeleportBlockType _teleportType = TeleportBlockType.None;
	public TeleportBlockType TeleportType {
		get {
			return _teleportType;
		}
		set {
			_teleportType = value;
			this.Type = BlockType.Custom;
		}
	}

	//Returns true if this block is any type of teleport block
	public bool IsTeleport() {
		return this.TeleportType != TeleportBlockType.None;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
