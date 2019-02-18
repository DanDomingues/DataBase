using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class CoroutineUtility
{
    public static Coroutine WaitUntil(MonoBehaviour mono, System.Func<bool> func, float delay, UnityAction action)
    {
        return mono.StartCoroutine(WaitUntil_Routine(func, delay, action));
    }
    public static Coroutine WaitUntil(MonoBehaviour mono, System.Func<bool> func, UnityAction action)
    {
        return mono.StartCoroutine(WaitUntil_Routine(func, 0f, action));
    }


    static IEnumerator WaitUntil_Routine(System.Func<bool> func, float delay, UnityAction action)
    {
        yield return new WaitUntil(func);
        yield return new WaitForSeconds(delay);

        action();
    }

    public static void StopCorout(MonoBehaviour mono, Coroutine[] corouts)
    {
        foreach(Coroutine corout in corouts) StopCorout(mono ,corout);
    }
    public static void StopCorout(MonoBehaviour mono, Coroutine corout)
    {
        if(corout != null) mono.StopCoroutine(corout);
    }
    public static void StopCorout(MonoBehaviour mono, List<Coroutine> corouts)
    {
        StopCorout(mono, corouts.ToArray());
    }

    public static IEnumerator CurveRoutine(float duration, AnimationCurve curve, System.Action startAction, System.Action<float, float> midAction, System.Action endAction)
    {
        startAction?.Invoke();

        var t = 0f;
        while(t < 1.0f)
        {
            t += Time.deltaTime / duration;
            t = Mathf.Clamp01(t);
            midAction?.Invoke(t, curve.Evaluate(t));

            yield return new WaitForEndOfFrame();
        }

        endAction?.Invoke();
    }
    public static IEnumerator CurveRoutine(float duration, AnimationCurve curve, System.Action startAction, System.Action<float, float> midAction)
    {
        return CurveRoutine(duration, curve, startAction, midAction, null);
    }
    public static IEnumerator CurveRoutine(float duration, AnimationCurve curve, System.Action<float, float> midAction, System.Action endAction)
    {
        return CurveRoutine(duration, curve, null, midAction, endAction);
    }
    public static IEnumerator CurveRoutine(float duration, AnimationCurve curve, System.Action<float, float> midAction)
    {
        return CurveRoutine(duration, curve, null, midAction, null);
    }
    public static IEnumerator CurveRoutine(float duration, System.Action startAction, System.Action<float, float> midAction, System.Action endAction)
    {
        var curve = new AnimationCurve();
        curve.AddKey(0f, 0f);
        curve.AddKey(1f, 1f);
        
        return CurveRoutine(duration, curve, null, midAction, null);
    }
    public static IEnumerator CurveRoutine(float duration, System.Action<float, float> midAction, System.Action endAction)
    {
        return CurveRoutine(duration, () => { }, midAction, endAction);
    }
    public static IEnumerator CurveRoutine(float duration, System.Action startAction, System.Action<float, float> midAction)
    {
        return CurveRoutine(duration, startAction, midAction, () => { });
    }
    public static IEnumerator CurveRoutine(float duration, System.Action<float, float> midAction)
    {
        return CurveRoutine(duration, () => { }, midAction, () => { });
    }
}

public static class CoroutineUtility_Extensions
{
    public static Coroutine WaitUntil(this MonoBehaviour mono, System.Func<bool> func, float delay, UnityAction action)
    {
        return CoroutineUtility.WaitUntil(mono, func, delay, action);
    }
    public static Coroutine WaitUntil(this MonoBehaviour mono, System.Func<bool> func, UnityAction action)
    {
        return CoroutineUtility.WaitUntil(mono, func, 0f, action);
    }
    
}