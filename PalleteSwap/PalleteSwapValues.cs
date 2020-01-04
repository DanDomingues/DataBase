using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PalleteValues", menuName = "Data/Pallete/SwapValues")]
public class PalleteSwapValues : PalleteSwapValuesBase
{
    [SerializeField] Color[] colors;

    public override Color[] Colors => colors;

    void Reset()
    {
        colors = new Color[10];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = new Color(1, 1, 1, 0);
        }
    }
}
