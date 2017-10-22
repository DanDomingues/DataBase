using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public static class UnityActionExtensions
{ 
    /// <summary>
    /// Runs Action only if it is not null
    /// </summary>
    /// <param name="a"></param>
    public static void Execute(this UnityAction a)
    {
        if (a != null) a();
    }

    public static void Toggle(this GameObject o)
    {
        o.SetActive(!o.activeSelf);
    }

    public static string ConcatArray(this System.String[] array)
    {
        string result = "";

        for (int i = 0; i < array.Length; i++)
        {
            result += '\n' + array[i];
        }

        return result;
    }

    public static Transform GetLastChild (this Transform t)
    {
        return t.GetChild(t.childCount - 1);
    }

}
