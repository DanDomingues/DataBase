using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AxisInput
{
    [SerializeField, Tooltip("Axis names for the axes used.\nTo setup axes, go to 'Edit/Project Settings/Input and select 'Axes''")]
    string[] axisNames;

    public float Value
    {
        get
        {
            float value = 0f;
            float tempValue;
            int count = 0;

            for (int i = 0; i < axisNames.Length; i++)
            {
                tempValue = Input.GetAxis(axisNames[i]);
                if(Mathf.Abs(tempValue) > 0.1f)
                {
                    value += tempValue;
                    count++;
                }
            }

            return count > 0 ? value / count : 0;
        }

    }

    public float ValueRaw
    {
        get
        {
            float value = 0f;
            float tempValue = 0f;
            int count = 0;

            for (int i = 0; i < axisNames.Length; i++)
            {
                tempValue = Input.GetAxisRaw(axisNames[i]);
                if (Mathf.Abs(tempValue) > 0.1f)
                {
                    value += tempValue;
                    count++;
                }
            }

            return count > 0 ? value / count : 0;

        }
    }

    public int ValueInt => (int)(Value * 1.2f);

    public int ValueIntRaw => (int)(ValueRaw * 1.2f);

    public AxisInput()
    {
        axisNames = new string[] { "My Axis" };
    }
}
