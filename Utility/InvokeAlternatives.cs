using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class InvokeAlternatives
{
    
    private static Coroutine[] InvokeRoutine;
    private static Coroutine[] InvokeRepeatingRoutine;

    public enum InvokeType { Invoke, InvokeRepeating, All}

    /// <summary>
    /// Waits a delay, runs an UnityAction and repeats the same action every further delay
    /// </summary>
    /// <param name="starterScript">Script to start the Coroutine</param>
    /// <param name="startDelay">Delay before the first execution</param>
    /// <param name="repeatDelay">Delay between every </param>
    /// <param name="action">UnityAction to be executed</param>
    /// <returns>Returns in Coroutine format</returns>
    public static Coroutine InvokeRepeating(MonoBehaviour starterScript, float startDelay, float repeatDelay, UnityAction action)
    {
        Coroutine routine = starterScript.StartCoroutine(InvokeRepeatingCorout(startDelay, repeatDelay, action));
        InvokeRepeatingRoutine.ExtendArray(routine);
        return routine;
    }

    private static IEnumerator InvokeRepeatingCorout(float startDelay, float repeatDelay, UnityAction action)
    {
        yield return new WaitForSeconds(startDelay);

        if(action != null) action();
        int i = 0;
        while(i < 1)
        {
            yield return new WaitForSeconds(repeatDelay);
            if (action != null) action();
        }

    }

    /// <summary>
    /// Waits a 'delay' and runs an UnityAction after it
    /// </summary>
    /// <param name="starterScript">MonoScript that will start the sequence</param>
    /// <param name="delay">Delay to be waited</param>
    /// <param name="action">Action to be executed after the delay</param>
    public static Coroutine InvokeRealtime(MonoBehaviour starterScript, float delay, UnityAction action)
    {
        Coroutine routine = starterScript.StartCoroutine(InvokeCoroutRealtime(delay, action));
        InvokeRoutine.ExtendArray(routine);

        return routine;
    }

    private static IEnumerator InvokeCoroutRealtime(float delay, UnityAction action)
    {
        yield return new WaitForSecondsRealtime(delay);
        action();
    }

    /// <summary>
    /// Waits a 'delay' and runs an UnityAction after it
    /// </summary>
    /// <param name="starterScript">MonoScript that will start the sequence</param>
    /// <param name="delay">Delay to be waited</param>
    /// <param name="action">Action to be executed after the delay</param>
    public static Coroutine Invoke(MonoBehaviour starterScript, float delay, UnityAction action)
    {
        Coroutine routine = starterScript.StartCoroutine(InvokeCorout(delay, action));
        InvokeRoutine.ExtendArray(routine);

        return routine;
    }

    private static IEnumerator InvokeCorout( float delay, UnityAction action)
    {
        yield return new WaitForSeconds(delay);
        action();
    }

    public static Coroutine InvokeAfterFrame(MonoBehaviour starterScript, UnityAction action)
    {
        return starterScript.StartCoroutine(InvokeAfterFrame_Routine(action));
    }

    private static IEnumerator InvokeAfterFrame_Routine(UnityAction action)
    {
        yield return new WaitForEndOfFrame();
        action.Execute();
    }

    /// <summary>
    /// Stops all Invoke routines of a determined type
    /// </summary>
    /// <param name="stopperScript"></param>
    /// <param name="type"></param>
    public static void StopInvoke(MonoBehaviour stopperScript, InvokeType type)
    {
        switch (type)
        {
            case InvokeType.Invoke:
                InvokeRoutine.StopAll(stopperScript);
                break;

            case InvokeType.InvokeRepeating:
                InvokeRepeatingRoutine.StopAll(stopperScript);
                break;

            case InvokeType.All:
                StopInvoke(stopperScript, InvokeType.Invoke);
                StopInvoke(stopperScript, InvokeType.InvokeRepeating);
                break;

            default:
                break;
        }

    }
}
