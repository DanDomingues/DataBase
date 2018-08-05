using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public static class ToolUtility
{
#if UNITY_EDITOR

    public static void Title(string text, int space, int fontSize , bool bold, TextAnchor alignment)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(space);

        GUIStyle style = new GUIStyle(GUI.skin.label);
        if (bold) style.fontStyle = FontStyle.Bold;
        style.fontSize = fontSize;
        style.alignment = alignment;

        GUILayout.Label(text, style);
        GUILayout.EndHorizontal();
    }

    public static void Title(string text, float size, int fontSize, bool bold, TextAnchor alignment)
    {
        GUILayout.BeginHorizontal();

        GUIStyle style = new GUIStyle(GUI.skin.label);
        if (bold) style.fontStyle = FontStyle.Bold;
        style.fontSize = fontSize;
        style.alignment = alignment;

        GUILayout.Label(text, style, GUILayout.MinWidth(size));
        GUILayout.EndHorizontal();

    }

    public static void TextField(string label, ref string input, int space)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(label);
        GUILayout.Space(space);
        input = EditorGUILayout.TextField(input);
        GUILayout.EndHorizontal();

    }


    /// <summary>
    /// Slider that returns a int value
    /// </summary>
    /// <param name="label"></param>
    /// <param name="space"></param>
    /// <param name="input"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static int IntSlider(string label, int space ,int input, int min, int max)
    {
        GUILayout.BeginHorizontal();

        GUILayout.Label(label);
        GUILayout.Space(15);
        input = EditorGUILayout.IntSlider(input, min, max);

        GUILayout.EndHorizontal();

        return input;

    }

    public static Object ObjectField(string label, Object obj, System.Type type, bool allowSceneObjects , int width)
    {
        GUILayout.BeginHorizontal( GUILayout.MaxWidth(width));

        GUILayout.Label(label);
        GUILayout.Space(5);
        Object result = EditorGUILayout.ObjectField(obj, type, allowSceneObjects);
        GUILayout.EndHorizontal();

        return result;
    }
#endif
}
