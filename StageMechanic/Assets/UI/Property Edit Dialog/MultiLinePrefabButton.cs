using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiLinePrefabButton : MonoBehaviour
{
	public GameObject TextEditorPrefab;

	public void OnClick()
	{
		Canvas mainCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
		GameObject editor = Instantiate(TextEditorPrefab, mainCanvas.transform);
		
	}
}
