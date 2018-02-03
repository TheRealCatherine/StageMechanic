/*  
 * Copyright (C) Catherine. All rights reserved.  
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
    public ButtonMappingDialog ButtonMappingDialog;
    public MainMenu MainMenu;
    public ScrollRect BlockTypesList;
    public ScrollRect PlayerStartPositionsList;
    public BlockEditDialog BlockEditDialog;
    public GameObject FileBrowserPrefab;

    public bool ShowOnscreenControlls;
    public SimpleButton GrabButton;
    public SimpleButton SetStartPosButton;
    public SimpleButton DeleteBlockButton;
    public Dpad FurtherCloserButtons;
    public Dpad DirectionButtons;

    public GameObject UndoButton;
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

    public static bool IsAnyInputDialogOpen
    {
        get
        {
            Debug.Assert(Instance != null);
            return IsBlockEditDialogOpen || IsSinglePlayerDeathDialogOpen || Instance.BlockEditDialog.isActiveAndEnabled || FileBrowser.IsOpen || Instance.MainMenu.isActiveAndEnabled || Instance.ButtonMappingDialog.CurrentState == ButtonMappingDialog.State.WaitingForKey;
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

    private void Start()
    {
        Instance = this;
        ShowOnscreenControlls = Input.touchSupported;
    }

    private void Update()
    {
        if (ShowOnscreenControlls && !MainMenu.gameObject.activeInHierarchy) {
            DirectionButtons.gameObject.SetActive(true);
            FurtherCloserButtons.gameObject.SetActive(!BlockManager.PlayMode);
            GrabButton.gameObject.SetActive(BlockManager.PlayMode);
        }
        else
        {
            DirectionButtons.gameObject.SetActive(false);
            FurtherCloserButtons.gameObject.SetActive(false);
            GrabButton.gameObject.SetActive(false);
        }
        DeleteBlockButton.gameObject.SetActive(!MainMenu.isActiveAndEnabled && !BlockManager.PlayMode && BlockManager.ActiveBlock != null);
        PlayerStartPositionsList.gameObject.SetActive(!BlockManager.PlayMode && !MainMenu.isActiveAndEnabled && BlockManager.ActiveBlock != null);
        BlockTypesList.gameObject.SetActive(!BlockManager.PlayMode && !MainMenu.isActiveAndEnabled);
        UndoButton.SetActive(BlockManager.PlayMode && Serializer.AvailableUndoCount > 0);
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
        fileBrowserScript.SaveFilePanel(Instance, "SaveFileUsingPath", "MyLevels", "json");
    }
    private void SaveFileUsingPath(string path) { Serializer.SaveFileUsingPath(path); }

    public static void LoadFromJson()
    {
        GameObject fileBrowserObject = Instantiate(Instance.FileBrowserPrefab, BlockManager.Instance.Stage.transform);

        fileBrowserObject.name = "FileBrowser";
        FileBrowser fileBrowserScript = fileBrowserObject.GetComponent<FileBrowser>();
        fileBrowserScript.SetupFileBrowser(ViewMode.Landscape, PlayerPrefs.GetString("LastLoadDir"));

        fileBrowserScript.OpenFilePanel(Instance, "LoadFileUsingPath", "json");
    }
    private void LoadFileUsingPath(string path) { Serializer.LoadFileUsingLocalPath(path); }

}
