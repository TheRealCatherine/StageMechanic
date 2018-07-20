using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloxelsStoryBlock : AbstractBloxelsBlock
{
	public TextMesh  TextMeshObject;
	private string   Text = "";
	private string   TextColor = "#ffffff";
	private int	 TextSize = 0;
	private const string	PROPNAME_TEXT = "Text";
	private const string	PROPNAME_TEXT_SIZE = "Size";
	private const string 	PROPNAME_TEXT_COLOR = "Color (#rrggbb)";

	private const string	DEFAULT_TEXT = "Press Enter to edit text";
	private const int	DEFAULT_TEXT_SIZE = 0;
	private const string 	DEFAULT_TEXT_COLOR = "#ffffff";

	public override void Awake()
	{
		base.Awake();
		DensityFactor = 0.0f;
	}

	public override string TypeName
	{
		get
		{
			return "Story";
		}

		set
		{
			throw new System.NotImplementedException();
		}
	}

	public static Color hexToColor(string hex) {
		hex = hex.Replace ("0x", "");//in case the string is formatted 0xFFFFFF
		hex = hex.Replace ("#", "");//in case the string is formatted #FFFFFF
		byte a = 255;//assume fully visible unless specified in hex
		byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
		//Only use alpha if the string has enough characters
		if(hex.Length == 8){
		a = byte.Parse(hex.Substring(6,2), System.Globalization.NumberStyles.HexNumber);
		}
		return new Color32(r,g,b,a);
	}

	public override Dictionary<string, DefaultValue> DefaultProperties
	{
		get
		{
			Dictionary<string, DefaultValue> ret = base.DefaultProperties;
			ret.Add(PROPNAME_TEXT,   new DefaultValue { TypeInfo = typeof(MultilinePlaintext), Value = DEFAULT_TEXT } );
			ret.Add(PROPNAME_TEXT_COLOR,   new DefaultValue { TypeInfo = typeof(string), Value = DEFAULT_TEXT_COLOR } );
			ret.Add(PROPNAME_TEXT_SIZE,   new DefaultValue { TypeInfo = typeof(int), Value = DEFAULT_TEXT_SIZE.ToString() } );
			return ret;
		}
	}

	public override Dictionary<string, string> Properties
	{
		get
		{
			Dictionary<string, string> ret = base.Properties;
			ret.Add(PROPNAME_TEXT, Text);
			ret.Add(PROPNAME_TEXT_COLOR, TextColor);
			ret.Add(PROPNAME_TEXT_SIZE, TextSize.ToString());
			return ret;
		}
		set
		{
			base.Properties = value;
			if (value.ContainsKey(PROPNAME_TEXT)) {
				Text = value[PROPNAME_TEXT];
				TextMeshObject.text = Text;
			}
			if (value.ContainsKey(PROPNAME_TEXT_COLOR)) {
				TextColor = value[PROPNAME_TEXT_COLOR];
				TextMeshObject.color = hexToColor(TextColor);
			}
			if (value.ContainsKey(PROPNAME_TEXT_SIZE)) {
				TextSize = int.Parse(value[PROPNAME_TEXT_SIZE]);
				TextMeshObject.fontSize = 100 + TextSize*10;
			}
		}
	}

	public override void ApplyTheme(AbstractBlockTheme theme)
	{
		throw new System.NotImplementedException();
	}
}
