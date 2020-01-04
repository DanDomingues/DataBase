using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalleteSwapper : PalleteSwapperBase
{
    [SerializeField] SpriteRenderer render;

    protected override Material Mat
    {
        get
        {
#if UNITY_EDITOR
            if(Application.isPlaying)
            {
                return render.material;
            }
            else
            {
                return render.sharedMaterial;
            }
#endif
            return render.material;
        }
    }

    protected override void Reset()
    {
        base.Reset();
        render = GetComponentInChildren<SpriteRenderer>();
    }

}
