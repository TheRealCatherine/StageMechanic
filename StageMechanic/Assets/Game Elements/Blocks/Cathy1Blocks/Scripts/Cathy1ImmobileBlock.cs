public class Cathy1ImmobileBlock : Cathy1Block {

	public override Cathy1Block.BlockType Type {
		get {
			return Cathy1Block.BlockType.Immobile;
		}
	}

	public override void Awake () {
        base.Awake();
		WeightFactor = 0f;
	}
}
