using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputCentral
{

    public static bool confirm
    {
        get { return GetControllerValue(0, ButtonState.Pressed); }
    }

    public static bool confirmReleased
    {
        get { return GetControllerValue(0, ButtonState.Released); }
    }

    public static bool back
    {
        get { return GetControllerValue(1); }
    }

    public static bool attack
    {
        get { return GetControllerValue(2); }
    }

    public static bool special
    {
        get { return GetControllerValue(3); }
    }

    public static bool rightBumper
    {
        get { return GetControllerValue(5); }
    }

    public static bool leftBumper
    {
        get { return  GetControllerValue(4); }
    }

    public static bool start
    {
        get { return GetControllerValue(7); }
    }

    private enum ButtonState { Pressed, Held, Released}

    private static bool[] GetControllerValues(int buttonIndex, ButtonState state)
    {
        List<bool> values = new List<bool>();

        for (int i = 0; i < 4; i++)
        {
            bool value = false;
            KeyCode code = GetControllerKeyCode(i, buttonIndex);

            switch (state)
            {
                case ButtonState.Pressed:
                    value = Input.GetKeyDown(code);
                    break;

                case ButtonState.Held:
                    value = Input.GetKey(code) && !Input.GetKeyDown(code);
                    break;

                case ButtonState.Released:
                    value = Input.GetKeyUp(code);
                    break;
            }

            values.Add(value);
        }

        return values.ToArray();
    }

    private static bool[] GetControllerValues(int buttonIndex)
    {
        return GetControllerValues(buttonIndex, ButtonState.Pressed);
    }

    private static bool GetControllerValue(int buttonIndex)
    {
        return GetControllerValue(buttonIndex, ButtonState.Pressed);
    }

    private static bool GetControllerValue(int buttonIndex, ButtonState state)
    {
        return GenericUtility.ConcatBool(GetControllerValues(buttonIndex, state));
    }

    public static KeyCode GetControllerKeyCode(int controllerIndex, int buttonIndex)
    {
        return (KeyCode) Enum.Parse(typeof(KeyCode), string.Format("Joystick{0}Button{1}", controllerIndex + 1, buttonIndex));
    }

}
