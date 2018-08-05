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

}
