using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DevBoolField : DevFieldBase
{
    [SerializeField] Toggle toggle;

    public override void SendChanges()
    {
        if (ignoreInput) return;

        valueOutput = toggle.isOn.ToString();
        base.SendChanges();
    }

    public override void SetValue(string value)
    {
        var output = false;
        bool.TryParse(value, out output);
        toggle.isOn = output;

        base.SetValue(value);
    }

}
