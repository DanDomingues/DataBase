using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PanelProfile",  menuName = "Data/PanelProfile")]
public class PanelProfile : ScriptableObject
{

    public PanelTransitionData[] openTransitions;
    public PanelTransitionData[] closeTransitions;

    [Space(10)]
    public AppearType openTransitionType;
    public AppearType closeTransitionType;

    public float openTransitionSpeed;
    public float closeTransitionSpeed;

    [Space(10), Header("Sound Clips")]
    public AudioClip openSound;
    public AudioClip closeSound;
    public AudioClip selectSound;
    public AudioClip confirmSound;
    public AudioClip returnSound;

    public PanelProfile()
    {
        int length = System.Enum.GetNames(typeof(AppearType)).Length;
        openTransitions = new PanelTransitionData[length];
        closeTransitions = new PanelTransitionData[length];

        openTransitionSpeed = 1.0f;
        closeTransitionSpeed = 1.0f;
    }

    public PanelTransitionData GetTransition(bool transitionNature)
    {
        var transitions = transitionNature ? openTransitions : closeTransitions;
        var type = transitionNature ? openTransitionType : closeTransitionType;
        return transitions[(int)type];

    }

}

public enum AppearType
{
    Pop,
    BottomToTop,
    TopToBottom,
    LeftToRight,
    RightToLeft,
    Instant
}

