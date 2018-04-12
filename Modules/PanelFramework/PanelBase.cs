using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasGroup))]
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

    public delegate void OnToggleEvent(Boolean value);
    public event OnToggleEvent OnPanelToggle;
    public event OnToggleEvent OnPanelToggleLate;

    public bool Active { get { return group.blocksRaycasts; } }
    public bool InTransition { get { return transitionCorout != null; } }

    private void Awake()
    {

    }

    // Use this for initialization
    protected virtual void Start ()
    {
        if (group == null) return;

        group.alpha = 0;
        refSize = transform.localScale;
        refPosition = rect.anchoredPosition;

    }

    private void Reset()
    {
        if(profile == null)
        {
            var newProfile = Resources.Load<PanelProfile>("DataObjects/PanelProfiles/DefaultPanelProfile");
            profile = newProfile;
        }
    }

    #region Raw Toggle Overloads

    /// <summary>
    /// Toggles the panel without triggering the 'OnPanelToggle' event.
    /// </summary>
    /// <param name="type">Transition Type</param>
    /// <param name="value">If the panel is being turned on or off</param>
    /// <param name="action">Action to be executed before or after the Toggle Transition</param>
    public void RawToggle(AppearType type, bool value, UnityAction action)
    {

        if (transitionCorout != null) StopCoroutine(transitionCorout);

        if(!overrideSound)
        {
            if (group.blocksRaycasts) SoundManager.instance.PlaySingle(profile.closeSound);
            else SoundManager.instance.PlaySingle(profile.openSound);
        }

        transform.localScale = refSize;
        rect.anchoredPosition = refPosition;

        IEnumerator routine = null;
        switch (type)
        {
            case AppearType.Pop:
                routine = PopRoutine(value, action);
                 break;

            case AppearType.BottomToTop:
                routine = MovementRoutine(value, new Vector2(0, 1.0f), action);
                break;

            case AppearType.TopToBottom:
                routine = MovementRoutine(value, new Vector2(0, -1.0f), action);
                break;

            case AppearType.RightToLeft:
                routine = MovementRoutine(value, new Vector2(-1.0f, 0.0f), action);
                break;

            case AppearType.LeftToRight:
                routine = MovementRoutine(value, new Vector2(1.0f, 0.0f), action);
                break;

            case AppearType.Instant:
                group.Toggle(value);
                action.Execute();
                return;

            default:
                return;

        }

        transitionCorout =  StartCoroutine(routine);

    }

    /// <summary>
    /// Toggles the panel without triggering the 'OnPanelToggle' event.
    /// </summary>
    /// <param name="value">If the panel is being turned on or off</param>
    /// <param name="action">Action to be executed before or after the Toggle Transition</param>
    public void RawToggle(bool value, UnityAction action)
    {
        AppearType type = group.blocksRaycasts ? profile.closeTransitionType : profile.openTransitionType;
        RawToggle(type, value, action);
    }

    /// <summary>
    /// Toggles the panel without triggering the 'OnPanelToggle' event.
    /// </summary>
    /// <param name="value">If the panel is being turned on or off</param>
    /// <param name="delay">Delay between action being called and it's execution</param>
    /// <param name="action">Action to be executed before or after the Toggle Transition</param>
    public void RawToggle(bool value, float delay, UnityAction action)
    {
        RawToggle(value, () =>
        {
            InvokeAlternatives.InvokeRealtime(this, delay, action);

        });

    }

    /// <summary>
    /// Toggles the panel without triggering the 'OnPanelToggle' event.
    /// </summary>
    /// <param name="value">If the panel is being turned on or off</param>
    public void RawToggle(bool value)
    {
        RawToggle(value, null);
    }

    /// <summary>
    /// Toggles the panel without triggering the 'OnPanelToggle' event.
    /// </summary>
    public void RawToggle()
    {
         RawToggle(!group.blocksRaycasts);
    }

    #endregion

    #region Toggle Overloads

    /// <summary>
    /// Toggles the panel
    /// </summary>
    public virtual void Toggle()
    {
        Toggle(!group.blocksRaycasts);
    }

    /// <summary>
    /// Toggles the panel
    /// </summary>
    /// <param name="value">If the panel is being turned on or off</param>
    public virtual void Toggle(bool value)
    {
        Toggle(value, null);
    }

    /// <summary>
    /// Toggles the panel
    /// </summary>
    /// <param name="action">Action to be executed before or after the Toggle Transition</param>
    public void Toggle(UnityAction action)
    {
        Toggle(!group.blocksRaycasts, action);
    }

    /// <summary>
    /// Toggles the panel
    /// </summary>
    /// <param name="value">If the panel is being turned on or off</param>
    /// <param name="action">Action to be executed before or after the Toggle Transition</param>
    public virtual void Toggle(bool value, UnityAction action)
    {
        action = GetEventInclusiveAction(value, action);

        RawToggle(value, action);
        LocalOnToggle(value);
    }

    /// <summary>
    /// Toggles the panel
    /// </summary>
    /// <param name="delay">Delay between action being called and it's execution</param>
    /// <param name="action">Action to be executed before or after the Toggle Transition</param>
    public virtual void Toggle(float delay, UnityAction action)
    {
        Toggle(!group.blocksRaycasts, delay, action);
    }

    /// <summary>
    /// Toggles the panel
    /// </summary>
    /// <param name="value">If the panel is being turned on or off</param>
    /// <param name="delay">Delay between action being called and it's execution</param>
    /// <param name="action">Action to be executed before or after the Toggle Transition</param>
    public virtual void Toggle(bool value, float delay, UnityAction action)
    {
        action = GetEventInclusiveAction(value, action);

        RawToggle(value, delay, action);
        LocalOnToggle(value);
    }
