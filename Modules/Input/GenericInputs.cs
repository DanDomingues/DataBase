using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Wrapper class for generic movement axis inputs
/// </summary>
public static class GenericInputs
{
    public static Vector2 Movement
    {
        get
        {
            return new Vector2(LeftHorizontal, LeftVertical);
        }
    }
    public static Vector2 MovementRaw
    {
        get
        {
            return new Vector2(LeftHorizontalRaw, LeftVerticalRaw);
        }
    }
    public static Vector2Int MovementRawInt
    {
        get
        {
            var movement = MovementRaw;
            return new Vector2Int((int) movement.x, (int) movement.y);
        }
    }

    public static float LeftHorizontal
    {
        get
        {
            return Input.GetAxis("Horizontal");
        }
    }
    public static float LeftHorizontalRaw
    {
        get
        {
            return Input.GetAxisRaw("Horizontal");
        }
    }

    public static float LeftVertical
    {
        get
        {
            return Input.GetAxis("Vertical");
        }
    }
    public static float LeftVerticalRaw
    {
        get
        {
            return Input.GetAxisRaw("Vertical");
        }
    }

    public static int MouseScrollInt
    {
        get
        {
            var max = 1.1f;
            return (int) (Mathf.Clamp(Input.mouseScrollDelta.y, -max, max) * 1.1f / max);
        }
    }
}
