using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MultiLanguageEnum
{
    [SerializeField]
    public System.Type type;
    [SerializeField]
    public MultiLanguageString[] values;

    public MultiLanguageEnum(System.Type type)
    {
        this.type = type;

        var enums = GenericUtility.GetEnums(type);
        values = new MultiLanguageString[enums.Length];
        for (int i = 0; i < enums.Length; i++)
        {
            values[i] = new MultiLanguageString(enums[i].ToString());
        }

    }


}
