/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using UnityEngine;
using UnityEngine.UI;

public class TogglePlayMode : MonoBehaviour {

    public Sprite PlayModeIcon;
    public Sprite EditModeIcon;

    public void OnPressed()
    {
        BlockManager.Instance.TogglePlayMode();
        if(BlockManager.PlayMode)
        {
            GetComponent<Button>().image.sprite = EditModeIcon;
        }
        else
        {
            GetComponent<Button>().image.sprite = PlayModeIcon;
        }
    }

    private void Update()
    {
        if (BlockManager.PlayMode)
        {
            GetComponent<Button>().image.sprite = EditModeIcon;
        }
        else
        {
            GetComponent<Button>().image.sprite = PlayModeIcon;
        }

    }
}
