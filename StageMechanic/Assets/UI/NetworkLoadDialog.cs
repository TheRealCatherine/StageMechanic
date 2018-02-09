using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkLoadDialog : MonoBehaviour {

	public InputField UrlInput;
	public Button GoButton;
	public Button CancelButton;

	void Update () {
		if (string.IsNullOrEmpty(UrlInput.text))
			GoButton.interactable = false;
		else
			GoButton.interactable = true;
	}

	public void OnGoClicked()
	{
		Uri uriResult;
		bool result = Uri.TryCreate(UrlInput.text, UriKind.Absolute, out uriResult)
			&& (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
		if (result) {
			Serializer.LoadFileUsingHTTP(uriResult);
			gameObject.SetActive(false);
		}
		else
		{
			UrlInput.text = null;
			UrlInput.placeholder.GetComponent<Text>().text = "Error...";
		}

	}

	public void OnCancelClicked()
	{
		UIManager.ShowMainMenu();
		gameObject.SetActive(false);
	}
}
