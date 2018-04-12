using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ImageExtensions
{
    public static Coroutine LerpAlpha(this Image i, float duration, float targetAlpha)
    {
         return i.StartCoroutine(LerpAlpha_Routine(i, duration, targetAlpha));

    }

    private static IEnumerator LerpAlpha_Routine(Image input, float duration, float targetAlpha)
    {
        targetAlpha = Mathf.Clamp01(targetAlpha);

        if (input.color.a < targetAlpha)
        {
            while (input.color.a < targetAlpha)
            {
                input.color += new Color(0, 0, 0, 1) * Time.deltaTime / duration;
                yield return new WaitForEndOfFrame();
            }

        }

        else if (input.color.a > targetAlpha)
        {
            while (input.color.a > targetAlpha)
            {
                input.color -= new Color(0, 0, 0, 1) * Time.deltaTime / duration;
                yield return new WaitForEndOfFrame();
            }

        }

        input.SetAlpha(targetAlpha); 

    }

    public static void SetAlpha(this Image input, float alpha)
    {
        var color = input.color;
        color.a = alpha;
        input.color = color;
    }

}
