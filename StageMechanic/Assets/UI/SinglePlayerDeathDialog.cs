using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SinglePlayerDeathDialog : MonoBehaviour {

    public Button RestartFromBeginningButton;
    public Button UndoLasMoveButton;
    public Button LoadAndEditButton;
    public Button ExitPlayModeButton;
    public Button QuitButton;
    public Toggle AutoPlay;

	void Start () {
        RestartFromBeginningButton.onClick.AddListener(OnRestartFromBeginningClicked);
        UndoLasMoveButton.onClick.AddListener(OnUndoLastMoveClicked);
        LoadAndEditButton.onClick.AddListener(OnLoadAndEditClicked);
        ExitPlayModeButton.onClick.AddListener(OnExitPlayModeClicked);
        QuitButton.onClick.AddListener(OnQuitClicked);
        AutoPlay.onValueChanged.AddListener(OnAutoPlayChecked);
    }
	
    public void Show(AudioClip deathRattle = null)
    {
        PlayerManager.HidePlayers();
        if (BlockManager.AvailableUndoCount > 0)
            UndoLasMoveButton.interactable = true;
        else
            UndoLasMoveButton.interactable = false;
        AutoPlay.isOn = (PlayerPrefs.GetInt("AutoPlayOnLoad", 0) == 1);
        gameObject.SetActive(true);
        if (deathRattle != null)
            GetComponent<AudioSource>()?.PlayOneShot(deathRattle);
    }

    void OnAutoPlayChecked(bool value)
    {
        PlayerPrefs.SetInt("AutoPlayOnLoad", value ? 1 : 0);
    }

    void OnRestartFromBeginningClicked()
    {
        BlockManager.ReloadStartState();
        PlayerManager.SpawnPlayers();
        gameObject.SetActive(false);
    }

    void OnUndoLastMoveClicked()
    {
        gameObject.SetActive(false);
        PlayerManager.SpawnPlayers();
        BlockManager.Undo();
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

    void OnExitPlayModeClicked()
    {
        gameObject.SetActive(false);
        BlockManager.Instance.TogglePlayMode();
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
