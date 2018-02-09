/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
 using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button SaveButton;
    public Toggle AutoPlay;
    public Toggle FogToggle;
    public Toggle VisiblePlatformToggle;
    public Toggle CameraEffectsToggle;
    public Toggle MotionDebugToggle;
    public ParticleSystem Fog;
    public TogglePlayMode TogglePlayModeButton;
    public Button ToggleTouchScreenButton;
    public Button CameraPerspectiveButton;
    public Button SettingsButton;
    public AudioClip StartupSound;
    public Camera MainCamera;
    public GameObject SettingsWindow;
	public GameObject CreditsDialog;

    private void OnEnable()
    {
        BlockManager.Cursor?.SetActive(false);
        AutoPlay.isOn = (PlayerPrefs.GetInt("AutoPlayOnLoad", 1) == 1);
        FogToggle.isOn = (PlayerPrefs.GetInt("Fog", 1) == 1);
        CameraEffectsToggle.isOn = (PlayerPrefs.GetInt("PostProcessing", 1) == 1);
        MotionDebugToggle.isOn = (PlayerPrefs.GetInt("MotionDebug", 0) == 1);
        MotionDebugToggle.gameObject.SetActive(CameraEffectsToggle.isOn);
        //VisiblePlatformToggle.isOn = (PlayerPrefs.GetInt("PlatformVisible", 1) == 1);
        SaveButton.gameObject.SetActive(true);
        TogglePlayModeButton.gameObject.SetActive(false);
        if (string.IsNullOrWhiteSpace(Serializer.LastAccessedFileName))
        {
            SaveButton.gameObject.SetActive(false);
        }
        else
        {
            SaveButton.gameObject.SetActive(true);
        }
        if (StartupSound != null)
            GetComponent<AudioSource>()?.PlayOneShot(StartupSound);
    }

    private void OnDisable()
    {
        if(!BlockManager.PlayMode)
        {
            BlockManager.Cursor?.SetActive(true);
        }
        TogglePlayModeButton.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (gameObject.activeInHierarchy && Input.GetKeyDown(KeyCode.Escape))
            OnEscPressed();
        //if(BlockManager.ActiveFloor != null)
        //    BlockManager.ActiveFloor.GetComponent<Renderer>().enabled = VisiblePlatformToggle.isOn;
    }

    void OnEscPressed()
    {
        gameObject.SetActive(false);
    }

    public void OnAutoPlayChecked(bool value)
    {
        PlayerPrefs.SetInt("AutoPlayOnLoad", value ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void OnPlatformVisibleChecked(bool value)
    {
        PlayerPrefs.SetInt("PlatformVisible", value ? 1 : 0);
        PlayerPrefs.Save();
        if(BlockManager.ActiveFloor != null)
            BlockManager.ActiveFloor.GetComponent<Renderer>().enabled = value;
    }

    public void OnCameraEffectsChecked(bool value)
    {
        PlayerPrefs.SetInt("PostProcessing", value ? 1 : 0);
        PlayerPrefs.Save();
        VisualEffectsManager.EnablePostProcessing(value);
        MotionDebugToggle.isOn = (PlayerPrefs.GetInt("MotionDebug", 0) == 1);
        VisualEffectsManager.EnableMotionDebug(PlayerPrefs.GetInt("MotionDebug", 0) == 1);
        MotionDebugToggle.gameObject.SetActive(value);
    }

    public void OnMotionDebugChecked(bool value)
    {
        PlayerPrefs.SetInt("MotionDebug", value ? 1 : 0);
        PlayerPrefs.Save();
        VisualEffectsManager.EnableMotionDebug(value);
    }

    public void OnLoadAndEditClicked()
    {
        gameObject.SetActive(false);
        if (BlockManager.PlayMode)
        {
            BlockManager.Instance.TogglePlayMode();
        }
        UIManager.LoadFromJson();
    }

    public void OnCreateClicked()
    {
        gameObject.SetActive(false);
        if (BlockManager.PlayMode)
            BlockManager.Instance.TogglePlayMode();
        BlockManager.Clear();
    }

    public void OnQuitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
				Application.Quit ();
#endif
    }

    public void OnPlayPauseButtonClicked()
    {
        MusicManager.TogglePause();
    }
    public void OnNextTrackButtonClicked()
    {
        MusicManager.PlayNextTrack();
    }
    public void OnPrevTrackButtonClicked()
    {
        MusicManager.PlayPreviousTrack();
    }
    public void OnVolumeUpButtonClicked()
    {
        MusicManager.VolumeUp();
    }
    public void OnVolumeDownButtonClicked()
    {
        MusicManager.VolumeDown();
    }
    public void OnCycleBackgroundButtonClicked()
    {
        SkyboxManager.NextSkybox();
    }

    public void OnFogValueChanged(bool value)
    {
        if (BlockManager.PlayMode || !value)
            Fog.gameObject.SetActive(value);
        PlayerPrefs.SetInt("Fog", value ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void OnSaveClicked()
    {
        Serializer.QuickSave();
        SaveButton.gameObject.SetActive(false);
    }

    public void OnSaveAsClicked()
    {
        UIManager.SaveToJson();
    }

    public void OnInputsClicked()
    {
        UIManager.ToggleButtonMappingDialog();
        if (!BlockManager.PlayMode)
            BlockManager.Instance.TogglePlayMode();
        gameObject.SetActive(false);
    }

    public void OnOnscreenControllsClicked()
    {
        UIManager.ToggleOnscreenControlls();
        if (UIManager.Instance.ShowOnscreenControlls)
            ToggleTouchScreenButton.image.color = Color.green;
        else
            ToggleTouchScreenButton.image.color = Color.white;
    }

    public void OnSettingsClicked()
    {
        SettingsWindow.gameObject.SetActive(!SettingsWindow.gameObject.activeInHierarchy);
        if (SettingsWindow.gameObject.activeInHierarchy)
            SettingsButton.image.color = Color.green;
        else
            SettingsButton.image.color = Color.white;
    }

    public void OnCameraPerspectiveClicked()
    {
        MainCamera.orthographic = !MainCamera.orthographic;
        if (MainCamera.orthographic)
            CameraPerspectiveButton.image.color = Color.green;
        else
            CameraPerspectiveButton.image.color = Color.white;
    }

	public void OnNetworkLoadClicked()
	{
		UIManager.ShowNetworkLoadDialog();
	}

	public void OnShowCreditsClicked()
	{
		CreditsDialog.SetActive(true);
	}

	public void OnDismissCreditsClicked()
	{
		CreditsDialog.SetActive(false);
	}
}
