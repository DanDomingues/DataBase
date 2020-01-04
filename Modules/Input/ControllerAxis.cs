using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InstantAxis
{
    [SerializeField] ControllerKeyCode controllerNegative;
    [SerializeField] ControllerKeyCode controllerPositive;

    [SerializeField] KeyCode negative;
    [SerializeField] KeyCode positive;

    public int Value
    {
        get
        {
            int value = 0;
            if(Input.GetKeyDown(negative) || 
                ControllerInputs.GetControllerValue(controllerNegative, ControllerInputs.ButtonState.Pressed))
            {
                value--;
            }
            if (Input.GetKeyDown(positive) ||
                ControllerInputs.GetControllerValue(controllerPositive, ControllerInputs.ButtonState.Pressed))
            {
                value++;
            }

            return value;
        }
    }
}
