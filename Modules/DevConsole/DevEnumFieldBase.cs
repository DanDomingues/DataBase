using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public abstract class DevEnumFieldBase : DevFieldBase
{
    [SerializeField] Dropdown dropdown;
    protected abstract System.Type enumType { get; }

    private void Start()
    {
        var names = System.Enum.GetNames(enumType);
        var options = new List<Dropdown.OptionData>();
        for (int i = 0; i < names.Length; i++)
        {
            options.Add(new Dropdown.OptionData(names[i]));
        }

        dropdown.options = options;
    }

    public override void SendChanges()
    {
        //Gets string from dropdown and removes it's spaces
        var text = dropdown.options[dropdown.value].text;
        if(text.Contains(" "))
        {
            var split = text.Split(' ');
            text = "";
            for (int i = 0; i < split.Length; i++)
            {
                text += split[i];
            }
        }

        //Parses if string is a valid one
        var names = new List<string>(System.Enum.GetNames(enumType));
        if(names.Contains(text))
        {
            valueOutput = System.Enum.Parse(enumType, text).ToString();
            base.SendChanges();
        }

    }

    public override void SetValue(string value)
    {
        base.SetValue(value);
        dropdown.value = (int)System.Enum.Parse(enumType, value);
    }
}
