/*  
 * Copyright (C) 2018 You're Perfect Studio. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using UnityEngine;

public class ShowMainMenu : MonoBehaviour {

	public GameObject ConfirmationDialog;

	public void onButtonClicked()
    {
		if(!BlockManager.PlayMode)
		{
			ConfirmationDialog.SetActive(true);
		}
		else
		{
			UIManager.ShowMainMenu();
		}
		UIManager.Instance.HamburgerMenuButton.HideMenu();
    }
}
