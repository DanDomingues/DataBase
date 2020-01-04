using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PalleteSwapValuesBase : ScriptableObject
{
    public abstract Color[] Colors { get; }
    public virtual Material Mat { get; }

    protected Color ColorFromHSV(string code)
    {
        Color c;
        ColorUtility.TryParseHtmlString(code, out c);
        return c;
    }
}
