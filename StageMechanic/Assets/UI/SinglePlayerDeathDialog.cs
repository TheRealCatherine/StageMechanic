using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SinglePlayerDeathDialog : MonoBehaviour {

    public Button RestartFromBeginningButton;

	void Start () {
        RestartFromBeginningButton.onClick.AddListener(OnRestartFromBeginningClicked);
    }
	
    public void Show(AudioClip deathRattle = null)
    {
        PlayerManager.HidePlayers();
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
}
