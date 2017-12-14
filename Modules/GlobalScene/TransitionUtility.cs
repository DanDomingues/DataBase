using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public static class TransitionUtility
{
    public const string filterName = "TransitionFilter";
    public static Coroutine corout;

    public static Image TransitionImage
    {
        get { return GameObject.Find(filterName).GetComponent<Image>(); }
    }

    private static Image GetFilter()
    {
        if(corout == null) return GameObject.Find(filterName).GetComponent<Image>();

        Debug.Log("Transition busy when method was called.");
        return null;
    }

    /// <summary>
    /// Fade transition with Coroutine parameter that runs between fades
    /// </summary>
    /// <param name="imageName">Transition filter name</param>
    /// <param name="duration">Total transition duration</param>
    /// <param name="action">Coroutine to be run between fades</param>
    /// <returns></returns>
    public static Coroutine Transition(float duration, IEnumerator action)
    {
        Image img = GetFilter();

        if(img != null)
            return img.StartCoroutine(TransitionEffect(img, duration, action));

        return null;
    }

    /// <summary>
    /// Fade transition with UnityAction parameter that runs between fades
    /// </summary>
    /// <param name="imageName">Transition filter name</param>
    /// <param name="duration">Total transition duration</param>
    /// <param name="action">Action to be run between fades</param>
    /// <returns></returns>
    public static Coroutine Transition(float duration, UnityAction action)
    {
        Image img = GetFilter();

        if(img != null)
            return img.StartCoroutine(TransitionEffect(img, duration, action));

        return null;
    }

    public static Coroutine Transition(float duration, IEnumerator routine, UnityAction action)
    {
        Image img = GetFilter();

        if (img != null)
            return img.StartCoroutine(TransitionEffect(img, duration, routine,  action));

        return null;
    }


    //Coroutine effect for Transition function with Coroutine support
    private static IEnumerator  TransitionEffect (Image image, float duration, IEnumerator action)
    {
        while (image.color.a < 1)
        {
            image.color += new Color(0, 0, 0, 1) * Time.unscaledDeltaTime / (duration / 2);
            yield return new WaitForEndOfFrame();
        }

        yield return image.StartCoroutine(action);
        yield return new WaitForEndOfFrame();

        while (image.color.a > 0)
        {
            image.color -= new Color(0, 0, 0, 1) * Time.unscaledDeltaTime / (duration / 2);
            yield return new WaitForEndOfFrame();
        }

        Time.timeScale = 1;

    }

    //Coroutine effect for Transition function with UnityAction support
    private static IEnumerator TransitionEffect(Image image, float duration, UnityAction action)
    {
        while (image.color.a < 1)
        {
            image.color += new Color(0, 0, 0, 1) * Time.unscaledDeltaTime / (duration / 2);
            yield return new WaitForEndOfFrame();
        }

        action();
        yield return new WaitForEndOfFrame();

        while (image.color.a > 0)
        {
            image.color -= new Color(0, 0, 0, 1) * Time.unscaledDeltaTime / (duration / 2);
            yield return new WaitForEndOfFrame();
        }

        Time.timeScale = 1;

    }

    private static IEnumerator TransitionEffect(Image image, float duration, IEnumerator routine, UnityAction action)
    {
        while (image.color.a < 1)
        {
            image.color += new Color(0, 0, 0, 1) * Time.unscaledDeltaTime / (duration / 2);
            yield return new WaitForEndOfFrame();
        }

        yield return image.StartCoroutine(routine);

        action();
        yield return new WaitForEndOfFrame();

        while (image.color.a > 0)
        {
            image.color -= new Color(0, 0, 0, 1) * Time.unscaledDeltaTime / (duration / 2);
            yield return new WaitForEndOfFrame();
        }

        Time.timeScale = 1;

    }

}
