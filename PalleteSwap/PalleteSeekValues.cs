using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PalleteSeekData", menuName = "Data/Pallete/SeekData")]
public class PalleteSeekValues : ScriptableObject
{
    public Color[] colors;

    void Reset()
    {
        colors = new Color[10];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = new Color(1, 1, 1, 0);
        }
    }
}
