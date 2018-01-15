using CnControls;
using GracesGames.SimpleFileBrowser.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public BlockInfoBoxController BlockInfoBox;
    public SinglePlayerDeathDialog SinglePlayerDeathDialog;
    public ButtonMappingDialog ButtonMappingDialog;
    public MainMenu MainMenu;
    public ScrollRect BlockTypesList;

    public SimpleButton SetStartPosButton;
    public SimpleButton NextBlockTypButton;
    public SimpleButton PrevBlockTypeButton;
    public SimpleButton DeleteBlockButton;
    public Dpad FurtherCloserButtons;

    //TODO Singleton flame war
    public static UIManager Instance;
    public static bool IsSinglePlayerDeathDialogOpen {
        get {
            Debug.Assert(Instance != null);
            return Instance.SinglePlayerDeathDialog.gameObject.activeInHierarchy;
        }
    }

    public static bool IsAnyInputDialogOpen
    {
        get
        {
            return IsSinglePlayerDeathDialogOpen || FileBrowser.IsOpen || Instance.MainMenu.isActiveAndEnabled || Instance.ButtonMappingDialog.CurrentState == ButtonMappingDialog.State.WaitingForKey;
        }
    }

    private void Start()
    {
        Instance = this;
    }

    private void Update()
    {
        /* for Android
        SetStartPosButton.gameObject.SetActive(!BlockManager.PlayMode);
        NextBlockTypButton.gameObject.SetActive(!BlockManager.PlayMode);
        PrevBlockTypeButton.gameObject.SetActive(!BlockManager.PlayMode);
        DeleteBlockButton.gameObject.SetActive(!BlockManager.PlayMode);
        FurtherCloserButtons.gameObject.SetActive(!BlockManager.PlayMode);*/

        BlockTypesList.gameObject.SetActive(!BlockManager.PlayMode && !MainMenu.isActiveAndEnabled);
    }

    public static void ShowSinglePlayerDeathDialog(AudioClip deathRattle = null)
    {
        Instance.SinglePlayerDeathDialog.Show(deathRattle);
    }

    public static void RefreshButtonMappingDialog()
    {
        Instance.ButtonMappingDialog.Refresh();
    }

    public static void ShowMainMenu()
    {
        Instance.MainMenu.enabled = true;
        Instance.MainMenu.gameObject.SetActive(true);
    }
}
