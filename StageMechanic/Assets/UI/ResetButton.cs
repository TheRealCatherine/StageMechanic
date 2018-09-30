using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetButton : MonoBehaviour
{
	Button _asButton;

	private void Start()
	{
		_asButton = GetComponent<Button>();
	}

	/// <summary>
	/// If for some reason Serializer doesn't have a start state we disable this button.
	/// </summary>
	void Update()
    {
		_asButton.enabled = Serializer.HasStartState();
    }

	public void OnClick()
	{
		if (BlockManager.PlayMode)
			BlockManager.Instance.TogglePlayMode();
		Serializer.ReloadStartState();
		BlockManager.Instance.TogglePlayMode(0.1f);

		UIManager.Instance.HamburgerMenuButton.HideMenu();
	}
}
