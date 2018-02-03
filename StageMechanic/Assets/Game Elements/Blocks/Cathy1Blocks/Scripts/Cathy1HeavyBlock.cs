public class Cathy1HeavyBlock : Cathy1Block {

	public override Cathy1Block.BlockType Type {
		get {
			return Cathy1Block.BlockType.Heavy;
		}
	}

	internal override void Start () {
        base.Start();
        WeightFactor = 2.5f;
	}
}
