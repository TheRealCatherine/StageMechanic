using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushPullGameboardBlock : AbstractPushPullBlock {

	public GameObject Variation1;
	public GameObject Variation2;
	public int CurrentVariation = 0;

	public override string TypeName
	{
		get
		{
			return "Gameboard";
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
		if (variation == 1)
			CurrentModel = Instantiate(Variation1, transform);
		else
			CurrentModel = Instantiate(Variation2, transform);
		CurrentVariation = variation;

	}

	public override void ApplyTheme(PushPullBlockTheme theme)
	{
		Debug.Assert(theme.Gameboard1 != null);
		Debug.Assert(theme.Gameboard2 != null);

		Variation1 = theme.Gameboard1;
		Variation2 = theme.Gameboard2;

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
