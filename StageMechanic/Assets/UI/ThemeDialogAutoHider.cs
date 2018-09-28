using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThemeDialogAutoHider : MonoBehaviour
{
	public ScrollRect BlockThemes;
	public ScrollRect ItemThemes;

    void OnEnable()
    {
		BlockThemes.gameObject.SetActive(true);
		ItemThemes.gameObject.SetActive(true);
	}

    void Update()
    {
		if (BlockThemes.gameObject.activeInHierarchy || ItemThemes.gameObject.activeInHierarchy)
			return;
		gameObject.SetActive(false);
    }
}
