/*  
 * Copyright (C) 2018 You're Perfect Studio. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	public Button SaveButton;
	public Button SaveAsButton;
	public Toggle AutoPlay;
	public Toggle FogToggle;
	public Toggle DestructivePlayMode;
	public Toggle VisiblePlatformToggle;
	public Toggle CameraEffectsToggle;
	public Toggle MinimizePanning;
	public ParticleSystem Fog;
	public TogglePlayMode TogglePlayModeButton;
	public Button ToggleTouchScreenButton;
	public Button CameraPerspectiveButton;
	public Button SettingsButton;
	public Button PlayButton;
	public Button LoadURLButton;
	public Button ShareButton;
	public Button InputsButton;
	public AudioClip StartupSound;
	public Camera MainCamera;
	public GameObject SettingsWindow;
	public GameObject CreditsDialog;
	public Text FlavorTextBox;
	public Text CurrentTrackName;
	public Text LiteText;
	public Text Version;

	public Dropdown TutorialLevel;
	public Dropdown Cat5Level;
	public Dropdown PrincessLevel;
	public GameObject LevelSelectPanel;

	private void OnEnable()
	{
		BlockManager.Cursor?.SetActive(false);
		MinimizePanning.isOn = !UIManager.MinimizePanning;
		AutoPlay.isOn = (PlayerPrefs.GetInt("AutoPlayOnLoad", 1) == 1);
		FogToggle.isOn = (PlayerPrefs.GetInt("Fog", 1) == 1);
		DestructivePlayMode.isOn = (PlayerPrefs.GetInt("DestructivePlayMode", 0) == 1);
		CameraEffectsToggle.isOn = (PlayerPrefs.GetInt("PostProcessing", 1) == 1);
		//VisiblePlatformToggle.isOn = (PlayerPrefs.GetInt("PlatformVisible", 1) == 1);
		Version.text = "v" + Application.version;
		if (GameManager.IsLiteBuild)
		{
			SettingsButton.gameObject.SetActive(false);
			LiteText.gameObject.SetActive(true);
		}
		if (Application.platform == RuntimePlatform.Android)
			InputsButton.gameObject.SetActive(false);

		TogglePlayModeButton.gameObject.SetActive(false);
		if (string.IsNullOrWhiteSpace(Serializer.LastAccessedFileName))
		{
			SaveButton.gameObject.SetActive(false);
		}
		else
		{
			SaveButton.gameObject.SetActive(true);
		}
		if (BlockManager.BlockCount == 0)
		{
			PlayButton.gameObject.SetActive(false);
			SaveAsButton.gameObject.SetActive(false);
			ShareButton.gameObject.SetActive(false);
		}
		else
		{
			PlayButton.gameObject.SetActive(true);
			SaveAsButton.gameObject.SetActive(true);
			ShareButton.gameObject.SetActive(true);
		}

		Debug.Assert(FlavorText.Entries != null);
		Debug.Assert(FlavorText.Entries.Length > 1);
		FlavorTextBox.text = FlavorText.Entries[Random.Range(0, FlavorText.Entries.Length - 1)];

		if (StartupSound != null)
			GetComponent<AudioSource>()?.PlayOneShot(StartupSound);

		//if (Application.platform == RuntimePlatform.Android)
		//	LoadURLButton.gameObject.SetActive(false);

		UpdateTrackName();
	}

	private void OnDisable()
	{
		if(!BlockManager.PlayMode)
		{
			if (BlockManager.Cursor && BlockManager.Cursor.gameObject) //prevent crash while quiting
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

	public void OnMinimizePanningChecked(bool value)
	{
		UIManager.MinimizePanning = !value;
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
	}

	public void OnLoadAndEditClicked()
	{
		GameManager.PlayerScores[0] = 0;
		gameObject.SetActive(false);
		if (BlockManager.PlayMode)
		{
			BlockManager.Instance.TogglePlayMode();
		}
		UIManager.LoadFromJson();
	}

	public void OnCreateClicked()
	{
		GameManager.PlayerScores[0] = 0;
		if (BlockManager.BlockCount == 0) {
			gameObject.SetActive(false);
			if (BlockManager.PlayMode)
				BlockManager.Instance.TogglePlayMode();
			BlockManager.Clear();
		} else {
			gameObject.SetActive(false);
			UIManager.ShowCreateEditLevelDialog();
		}
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
		UpdateTrackName();
	}
	public void OnNextTrackButtonClicked()
	{
		MusicManager.PlayNextTrack();
		UpdateTrackName();
	}
	public void OnPrevTrackButtonClicked()
	{
		MusicManager.PlayPreviousTrack();
		UpdateTrackName();
	}
	public void OnVolumeUpButtonClicked()
	{
		MusicManager.VolumeUp();
		UpdateTrackName();
	}
	public void OnVolumeDownButtonClicked()
	{
		MusicManager.VolumeDown();
		UpdateTrackName();
	}
	public void OnCycleBackgroundButtonClicked()
	{
		SkyboxManager.NextSkybox();
		UpdateTrackName();
	}

	private void UpdateTrackName()
	{
		if (MusicManager.IsPaused())
			CurrentTrackName.text = "Paused";
		else if (string.IsNullOrWhiteSpace(MusicManager.TrackName()))
			CurrentTrackName.text = "Unknown Title";
		else
			CurrentTrackName.text = MusicManager.TrackName();
	}

	public void OnFogValueChanged(bool value)
	{
		if (BlockManager.PlayMode || !value)
			Fog.gameObject.SetActive(value);
		PlayerPrefs.SetInt("Fog", value ? 1 : 0);
		PlayerPrefs.Save();
	}

	public void OnDestructivePlaymodeChanged(bool value)
	{
		PlayerPrefs.SetInt("DestructivePlayMode", value ? 1 : 0);
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
		if (BlockManager.PlayMode)
			BlockManager.Instance.TogglePlayMode();

		string file;
		if (Application.platform == RuntimePlatform.Android)
			file = "/InputConfig.bin";
		else
			file = Application.streamingAssetsPath + "/InputConfig.json";
		Serializer.LoadFileUsingLocalPath(file);
		if (!BlockManager.PlayMode)
			BlockManager.Instance.TogglePlayMode();
		StartCoroutine(OnInputsClickedHelper());
	}

	private IEnumerator OnInputsClickedHelper()
	{
		yield return new WaitForSeconds(0.25f);
		UIManager.ToggleButtonMappingDialog();
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
		GameManager.PlayerScores[0] = 0;
	}

	public void OnShowCreditsClicked()
	{
		CreditsDialog.SetActive(true);
	}

	public void OnDismissCreditsClicked()
	{
		CreditsDialog.SetActive(false);
	}

	public void OnPlayClicked()
	{
		gameObject.SetActive(false);
		if (!BlockManager.PlayMode)
			BlockManager.Instance.TogglePlayMode();
		GameManager.PlayerScores[0] = 0;
	}

	public void OnShowLevelSelectClicked()
	{
		LevelSelectPanel.SetActive(true);
		GameManager.PlayerScores[0] = 0;
	}

	public void OnCloseLevelSelectClicked()
	{
		LevelSelectPanel.SetActive(false);
	}

	public void OnShareClicked()
	{
		Serializer.SaveToPastebin();
	}

	public void OnOpenTutorialClicked()
	{
		GameManager.PlayerScores[0] = 0;
		if (BlockManager.PlayMode)
			BlockManager.Instance.TogglePlayMode();
		string file;
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer)
			file = "/Tutorial/Tutorial" + TutorialLevel.options[TutorialLevel.value].text + ".bin";
		else
			file = Application.streamingAssetsPath + "/Tutorial/Tutorial" + TutorialLevel.options[TutorialLevel.value].text + ".json";
		LevelSelectPanel.SetActive(false);
		Serializer.LoadFileUsingLocalPath(file);
		gameObject.SetActive(false);
		if (!BlockManager.PlayMode)
			BlockManager.Instance.TogglePlayMode();
	}

	public void OnOpenCat5Clicked()
	{
		if (BlockManager.PlayMode)
			BlockManager.Instance.TogglePlayMode();
		string file;
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer)
			file = "/Cat5/" + Cat5Level.options[Cat5Level.value].text + ".bin";
		else
			file = Application.streamingAssetsPath + "/Cat5/" + Cat5Level.options[Cat5Level.value].text + ".json";
		LevelSelectPanel.SetActive(false);
		Serializer.LoadFileUsingLocalPath(file);
		gameObject.SetActive(false);
		if (!BlockManager.PlayMode)
			BlockManager.Instance.TogglePlayMode();
	}

	public void OnOpenPrincessClicked()
	{
		if (BlockManager.PlayMode)
			BlockManager.Instance.TogglePlayMode();
		string file;
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer)
			file = "/Princess/" + PrincessLevel.options[PrincessLevel.value].text + ".bin";
		else
			file = Application.streamingAssetsPath + "/Princess/" + PrincessLevel.options[PrincessLevel.value].text + ".json";
		LevelSelectPanel.SetActive(false);
		Serializer.LoadFileUsingLocalPath(file);
		gameObject.SetActive(false);
		if (!BlockManager.PlayMode)
			BlockManager.Instance.TogglePlayMode();
	}
}
