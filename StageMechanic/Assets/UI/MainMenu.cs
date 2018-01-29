using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public Button LoadButton;
    public Button CreateButton;
    public Button QuitButton;
    public Toggle AutoPlay;
    public Button SaveButton;
    public Button SaveAsButton;

    public Button PlayPauseButton;
    public Button NextTrackButton;
    public Button PrevTrackButton;
    public Button VolumeUpButton;
    public Button VolumeDownButton;
    public Button CycleBackgroundButton;
    public Toggle FogToggle;
    public ParticleSystem Fog;

    public AudioClip StartupSound;

    void Start()
    {
        LoadButton.onClick.AddListener(OnLoadAndEditClicked);
        CreateButton.onClick.AddListener(OnCreateClicked);
        QuitButton.onClick.AddListener(OnQuitClicked);
        AutoPlay.onValueChanged.AddListener(OnAutoPlayChecked);

        SaveButton.onClick.AddListener(onSaveClicked);
        SaveAsButton.onClick.AddListener(onSaveAsClicked);

        PlayPauseButton.onClick.AddListener(onPlayPauseButtonClicked);
        NextTrackButton.onClick.AddListener(onNextTrackButtonClicked);
        PrevTrackButton.onClick.AddListener(onPrevTrackButtonClicked);
        VolumeUpButton.onClick.AddListener(onVolumeUpButtonClicked);
        VolumeDownButton.onClick.AddListener(onVolumeDownButtonClicked);
        CycleBackgroundButton.onClick.AddListener(onCycleBackgroundButtonClicked);

        FogToggle.onValueChanged.AddListener(onFogValueChanged);
    }

    private void OnEnable()
    {
        BlockManager.Cursor?.SetActive(false);
        AutoPlay.isOn = (PlayerPrefs.GetInt("AutoPlayOnLoad", 0) == 1);
        FogToggle.isOn = (PlayerPrefs.GetInt("Fog", 0) == 1);
        SaveButton.gameObject.SetActive(true);
        if (string.IsNullOrWhiteSpace(BlockManager.Instance.LastAccessedFileName))
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

    void OnAutoPlayChecked(bool value)
    {
        PlayerPrefs.SetInt("AutoPlayOnLoad", value ? 1 : 0);
    }

    void OnLoadAndEditClicked()
    {
        gameObject.SetActive(false);
        if (BlockManager.PlayMode)
        {
            BlockManager.Instance.TogglePlayMode();
        }
        BlockManager.Instance.LoadFromJson();
    }

    void OnCreateClicked()
    {
        gameObject.SetActive(false);
        if (BlockManager.PlayMode)
            BlockManager.Instance.TogglePlayMode();
        BlockManager.Clear();
    }

    void OnQuitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
				Application.Quit ();
#endif
    }

    public void onPlayPauseButtonClicked()
    {
        MusicManager.TogglePause();
    }
    public void onNextTrackButtonClicked()
    {
        MusicManager.PlayNextTrack();
    }
    public void onPrevTrackButtonClicked()
    {
        MusicManager.PlayPreviousTrack();
    }
    public void onVolumeUpButtonClicked()
    {
        MusicManager.VolumeUp();
    }
    public void onVolumeDownButtonClicked()
    {
        MusicManager.VolumeDown();
    }
    public void onCycleBackgroundButtonClicked()
    {
        SkyboxManager.NextSkybox();
    }

    public void onFogValueChanged(bool value)
    {
        if (BlockManager.PlayMode || !value)
            Fog.gameObject.SetActive(value);
        PlayerPrefs.SetInt("Fog", value ? 1 : 0);
    }

    public void onSaveClicked()
    {
        BlockManager.Instance.QuickSave();
        SaveButton.gameObject.SetActive(false);
    }

    public void onSaveAsClicked()
    {
        BlockManager.Instance.SaveToJson();
    }
}
