using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiLinePrefabButton : MonoBehaviour
{
	public GameObject TextEditorPrefab;
	public InputField Display;
	private GameObject Editor;

	public void OnClick()
	{
		Canvas mainCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
		Editor = Instantiate(TextEditorPrefab, mainCanvas.transform);
		Editor.GetComponentInChildren<InputField>().text = Display.text;
	}

	private void Update()
	{
		if (Editor != null)
		{
			Display.text = Editor.GetComponentInChildren<InputField>().text;

			if (!Editor.activeInHierarchy)
			{
				Destroy(Editor);
				Editor = null;
			}
		}
	}
}
