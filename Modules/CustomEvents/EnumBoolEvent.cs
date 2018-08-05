using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class EnumBoolEvent : UnityEvent<Enum, Boolean>
{
    

}

public static class EnumBool_Methods
{
    public static void Subscribe(this EnumBoolEvent e, UnityAction<Enum, Boolean> action)
    {
        if (e == null || action == null) return;

        e.AddListener(action);

    }

    public static void SafeInvoke(this EnumBoolEvent e, Enum enumValue, Boolean boolValue)
    {
        if (e == null || enumValue == null) return;

        e.Invoke(enumValue, boolValue);
    }

}
