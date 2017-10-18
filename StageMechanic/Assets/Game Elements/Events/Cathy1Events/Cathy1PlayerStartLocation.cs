/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cathy1PlayerStartLocation : Cathy1AbstractEvent, IPlayerEventExtension {
    public int PlayerNumber;

    int IPlayerEventExtension.PlayerNumber
    {
        get
        {
            return PlayerNumber;
        }

        set
        {
            PlayerNumber = value;
        }
    }

    public override void Awake()
    {
        base.Awake();
        Type = EventType.PlayerStart;
    }
}
