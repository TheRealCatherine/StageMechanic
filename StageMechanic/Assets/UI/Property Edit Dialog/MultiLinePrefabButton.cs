using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiLinePrefabButton : MonoBehaviour
{
	public GameObject TextEditorPrefab;
	public InputField Display;
	private GameObject Editor;
	public string FunctionName;

	public void OnClick()
	{
		Canvas mainCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
		Editor = Instantiate(TextEditorPrefab, mainCanvas.transform);
		if (GameManager.IsLiteBuild && Application.platform == RuntimePlatform.Android)
		{
			Editor.GetComponentInChildren<InputField>().text = Display.text;
			Editor.GetComponentInChildren<InputField>().gameObject.SetActive(true);
			Editor.GetComponentInChildren<CodeEditor>().gameObject.SetActive(false);
		}
		else
		{
			Editor.GetComponentInChildren<CodeEditor>().mainInput.text = Display.text;
			Editor.GetComponentInChildren<CodeEditor>().WriteEvent(Display.text);
			Editor.GetComponentInChildren<CodeEditor>().gameObject.SetActive(true);
			Editor.GetComponentInChildren<InputField>().gameObject.SetActive(false);
		}
		foreach(Text field in Editor.GetComponentsInChildren<Text>())
		{
			if (field.name == "FunctionName")
				field.text = FunctionName;
		}

	}

	private void Update()
	{
		if (Editor != null)
		{
			if(GameManager.IsLiteBuild && Application.platform == RuntimePlatform.Android)
				Display.text = Editor.GetComponentInChildren<InputField>().text;
			else
				Display.text = Editor.GetComponentInChildren<CodeEditor>().mainInput.text;
			if (!Editor.activeInHierarchy)
			{
				Destroy(Editor);
				Editor = null;
			}
		}
	}
}
