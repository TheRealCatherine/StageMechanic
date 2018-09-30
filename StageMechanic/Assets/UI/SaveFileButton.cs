using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveFileButton : MonoBehaviour
{
    public void OnClicked()
	{
		UIManager.SaveToJson();
		UIManager.Instance.HamburgerMenuButton.HideMenu();
	}
}
