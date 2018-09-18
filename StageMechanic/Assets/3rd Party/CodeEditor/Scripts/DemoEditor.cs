using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoEditor : MonoBehaviour {

	public CodeEditor codeEditor;
	public Text linCol;

	/*
		This Script is made for demonstration purposes
		The CodeEditor component doesn't include the window included in this demo
		Feel free to use and modify it wherever you want
	 */

	void Start () {
		codeEditor.mainInput.onValueChanged.AddListener(GetLinCol);
	}

	void Update(){
		if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.Backspace) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetMouseButton(0)){
			GetLinCol();
		}
	}

	public void GetLinCol(string input = null){
		int lin = codeEditor.mainText.textInfo.characterInfo[codeEditor.mainInput.caretPosition].lineNumber;
		int col = ActualCol(codeEditor.mainInput.caretPosition, lin);
		linCol.text = "LIN " + (lin + 1) + ", COL " + (col + 1) ;
	}

	public int ActualCol(int cursorPos, int lin){
		int charCount = 0;
		for (int i = 0; i < lin; i++)
		{
			charCount += codeEditor.mainText.textInfo.lineInfo[i].characterCount;
		}

		return codeEditor.mainInput.caretPosition - charCount;
	}
	
}
