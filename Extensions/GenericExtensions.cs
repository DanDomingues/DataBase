using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class GenericExtensions
{
    public static Single Increment01 (this Single v, Single add)
    {
        v += add;
        return Mathf.Clamp(v, -1.0f, 1.0f);
    }


    public static Boolean Add(this Boolean b, Boolean newValue)
    {
        Boolean result = ((b && newValue) || (!b && newValue));
        return result;
    }

    public static List<int> GetRandomOrder(int listLength)
    {
        List<int> result = new List<int>();

        for (int i = 0; i < listLength; i++)
        {
            int n = UnityEngine.Random.Range(0, listLength);

            while (result.Contains(n))
            {
                n = UnityEngine.Random.Range(0, listLength);
            }

            result.Add(n);
        }

        return result;
    }


}
