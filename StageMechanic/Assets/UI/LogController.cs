/*  
 * Copyright (C) 2018 You're Perfect Studio. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System;
using UnityEngine;

public static class LogController
{

    static public string LastMessage;
    static public string LastMessageTime;

    static public void Log(string message)
    {
        if (LastMessage != message)
        {
            Debug.Log(message);
            LastMessage = message;
        }
        LastMessageTime = DateTime.Now.ToString("HH:mm:ss");
    }
}
