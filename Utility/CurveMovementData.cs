using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CurveMovementData
{

    public GameObject particle;
    public float[] values;
    public AnimationCurve curve;
    public AnimationCurve[] curves;

    [Range(0.0f, 5.0f)]
    public float[] durations;

    public Vector2 movementScale;

}
