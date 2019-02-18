using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] CanvasGroup group;

    [Header("Avatar")]
    [SerializeField] Animator avatarAnim;
    [SerializeField] Image avatarImage;

    [Header("Text")]
    [SerializeField] Text text;
    [SerializeField] float textFadeDuration;
    [SerializeField] string textValueBase;
    [SerializeField] string textAppend;
    [SerializeField] float textAddTime;
    [SerializeField] float textEndDelay;

    [Header("Fade")]
    [SerializeField] float fadeDuration;
    [SerializeField] float stayDuration;

    public IEnumerator Fade_Routine(IEnumerator midRoutine, System.Action midAction, bool isLoading, float delay)
    {
        avatarImage.enabled = isLoading;

        text.color = Color.clear;
        StartCoroutine(Text_Routine());        
        StartCoroutine(CurveRoutine(textFadeDuration, (t, value) => text.color = new Color(1, 1, 1, value)));

        yield return StartCoroutine(CurveRoutine(fadeDuration, 
        (t, value) => group.alpha = value));

        yield return StartCoroutine(midRoutine);
        yield return new WaitForSecondsRealtime(delay);

        yield return StartCoroutine(CurveRoutine(fadeDuration, 
        (t, value) => group.alpha = 1 - value));

        if(midAction != null) midAction();
    }
    public IEnumerator Fade_Routine(IEnumerator midRoutine, System.Action midAction, bool isLoading)
    {
        return Fade_Routine(midRoutine, midAction, isLoading, stayDuration);
    }
    public IEnumerator Fade_Routine(IEnumerator midRoutine, System.Action midAction)
    {
        return Fade_Routine(midRoutine, midAction, true, stayDuration);
    }

    IEnumerator CurveRoutine(float duration, System.Action<float, float> action)
    {
        var t = 0f;
        while(t < 1.0f)
        {
            t += Time.unscaledDeltaTime / duration;
            t = Mathf.Clamp01(t);
            if(action != null) action(t, t);
            
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator Text_Routine()
    {
        string value = textValueBase;
        for (int i = 0; i < textAppend.Length; i++)
        {
            yield return new WaitForSecondsRealtime(textAddTime);
            value += textAppend[i];
            text.text = value;
        }

        yield return new WaitForSecondsRealtime(textEndDelay);
        if(group.alpha > 0f) StartCoroutine(Text_Routine());
    }

}
