using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushPullBlock : AbstractPushPullBlock {

	public GameObject[] Variations;
	public int CurrentVariation = 0;

	public override string TypeName
	{
		get
		{
			return "Block";
		}

		set
		{
			throw new System.NotImplementedException();
		}
	}

	public void ShowVariation(int variation)
	{
		if (variation < 1)
			variation = 1;
		if (variation == CurrentVariation)
			return;
		if (CurrentModel != null)
			Destroy(CurrentModel);
		Debug.Assert(Variations.Length >= variation);
		CurrentModel = Instantiate(Variations[variation-1], transform);
		CurrentVariation = variation;
		BlockManager.AddBlockToGroup(this,variation);
	}

	public override void ApplyTheme(PushPullBlockTheme theme)
	{
		Debug.Assert(theme.Blocks != null);
		Debug.Assert(theme.Blocks.Length > 0);
		Variations = theme.Blocks;

		int oldVariation = CurrentVariation;
		CurrentVariation = 0;
		ShowVariation(oldVariation);
	}

	public override Dictionary<string, string> Properties
	{
		get
		{
			Dictionary<string, string> ret = base.Properties;
			if (CurrentVariation > 1)
				ret.Add("Model Variant", CurrentVariation.ToString());
			return ret;
		}
		set
		{
			base.Properties = value;
			if (value.ContainsKey("Model Variant"))
			{
				ShowVariation(int.Parse(value["Model Variant"]));
				Debug.Log(int.Parse(value["Model Variant"]));
			}
		}
	}
}
