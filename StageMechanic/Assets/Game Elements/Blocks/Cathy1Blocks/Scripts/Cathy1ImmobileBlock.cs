using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cathy1ImmobileBlock : Cathy1Block {

	public override Cathy1Block.BlockType Type {
		get {
			return Cathy1Block.BlockType.Immobile;
		}
	}

	// Use this for initialization
	void Start () {
		WeightFactor = 0f;
	}
}
