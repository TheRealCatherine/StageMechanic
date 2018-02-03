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
