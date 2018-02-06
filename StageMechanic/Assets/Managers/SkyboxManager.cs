/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
 using UnityEngine;

public class SkyboxManager : MonoBehaviour {

    private static SkyboxManager Instance;
    public Camera MainCamera;
    public Material[] Skyboxes;

	// Use this for initialization
	void Start () {
        Instance = this;
        SetSkybox(PlayerPrefs.GetInt("SkyboxIndex", 0));
	}

    public static void SetSkybox(int index = 0)
    {
        Debug.Assert(Instance.Skyboxes.Length >= index);
        if (index >= Instance.Skyboxes.Length)
            index = 0;
        RenderSettings.skybox = Instance.Skyboxes[index];
        PlayerPrefs.SetInt("SkyboxIndex", index);
        Debug.Assert(Instance.MainCamera != null);
        Instance.MainCamera.clearFlags = CameraClearFlags.Skybox;
        Instance.MainCamera.backgroundColor = Color.black;
        DynamicGI.UpdateEnvironment();

        LogController.Log("Set Background " + index);
    }

    public static void NextSkybox()
    {
        SetSkybox(PlayerPrefs.GetInt("SkyboxIndex", 0) + 1);
    }
}
