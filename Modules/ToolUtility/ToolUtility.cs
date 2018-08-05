#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class ToolUtility
{

    public static void Label(string text, int space, int fontSize , bool bold, TextAnchor alignment)
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

    public static void Label(string text, float size, int fontSize, bool bold, TextAnchor alignment)
    {
        GUILayout.BeginHorizontal();

        GUIStyle style = new GUIStyle(GUI.skin.label);
        if (bold) style.fontStyle = FontStyle.Bold;
        style.fontSize = fontSize;
        style.alignment = alignment;

        GUILayout.Label(text, style, GUILayout.MinWidth(size));
        GUILayout.EndHorizontal();

    }

    public static void Label(string text, bool bold, TextAnchor aligment)
    {
        Label(text, 0, GUI.skin.label.fontSize, bold, aligment);
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
        GUILayout.Space(space);
        input = EditorGUILayout.IntSlider(input, min, max);

        GUILayout.EndHorizontal();

        return input;

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
    public static int IntSlider(string label, int minWidth, int maxWidth, int input, int min, int max)
    {
        GUILayout.BeginHorizontal(GUILayout.MinWidth(minWidth), GUILayout.MaxWidth(maxWidth));

        if (label.Length > 0)
            GUILayout.Label(label);

        input = EditorGUILayout.IntSlider(input, min, max);

        GUILayout.EndHorizontal();

        return input;

    }

    /// <summary>
    /// Slider that returns a float value
    /// </summary>
    /// <param name="label"></param>
    /// <param name="space"></param>
    /// <param name="input"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static float FloatSlider(string label, float minWidth, float maxWidth, float space, float input, float min, float max)
    {
        GUILayout.BeginHorizontal(GUILayout.MinWidth(minWidth), GUILayout.MaxWidth(maxWidth));

        if (label.Length > 0)
            GUILayout.Label(label);

        if (space > 0.0f)
            GUILayout.Space(space);

        GUILayout.FlexibleSpace();

        input = EditorGUILayout.Slider(input, min, max);

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

    public static AudioClip AudioClipField(string label, AudioClip input)
    {
        return (AudioClip)EditorGUILayout.ObjectField(label, input, typeof(AudioClip), false);
    }

}

#endif
