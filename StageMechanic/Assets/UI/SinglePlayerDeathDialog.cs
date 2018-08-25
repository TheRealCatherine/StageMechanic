/*  
 * Copyright (C) 2018 You're Perfect Studio. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
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
        PlayerManager.DestroyAllPlayers();
        if (Serializer.AvailableUndoCount > 0)
            UndoLasMoveButton.interactable = true;
        else
            UndoLasMoveButton.interactable = false;
        AutoPlay.isOn = (PlayerPrefs.GetInt("AutoPlayOnLoad", 0) == 1);
        gameObject.SetActive(true);
		if (deathRattle != null)
			AudioEffectsManager.PlaySound(deathRattle);
    }

    void OnAutoPlayChecked(bool value)
    {
        PlayerPrefs.SetInt("AutoPlayOnLoad", value ? 1 : 0);
    }

    void OnRestartFromBeginningClicked()
    {
        Serializer.ReloadStartState();
		//TODO(ItemManager)
		//PlayerManager.InstantiateAllPlayers();
        gameObject.SetActive(false);
    }

    void OnUndoLastMoveClicked()
    {
        gameObject.SetActive(false);
		//TODO(ItemManager)
		//PlayerManager.InstantiateAllPlayers();
        Serializer.Undo();
    }

    void OnLoadAndEditClicked()
    {
        gameObject.SetActive(false);
        if (BlockManager.PlayMode)
        {
            BlockManager.Instance.TogglePlayMode();
        }
        UIManager.LoadFromJson();
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
