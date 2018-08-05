using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TextLanguageSetting
{
#if UNITY_EDITOR
    [SerializeField]
    string name;
#endif
    [SerializeField]
    Language language;
    [SerializeField]
    string value;

    public Language Type { get { return language; } }
    public string Value
    {
        get { return value; }
    }

    public TextLanguageSetting(Language language, string value)
    {
#if UNITY_EDITOR
        name = language.ToString();
#endif
        this.language = language;
        this.value = value;

    }

    public static TextLanguageSetting[] GetStartingSetup(string baseValue)
    {
        int length = System.Enum.GetNames(typeof(Language)).Length;

        var settings = new TextLanguageSetting[length];
        for (int i = 0; i < length; i++)
        {
            var value = (Language)i != Language.Unity ? baseValue : "Unity";
            settings[i] = new TextLanguageSetting((Language)i, value);
        }

        return settings;
    }

    public void SetValue(string value)
    {
        this.value = value;
    }

}