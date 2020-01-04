using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AudioClipEditor : DevFieldBase
{
    [Header("Audio Field")]
    [SerializeField] string folderPath;
    [SerializeField] TextMeshProUGUI textOutput;
    [SerializeField] int characterLimit;
    [SerializeField] AudioClip[] clips;
    [SerializeField] int selectedIndex;

    void UpdateOutput(AudioClip clip)
    {
        var text = clip.name;
        if (text.Length > characterLimit) text = text.Substring(0, characterLimit);

        textOutput.text = text;
    }
    void UpdateOutput()
    {
        UpdateOutput(clips[selectedIndex]);
    }

    public void ChangeClip(int way)
    {
        if (clips.Length == 0) return;

        selectedIndex += way;
        while (selectedIndex >= clips.Length) selectedIndex -= clips.Length;
        while (selectedIndex < 0) selectedIndex += clips.Length;

        UpdateOutput();

        valueOutput = string.Format("{0}/{1}", folderPath, clips[selectedIndex].name);
        editor.GetChanges(key, valueOutput);
    }

    public override void SetEditor(DevObjectEditor value)
    {
        base.SetEditor(value);
        //GetClips();
    }

    public void PlayClip()
    {
        Debug.Log("Sound player not set! Please change this line for your preferred method ...");
        //SoundManager.Instance.PlaySingle(clips[selectedIndex]);
    }

    public override void SetValue(string value)
    {
        Debug.Log(value);
        for (int i = 0; i < clips.Length; i++)
        {
            if(string.Format("{0}/{1}", folderPath, clips[i].name) == value)
            {
                selectedIndex = i;
                break;
            }
        }

        UpdateOutput();
        base.SetValue(value);
    }

    [ContextMenu("Get Clips")]
    public void GetClips()
    {
        clips = Resources.LoadAll<AudioClip>("AudioTests/" + folderPath);
        var list = new List<AudioClip>(clips);
        list.Reverse();

        clips = list.ToArray();
    }
}
