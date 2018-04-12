using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PanelTransitionData
{
    public AnimationCurve openCurve;
    public AnimationCurve closeCurve;
    public bool useLocalSpeed;
    public float speedModifier;

    public PanelTransitionData()
    {
        speedModifier = 1.0f;
        useLocalSpeed = true;
        openCurve = NewCurve(0.0f, 1.0f);
        closeCurve = NewCurve(1.0f, 0.0f);
    }

    AnimationCurve NewCurve(float start, float finish)
    {
        var keyframes = new Keyframe[2];
        keyframes[0] = new Keyframe(0, start);
        keyframes[1] = new Keyframe(1, finish);

        AnimationCurve curve = new AnimationCurve(keyframes);
        return curve;
    }


}
