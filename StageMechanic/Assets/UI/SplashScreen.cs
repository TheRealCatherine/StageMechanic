using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour
{
	public GameObject Edition;
	public GameObject MainMenuLogo;
	public AudioClip StartupSound;

	private void Start()
	{
		Edition.transform.DOShakePosition(StartupSound.length, 40f, 20).OnComplete(Hide).OnStart(PlaySound);
	}

	public void PlaySound()
	{
		AudioEffectsManager.PlaySound(StartupSound);
	}
	public void Hide()
	{
		transform.DOScale(0, 0.5f).OnComplete(OnHideComplete);
		transform.DOMove(MainMenuLogo.transform.position, 0.5f);
	}

	public void OnHideComplete()
	{
		//to be used for a trophy later
		int launchCount = PlayerPrefs.GetInt("LaunchCount", 0);
		if (launchCount == 0)
		{
			UIManager.Instance.MainMenu.FirstLaunchDialog.SetActive(true);
		}
		PlayerPrefs.SetInt("LaunchCount", launchCount + 1);
		PlayerPrefs.Save();

		gameObject.SetActive(false);
		Destroy(gameObject);
	}
}
