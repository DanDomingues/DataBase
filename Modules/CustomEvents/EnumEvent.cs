using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class EnumEvent : UnityEvent<Enum>
{


}

public static class EnumEvent_Methods
{
    public static void Subscribe(this EnumEvent e, UnityAction<Enum> action)
    {
        if (e == null || action == null) return;

        e.AddListener(action);

    }

    public static void SafeInvoke(this EnumEvent e, Enum value)
    {
        if (e == null || value == null) return;

        e.Invoke(value);
    }

}
