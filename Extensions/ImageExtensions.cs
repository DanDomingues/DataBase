using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ImageExtensions
{
    public static void LerpAlpha(this Image i, float duration, float targetAlpha)
    {
         i.StartCoroutine(LerpImageAlpha(i, duration, targetAlpha));

    }

    private static IEnumerator LerpImageAlpha(Image input, float duration, float to)
    {
        to = Mathf.Clamp01(to);

        if (input.color.a < to)
            while (input.color.a < to)
            {
                input.color += new Color(0, 0, 0, 1) * Time.deltaTime / duration;
                yield return new WaitForEndOfFrame();
            }

        else if (input.color.a > to)
            while (input.color.a > to)
            {
                input.color -= new Color(0, 0, 0, 1) * Time.deltaTime / duration;
                yield return new WaitForEndOfFrame();
            }

        else
            Debug.Log("Error in alpha lerping");


    }

}
