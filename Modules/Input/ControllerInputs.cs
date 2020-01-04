using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ControllerInputs
{

    public static bool ActionBottomButton
    {
        get { return GetControllerValue(0, ButtonState.Pressed); }
    }
    public static bool ActionBottomButtonReleased
    {
        get { return GetControllerValue(0, ButtonState.Released); }
    }
    public static bool ActionRightButtonDown
    {
        get { return GetControllerValue(1, ButtonState.Pressed); }
    }
    public static bool ActionRightButtonReleased
    {
        get { return GetControllerValue(1, ButtonState.Released); }
    }
    public static bool ActionRightButton
    {
        get { return GetControllerValue(1, ButtonState.Held); }
    }
    public static bool ActionLeftButton
    {
        get { return GetControllerValue(2); }
    }
    public static bool ActionTopButton
    {
        get { return GetControllerValue(3); }
    }

    public static bool RightBumper
    {
        get { return GetControllerValue(5); }
    }

    public static bool LeftBumper
    {
        get { return GetControllerValue(4); }
    }

    public static bool Start
    {
        get { return GetControllerValue(7); }
    }
    public static bool Select
    {
        get { return GetControllerValue(6); }
    }

    public static bool LeftStick
    {
        get { return GetControllerValue(8); }
    }
    public static bool RightStick
    {
        get { return GetControllerValue(9); }
    }

    public static Vector2Int DPad
    {
        get
        {
            return new Vector2Int((int)Input.GetAxisRaw("DPadHorizontal"), (int)Input.GetAxisRaw("DPadVertical"));
        }
    }

    public enum ButtonState { Pressed, Held, Released }

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

    private static bool GetControllerValue(int buttonIndex)
    {
        return GetControllerValue(buttonIndex, ButtonState.Pressed);
    }
    public static bool GetControllerValue(int buttonIndex, ButtonState state)
    {
        return ConcatBool(GetControllerValues(buttonIndex, state));
    }
    public static bool GetControllerValue(ControllerKeyCode button, ButtonState state)
    {
        return ConcatBool(GetControllerValues((int)button, state));
    }

    public static KeyCode GetControllerKeyCode(int controllerIndex, int buttonIndex)
    {
        return (KeyCode)Enum.Parse(typeof(KeyCode), string.Format("Joystick{0}Button{1}", controllerIndex + 1, buttonIndex));
    }

    static bool ConcatBool(bool[] values)
    {
        for (int i = 0; i < values.Length; i++)
        {
            if (values[i]) return true;
        }

        return false;
    }
}
