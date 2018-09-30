using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamburgerMenuButton : MonoBehaviour
{
	public GameObject HamburgerMenu;
	private Vector3 MenuPosition;

	private void Start()
	{
		MenuPosition = HamburgerMenu.transform.position;
	}

	public void OnClicked()
	{
		if (HamburgerMenu.IsPrefab())
			HamburgerMenu = Instantiate(HamburgerMenu, transform);

		if (HamburgerMenu.activeInHierarchy)
		{
			HideMenu();
		}
		else
		{
			ShowMenu();
		}
	}

	public void ShowMenu()
	{
		Time.timeScale = 0;
		HamburgerMenu.SetActive(true);
		HamburgerMenu.transform.localScale = Vector3.zero;
		HamburgerMenu.transform.position = transform.position;
		HamburgerMenu.transform.DOScale(1, 0.25f).SetUpdate(true);
		HamburgerMenu.transform.DOMove(MenuPosition, 0.25f).SetUpdate(true);
		transform.DORotate(new Vector3(0, 0, 90), 0.25f).SetUpdate(true);
	}

	public void HideMenu()
	{
		HamburgerMenu.transform.DOMove(transform.position, 0.25f).SetUpdate(true);
		HamburgerMenu.transform.DOScale(0, 0.25f).OnComplete(OnHideMenuComplete).SetUpdate(true);
		transform.DORotate(new Vector3(0, 0, 0), 0.25f).SetUpdate(true);
	}

	private void OnHideMenuComplete()
	{
		HamburgerMenu.SetActive(false);
		Time.timeScale = 1;
	}
}
