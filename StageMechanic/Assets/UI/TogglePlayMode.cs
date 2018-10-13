/*  
 * Copyright (C) 2018 You're Perfect Studio. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using UnityEngine;
using UnityEngine.UI;

public class TogglePlayMode : MonoBehaviour {

    public Sprite PlayModeIcon;
    public Sprite EditModeIcon;
	public Text Text;

	private void Start()
	{
		Text = GetComponentInChildren<Text>();
	}

	public void OnPressed()
    {
        BlockManager.Instance.TogglePlayMode();
        if(BlockManager.PlayMode)
        {
            GetComponent<Button>().image.sprite = EditModeIcon;
			if (Text != null)
				Text.text = "Edit this Stage";
        }
        else
        {
            GetComponent<Button>().image.sprite = PlayModeIcon;
			if (Text != null)
				Text.text = "Play this Stage";
		}
	}

    private void Update()
    {
        if (BlockManager.PlayMode)
        {
            GetComponent<Button>().image.sprite = EditModeIcon;
			if (Text != null)
				Text.text = "Edit this Stage";
		}
		else
        {
            GetComponent<Button>().image.sprite = PlayModeIcon;
			if (Text != null)
				Text.text = "Play this Stage";
		}
	}
}
