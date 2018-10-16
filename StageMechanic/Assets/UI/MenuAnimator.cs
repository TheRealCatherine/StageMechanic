using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MenuAnimator : MonoBehaviour
{
	public GameObject[] Elements;
	public float AnimationDuration = 0.25f;

	private List<Vector3> ButtonLocations = new List<Vector3>();

	private Vector3 ExampleStagesButtonPosition;
	private Vector3 LoadFromCloudButtonPosition;
	private Vector3 LoadFileButtonPosition;


	private void _RESET()
	{
		if(ButtonLocations.Count != Elements.Length)
		{
			ButtonLocations.Clear();
			foreach (GameObject element in Elements)
			{
				if(element != null)
					ButtonLocations.Add(element.transform.position);
			}
		}

		foreach (GameObject element in Elements)
		{
			if (element == null)
				continue;
			element.transform.position = transform.position;
			element.transform.localScale = Vector3.zero;
			element.transform.gameObject.SetActive(false);
		}
	}

	void Start()
    {
		_RESET();
	}

	public void OnClicked()
	{
		Debug.Assert(Elements.Length > 0);
		if (Elements[0].gameObject.activeInHierarchy)
			Hide();
		else
			Show();
	}

	public void Show()
	{
		foreach (MenuAnimator menu in UIManager.Instance.MainMenu.MenuButtons)
		{
			if(menu != null && menu != this)
				menu.Hide();
		}
		for (int i = 0; i < Elements.Length; ++i)
		{
			Elements[i].gameObject.SetActive(true);
			Elements[i].transform.DOMove(ButtonLocations[i], AnimationDuration);
			Elements[i].transform.DOScale(1, AnimationDuration);
		}
	}

	public void Hide()
	{
		for (int i = 0; i < Elements.Length; ++i)
		{
			if (Elements[i] == null)
				continue;
			Elements[i].transform.DOMove(transform.position, AnimationDuration);
			Elements[i].transform.DOScale(0, AnimationDuration).OnComplete(OnHideComplete);
		}
	}

	public void OnHideComplete()
	{
		for (int i = 0; i < Elements.Length; ++i)
		{
			if (Elements[i] == null)
				continue;
			Elements[i].gameObject.SetActive(false);
		}

	}
}
