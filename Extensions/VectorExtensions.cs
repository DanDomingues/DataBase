using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorExtensions
{
	
	//Dependencies: GenericExtensions
	
	public static Vector2 DivideVectors(this Vector2 one, Vector2 two)
    {
        return new Vector2(one.x / two.x, one.y / two.y);
    }

    public static Vector2 MultiplyVectors(this Vector2 one, Vector2 two)
    {
        return new Vector2(one.x * two.x, one.y * two.y);
    }
	
	public static Vector2 Increment01(this Vector2 v, Vector2 add)
    {
        return new Vector2(v.x.Increment01(add.x), v.y.Increment01(add.y));
    }

    public static Vector2 AddX(this Vector2 v, float add)
    {
        return new Vector2(v.x + add, v.y);
    }

    public static Vector3 SetX (this Vector3 v, float value)
    {
        return new Vector3(value, v.y, v.z);
    }
		
	public static Vector3 MultiplyVectors(this Vector3 v, Vector3 multiplier)
    {
        return new Vector3(v.x * multiplier.x, v.y * multiplier.y, v.z * multiplier.z);
    }

	public static Vector3 DivideVectors(this Vector3 one, Vector3 two)
    {
        return new Vector3(one.x / two.x, one.y / two.y, one.z / two.z);
    }
    public static Vector3 Increment01(this Vector3 v, Vector3 add)
    {
        return new Vector3(v.x.Increment01(add.x), v.y.Increment01(add.y), v.z.Increment01(add.z));
    }

    public static Vector3 GetXY(this Vector3 v)
    {
        return new Vector3(v.x, v.y, 0);
    }

    public static float GetMagnitude(this Vector2 v)
    {
        return Mathf.Abs(v.x) + Mathf.Abs(v.y);
    }

    public static Vector2 SafeNormalize(this Vector2 v)
    {
        return new Vector2(Mathf.Clamp(v.x, -1, 1), Mathf.Clamp(v.y, -1, 1));
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


    /// <summary>
    /// Gets the quadrant that the current direction is contained in in a 360 circle.
    /// </summary>
    /// <param name="dir">Input Direction</param>
    /// <param name="divider">Number of quadrants the circle contains</param>
    /// <param name="startOffset">Offset to start for quadrant checking. 0.5f is half of a quadrant offset</param>
    /// <returns></returns>
    public static int GetQuadrant(this Vector2 dir, int divider, float startOffset)
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

    public static string ToStringRaw(this Vector3 vector)
    {
        return string.Format("{0}|{1}|{2}", vector.x.ToString("0.0"), vector.y.ToString("0.0"), vector.z.ToString("0.0"));
    }

    public static string ToStringRaw(this Vector2 vector)
    {
        return ((Vector3)vector).ToStringRaw();
    }

}
