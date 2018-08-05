using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StringUtility
{
    public static string SpaceAtCapitals(object input)
    {
        if (input == null) throw new System.Exception("Inputed object is null");

        var value = input.ToString();
        return System.Text.RegularExpressions.Regex.Replace(value, "([a-z])([A-Z])", "$1 $2");
    }

}
