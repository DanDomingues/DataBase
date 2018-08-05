using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEngine.UI;

public static class BelugaExtensions
{ 
    public static Coroutine[] ExtendArray(this Coroutine[] array, Coroutine newElement)
    {
        if (array == null) array = new Coroutine[0];

        List<Coroutine> list = new List<Coroutine>(array);
        list.Add(newElement);

        return list.ToArray();
    }

    public static void StopAll(this Coroutine[] array, MonoBehaviour stopperScript)
    {
        for (int i = 0; i < array.Length; i++)
        {
            stopperScript.StopCoroutine(array[i]);
        }
    }

    public static Color SetAlpha(this Color c, float alpha)
    {
        c = new Color(c.r, c.g, c.b, alpha);
        return c;
    }

    /// <summary>
    /// Returns the curve's base duration in seconds.
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public static float GetLength(this AnimationCurve c)
    {
        return (c.keys[c.length - 1].time);
    }

    /// <summary>
    /// Waits for a desired height and calls an Action when that height is acheived
    /// </summary>
    /// <param name="rb"></param>
    /// <param name="starter"></param>
    /// <param name="target"></param>
    /// <param name="action"></param>
    public static void WaitForHeight(this Transform rb, MonoBehaviour starter, float target, UnityAction action)
    {
        starter.StartCoroutine(WaitForHeightEffect(rb, target, action));
        
    }

    private static IEnumerator  WaitForHeightEffect(Transform t, float target, UnityAction action)
    {
        int way = t.position.y < target ? 1 : -1;

        yield return new WaitWhile(() => { return (t.position.y * way < target * way); });

        action();
    }

    //Move to Transform Extensions when such is created
    /// <summary>
    /// Returns the aumount of children contained by the child objects if there are any.
    /// </summary>
    /// <param name="parent"></param>
    /// <returns></returns>
    public static int CountGrandChildren(this Transform parent)
    {
        int counter = 0;

        for (int i = 0; i < parent.childCount; i++)
            counter += parent.GetChild(i).childCount;

        return counter;
    }


}