/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System;
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

    public static bool AlmostEquals(this double double1, double double2, double precision)
    {
        return (Math.Abs(double1 - double2) <= precision);
    }

    public static Vector3 Round(Vector3 vector, int places)
    {
        return new Vector3((float)Math.Round(vector.x, places), (float)Math.Round(vector.y, places), (float)Math.Round(vector.z, places));
    }

    public static void AddIfNotNull<T>(this IList<T> list, T value)
    {
        if ((object)value != null)
            list.Add(value);
    }

    public static T GetGameObjectAt<T>(Vector3 position, float radius = 0.01f) where T : MonoBehaviour
    {
        foreach (Collider collider in Physics.OverlapSphere(position, radius))
        {
            T obj = collider.GetComponent<T>();
            if (obj != null && obj.transform.position == position)
                return obj;
        }
        return null;
    }

    public static T GetGameObjectNear<T>(Vector3 position, float radius = 0.01f) where T : MonoBehaviour
    {

        foreach (Collider collider in Physics.OverlapSphere(position, radius))
        {
            T obj = collider.GetComponent<T>();
            if (obj != null)
                return obj;
        }
        return null;
    }

    public static List<T> GetGameObjectsNear<T>(Vector3 position, float radius = 0.01f) where T : MonoBehaviour
    {
        List<T> ret = new List<T>();
        foreach (Collider collider in Physics.OverlapSphere(position, radius))
        {
            ret.AddIfNotNull(collider.GetComponent<T>());
        }
        return ret;
    }
}
