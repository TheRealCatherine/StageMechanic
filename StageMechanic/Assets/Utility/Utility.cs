using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{

    static string _lastLogMessage;

    /// <summary>
    /// Take a string in the form of (0,0,0) or 0,0,0 and converts
    /// it into a Vector 3
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    public static Vector3 StringToVector3(string vector)
    {
        //TODO error checking
        // Remove the parentheses
        if (vector.StartsWith("(") && vector.EndsWith(")"))
        {
            vector = vector.Substring(1, vector.Length - 2);
        }

        // split the items
        string[] array = vector.Split(',');

        // store as a Vector3
        Vector3 result = new Vector3(
            float.Parse(array[0]),
            float.Parse(array[1]),
            float.Parse(array[2]));

        return result;
    }

    public static void DebugLogOnce( string message )
    {
        if(_lastLogMessage != message)
        {
            _lastLogMessage = message;
            Debug.Log(message);
        }
    }
}
