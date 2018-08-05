using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class EventExtensions
{
    //Unity Event
    public static void Subscribe(this UnityEvent e, UnityAction action)
    {
        if (e == null) e = new UnityEvent();
        e.AddListener(action);
    }

    public static void SafeInvoke(this UnityEvent e)
    {
        if (e == null)
        {
            Debug.Log("Event is empty, Invoke aborted");
            return;
        }

        e.Invoke();
    }

    //Transform Event
    public static void Subscribe(this TransformEvent e, UnityAction<Transform> action)
    {
        if (e == null) e = new TransformEvent();
        e.AddListener(action);
    }

    public static void SafeInvoke(this TransformEvent e, Transform target)
    {
        if (e == null)
        {
            Debug.Log("Event is empty, Invoke aborted");
            return;
        }

        e.Invoke(target);
    }

    //Int Event
    public static void Subscribe(this IntEvent e, UnityAction<int> action)
    {
        if (e == null) e = new IntEvent();
        e.AddListener(action);
    }

    public static void SafeInvoke(this IntEvent e, int target)
    {
        if (e == null)
        {
            Debug.Log("Event is empty, Invoke aborted");
            return;
        }

        e.Invoke(target);
    }

    //Bool Event
    public static void Subscribe(this BoolEvent e, UnityAction<bool> action)
    {
        if (e == null) e = new BoolEvent();
        e.AddListener(action);
    }

    public static void SafeInvoke(this BoolEvent e, bool target)
    {
        if (e == null)
        {
            Debug.Log("Event is empty, Invoke aborted");
            return;
        }

        e.Invoke(target);
    }


}

