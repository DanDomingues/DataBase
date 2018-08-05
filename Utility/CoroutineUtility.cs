﻿using System;
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

        action.Execute();
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