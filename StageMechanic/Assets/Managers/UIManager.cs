using CnControls;
using GracesGames.SimpleFileBrowser.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public BlockInfoBoxController BlockInfoBox;
    public SinglePlayerDeathDialog SinglePlayerDeathDialog;
    public ButtonMappingDialog ButtonMappingDialog;
    public MainMenu MainMenu;
    public ScrollRect BlockTypesList;
    public BlockEditDialog BlockEditDialog;

    public SimpleButton SetStartPosButton;
    public SimpleButton NextBlockTypButton;
    public SimpleButton PrevBlockTypeButton;
    public SimpleButton DeleteBlockButton;
    public Dpad FurtherCloserButtons;

    public GameObject UndoPanel;
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
    }

    private void Update()
    {
        // for Android
#if UNITY_ANDROID
        SetStartPosButton.gameObject.SetActive(!BlockManager.PlayMode);
        NextBlockTypButton.gameObject.SetActive(!BlockManager.PlayMode);
        PrevBlockTypeButton.gameObject.SetActive(!BlockManager.PlayMode);
        DeleteBlockButton.gameObject.SetActive(!BlockManager.PlayMode);
        FurtherCloserButtons.gameObject.SetActive(!BlockManager.PlayMode);
#else
        BlockTypesList.gameObject.SetActive(!BlockManager.PlayMode && !MainMenu.isActiveAndEnabled);
#endif

        UndoPanel.SetActive(BlockManager.PlayMode && BlockManager.AvailableUndoCount > 0);
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
}
