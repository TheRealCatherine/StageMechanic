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
		AudioEffectsManager.PlaySound(StartupSound);
		Edition.transform.DOShakePosition(StartupSound.length, 40f, 20).OnComplete(Hide);
	}

	public void Hide()
	{
		transform.DOScale(0, 0.5f).OnComplete(OnHideComplete);
		transform.DOMove(MainMenuLogo.transform.position, 0.5f);
	}

	public void OnHideComplete()
	{
		gameObject.SetActive(false);
		Destroy(gameObject);
	}
}
