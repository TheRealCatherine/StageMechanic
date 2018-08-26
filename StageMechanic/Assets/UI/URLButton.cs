using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class URLButton : MonoBehaviour {

	public string URL;

	public void OnClicked()
	{
		Application.OpenURL(URL);
	}
}
