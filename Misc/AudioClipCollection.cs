using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "AudioClipCollection", menuName = "Data/References/Audio Clip Collection")]
public class AudioClipCollection : ScriptableObject
{
    [SerializeField] AudioClip[] audioClips;
    public AudioClip RandomClip
    {
        get
        {
            return audioClips.Length > 0 ? audioClips[Random.Range(0, audioClips.Length)] : null;
        }
    }

    public static implicit operator AudioClip(AudioClipCollection values)
    {
        if (values != null) return values.RandomClip;
        return null;
    }
}
