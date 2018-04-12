using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class MonoExtensions
{

    public static Coroutine InvokeRepeating(this MonoBehaviour starterScript, float startDelay, float repeatDelay, UnityAction action)
    {
        return InvokeAlternatives.InvokeRepeating(starterScript, startDelay, repeatDelay, action);
    }

    public static Coroutine InvokeAfterFrame(this MonoBehaviour starterScript, UnityAction action)
    {
        return InvokeAlternatives.InvokeAfterFrame(starterScript, action);
    }
}
