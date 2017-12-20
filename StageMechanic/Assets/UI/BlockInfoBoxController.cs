/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BlockInfoBoxController : MonoBehaviour {

    public BlockManager blockManager;

    public Text blockPosition;
    public Text blockRotation;
    public Text blockName;
    public Text blockType;
    public Text itemType;

    public Text blockCount;
    public Text logTime;
    public Text logMessage;

    Cathy1Block lastBlock = null;

    public void ToggleVisibility()
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
    }

	// Use this for initialization
	void Start () {
    }

    // Update is called once per frame
    void Update()
    {
        // TODO make this event based instead of updating every frame
        lastBlock = blockManager.ActiveBlock;
        if (lastBlock != null)
        {
            blockPosition.text = lastBlock.Position.ToString();
            blockRotation.text = lastBlock.Rotation.ToString();
            blockName.text = lastBlock.Name;
            blockType.text = lastBlock.TypeName;
            //TODO support item types once there is an Item class
            if(lastBlock.FirstItem != null)
                itemType.text = lastBlock.FirstItem.name;
           
        }
        else
        {
            blockPosition.text = blockManager.Cursor.transform.localPosition.ToString();
            blockRotation.text = blockManager.Cursor.transform.localRotation.ToString();
            blockName.text = String.Empty;
            blockType.text = String.Empty;
            itemType.text = String.Empty;
        }

        blockCount.text = blockManager.BlockCount().ToString();
        logTime.text = LogController.LastMessageTime;
        logMessage.text = LogController.LastMessage;
    }
}
