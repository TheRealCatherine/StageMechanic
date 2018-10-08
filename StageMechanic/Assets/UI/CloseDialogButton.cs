using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDialogButton : MonoBehaviour
{
	public GameObject Dialog;
	public bool ShowMainMenu;

	public void OnClicked()
	{
		if (ShowMainMenu)
			UIManager.ShowMainMenu();
		Dialog.gameObject.SetActive(false);
	}
}
