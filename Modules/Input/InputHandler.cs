using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : AutomatedSingleton<InputHandler>
{
    public CartesianAxis leftStickAxis;
    public Axis triggerAxis;
    public CartesianAxis rightStickAxis;
    public CartesianAxis dPadAxis;
    public Vector2Int Movement
    {
        get
        {
            var value = dPadAxis.PressedValue + leftStickAxis.PressedValue;
            if (value.magnitude > 1) value = new Vector2Int(Mathf.Clamp(value.x, -1, 1), Mathf.Clamp(value.y, -1, 1));
            return value;
        }
    }

    private void Update()
    {
        triggerAxis.Update();
        leftStickAxis.Update();
        rightStickAxis.Update();
        dPadAxis.Update();
    }

    public bool GetKeyDown(ControllerKeyCode keyCode)
    {
        if(keyCode <= ControllerKeyCode.RightStick)
        {
            return ControllerInputs.GetControllerValue((int) keyCode, ControllerInputs.ButtonState.Pressed);
        }

        var key = keyCode.ToString().ToLower();
        if (key.Contains("dpad"))
        {
            var axis = dPadAxis.PressedValue;
            switch (keyCode)
            {
                case ControllerKeyCode.DPadUp:
                    return axis.y == 1;
                case ControllerKeyCode.DPadRight:
                    return axis.x == 1;
                case ControllerKeyCode.DPadDown:
                    return axis.y == -1;
                case ControllerKeyCode.DPadLeft:
                    return axis.x == -1;
            }
        }

        if(key.Contains("trigger"))
        {
            if (keyCode == ControllerKeyCode.LeftTrigger) return triggerAxis.pressedValue == -1;
            if (keyCode == ControllerKeyCode.RightTrigger) return triggerAxis.pressedValue == 1;
        }

        return false;
    }
    public bool GetKey(ControllerKeyCode keyCode)
    {
        if (keyCode <= ControllerKeyCode.RightStick)
        {
            return ControllerInputs.GetControllerValue((int)keyCode, ControllerInputs.ButtonState.Held);
        }

        return false;
    }

    [System.Serializable]
    public class Axis
    {
        [SerializeField] string name;

        public float lastValue;
        public int lastValueInt;
        public int pressedValue;

        public void Update()
        {
            var value = Input.GetAxisRaw(name);
            var intValue = Mathf.RoundToInt(value);
            pressedValue = 0;

            if(intValue != lastValueInt)
            {
                if (intValue != 0) pressedValue = intValue;
            }

            lastValueInt = Mathf.RoundToInt(value);
            lastValue = value;
        }
    }

    [System.Serializable]
    public class CartesianAxis
    {
        [SerializeField] Axis horizontal;
        [SerializeField] Axis vertical;

        public Vector2 Value => new Vector2(horizontal.lastValue, vertical.lastValue);
        public Vector2Int ValueInt => new Vector2Int(horizontal.lastValueInt, vertical.lastValueInt);
        public Vector2Int PressedValue => new Vector2Int(horizontal.pressedValue, vertical.pressedValue);

        public void Update()
        {
            horizontal.Update();
            vertical.Update();
        }
    }
}
