using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PixelFilledIcon : PixelFill
{
    [Header("Appear")]
    [SerializeField] AnimationCurve curve;
    [SerializeField] float duration;

    bool active;

    public virtual void SetActive(bool value)
    {
        if (active == value) return;
        active = value;

        StartCoroutine(SetActive_Routine(value));
    }

    IEnumerator SetActive_Routine(bool active)
    {
        var t = 0f;
        float v;
        var startFill = fillImage.fillAmount;
        var targetFill = active ? 1f : 0f;

        float value;
        while(t < 1.0f)
        {
            t += Time.deltaTime / duration;
            t = Mathf.Clamp01(t);
            v = curve.Evaluate(t);

            value = Mathf.Lerp(startFill, targetFill, v);
            SetFillAmount(value);

            yield return new WaitForEndOfFrame();
        }

        bgImage.fillAmount = targetFill;
    }
}
