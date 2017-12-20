using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LogController
{

    static public string LastMessage;
    static public string LastMessageTime;

    static public void Log(string message)
    {
        Debug.Log(message);
        LastMessage = message;
        LastMessageTime = DateTime.Now.ToString("HH:mm:ss");
    }
}
