using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class BoolEvent : UnityEvent<Boolean>
{


}

public static class EventMethods_BoolEvent
{
    public static void Subscribe(this BoolEvent e, UnityAction<Boolean> action)
    {
        if (e == null || action == null) return;

        e.AddListener(action);

    }

    public static void SafeInvoke(this BoolEvent e, Boolean value)
    {
        if (e == null) return;

        e.Invoke(value);
    }

    }
