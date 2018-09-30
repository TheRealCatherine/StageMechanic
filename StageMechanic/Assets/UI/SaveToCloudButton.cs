using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveToCloudButton : MonoBehaviour
{
	public void OnClicked()
	{
		Serializer.SaveToGameJolt();
		UIManager.Instance.HamburgerMenuButton.HideMenu();
	}
}
