/*  
 * Copyright (C) 2018 You're Perfect Studio. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using UnityEngine;
using UnityEngine.UI;

public class CreateEditLevelDialog : MonoBehaviour
{

	public Button CreateNewButton;
	public Button EditLevelButton;

	void Start()
	{
		CreateNewButton.onClick.AddListener(OnCreateNewButtonClicked);
		EditLevelButton.onClick.AddListener(OnEditLevelClicked);
	}

	public void Show()
	{
		//PlayerManager.DestroyAllPlayers();
		gameObject.SetActive(true);
	}

	void OnCreateNewButtonClicked()
	{
		Debug.Log("Create clicked");
		if (BlockManager.PlayMode)
			BlockManager.Instance.TogglePlayMode();
		BlockManager.Clear();
		gameObject.SetActive(false);
	}

	void OnEditLevelClicked()
	{
		Debug.Log("Edit clicked");
		if (BlockManager.PlayMode)
			BlockManager.Instance.TogglePlayMode();
		gameObject.SetActive(false);
	}

}
