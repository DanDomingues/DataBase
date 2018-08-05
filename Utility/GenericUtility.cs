using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GenericUtility<T>
{
    public static int EqualCount(T[] collection, T compare)
    {
        int value = 0;
        foreach (var item in collection)
        {
            if (item.Equals(compare)) value++;
        }

        return value;
    }
}