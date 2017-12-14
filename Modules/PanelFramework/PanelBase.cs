using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PanelBase : MonoBehaviour
{

    private CanvasGroup m_group;
    public CanvasGroup group
    {
        get
        {
            if (m_group == null)
                m_group = GetComponent<CanvasGroup>();

            return m_group;
        }
    }

    private RectTransform m_rect;
    public RectTransform rect
    {
        get
        {
            if (m_rect == null)
                m_rect = GetComponent<RectTransform>();

            return m_rect;
        }
    }

    public PanelProfile profile;

    [HideInInspector]
    public Vector3 refSize = Vector3.zero;
    [HideInInspector]
    public Vector2 refPosition = Vector2.zero;

    public bool overrideSound;
    protected Coroutine transitionCorout;

    // Use this for initialization
    protected virtual void Start ()
    {
        if (group == null) return;

        group.alpha = 0;
        refSize = transform.localScale;
        refPosition = rect.anchoredPosition;

    }

    #region TogglePanel Overloads
    protected void RawToggle(bool nature, UnityAction action)
    {
        if (transitionCorout != null) return;

        transform.localScale = refSize;
        rect.anchoredPosition = refPosition;

        AppearType type = group.blocksRaycasts ? profile.closeTransition : profile.openTransition;
        IEnumerator routine = null;
        switch (type)
        {
            case AppearType.Pop:
                routine = PopRoutine(nature, action);

                 break;

            case AppearType.Vertical:
                routine = VerticalRoutine(nature, action);
                break;

            case AppearType.Instant:
                group.Toggle(nature);
                action();
                break;

            default:
                break;
        }

        if(routine != null)
        {
            transitionCorout = StartCoroutine(routine);
        }

        if (!overrideSound)
        {
            //AudioClip sfx = nature ? SoundManager.instance.sfxBank.openMenu : SoundManager.instance.sfxBank.closeMenu;
            //SoundManager.instance.PlaySingle(sfx);

        }
    }

    protected void RawToggle(bool nature)
    {
        RawToggle(nature, null);
    }

    protected void RawToggle(UnityAction action)
    {
        RawToggle(!group.blocksRaycasts, action);
    }

    protected void RawToggle()
    {
         RawToggle(null);
    }
    #endregion

    public virtual void Toggle()
    {
        if (group.blocksRaycasts)
        {
            InvokeAlternatives.InvokeRealtime(this, 0.2f, () =>
            {
                PanelEvents.instance.OnPanelClose.SafeInvoke();
            });
        }

        RawToggle();
    }

    public IEnumerator PopRoutine(bool nature, UnityAction action )
    {

        float t = 0;
        int way = nature ? 1 : -1;

        AnimationCurve curve = nature ? profile.popUpCurve : profile.popOutCurve;

        if (way > 0)
        {
            group.Toggle(nature);

            action.Execute();

        }

        while (t < curve.GetLength())
        {
            transform.localScale = refSize * curve.Evaluate(t);
            yield return new WaitForEndOfFrame();

            t += Time.unscaledDeltaTime * profile.speedModifier;
        }

        transform.localScale = refSize * curve.Evaluate(1.0f);

        if (way < 0)
        {
            group.Toggle(nature);
            action.Execute();
        }

        transitionCorout = null;
    }

    public IEnumerator VerticalRoutine(bool nature, UnityAction action)
    {
        Vector2 plusSize = new Vector2(1920, 540 + (rect.sizeDelta.y/2));
        int way = nature ? 1 : -1;

        float target = nature ? refPosition.y : rect.anchoredPosition.y - plusSize.y;
        AnimationCurve curve = nature ? profile.verticalAppearCurve : profile.verticalHideCurve;
        
        if (way > 0)
        {
            rect.anchoredPosition -= Vector2.up * plusSize.y;
            group.Toggle(nature);
            action.Execute();
        }

        //rect.anchoredPosition = Vector2.zero;

        float t = 0;
        while (t < 1.0f)
        {
            rect.anchoredPosition = refPosition -  ((plusSize.y * Vector2.up) * (1 - curve.Evaluate(t)));
            //rect.anchoredPosition += Vector2.up * plusSize.y * Time.deltaTime * profile.verticalSpeed * way;
            yield return new WaitForEndOfFrame();

            t += Time.deltaTime * profile.verticalSpeed;
        }

        rect.anchoredPosition = refPosition - ((plusSize.y * Vector2.up) * (way > 0 ? 0 : 1));

        if (way < 0)
        {
            group.Toggle(nature)  ;
            action.Execute();
        }

        transitionCorout = null;

    }

}
