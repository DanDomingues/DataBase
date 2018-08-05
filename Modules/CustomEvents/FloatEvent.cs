using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class FloatEvent : UnityEvent<Single>
{


}

public static class EventMethods_FloatEvent
{
    public static void Subscribe(this FloatEvent e, UnityAction<Single> action)
    {
        if (e == null || action == null) return;

        e.AddListener(action);

    }

    public static void SafeInvoke(this FloatEvent e, Single value)
    {
        if (e == null) return;

        e.Invoke(value);
    }

    }
