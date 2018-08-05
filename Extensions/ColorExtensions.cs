using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorUtility
{
    public static Color SetColor(Color rgb, float a)
    {
        var color = rgb;
        color.a = a;
        return color;
    }

    public static Color SetColor(Color rgb)
    {
        return SetColor(rgb, 1.0f);
    }

    public static Color SetRgb(Color source, Color rgb)
    {
        return SetColor(rgb, source.a);
    }

}
