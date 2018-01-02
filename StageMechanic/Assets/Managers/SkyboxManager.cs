using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxManager : MonoBehaviour {

    private static SkyboxManager Instance;
    public Material[] Skyboxes;

	// Use this for initialization
	void Start () {
        Instance = this;
        SetSkybox(PlayerPrefs.GetInt("SkyboxIndex", 0));
	}

    public static void SetSkybox(int index = 0)
    {
        Debug.Assert(Instance.Skyboxes.Length > index);
        if (index >= Instance.Skyboxes.Length)
            index = 0;
        RenderSettings.skybox = Instance.Skyboxes[index];
        PlayerPrefs.SetInt("SkyboxIndex", index);
        DynamicGI.UpdateEnvironment();
        LogController.Log("Set Background " + index);
    }

    public static void NextSkybox()
    {
        SetSkybox(PlayerPrefs.GetInt("SkyboxIndex", 0) + 1);
    }
}
