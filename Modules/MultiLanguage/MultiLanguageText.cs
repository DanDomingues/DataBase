using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof (Text))]
public class MultiLanguageText : MonoBehaviour
{
    [SerializeField]
    TextLanguageSetting[] settings;

    Text text;
    private void Start()
    {
        text = GetComponent<Text>();
        SetLanguage(MultiLanguage.CurrentLanguage);

        MultiLanguage.OnLanguageChange.Subscribe(SetLanguage);
    }

    private void Reset()
    {
        settings = TextLanguageSetting.GetStartingSetup(GetComponent<Text>().text);
    }

    void SetLanguage(System.Enum type)
    {
        var language = (Language)type;
        int index = (int)language;

        for (int i = 0; i < settings.Length; i++)
        {
            if(language == settings[i].Type)
            {
                text.text = settings[i].Value;
                break;
            }
        }

    }
}
