using System;
using UnityEngine;
using UnityEngine.UI;

public class NetworkLoadDialog : MonoBehaviour {

	public InputField UrlInput;
	public Text Label;
	public Button GoButton;
	public Button CancelButton;
	public Button CopyButton;

	void Update () {
		if (string.IsNullOrEmpty(UrlInput.text) || UrlInput.text.Length < 6)
			GoButton.interactable = false;
		else
			GoButton.interactable = true;
	}

	public void OnGoClicked()
	{
		if (UrlInput.text.Contains("//") || UrlInput.text.Contains("."))
		{
			Uri uriResult;
			bool result = Uri.TryCreate(UrlInput.text, UriKind.Absolute, out uriResult)
				&& (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
			if (result)
			{
				Serializer.LoadFileUsingHTTP(uriResult);
				gameObject.SetActive(false);
			}
			else
			{
				UrlInput.text = null;
				UrlInput.placeholder.GetComponent<Text>().text = "Error...";
			}
		}
		else
		{
			Serializer.LoadFromGameJolt(UrlInput.text);
			gameObject.SetActive(false);
		}
	}

	private void OnDisable()
	{
		UrlInput.text = null;
	}

	public void OnCancelClicked()
	{
		UIManager.ShowMainMenu();
		gameObject.SetActive(false);
	}

	public void OnCopyClicked()
	{
		UrlInput.text.CopyToClipboard();
	}
}
