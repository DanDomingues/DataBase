using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DevFloatField : DevFieldBase
{
    [SerializeField] TMP_InputField inputField;

    public override void SendChanges()
    {
        if (ignoreInput) return;

        var value = 0f;
        float.TryParse(inputField.text, NumberStyles.Float, new CultureInfo("en-US"), out value);
        valueOutput = value.ToString();

        base.SendChanges();
    }

    public override void SetValue(string value)
    {
        if (value == null)
        {
            Debug.Log(string.Format("Could not use value [{0}] at {1}", value, key));
        }

        if(value.Contains(","))
        {
            var parts = value.Split(',');
            value = parts[0] + "." + parts[1];
        }

        base.SetValue(value);
        inputField.text = value;
    }


}
