using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevIntField : DevFieldBase
{
    [SerializeField] TMPro.TMP_InputField inputField;
    [SerializeField] int minValue = 0;
    [SerializeField] int maxValue = 100;

    public override void SendChanges()
    {
        if (ignoreInput) return;

        var value = 0;
        int.TryParse(inputField.text, out value);

        if(value < minValue || value > maxValue)
        {
            value = Mathf.Clamp(value, minValue, maxValue);
            inputField.text = value.ToString();
            return;
        }

        valueOutput = value.ToString();
        base.SendChanges();
    }

    public override void SetValue(string value)
    {
        base.SetValue(value);
        inputField.text = value.ToString();
    }
}
