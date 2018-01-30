/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BlockInfoBoxController : MonoBehaviour {

    public Text filename;
    public Text blockPosition;
    public Text blockName;
    public Text blockType;
    public Text itemType;

    public Text blockCount;
    public Text player1State;
    public Text fpsCount;
    public Text logTime;
    public Text logMessage;

    Cathy1Block lastBlock = null;

    //FPS couter stuff
    public float updateInterval = 1.5F;
    private float accum = 0;
    private int frames = 0;
    private float timeleft;

    public void ToggleVisibility()
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
    }

	// Use this for initialization
	void Start () {
        timeleft = updateInterval;
    }

    // Update is called once per frame
    void Update()
    {
        filename.text = Path.GetFileName(Serializer.LastAccessedFileName);
        // TODO make this event based instead of updating every frame
        lastBlock = BlockManager.Instance.ActiveBlock;
        if (lastBlock != null)
        {
            blockPosition.text = lastBlock.Position.ToString();
            blockName.text = lastBlock.Name;
            blockType.text = lastBlock.TypeName;
            //TODO support item types once there is an Item class
            if(lastBlock.FirstItem != null)
                itemType.text = lastBlock.FirstItem.name;
#if UNITY_EDITOR
            Selection.activeGameObject = lastBlock.gameObject;
#endif
        }
        else
        {
            blockPosition.text = BlockManager.Cursor.transform.localPosition.ToString();
            blockName.text = String.Empty;
            blockType.text = String.Empty;
            itemType.text = String.Empty;
        }

        blockCount.text = BlockManager.BlockCount.ToString();
        logTime.text = LogController.LastMessageTime;
        logMessage.text = LogController.LastMessage;

        player1State.text = PlayerManager.PlayerStateName();


        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;
        if (timeleft <= 0.0)
        {
            float fps = accum / frames;
            fpsCount.text = System.String.Format("{0:F2}", fps);
            timeleft = updateInterval;
            accum = 0.0F;
            frames = 0;
        }
    }
}
