using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PanelProfile",  menuName = "Data/PanelProfile")]
public class PanelProfile : ScriptableObject
{

    [Header("Pop Appear  Properties")]
    public AnimationCurve popUpCurve;
    public AnimationCurve popOutCurve;

    [Range(0.2f, 3.0f)]
    public float speedModifier = 1.0f;

    [Header("Vertical Appear Properties")]

    public AnimationCurve verticalAppearCurve;
    public AnimationCurve verticalHideCurve;

    [Range(0.2f, 3.0f)]
    public float verticalSpeed = 1.0f;

    [Space(10)]
    public AppearType openTransition;
    public AppearType closeTransition;

    [Space(10), Header("Sound Clips")]
    public AudioClip openSound;
    public AudioClip closeSound;
    public AudioClip selectSound;
    public AudioClip confirmSound;
    public AudioClip returnSound;
}

public enum AppearType
{
    Pop,
    Vertical,
    Instant
}

