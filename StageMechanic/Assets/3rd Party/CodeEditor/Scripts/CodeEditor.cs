using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CodeEditor : MonoBehaviour {

	[Header("References")]
	public TMP_InputField mainInput;
	public TextMeshProUGUI mainText;
	public TextMeshProUGUI linesText;
	public TextMeshProUGUI coloredText;
	public RectTransform highlight;
	public GameObject[] styleRefs;
	[Space]
	[Header("Editor Style")]
	public Color caretColor;
	public Color background;
	public Color lineHighlight;
	public Color lineCountBg;
	public Color lineCountTxt;
	public Color scrollbar;

	[Space]
	[Header("Syntax Style")]
	public Color keywordColor;
	public Color classColor;
	public Color typeColor;
	public Color functionColor;
	public Color blacklistColor;
	public Color commentColor;
	public Color stringColor;
	private int lineCount = 0;
	private bool workingParse = true;

	void Start(){

		LoadTheme();
		mainInput.onValueChanged.AddListener(WriteEvent);

	}

	void Update(){

		if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.Backspace) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetMouseButton(0)){
			HighlightLine();
		}

		//Input field paste bugfix
		if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyUp(KeyCode.V)){
			workingParse = false;
		}
		
		if (Input.GetKeyUp(KeyCode.LeftControl)){
			workingParse = true;
			WriteEvent(mainInput.text);
		}

	}

	public void HighlightLine(string input = null){

		highlight.anchoredPosition = new Vector2(5, mainText.textInfo.lineInfo[mainText.textInfo.characterInfo[0].lineNumber].lineHeight * (-mainText.textInfo.characterInfo[mainInput.caretPosition].lineNumber) - 4 + mainText.GetComponent<RectTransform>().anchoredPosition.y);
	
	}

	public void WriteEvent(string input){

		coloredText.text = ParseOutput(input);

		if (mainInput.verticalScrollbar.size < 1){
			mainInput.verticalScrollbar.gameObject.SetActive(true);
		} else {
			mainInput.verticalScrollbar.gameObject.SetActive(false);
		}

		int actualCount = input.Split('\n').Length;

		if (actualCount != lineCount){
			lineCount = actualCount;
			string lineString = "";
			for (int i = 1; i < actualCount+1; i++)
			{
				lineString += i + "\n";
			}
			linesText.text = lineString;
		}

		HighlightLine();
		
	}

	string ParseOutput(string input){

			if(!workingParse){ return input; }
			
		    //string keywords = @"\b(public|private|partial|static|namespace|class|using|void|foreach|in|return|string|float|int|bool)\b";
            MatchCollection keywordMatches = Regex.Matches(input, LuaScriptingManager.Keywords);
			int delta = 0;
			string color = ColorUtility.ToHtmlStringRGB(keywordColor);

			foreach (Match m in keywordMatches)
            {
				input = input.Insert(m.Index + delta, "<#" + color + ">");
				delta += 9;
				input = input.Insert(m.Index + m.Length + delta, "</color>");
				delta += 8;
            }

		//string functions = @"\b(Color|Color32|Vector2|Vector3|GameObject|MonoBehaviour)\b";
		MatchCollection functionMatches = Regex.Matches(input, LuaScriptingManager.Functions, RegexOptions.None);
		delta = 0;
		color = ColorUtility.ToHtmlStringRGB(functionColor);

		foreach (Match m in functionMatches)
		{
			input = input.Insert(m.Index + delta, "<#" + color + ">");
			delta += 9;
			input = input.Insert(m.Index + m.Length + delta, "</color>");
			delta += 8;
		}

		//string functions = @"\b(Color|Color32|Vector2|Vector3|GameObject|MonoBehaviour)\b";
		MatchCollection blackListMatches = Regex.Matches(input, LuaScriptingManager.Blacklist, RegexOptions.None);
		delta = 0;
		color = ColorUtility.ToHtmlStringRGB(blacklistColor);

		foreach (Match m in blackListMatches)
		{
			input = input.Insert(m.Index + delta, "<#" + color + ">");
			delta += 9;
			input = input.Insert(m.Index + m.Length + delta, "</color>");
			delta += 8;
		}

		// string classes = @"\b(System|UnityEngine|TMPro)\b";
		MatchCollection classesMatches = Regex.Matches(input,LuaScriptingManager.Classes);
			delta = 0;
			color = ColorUtility.ToHtmlStringRGB(classColor);

            foreach (Match m in classesMatches)
            {
				input = input.Insert(m.Index + delta, "<#" + color + ">");
				delta += 9;
				input = input.Insert(m.Index + m.Length + delta, "</color>");
				delta += 8;
            }

			//string types = @"\b(Color|Color32|Vector2|Vector3|GameObject|MonoBehaviour)\b";
            MatchCollection typeMatches = Regex.Matches(input, LuaScriptingManager.Types);
			delta = 0;
			color = ColorUtility.ToHtmlStringRGB(typeColor);

            foreach (Match m in typeMatches)
            {
				input = input.Insert(m.Index + delta, "<#" + color + ">");
				delta += 9;
				input = input.Insert(m.Index + m.Length + delta, "</color>");
				delta += 8;
            }

			string comments = @"(\/\/.+?$|\/\*.+?\*\/)";   
            MatchCollection commentMatches = Regex.Matches(input, comments, RegexOptions.Multiline);
			delta = 0;
			color = ColorUtility.ToHtmlStringRGB(commentColor);

			foreach (Match m in commentMatches)
            {
				input = input.Insert(m.Index + delta, "<#" + color + ">");
				delta += 9;
				input = input.Insert(m.Index + m.Length + delta, "</color>");
				delta += 8;
            }

            string strings = "\".+?\"";
            MatchCollection stringMatches = Regex.Matches(input, strings);
			delta = 0;
			color = ColorUtility.ToHtmlStringRGB(stringColor);

			foreach (Match m in stringMatches)
            {
				input = input.Insert(m.Index + delta, "<#" + color + ">");
				delta += 9;
				input = input.Insert(m.Index + m.Length + delta, "</color>");
				delta += 8;
            }

			return input;

	}

	public void LoadTheme(){
		mainInput.caretColor = caretColor;
		styleRefs[0].GetComponent<Image>().color = background;
		highlight.GetComponent<Image>().color = lineHighlight;
		styleRefs[1].GetComponent<Image>().color = lineCountBg;
		linesText.color = lineCountTxt;
		styleRefs[2].GetComponent<Image>().color = scrollbar;
	}
	
}