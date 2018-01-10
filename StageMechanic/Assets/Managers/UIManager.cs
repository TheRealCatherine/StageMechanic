﻿using GracesGames.SimpleFileBrowser.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    public BlockInfoBoxController BlockInfoBox;
    public SinglePlayerDeathDialog SinglePlayerDeathDialog;
    public ButtonMappingDialog ButtonMappingDialog;
    public MainMenu MainMenu;

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
