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
    public Text trapType;
    public Text trapTime;
    public Text isBomb;
    public Text bombTime;
    public Text bombRadius;
    public Text isCollapse;
    public Text collapseStep;
    public Text collapseGrab;
    public Text teleportType;
    public Text teleportTo;

    internal string[] typeList;
    Cathy1Block lastBlock = null;

    public void ToggleVisibility()
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
    }

	// Use this for initialization
	void Start () {
        //TODO get from description annotations
        typeList = Enum.GetNames(typeof(Cathy1Block.BlockType));
    }

    // Update is called once per frame
    void Update()
    {
        // TODO make this event based instead
        lastBlock = blockManager.ActiveBlock;
        if (lastBlock != null)
        {
            blockPosition.text = lastBlock.transform.localPosition.ToString();
            blockName.text = lastBlock.Name;
            blockType.text = typeList.ElementAt((int)lastBlock.Type);
            if (lastBlock.IsTrap)
            {
                trapType.text = lastBlock.TrapType.ToString();
                trapTime.text = "<?>";
            }
            else
            {
                trapType.text = "None";
                trapTime.text = String.Empty;
            }
            if (lastBlock.IsBomb)
            {
                isBomb.text = "Yes";
                bombTime.text = lastBlock.BombTimeMS.ToString();
                bombRadius.text = lastBlock.BombRadius.ToString();
            }
            else
            {
                isBomb.text = "No";
                bombTime.text = String.Empty;
                bombRadius.text = String.Empty;
            }
            if(lastBlock.IsCollapseOnStep || lastBlock.IsCollapseOnGrab)
            {
                isCollapse.text = "Yes";
                collapseStep.text = lastBlock.CollapseAfterNSteps.ToString();
                collapseGrab.text = lastBlock.CollapseAfterNGrabs.ToString();
            }
            else
            {
                isCollapse.text = "No";
                collapseStep.text = String.Empty;
                collapseGrab.text = String.Empty;
            }
            if(lastBlock.IsTeleport)
            {
                teleportType.text = lastBlock.TeleportType.ToString();
                teleportTo.text = lastBlock.TeleportDistance.ToString();
            }
            else
            {
                teleportType.text = "None";
                teleportTo.text = String.Empty;
            }
        }
        else
        {
            blockPosition.text = blockManager.Cursor.transform.localPosition.ToString();
            blockRotation.text = blockManager.Cursor.transform.localRotation.ToString();
            blockName.text = String.Empty;
            blockType.text = String.Empty;
            trapType.text = String.Empty;
            trapTime.text = String.Empty;
            isBomb.text = String.Empty;
            bombTime.text = String.Empty;
            bombRadius.text = String.Empty;
            isCollapse.text = String.Empty;
            collapseStep.text = String.Empty;
            collapseGrab.text = String.Empty;
            teleportType.text = String.Empty;
            teleportTo.text = String.Empty;
        }
    }
}
