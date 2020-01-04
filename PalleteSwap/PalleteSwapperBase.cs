using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PalleteSwapperBase : MonoBehaviour
{
    [SerializeField] PalleteSeekValues seekDefinition;
    [SerializeField] PalleteSwapValuesBase SwapDefinition;

    protected abstract Material Mat { get; }

    protected virtual void Reset()
    {
        var seekDefs = Resources.LoadAll<PalleteSeekValues>("");
        if (seekDefs.Length > 0) seekDefinition = seekDefs[0];
    }

    public virtual void SwapColors(PalleteSwapValuesBase values)
    {
#if UNITY_EDITOR
        SwapDefinition = values;
#endif
        if (Mat == null || seekDefinition == null || values == null) return;

        Mat.SetColorArray("_SeekColors", seekDefinition.colors);

        var SwapColors = new List<Color>(values.Colors);
        while (SwapColors.Count < 10) SwapColors.Add(Color.clear);
        Mat.SetColorArray("_OutputColors", SwapColors.ToArray());
    }
    public void SwapColors()
    {
        SwapColors(SwapDefinition);
    }

#if UNITY_EDITOR
    protected void OnDrawGizmos()
    {
        SwapColors();
    }


    protected void Update()
    {
        SwapColors();
    }
#endif

}
