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
        if(Instance.Skyboxes[index].GetTag("ChromaKey",false,"Nothing") != "Nothing" || Instance.Skyboxes[index].name.Contains("Chroma"))
        {
            Debug.Assert(Instance.MainCamera != null);
            //cam.clearFlags = CameraClearFlags.Skybox;
            //if (Instance.Skyboxes[index].name.Contains("Blue"))
            //    cam.backgroundColor = new Color(0, 71, 187);
            //else
            Instance.MainCamera.backgroundColor = Color.green;
            Instance.MainCamera.clearFlags = CameraClearFlags.SolidColor;
        }
        else
        {
            Debug.Assert(Instance.MainCamera != null);
            Instance.MainCamera.clearFlags = CameraClearFlags.Skybox;
            Instance.MainCamera.backgroundColor = Color.black;
            DynamicGI.UpdateEnvironment();

        }
        LogController.Log("Set Background " + index);
    }

    public static void NextSkybox()
    {
        SetSkybox(PlayerPrefs.GetInt("SkyboxIndex", 0) + 1);
    }
}
