using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Example class of wrapping the Base Swap values in a friendlier, more specific version
/// </summary>
[CreateAssetMenu(menuName = "Data/Pallete/Pallete: Dog", fileName = "DogPallete")]
public class DogPalleteDefiniton : PalleteSwapValuesBase
{
    [Header("Flames")]
    [SerializeField] Color flamePreBaseColor;
    [SerializeField] Color flameBaseColor;
    [SerializeField] Color flameMidColor;
    [SerializeField] Color flameEndColor;

    [Header("Body")]
    [SerializeField] Color bodyMainColor;
    [SerializeField] Color bodySideColor;
    [SerializeField] Color faceMainColor;
    [SerializeField] Color faceCenterColor;

    [Space(10)]
    [SerializeField] Material material;

    public override Color[] Colors
    {
        get
        {
            return new Color[]
            {
                flamePreBaseColor,
                flameBaseColor,
                flameMidColor,
                flameEndColor,

                bodySideColor,
                bodyMainColor,
                faceMainColor,
                faceCenterColor
            };
        }
    }
    public override Material Mat => material;

    void Reset()
    {
        flamePreBaseColor = ColorFromHSV("#88439F");
        flameBaseColor = ColorFromHSV("#B82161");
        flameMidColor = ColorFromHSV("#E35433");
        flameEndColor = ColorFromHSV("#F7B238");

        bodySideColor = ColorFromHSV("#422F9E");
        bodyMainColor = ColorFromHSV("#353564");
        faceMainColor = ColorFromHSV("#5640A9");
        faceCenterColor = ColorFromHSV("#854FC2");

    }

}
