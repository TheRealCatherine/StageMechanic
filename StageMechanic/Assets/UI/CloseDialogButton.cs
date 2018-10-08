using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDialogButton : MonoBehaviour
{
	public GameObject Dialog;
	public bool ShowMainMenu;
	public bool EnterEditMode;
	public bool EnterPlayMode;

	private void OnEnable()
	{
		Time.timeScale = 0;
	}

	public void OnClicked()
	{
		if (ShowMainMenu)
			UIManager.ShowMainMenu();
		else if (EnterEditMode)
		{

			if (BlockManager.PlayMode)
				BlockManager.Instance.TogglePlayMode();
		}
		else if (EnterPlayMode)
		{
			if (BlockManager.PlayMode)
				BlockManager.Instance.TogglePlayMode();
			BlockManager.Instance.TogglePlayMode();
		}
		Time.timeScale = 1;
		Dialog.gameObject.SetActive(false);
	}
}
