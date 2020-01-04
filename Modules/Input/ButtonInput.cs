using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ButtonInput
{
    [SerializeField] ControllerInputs.ButtonState inputType;
    [SerializeField, Tooltip("Key codes derived from general key codes")]
    KeyCode[] keyboardInputs;
    [SerializeField, Tooltip("Key codes specific for controllers")]
    ControllerKeyCode[] controllerInputs;

    public bool Value
    {
        get
        {
            var values = new List<bool>();
            for (int i = 0; i < keyboardInputs.Length; i++)
            {
                switch (inputType)
                {
                    case ControllerInputs.ButtonState.Pressed:
                        values.Add(Input.GetKeyDown(keyboardInputs[i]));
                        break;
                    case ControllerInputs.ButtonState.Held:
                        values.Add(Input.GetKey(keyboardInputs[i]));
                        break;
                    case ControllerInputs.ButtonState.Released:
                        values.Add(Input.GetKeyUp(keyboardInputs[i]));
                        break;
                }
            }
            for (int i = 0; i < controllerInputs.Length; i++)
            {
                values.Add(ControllerInputs.GetControllerValue((int)controllerInputs[i], inputType));
            }

            return ConcatBool(values.ToArray());
        }
    }

    static bool ConcatBool(bool[] values)
    {
        for (int i = 0; i < values.Length; i++)
        {
            if (values[i]) return true;
        }

        return false;
    }

    public ButtonInput ()
    {
        keyboardInputs = new KeyCode[1];
        controllerInputs = new ControllerKeyCode[1];
    }

}
