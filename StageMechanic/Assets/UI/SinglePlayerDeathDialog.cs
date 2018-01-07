using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SinglePlayerDeathDialog : MonoBehaviour {

    public Button RestartFromBeginningButton;
    public Button UndoLasMoveButton;
    public Button ExitPlayModeButton;
    public Button QuitButton;

	void Start () {
        RestartFromBeginningButton.onClick.AddListener(OnRestartFromBeginningClicked);
        UndoLasMoveButton.onClick.AddListener(OnUndoLastMoveClicked);
        ExitPlayModeButton.onClick.AddListener(OnExitPlayModeClicked);
        QuitButton.onClick.AddListener(OnQuitClicked);
    }
	
    public void Show(AudioClip deathRattle = null)
    {
        PlayerManager.HidePlayers();
        if (BlockManager.AvailableUndoCount > 0)
            UndoLasMoveButton.interactable = true;
        else
            UndoLasMoveButton.interactable = false;
        gameObject.SetActive(true);
        if (deathRattle != null)
            GetComponent<AudioSource>()?.PlayOneShot(deathRattle);
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
