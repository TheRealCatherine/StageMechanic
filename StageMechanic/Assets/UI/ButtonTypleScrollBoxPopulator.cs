/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
 using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonTypleScrollBoxPopulator : MonoBehaviour {

    public Button ButtonPrefab;

	// Use this for initialization
	void Start () {
        AbstractBlockFactory factory = BlockManager.Instance.BlockFactories[0];
        string[] types = factory.BlockTypeNames;
        int count = 0;
        foreach(string name in types)
        {
            Sprite icon = factory.IconForType(name);
            if (icon != null)
            {
                Button newButton = Instantiate(ButtonPrefab, transform) as Button;
                newButton.GetComponentInChildren<Text>().text = name;
                newButton.image.sprite = icon;
                newButton.onClick.AddListener(OnBlockClicked);
                ++count;
            }
        }
	}

    void OnBlockClicked()
    {
        Button clickedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        BlockManager.CreateBlockAtCursor("Cathy1 Internal", clickedButton.GetComponentInChildren<Text>().text);
    }
}
