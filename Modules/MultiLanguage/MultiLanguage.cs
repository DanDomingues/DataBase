using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MultiLanguage
{
    /// <summary>
    /// Change when language selection is implemented. Defaults to English
    /// </summary>
    public static Language CurrentLanguage
    {
        get
        {
            return Language.EN;
        }
    }

    /// <summary>
    /// Change for a local event when possible. Defaults to a static event. To be called when the current language is changed.
    /// </summary>
    public static EnumEvent OnLanguageChange
    {
        get
        {
            return onLanguageChange;
        }
    }

    [SerializeField] static EnumEvent onLanguageChange;

}