/*  
 * Copyright (C) 2018 You're Perfect Studio. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using UnityEngine;

public class Platform : MonoBehaviour {

	public GameObject Plane;

	public void OnGameModeChanged(GameManager.GameMode newMode, GameManager.GameMode oldMode)
	{
		if (newMode == GameManager.GameMode.Play)
			Plane.SetActive(false);
		else
			Plane.SetActive(true);
	}
}
