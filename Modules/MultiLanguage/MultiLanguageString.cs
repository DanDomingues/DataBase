using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MultiLanguageString
{
    [SerializeField]
    protected TextLanguageSetting[] values;

    public string Value
    {
        get
        {
            return GetValue(MultiLanguage.CurrentLanguage);
        }
    }

    public TextLanguageSetting[] Values => values;

    public MultiLanguageString()
    {
        values = TextLanguageSetting.GetStartingSetup("");
    }

    public MultiLanguageString(string baseValue)
    {
        values = TextLanguageSetting.GetStartingSetup(baseValue);
    }

    public string GetValue(Language targetLanguage)
    {
        var value = values.FirstOrDefault(set => set.Type == targetLanguage);
        return value != null ? value.Value : "Please correct";
    }

    public override string ToString()
    {
        return Value;
    }

}
