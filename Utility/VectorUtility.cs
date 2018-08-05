using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorUtility
{
    public static float CheckAngle (Vector2 inputPosition)
    {
        float result = 0;

        Vector2 defaultPosition = Vector2.up.SafeNormalize();
        inputPosition = inputPosition.SafeNormalize();


        if (inputPosition.x >= defaultPosition.x)
            result = Vector2.Angle(defaultPosition, inputPosition);
        else
        {
            result = 180;
            result += Vector2.Angle(defaultPosition * -1, inputPosition);

        }

        return result;

    }

    /// <summary>
    /// Gets the quadrant that the current direction is contained in in a 360 circle.
    /// </summary>
    /// <param name="dir">Input Direction</param>
    /// <param name="divider">Number of quadrants the circle contains</param>
    /// <param name="startOffset">Offset to start for quadrant checking. 0.5f is half of a quadrant offset</param>
    /// <returns></returns>
    public static int GetQuadrant(this Vector3 dir, int divider, float startOffset)
    {
        int index = 0;
        int finalIndex = 0;
        startOffset = Mathf.Clamp01(startOffset);

        Vector3 finalDir = Vector3.zero;

        float dist = 360f;
        float increment = 360 / divider;

        for (float i = increment * startOffset; i < 360f; i += increment)
        {
            Vector3 compare = Quaternion.Euler(Vector3.up * i) * Vector3.forward;
            if (Vector3.Angle(dir, compare) < dist)
            {

                finalIndex = index;
                dist = Vector3.Angle(dir, compare);
                finalDir = compare;
            }

            index++;
        }

        return finalIndex;
    }

    public static Vector3 Parse(string value)
    {
        var stringValues = value.Split('|');
        var values = new float[3];

        for (int i = 0; i < 3; i++)
        {
            values[i] = stringValues.Length > i ? float.Parse(stringValues[i]) : 0f;
        }

        return new Vector3(values[0], values[1], values[2]);
    }


}
