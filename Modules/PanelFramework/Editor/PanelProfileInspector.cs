#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PanelProfile))]
public class PanelProfileInspector : Editor
{

    public override void OnInspectorGUI()
    {
        PanelProfile profile = (PanelProfile)target;

        TransitionGUI(ref profile.openTransitionType, profile.GetTransition(true), true);

        GUILayout.Space(10);

        TransitionGUI(ref profile.closeTransitionType, profile.GetTransition(false), false);

        GlobalSpeedsGUI(profile);

        GUILayout.Space(10);

        SfxGUI(profile);

        EditorUtility.SetDirty(profile);

    }

    void TransitionGUI(ref AppearType type, PanelTransitionData data, bool value)
    {
        ToolUtility.Label(string.Format("{0} transition", value ? "Open" : "Close"), true, TextAnchor.MiddleLeft);

        type = (AppearType)EditorGUILayout.EnumPopup("Type", type);

        var curve = value ? data.openCurve : data.closeCurve;
        curve = EditorGUILayout.CurveField("Curve", curve);

        //data.closeCurve = EditorGUILayout.CurveField("Close Curve", data.closeCurve);

        data.useLocalSpeed = EditorGUILayout.Toggle("Local Speed", data.useLocalSpeed);

        if(data.useLocalSpeed)
            data.speedModifier = EditorGUILayout.Slider("Speed Modifier", data.speedModifier, 0.5f, 3.0f);

    }

    void GlobalSpeedsGUI(PanelProfile profile)
    {
        bool openIsGlobal = !profile.GetTransition(true).useLocalSpeed;
        bool closeIsGlobal = !profile.GetTransition(false).useLocalSpeed;

        if(openIsGlobal || closeIsGlobal)
        {
            GUILayout.Space(10);
            ToolUtility.Label("Global Transition Multipliers", true, TextAnchor.MiddleLeft);

            if(openIsGlobal)
                profile.openTransitionSpeed = EditorGUILayout.Slider("Open Speed Modifier", profile.openTransitionSpeed, 0.5f, 3.0f);

            if(closeIsGlobal)
                profile.closeTransitionSpeed = EditorGUILayout.Slider("Close Speed Modifier", profile.closeTransitionSpeed, 0.5f, 3.0f);

        }

    }

    void SfxGUI(PanelProfile profile)
    {
        ToolUtility.Label("Sound Effects", true, TextAnchor.MiddleLeft);

        profile.openSound = ToolUtility.AudioClipField("Open Sound", profile.openSound);
        profile.closeSound = ToolUtility.AudioClipField("Close Sound", profile.closeSound);

    }

}
#endif