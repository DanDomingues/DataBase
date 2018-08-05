using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorInitializer : MonoBehaviour
{

    [SerializeField]
    string globalSceneName = "GlobalScene";
    [SerializeField]
    bool playSound;
    [SerializeField]
    int trackIndex = 1;

    // Use this for initialization
    void Awake ()
    {
        StartCoroutine(Routine());
	}

    IEnumerator Routine()
    {
        if (!SceneUtility.CheckForDomain("Global"))
        {
            yield return SceneUtility.LoadAdditive(this, globalSceneName);
        }
        yield return new WaitForEndOfFrame();

#if UNITY_EDITOR

        if(playSound)
            SoundManager.instance.PlayTrack(trackIndex);
#endif
    }
	
}
