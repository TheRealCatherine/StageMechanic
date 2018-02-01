using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button SaveButton;
    public Toggle AutoPlay;
    public Toggle FogToggle;
    public ParticleSystem Fog;
    public TogglePlayMode TogglePlayModeButton;
    public AudioClip StartupSound;

    private void OnEnable()
    {
        BlockManager.Cursor?.SetActive(false);
        AutoPlay.isOn = (PlayerPrefs.GetInt("AutoPlayOnLoad", 0) == 1);
        FogToggle.isOn = (PlayerPrefs.GetInt("Fog", 0) == 1);
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
    }

    void OnEscPressed()
    {
        gameObject.SetActive(false);
    }

    public void OnAutoPlayChecked(bool value)
    {
        PlayerPrefs.SetInt("AutoPlayOnLoad", value ? 1 : 0);
    }

    public void OnLoadAndEditClicked()
    {
        gameObject.SetActive(false);
        if (BlockManager.PlayMode)
        {
            BlockManager.Instance.TogglePlayMode();
        }
        BlockManager.LoadFromJson();
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
    }

    public void OnSaveClicked()
    {
        Serializer.QuickSave();
        SaveButton.gameObject.SetActive(false);
    }

    public void OnSaveAsClicked()
    {
        BlockManager.SaveToJson();
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
    }
}
