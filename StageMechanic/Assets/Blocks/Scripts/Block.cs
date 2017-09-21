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

	private int _bombTimeMS = 5000;
	public int BombTimeMS {
		get {
			return _bombTimeMS;
		}
		set {
			_bombTimeMS = value;
			this.Type = BlockType.Custom;
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
