using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TrailRendererExtensions
{

    /// <summary>
    /// Fades a trail renderer's width modifier to 0.
    /// </summary>
    /// <param name="trail"></param>
    /// <param name="starter">MonoBehaviour to host the Coroutine</param>
    /// <param name="duration">Duration of the fade</param>
    public static void FadeWidth(this TrailRenderer trail, MonoBehaviour starter, float duration)
    {
        starter.StartCoroutine ( FadeTrailOff_Routine(trail, duration) );
    }

    /// <summary>
    /// Fades a duration renderer's width modifier to 0.
    /// </summary>
    /// <param name="trail"></param>
    /// <param name="starter">MonoBehaviour to host the Coroutine</param>
    /// <param name="duration">Duration of the fade</param>
    public static void FadeDuration(this TrailRenderer trail, MonoBehaviour starter, float duration, float incrementFactor)
    {
         starter.StartCoroutine(FadeDuration_Routine (trail, duration, incrementFactor));
    }

    public static void SetColorAlpha (this TrailRenderer trail, float alpha)
    {
        trail.material.color = trail.material.color.SetAlpha(alpha);
    }

    private static IEnumerator FadeTrailOff_Routine(TrailRenderer trail, float duration)
    {
        float distance = trail.widthMultiplier;

        while (trail.widthMultiplier > 0)
        {
            trail.widthMultiplier -= distance * Time.deltaTime / duration;
            yield return new WaitForFixedUpdate();
        }

        trail.gameObject.SetActive(false);
        yield return new WaitForEndOfFrame();

         trail.widthMultiplier = distance;
    }

     private static IEnumerator FadeDuration_Routine(TrailRenderer trail, float duration, float incrementFactor)
    {
        float distance = trail.time;
        float speed = 1f;

        while (trail.time > 0)
        {
            trail.time -= distance * Time.deltaTime / duration * speed;
            yield return new WaitForFixedUpdate();

            speed *= incrementFactor;
        }

        trail.gameObject.SetActive(false);
        yield return new WaitForEndOfFrame();

        trail.time = distance;

    }

}
