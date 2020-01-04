using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KeySetting
{
    public KeyCode keyboardKey;
    public ControllerKeyCode controllerKey;

    public const char divider = '_';

    public KeySetting(KeyCode keyboardKey, ControllerKeyCode controllerKey)
    {
        this.keyboardKey = keyboardKey;
        this.controllerKey = controllerKey;
    }
    public KeySetting(string serialized)
    {
        var split = serialized.Split(divider);
        System.Enum.TryParse(split[0], out keyboardKey);
        System.Enum.TryParse(split[1], out controllerKey);
    }

    public bool IsPressedDown
    {
        get
        {
            return Input.GetKeyDown(keyboardKey) || InputHandler.Instance.GetKeyDown(controllerKey);
        }
    }
    public bool IsPressed
    {
        get
        {
            return Input.GetKey(keyboardKey) || InputHandler.Instance.GetKey(controllerKey);
        }
    }

    public override string ToString()
    {
        return string.Format("{0}{1}{2}", keyboardKey.ToString(), divider, controllerKey.ToString());
    }
}
