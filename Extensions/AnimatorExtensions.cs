using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class AnimatorExtensions
{

    /// <summary>
    /// Sets an Animator Boolean parameter after the next fixed update
    /// </summary>
    /// <param name="anim"></param>
    /// <param name="starter">MonoScript to start the function</param>
    /// <param name="key">Parameter's name</param>
    /// <param name="value">Value to be inputted</param>
    public static void SetBoolDelayed(this Animator anim,  MonoBehaviour starter, string key, bool value)
    {
        starter.StartCoroutine(DelayedActionRoutine( () => { anim.SetBool(key, value); }));
    }

    /// <summary>
    /// Sets an Animator Trigger parameter after the next fixed update
    /// </summary>
    /// <param name="anim"></param>
    /// <param name="starter">MonoScript to start the function</param>
    /// <param name="key">Parameter's name</param>
    public static void SetTriggerDelayed(this Animator anim, MonoBehaviour starter, string key )
    {
        starter.StartCoroutine(DelayedActionRoutine(() => { anim.SetTrigger(key); }));
    }

    private static IEnumerator  DelayedActionRoutine(UnityAction action)
    {
        yield return new WaitForFixedUpdate();
        if(action != null) action();
    }

    /// <summary>
    /// Lerps an Animator Float through time
    /// </summary>
    /// <param name="anim"></param>
    /// <param name="starter">Monobehaviour to host the action</param>
    /// <param name="floatName">Name of the float Parameter</param>
    /// <param name="targetValue">Value to be acheived</param>
    /// <param name="lerpTime">Desired time for the value transition</param>
    public static void LerpFloat(this Animator anim, MonoBehaviour starter, string floatName, float targetValue, float lerpTime)
    {
        starter.StartCoroutine(LerpFloatEffect(anim, floatName, targetValue, lerpTime));
    }

    private static IEnumerator LerpFloatEffect(Animator anim, string floatName, float targetValue, float lerpTime)
    {
        float diference = targetValue - anim.GetFloat(floatName);

        while (Mathf.Abs(anim.GetFloat(floatName) - targetValue) > 0.1f)
        {
            float value = anim.GetFloat(floatName);
            anim.SetFloat(floatName, value + (diference * Time.deltaTime / lerpTime));

            yield return new WaitForFixedUpdate();
        }
    }



}
