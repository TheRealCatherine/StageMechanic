/*  
 * Copyright (C) 2018 You're Perfect Studio. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System;
using System.Collections.Generic;
using System.Linq;
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

	public static bool IsValid(this Vector3 vector)
	{
		return !(float.IsNaN(vector.x) || float.IsNaN(vector.y) || float.IsNaN(vector.z));
	}

    public static void AddIfNotNull<T>(this IList<T> list, T value)
    {
        if ((object)value != null)
            list.Add(value);
    }

    private static Collider[] _colliderBuffer = new Collider[30];
    public static T GetGameObjectAt<T>(Vector3 position, float radius = 0.01f) where T : MonoBehaviour
    {
        int number = Physics.OverlapSphereNonAlloc(position, radius, _colliderBuffer);
        for(int i=0;i<number;++i)
        {
            T obj = _colliderBuffer[i].GetComponent<T>();
            if (obj != null && obj.transform.position == position)
                return obj;
        }
        return null;
    }

    public static T GetGameObjectNear<T>(Vector3 position, float radius = 0.01f) where T : MonoBehaviour
    {
        int number = Physics.OverlapSphereNonAlloc(position, radius, _colliderBuffer);
        for (int i = 0; i < number; ++i)
        {
            T obj = _colliderBuffer[i].GetComponent<T>();
            if (obj != null)
                return obj;
        }
        return null;
    }

    public static List<T> GetGameObjectsNear<T>(Vector3 position, float radius = 0.01f) where T : MonoBehaviour
    {
        List<T> ret = new List<T>();
        int number = Physics.OverlapSphereNonAlloc(position, radius, _colliderBuffer);
        for (int i = 0; i < number; ++i)
        {
            ret.AddIfNotNull(_colliderBuffer[i].GetComponent<T>());
        }
        return ret;
    }

    public static void SortChildrenByYCoord(GameObject o)
    {
        var children = new List<Transform>(o.GetComponentsInChildren<Transform>(true));
        children.Remove(o.transform);
        children.Sort(CompareYCoord);
        for (int i = 0; i < children.Count; i++)
            children[i].SetSiblingIndex(i);
    }

    private static int CompareYCoord(Transform lhs, Transform rhs)
    {
        if (lhs == rhs) return 0;
        var test = rhs.gameObject.activeInHierarchy.CompareTo(lhs.gameObject.activeInHierarchy);
        if (test != 0) return test;
        if (lhs.localPosition.y < rhs.localPosition.y) return -1;
        if (lhs.localPosition.y > rhs.localPosition.y) return 1;
        return 0;
    }

	public static void CopyToClipboard(this string s)
	{
		TextEditor te = new TextEditor();
		te.text = s;
		te.SelectAll();
		te.Copy();
	}

	private static System.Random random = new System.Random();
	public static string RandomString(int length)
	{
		const string chars = "ABCDEFGHIJKLMNPQRSTUVWXYZ123456789";
		return new string(Enumerable.Repeat(chars, length)
		  .Select(s => s[random.Next(s.Length)]).ToArray());
	}

#if ENABLE_UNSAFE
	static unsafe TDest ReinterpretCast<TSource, TDest>(TSource source)
	{
		var sourceRef = __makeref(source);
		var dest = default(TDest);
		var destRef = __makeref(dest);
		*(IntPtr*)&destRef = *(IntPtr*)&sourceRef;
		return __refvalue(destRef, TDest);
	}
#endif
}
