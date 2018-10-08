using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDialogButton : MonoBehaviour
{
	public GameObject Dialog;

	public void OnClicked()
	{
		Dialog.gameObject.SetActive(false);
	}
}
