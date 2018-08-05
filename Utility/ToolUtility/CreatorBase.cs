using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreatorBase : EditorWindow {


    protected enum PosMode { local, global}

    protected PosMode posMode;
    protected Transform parent;

    protected Vector2 startPos = Vector2.one;
    protected Vector2 startScale = Vector2.one;

    protected float waitTime;
    protected float cycleTime;

    protected Sprite baseSprite;
    protected int orderInLayer;

    protected delegate void GUIDel();

    // Use this for initialization
    protected void BaseGUI(GUIDel action)
    {
        GUILayout.BeginVertical();

        GUILayout.Space(5);

        //Source sprite for platforms
         baseSprite = (Sprite)EditorGUILayout.ObjectField("Source sprite", baseSprite , typeof(Sprite), false);
        orderInLayer = EditorGUILayout.IntField("Sorting order", orderInLayer);

        GUILayout.Space(10);

        //Position mode and parent field for  the created object position
        posMode = (PosMode)EditorGUILayout.EnumPopup("Position mode:", posMode, GUILayout.MaxWidth(250));


        if (posMode == PosMode.local)
        {
            parent = (Transform)EditorGUILayout.ObjectField("Parent", parent, typeof(Transform), true);

        }

        startPos  = EditorGUILayout.Vector2Field("Position ", startPos , GUILayout.MaxWidth(250));

        if (action != null) action();

        GUILayout.Space(2.5f);
         startScale = EditorGUILayout.Vector2Field("Scale ", startScale, GUILayout.MaxWidth(250));



    }

}
