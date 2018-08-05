using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class CanvasGroupExtensions
{
    public static void ToggleCanvasGroup(this GameObject o)
    {
        o.GetComponent<CanvasGroup>().Toggle();
    }

    public static void SetActive(this CanvasGroup canvas, bool value)
    {
        canvas.interactable = value;
        canvas.blocksRaycasts = value;
        canvas.alpha = value ? 1 : 0;
    }

    public static void SetBlocksRaycasts(this CanvasGroup group, bool value)
    {
        group.blocksRaycasts = value;
    }

    public static void Toggle(this CanvasGroup canvas)
    {
        canvas.Toggle(!canvas.interactable);
    }


    public static void Toggle(this CanvasGroup canvas, bool value)
    {
        canvas.interactable = value;
        canvas.blocksRaycasts = value;
        canvas.alpha = value ? 1 : 0;
    }

    public static Coroutine  TransitionToggle(this CanvasGroup group, MonoBehaviour starter, bool nature, float duration)
    {
        group.SetActive(nature);

        return starter.StartCoroutine(TransitionToggleRoutine(starter, group, nature, !nature ? 1 : 0, duration));

    }

    public static IEnumerator TransitionToggleRoutine(MonoBehaviour starter, CanvasGroup c, bool nature, float target, float lerpTime)
    {
        yield return starter.StartCoroutine(LerpAlphaEffect(c, target, lerpTime));

        c.SetActive(!nature);
    }

    public static Coroutine LerpAlpha(this CanvasGroup c, MonoBehaviour starter, float target, float lerpTime)
    {
        return starter.StartCoroutine(LerpAlphaEffect(c, target, lerpTime));
    }

    static IEnumerator LerpAlphaEffect(CanvasGroup c, float target, float lerpTime)
    {
        float diference = target - c.alpha;

        while (Mathf.Abs(c.alpha - target) > 0.1f)
        {
            float value = c.alpha;
            c.alpha = value + (diference * Time.deltaTime / lerpTime);

            yield return new WaitForEndOfFrame();
        }

        c.alpha = target;
    }

}
