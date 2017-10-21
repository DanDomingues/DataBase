using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public static class CanvasScripting
{

    public delegate void action();

    public static GameObject CreateCanvas()
    {
        GameObject canvas = new GameObject("Canvas" );

        RectTransform canvasRect = canvas.AddComponent<RectTransform>();
        Canvas canvasInfo = canvas.AddComponent<Canvas>();
        CanvasScaler scaler = canvas.AddComponent<CanvasScaler>();

        canvas.AddComponent<GraphicRaycaster>();
        canvas.AddComponent<EventSystem>();
        canvas.AddComponent<StandaloneInputModule>();

        canvasInfo.renderMode = RenderMode.ScreenSpaceOverlay;

        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

        return canvas;
    }

    private static RectTransform CreateImage(GameObject obj, Color color)
    {

        RectTransform rect = obj.GetComponent<RectTransform>();
        if (rect == null) rect = obj.AddComponent<RectTransform>();

        CanvasRenderer renderer = obj.GetComponent<CanvasRenderer>();
        if (renderer == null) renderer = obj.AddComponent<CanvasRenderer>();

        Image render =  obj.GetComponent<Image>();
        if (!render) render = obj.AddComponent<Image>();
        render.color = color;

        return rect;


    }

    public static RectTransform InstantiateBackground(GameObject obj, Color color)
    {

        RectTransform rect = obj.GetComponent<RectTransform>();
        if (rect == null) rect = obj.AddComponent<RectTransform>();

        CanvasRenderer renderer = obj.GetComponent<CanvasRenderer>();
        if (renderer == null) renderer = obj.AddComponent<CanvasRenderer>();

        Image render = obj.GetComponent<Image>();
        if (!render) render = obj.AddComponent<Image>();
        render.color = color;

        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;

        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;



        return rect;


    }

    public static RectTransform InstantiateBackground(GameObject obj,  Vector2 size,  Sprite sprite)
    {

        RectTransform rect = CreateImage(obj, Color.white);

        rect.anchorMin = (Vector2.one - size) / 2;
        rect.anchorMax = Vector2.one - rect.anchorMin;

        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        obj.GetComponent<Image>().sprite =  sprite;
        return rect;
     }

    public static RectTransform InstantiateBackground(GameObject obj,   Vector2 size , Vector2 offset, Sprite sprite)
     {


        RectTransform rect = InstantiateBackground(obj, size, sprite);
        rect.anchoredPosition = Vector2.zero ; 
        Vector3.Normalize(offset);

        float x = 0;
        float y = 0;

        if (offset.x > 0) x = -size.x; else if (offset.x == 0) x = 0; else x = size.x;
        if (offset.y > 0) y = -size.y; else if (offset.y == 0) y = 0; else y = size.y;

        Vector2  newOffset = new Vector2(x, y );
        //Debug.Log(size + " " + offset + " "  +newOffset);

        offset = (offset +  newOffset)  / 2;

        rect.anchorMin += offset;
        rect.anchorMax += offset;

        obj.GetComponent<Image>().sprite =  sprite;
        return rect;
     }

    public static Image InstantiateImage (GameObject obj, Vector2 size, Vector2 position, Sprite sprite)
    {
        RectTransform rect = CreateImage(obj, Color.white);

        Image img = obj.GetComponent <Image>();
        if (img == null) img = obj.AddComponent<Image>();

        img.sprite = sprite;

        rect.sizeDelta = size;
        rect.anchoredPosition = position;


        return img;
    }

    public static Button InstantiateButton(GameObject obj, Vector2 size,  Vector2 position, Sprite sprite)
    {
        InstantiateImage(obj, size, position, sprite);

        Button button = obj .AddComponent<Button>();


        return button;

    }

    public static Text AddText(GameObject parent, string textContent, int textSize, Color textColor, Font font)
    {
        GameObject obj = new GameObject();
        obj.name = parent.name + "'s text";
        Text t = obj.AddComponent<Text>();

        t.text = textContent;
        t.resizeTextMaxSize = textSize;
        t.resizeTextForBestFit = true;
        t.color = textColor;
        t.alignment = TextAnchor.MiddleCenter;

        if (font != null) t.font = font;
       
        t.raycastTarget = false;

        obj.transform.SetParent(parent.transform)  ;

        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;

        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        return t;

    }

    public static RectTransform CreateText  (GameObject parent,  TextAnchor  alignment ,string textContent, int textSize, Font textFont)
    {
        GameObject obj = new GameObject(textContent);

        Text data = obj.AddComponent<Text>();
        data.text = textContent;
        data.resizeTextForBestFit = true;
        data.resizeTextMaxSize = textSize;

        data.alignment = alignment;
        data.horizontalOverflow = HorizontalWrapMode.Wrap ;
        

        if(textFont != null)  data.font =    textFont;

        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.parent = parent.transform ;
        rect.localScale = Vector2.one ;

        rect.sizeDelta = new Vector2(300,textSize);

        return rect;


    }

     public static void SetRect(ref  RectTransform rect, Vector2  anchor, Vector2 pivot)
    {
        rect.anchorMin += anchor * 0.5f;
        rect.anchorMax += anchor * 0.5f;
        rect.pivot     += pivot * 0.5f;

    }


}
