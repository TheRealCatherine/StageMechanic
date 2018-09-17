using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alert
{
    public void Info(string message, float seconds = 1.5f)
	{
		UIManager.ShowMessage(message, seconds);
	}
}