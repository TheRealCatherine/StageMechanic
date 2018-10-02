using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThemeDialogAutoHider : MonoBehaviour
{
	public ScrollRect BlockThemes;
	public ScrollRect ItemThemes;
	private bool _restoreMainMenu;

    void OnEnable()
    {
		_restoreMainMenu = UIManager.Instance.MainMenu.isActiveAndEnabled;
		UIManager.Instance.MainMenu.gameObject.SetActive(false);
		Time.timeScale = 0;
	}

	private void OnDisable()
	{
		if(_restoreMainMenu)
			UIManager.Instance.MainMenu.gameObject.SetActive(true);
	}

	void Update()
    {
    }
}
