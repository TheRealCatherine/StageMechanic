using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public Button LoadButton;
    public Button CreateButton;
    public Button QuitButton;
    public Toggle AutoPlay;

    public AudioClip StartupSound;

	void Start () {
        LoadButton.onClick.AddListener(OnLoadAndEditClicked);
        CreateButton.onClick.AddListener(OnCreateClicked);
        QuitButton.onClick.AddListener(OnQuitClicked);
        AutoPlay.onValueChanged.AddListener(OnAutoPlayChecked);
    }

    private void OnEnable()
    {
        PlayerManager.HidePlayers();
        AutoPlay.isOn = (PlayerPrefs.GetInt("AutoPlayOnLoad", 0) == 1);
        if (StartupSound != null)
            GetComponent<AudioSource>()?.PlayOneShot(StartupSound);
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
        if(BlockManager.PlayMode)
            BlockManager.Instance.TogglePlayMode();
        BlockManager.Instance.Clear();
    }

    void OnQuitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
				Application.Quit ();
#endif
    }
}