#endregion

    private UnityAction GetEventInclusiveAction(bool value, UnityAction inputAction)
    {
        return 
        () =>
        {
            inputAction.Execute();

            if (OnPanelToggleLate != null)
                OnPanelToggleLate(value);
        };

    }

    public void LocalOnToggle(bool value)
    {

        if (!value)
        {
            InvokeAlternatives.InvokeAfterFrame(this, () =>
            {
                PanelEvents.instance.OnPanelClose.SafeInvoke();
            });

        }

        if (OnPanelToggle != null) OnPanelToggle(value);
    }

    IEnumerator PopRoutine(bool value, UnityAction action )
    {

        float t = 0;
        int way = value ? 1 : -1;

        var transition = profile.GetTransition(value);
        var curve = value ? transition.openCurve : transition.closeCurve;

        if (way > 0)
        {
            group.Toggle(value);

            action.Execute();

        }

        while (t < curve.GetLength())
        {
            transform.localScale = refSize * curve.Evaluate(t);
            yield return new WaitForEndOfFrame();

            t += Time.unscaledDeltaTime * transition.speedModifier;
        }

        transform.localScale = refSize * curve.Evaluate(1.0f);

        if (way < 0)
        {
            group.Toggle(value);
            action.Execute();
        }

        transitionCorout = null;
    }

    IEnumerator MovementRoutine(bool value, Vector2 movement, UnityAction action)
    {
        Vector2 plusSize = new Vector2( ((1920/2) + (rect.sizeDelta.x/2)) * movement.x, 
                                        ((1080/2) + (rect.sizeDelta.y/2)) * movement.y);

        int way = value ? 1 : -1;
        plusSize *= way;

        var transition = profile.GetTransition(value);
        var curve = value ? transition.openCurve : transition.closeCurve;

        Vector2 start = rect.anchoredPosition;
        Vector2 target = value ? refPosition : refPosition - plusSize;

        if (way > 0)
        {
            rect.anchoredPosition -= plusSize;
            group.Toggle(value);
            action.Execute();
        }

        //rect.anchoredPosition = Vector2.zero;

        float t = 0;
        float factor = 0f;
        while (t < 1.0f)
        {
            factor = 1 - curve.Evaluate(t);
            rect.anchoredPosition = refPosition -  (plusSize * factor);
            //rect.anchoredPosition += Vector2.up * plusSize.y * Time.deltaTime * profile.verticalSpeed * way;
            yield return new WaitForEndOfFrame();

            t += Time.unscaledDeltaTime * transition.speedModifier;
        }

        rect.anchoredPosition = target;

        if (way < 0)
        {
            group.Toggle(value);
            action.Execute();
        }

        transitionCorout = null;

    }



}
