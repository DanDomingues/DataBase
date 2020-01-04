using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PixelFill : MonoBehaviour
{
    [SerializeField] protected RectTransform rect;
    [SerializeField] protected Image bgImage;
    [SerializeField] protected Image fillImage;
    [SerializeField] int brimWidth;
    [SerializeField, Range(0f, 1f)] float fillAmount;

#if UNITY_EDITOR
    [SerializeField] bool updateOnGizmos;
#endif
    protected float PixelSize => 1f / (rect.sizeDelta.x);

    public float FillAmount
    {
        get { return fillAmount; }
        set { SetFillAmount(value); }
    }

    protected virtual void Reset()
    {
        rect = GetComponent<RectTransform>();
        brimWidth = 1;

        var imgs = GetComponentsInChildren<Image>();
        if (imgs.Length > 0)
        {
            bgImage = imgs[0];
            bgImage.fillMethod = Image.FillMethod.Horizontal;            
        }
        if (imgs.Length > 1)
        {
            fillImage = imgs[1];
            bgImage.fillMethod = Image.FillMethod.Horizontal;
        }
    }

    public virtual void SetFillAmount(float value)
    {
        value = Mathf.Round(value / PixelSize) * PixelSize;

        fillImage.fillAmount = value;
        bgImage.fillAmount = value + (PixelSize * brimWidth);

        fillAmount = value;
        //Auto step for killing the brim if the value is 0
        //if(value >= pixelSize && value <= 1 - pixelSize)
        //{

        //}
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if(updateOnGizmos) SetFillAmount(fillAmount);
    }
#endif
}
