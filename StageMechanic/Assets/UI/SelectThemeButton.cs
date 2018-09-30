using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectThemeButton : MonoBehaviour
{
	public GameObject ThemeSelectWindowPrefab;

	public void OnClicked()
	{
		if(ThemeSelectWindowPrefab.IsPrefab())
			ThemeSelectWindowPrefab = Instantiate(ThemeSelectWindowPrefab, transform);

		ThemeSelectWindowPrefab.SetActive(true);
		UIManager.Instance.HamburgerMenuButton.HideMenu();
	}
}
