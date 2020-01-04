using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PalleteSwapperUGUI : PalleteSwapperBase
{
    [SerializeField] Image image;

    protected override Material Mat
    {
        get
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
            {
                return image.material;
            }
            else
            {
                return image.material;
            }
#endif
            return image.material;
        }
    }

    protected override void Reset()
    {
        base.Reset();
        image = GetComponentInChildren<Image>();
    }

    public override void SwapColors(PalleteSwapValuesBase values)
    {
        image.material = values.Mat;
        base.SwapColors(values);
    }

}