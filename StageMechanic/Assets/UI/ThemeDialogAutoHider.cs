using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThemeDialogAutoHider : MonoBehaviour
{
	public ScrollRect BlockThemes;
	public ScrollRect ItemThemes;
	private bool _restoreMainMenu;
	private bool _showSampleLevel;

    void OnEnable()
    {
		_showSampleLevel = BlockManager.BlockCount == 0;

		if (_showSampleLevel)
		{
			if (BlockManager.PlayMode)
				BlockManager.Instance.TogglePlayMode();

			string file = Application.streamingAssetsPath + "/InputConfig.json";
			Serializer.LoadFileUsingLocalPath(file);
		}
		_restoreMainMenu = UIManager.Instance.MainMenu.isActiveAndEnabled;
		UIManager.Instance.MainMenu.gameObject.SetActive(false);
		if (!BlockManager.PlayMode)
			BlockManager.Instance.TogglePlayMode();
		//Time.timeScale = 0;
	}

	private void OnDisable()
	{
		if(_restoreMainMenu)
			UIManager.Instance.MainMenu.gameObject.SetActive(true);
		//Time.timeScale = 1;
	}

	void Update()
    {
    }
}
