/*  
 * Copyright (C) 2018 You're Perfect Studio. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using CnControls;
using GracesGames.SimpleFileBrowser.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

	public BlockInfoBoxController BlockInfoBox;
	public SinglePlayerDeathDialog SinglePlayerDeathDialog;
	public CreateEditLevelDialog CreateEditLevelDialog;
	public ButtonMappingDialog ButtonMappingDialog;
	public NetworkLoadDialog NetworkLoadDialog;

	public Button TogglePlayModeButton;
	public Button MainmenuButton;

	public MainMenu MainMenu;
	public ScrollRect BlockTypesList;
	public ScrollRect ItemTypesList;
	public BlockEditDialog BlockEditDialog;
	public GameObject FileBrowserPrefab;

	public bool ShowOnscreenControlls;
	public SimpleButton GrabButton;
	public SimpleButton SetStartPosButton;
	public SimpleButton DeleteBlockButton;
	public Dpad DirectionButtons;

	public GameObject CursorControls;

	public GameObject BlockThemeDialog;

	public GameObject ScoreBox;
	public Text Player1Score;
	public Text Player2Score;
	public Text Player3Score;
	public Text Player4Score;

	public Toggle BinaryFormat;

	public GameObject UndoButton;
	public Button Player1ItemButton;

	//TODO Singleton flame war
	public static UIManager Instance;
	public static bool IsSinglePlayerDeathDialogOpen
	{
		get
		{
			Debug.Assert(Instance != null);
			return Instance.SinglePlayerDeathDialog.gameObject.activeInHierarchy;
		}
	}

	public static bool IsCreateEditLevelDialogOpen
	{
		get
		{
			Debug.Assert(Instance != null);
			return Instance.CreateEditLevelDialog.gameObject.activeInHierarchy;
		}
	}

	public static bool IsAnyInputDialogOpen
	{
		get
		{
			Debug.Assert(Instance != null);
			return IsBlockEditDialogOpen 
				|| IsSinglePlayerDeathDialogOpen
				|| IsCreateEditLevelDialogOpen
				|| Instance.BlockEditDialog.isActiveAndEnabled
				|| FileBrowser.IsOpen
				|| Instance.MainMenu.isActiveAndEnabled
				|| Instance.NetworkLoadDialog.isActiveAndEnabled
				|| Instance.ButtonMappingDialog.CurrentState == ButtonMappingDialog.State.WaitingForKey
				|| Instance.BlockThemeDialog.activeInHierarchy;
		}
	}

	public static bool IsBlockEditDialogOpen
	{
		get
		{
			Debug.Assert(Instance != null);
			return Instance.BlockEditDialog.isActiveAndEnabled;
		}
	}

	/// <summary>
	/// For supporting issue #128: https://gitlab.com/youreperfectstudio/StageMechanic/issues/128
	/// intended for use until we move to proper Cinemachine intelligent camera movement
	/// </summary>
	public static bool MinimizePanning
	{
		get
		{
			return (PlayerPrefs.GetInt("MinimizePanning", 1) == 1);
		}
		set
		{
			PlayerPrefs.SetInt("MinimizePanning", value ? 1 : 0);
			PlayerPrefs.Save();
		}
	}

	private void Start()
	{
		Instance = this;
		ShowOnscreenControlls = Input.touchSupported;
		BetterStreamingAssets.Initialize();
	}

	private void Update()
	{
		if (ShowOnscreenControlls && !IsAnyInputDialogOpen) {

			DirectionButtons.gameObject.SetActive(BlockManager.PlayMode);
			CursorControls.gameObject.SetActive(!BlockManager.PlayMode);
			GrabButton.gameObject.SetActive(BlockManager.PlayMode);
		}
		else
		{
			DirectionButtons.gameObject.SetActive(false);
			CursorControls.gameObject.SetActive(false);
			GrabButton.gameObject.SetActive(false);
		}
		if (!BlockManager.PlayMode && !IsAnyInputDialogOpen)
			ItemTypesList.gameObject.SetActive(BlockManager.GetBlockNear(BlockManager.Cursor.transform.position + Vector3.down) != null);
		else
			ItemTypesList.gameObject.SetActive(false);
		DeleteBlockButton.gameObject.SetActive(!MainMenu.isActiveAndEnabled && !BlockManager.PlayMode && BlockManager.ActiveBlock != null);
		BlockTypesList.gameObject.SetActive(!BlockManager.PlayMode && !IsAnyInputDialogOpen);
	
		UndoButton.SetActive(BlockManager.PlayMode && Serializer.AvailableUndoCount > 0 && (!SinglePlayerDeathDialog.gameObject.activeInHierarchy));
		Player1ItemButton.image.sprite = PlayerManager.Player(0)?.Item?.Icon;
		Player1ItemButton.gameObject.SetActive(BlockManager.PlayMode && PlayerManager.Player(0) != null && PlayerManager.Player(0).Item != null && (!SinglePlayerDeathDialog.gameObject.activeInHierarchy));

		//TODO quick fix for Prototype 2 to keep people from doing things with the death dialog open
		if (MainmenuButton != null)
			MainmenuButton.gameObject.SetActive(!IsAnyInputDialogOpen);
		if(TogglePlayModeButton != null)
			TogglePlayModeButton.gameObject.SetActive(!IsAnyInputDialogOpen);

		UpdateScoreBox();
	}

	//TODO clean this up, don't hardcode 4 players
	public void UpdateScoreBox()
	{
		if(!BlockManager.PlayMode)
		{
			ScoreBox.SetActive(false);
			return;
		}
		ScoreBox.SetActive(true);

		AbstractPlayerCharacter player1 = PlayerManager.Player(0) as AbstractPlayerCharacter;
		AbstractPlayerCharacter player2 = PlayerManager.Player(1) as AbstractPlayerCharacter;
		AbstractPlayerCharacter player3 = PlayerManager.Player(2) as AbstractPlayerCharacter;
		AbstractPlayerCharacter player4 = PlayerManager.Player(3) as AbstractPlayerCharacter;
		if (player1 == null || player1.Score == 0)
		{
			Player1Score.gameObject.SetActive(false);
		}
		else if(player1 != null)
		{
			Player1Score.gameObject.SetActive(true);
			Player1Score.text = player1.Score.ToString();
		}
		if (player2 == null || player2.Score == 0)
		{
			Player2Score.gameObject.SetActive(false);
		}
		else if (player2 != null)
		{
			Player2Score.gameObject.SetActive(true);
			Player2Score.text = player2.Score.ToString();
		}
		if (player3 == null || player3.Score == 0)
		{
			Player3Score.gameObject.SetActive(false);
		}
		else if (player3 != null)
		{
			Player3Score.gameObject.SetActive(true);
			Player3Score.text = player3.Score.ToString();
		}
		if (player4 == null || player4.Score == 0)
		{
			Player4Score.gameObject.SetActive(false);
		}
		else if (player4 != null)
		{
			Player4Score.gameObject.SetActive(true);
			Player4Score.text = player4.Score.ToString();
		}
		ScoreBox.SetActive(Player1Score.isActiveAndEnabled || Player2Score.isActiveAndEnabled || Player3Score.isActiveAndEnabled || Player4Score.isActiveAndEnabled);
	}

	public static void ShowCreateEditLevelDialog()
	{
		Debug.Assert(Instance != null);
		Instance.CreateEditLevelDialog.Show();
	}

	public static void ShowSinglePlayerDeathDialog(AudioClip deathRattle = null)
	{
		Debug.Assert(Instance != null);
		Instance.SinglePlayerDeathDialog.Show(deathRattle);
	}

	public static void RefreshButtonMappingDialog()
	{
		Debug.Assert(Instance != null);
		Instance.ButtonMappingDialog.Refresh();
	}

	public static void ShowMainMenu()
	{
		Debug.Assert(Instance != null);
		Instance.MainMenu.gameObject.SetActive(true);
	}

	public static void ToggleMainMenu()
	{
		Debug.Assert(Instance != null);
		if (Instance.MainMenu.gameObject.activeInHierarchy)
			Instance.MainMenu.gameObject.SetActive(false);
		else
			Instance.MainMenu.gameObject.SetActive(true);
	}

	public static void ToggleBlockInfoDialog()
	{
		Debug.Assert(Instance != null);
		Instance.BlockInfoBox.ToggleVisibility();
	}

	public static void ToggleButtonMappingDialog()
	{
		Debug.Assert(Instance != null);
		Instance.ButtonMappingDialog.gameObject.SetActive(!Instance.ButtonMappingDialog.gameObject.activeInHierarchy);
	}

	public static void ShowBlockEditDialog(IBlock block = null)
	{
		Instance.BlockInfoBox.gameObject.SetActive(false);
		Instance.BlockEditDialog.Show(block);
	}

	public static void ShowNetworkLoadDialog()
	{
		Instance.MainMenu.gameObject.SetActive(false);
		Instance.NetworkLoadDialog.gameObject.SetActive(true);
	}

	public static void ToggleOnscreenControlls()
	{
		Instance.ShowOnscreenControlls = !Instance.ShowOnscreenControlls;
	}

	public static void SaveToJson()
	{
		GameObject fileBrowserObject = Instantiate(Instance.FileBrowserPrefab, BlockManager.Instance.Stage.transform);
		fileBrowserObject.name = "FileBrowser";
		FileBrowser fileBrowserScript = fileBrowserObject.GetComponent<FileBrowser>();
		fileBrowserScript.SetupFileBrowser(ViewMode.Landscape, PlayerPrefs.GetString("LastSaveDir"));
		if(Serializer.UseBinaryFiles)
			fileBrowserScript.SaveFilePanel(Instance, "SaveFileUsingPath", "MyLevels", "bin");
		else
			fileBrowserScript.SaveFilePanel(Instance, "SaveFileUsingPath", "MyLevels", "json");
	}
	private void SaveFileUsingPath(string path) { Serializer.SaveFileUsingPath(path); }

	public static void LoadFromJson()
	{
		GameObject fileBrowserObject = Instantiate(Instance.FileBrowserPrefab, BlockManager.Instance.Stage.transform);

		fileBrowserObject.name = "FileBrowser";
		FileBrowser fileBrowserScript = fileBrowserObject.GetComponent<FileBrowser>();
		fileBrowserScript.SetupFileBrowser(ViewMode.Landscape, PlayerPrefs.GetString("LastLoadDir"));
		if(Serializer.UseBinaryFiles)
			fileBrowserScript.OpenFilePanel(Instance, "LoadFileUsingPath", "bin");
		else
			fileBrowserScript.OpenFilePanel(Instance, "LoadFileUsingPath", "json");
	}
	private void LoadFileUsingPath(string path) { Serializer.LoadFileUsingLocalPath(path); }

}
