using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class SoundManager : MonoBehaviour 
{

    public static SoundManager Instance;

    public AudioSource sfxSource;
    public AudioSource[] trackSources;

    [SerializeField]
    private bool m_sfxOn = true;
    [SerializeField]
    private bool m_trackOn = true;

    /// <summary>
    /// Boolean for Sound Effects being turned On or Not.
    /// Setting this to false will not stop effects that have already started.
    /// </summary>
    public bool SfxOn
    {
        get { return m_sfxOn; }
        set
        {
            
            m_sfxOn = value;
            
        }
    }

    /// <summary>
    /// Boolean for Tracks being On or not.
    /// Changing this pauses or unpauses the trackSource by default. 
    /// </summary>
    public bool TrackOn
    {
        get { return m_trackOn; }
        set
        {
            m_trackOn = value;
            PauseTrack(!value);
        }
    }

    [Range(0,3)]
    public float pitchShiftCap = 0.5f;

    public AudioClip[] tracks;

    [SerializeField] float trackVolume;
    [SerializeField] int activeTrackSourceID;
    Coroutine multiTrackRoutine;
    Coroutine crossFadeRoutine;

    public AudioSource TrackSource => trackSources[activeTrackSourceID];
    public AudioSource SupportTrackSource => trackSources[1 - activeTrackSourceID];

    public float SfxVolume
    {
        get { return sfxSource.volume; }
        set { sfxSource.volume = value; }
    }

    public float TrackVolume
    {
        get { return trackVolume; }
        set 
        {
            TrackSource.volume = value;
            trackSources[1 - activeTrackSourceID].volume = 0;
            trackVolume = value;
        }
    }



    /// <summary>
    /// Toggles TrackOn to false if it is true and vice-versa.
    /// </summary>
    public void ToggleTrack()
    {
        TrackOn = !TrackOn;
    }

    /// <summary>
    /// Toggles SfxOn to false if it is true and vice-versa.
    /// </summary>
    public void ToggleSfx()
    {
        m_sfxOn =  !m_sfxOn;
    }

    #region PlaySingle Overloads

    /// <summary>
    /// Plays clip
    /// </summary>
    /// <param name="clip">AudioClip to be played</param>
    public void PlaySingle(AudioClip clip)
    {
        PlaySingle(clip, 1, 0, 0);
    }

    /// <summary>
    /// Plays a clip with volume modifier
    /// </summary>
    /// <param name="clip">AudioClip to be played</param>
    /// <param name="volume">Volume modifier</param>
    public void PlaySingle(AudioClip clip, float volume)
    {
        PlaySingle(clip, volume, 0, 0);
        
    }

    /// <summary>
    /// Plays clip and randomizes pitch on set range
    /// </summary>
    /// <param name="clip">AudioClip to be played</param>
    /// <param name="pitchMin">Lowest pitch shift allowed (negative)</param>
    /// <param name="pitchMax">Highest pitch shift allowed (positive)</param>
    public void PlaySingle(AudioClip clip, float pitchMin, float pitchMax)
    {
         PlaySingle(clip, 1, pitchMin, pitchMax) ;

    }

    /// <summary>
    /// Plays clip with volume modifier and randomizes pitch on set range
    /// </summary>
    /// <param name="clip">AudioClip to be played</param>
    /// <param name="volume">Volume modifier</param>
    /// <param name="pitchMin">Lowest pitch shift allowed (negative)</param>
    /// <param name="pitchMax">Highest pitch shift allowed (positive)</param>
    public void PlaySingle(AudioClip clip, float volume , float pitchMin, float pitchMax)
    {
        if (sfxSource == null) return;
        if (!SfxOn) return;

        //Mathf.Clamp01(volume);
        //sfxSource.volume = volume;

        pitchMin = Mathf.Clamp(pitchMin, -pitchShiftCap , 0);
        pitchMax = Mathf.Clamp(pitchMax, 0, pitchShiftCap);
        sfxSource.pitch = 1 + Random.Range(pitchMin, pitchMax);

        sfxSource.PlayOneShot(clip, volume);

    }

    /// <summary>
    /// Plays a random clip from an inputed array
    /// </summary>
    /// <param name="clips">AudioClip array from which an element will be played</param>
    public void PlaySingle(AudioClip[] clips)
    {
        PlaySingle(clips, 1, 0, 0);
    }

    /// <summary>
    /// Plays a clip with volume modifier
    /// </summary>
    /// <param name="clips">AudioClip array from which an element will be played</param>
    /// <param name="volume">Volume modifier</param>
    public void PlaySingle(AudioClip[] clips, float volume)
    {
        PlaySingle(clips, volume, 0, 0);

    }

    /// <summary>
    /// Plays clip and randomizes pitch on set range
    /// </summary>
    /// <param name="clips">AudioClip array from which an element will be played</param>
    /// <param name="pitchMin">Lowest pitch shift allowed (negative)</param>
    /// <param name="pitchMax">Highest pitch shift allowed (positive)</param>
    public void PlaySingle(AudioClip[] clips, float pitchMin, float pitchMax)
    {
        PlaySingle(clips, 1, pitchMin, pitchMax);

    }

    /// <summary>
    /// Plays clip with volume modifier and randomizes pitch on set range
    /// </summary>
    /// <param name="clips">AudioClip array from which an element will be played</param>
    /// <param name="volume">Volume modifier</param>
    /// <param name="pitchMin">Lowest pitch shift allowed (negative)</param>
    /// <param name="pitchMax">Highest pitch shift allowed (positive)</param>
    public void PlaySingle(AudioClip[] clips, float volume, float pitchMin, float pitchMax)
    {
        if (sfxSource == null) return;
        if (!SfxOn) return;
        if (clips == null) return;
        if (clips.Length == 0) return;


        //Mathf.Clamp01(volume);
        //sfxSource.volume = volume;

        pitchMin = Mathf.Clamp(pitchMin, -pitchShiftCap, 0);
        pitchMax = Mathf.Clamp(pitchMax, 0, pitchShiftCap);
        sfxSource.pitch = 1 + Random.Range(pitchMin, pitchMax);

        sfxSource.PlayOneShot(clips[Random.Range(0,clips.Length)] , volume);


    }

    /// <summary>
    /// Plays clip despite the SFX being On or Off.
    /// </summary>
    /// <param name="clip"></param>
    public void PlaySingleRaw(AudioClip clip)
    {
        if (sfxSource == null) return;

        sfxSource.PlayOneShot(clip);

    }

    #endregion PlaySingle Overloads

    /// <summary>
    /// Plays a index-specific AudioClip from the 'tracks' AudioClip array.
    /// </summary>
    /// <param name="i"></param>
    public void PlayTrack(int i)
    {
        if (tracks == null) return;
        if (tracks.Length == 0 || i >= tracks.Length || TrackSource.isPlaying)
        {
            Debug.Log("Track cancelled");
            return;
        }

        TrackSource.loop = true;

        PlayTrack(tracks[i]);
    }

    public void PlayTrack(AudioClip track)
    {
        if (TrackSource == null) return;
        TrackSource.clip = track;

        if (!TrackOn || TrackSource.isPlaying) return; 

        InterruptTrackRoutines(true);
        TrackSource.Play();

    }
    /// <summary>
    /// Plays a random AudioClip from the 'tracks' AudioClip array.
    /// </summary>
    public void PlayTrack()
    {
        PlayTrack(Random.Range(0, tracks.Length));
    }
    public void PlayTrack(AudioClip[] trackArray)
    {
        if (TrackSource == null || !TrackOn) return;

        InterruptTrackRoutines(true);
        multiTrackRoutine = StartCoroutine(TrackLoopRoutine(TrackSource, trackArray));
    }

    IEnumerator TrackLoopRoutine(AudioSource source, AudioClip[] trackArray)
    {
        List<AudioClip> playedTracks = new List<AudioClip>();

        int i = 0;
        while (i == 0)
        {
            
            AudioClip selectedClip = trackArray[Random.Range(0, trackArray.Length)];

            while(playedTracks.Contains(selectedClip))
            {
                selectedClip = trackArray[Random.Range(0, trackArray.Length)];

                if (playedTracks.Count == trackArray.Length)
                    playedTracks = new List<AudioClip>();
            }

            playedTracks.Add(selectedClip);
            source.clip = selectedClip;
            source.Play();

            while(TrackSource.time < selectedClip.length)
            {
                yield return new WaitForEndOfFrame();
            }

        }
    }

    void InterruptTrackRoutines(bool resetVolume)
    {
        if(multiTrackRoutine != null) StopCoroutine(multiTrackRoutine);
        multiTrackRoutine = null;

        if(crossFadeRoutine != null) StopCoroutine(crossFadeRoutine);
        crossFadeRoutine = null;

        if(resetVolume) TrackSource.volume = TrackVolume;
    }

    public void CrossFadeTrack(AudioClip[] tracks, float duration)
    {
        if(!TrackOn) return;

        InterruptTrackRoutines(false);
        multiTrackRoutine = StartCoroutine(TrackLoopRoutine(SupportTrackSource, tracks));

        SupportTrackSource.volume = 0f;
        SupportTrackSource.time = TrackSource.time;
        crossFadeRoutine = StartCoroutine(CrossFade_Routine(duration));
    }

    IEnumerator CrossFade_Routine(float duration)
    {
        StartCoroutine(LerpVolume_Routine(TrackSource, 0f, duration));
        StartCoroutine(LerpVolume_Routine(SupportTrackSource, 1f, duration));
     
        yield return new WaitForSeconds(duration);
        activeTrackSourceID = 1 - activeTrackSourceID;
    }

    IEnumerator LerpVolume_Routine(AudioSource source, float target, float duration)
    {
        var startVolume = source.volume;
        var t = 0.0f;
        while(t < 1.0f)
        {
            t += Time.deltaTime / duration;
            t = Mathf.Clamp01(t);

            source.volume = Mathf.Lerp(startVolume, target, t);

            yield return new WaitForEndOfFrame();
        }
    }

    /// <summary>
    /// Pauses and/or Unpauses the trackSource depending if it's currently playing
    /// </summary>
    public void PauseTrack()
    {
        PauseTrack(TrackSource.isPlaying, 0.1f);
    }
    public void PauseTrack(bool value)
    {
        PauseTrack(value, 0.1f);
    }
    /// <summary>
    /// Pauses (true) or Unpauses (false) the 'trackSource' depending on the nature parameter.
    /// </summary>
    /// <param name="nature">Parameter that determines if this function pauses or unpauses the 'trackSource'</param>
    public void PauseTrack(bool nature, float fadeDuration)
    {
        if (TrackSource == null) return;
        if (TrackSource.clip == null) return;

        float volume = nature ? 0f : TrackVolume;
        StartCoroutine(LerpVolume_Routine(TrackSource, volume, fadeDuration));
    }


    // Use this for initialization
     void Awake  ()
    {
        Time.timeScale = 1;
        trackVolume = 1f;

        int sourcesAmount = 3;
        if (sfxSource == null || TrackSource == null)
        {
            AudioSource[] sources = GetComponents<AudioSource>();

            if(sources == null)
            {
                print("Error");
                return;

            }
            if(sources.Length > sourcesAmount)
            {
                for (int i = sourcesAmount; i < sources.Length; i++)
                {
                    Destroy(sources[i]);
                }

            }
            else if (sources.Length < sourcesAmount)
            {

                AudioSource[] array = new AudioSource[sourcesAmount];

                for (int i =   0; i < array.Length; i++)
                {
                    if (i < sources.Length)
                        array[i] = sources[i];
                    else
                    {
                        array[i] = gameObject.AddComponent<AudioSource>();
                        array[i].playOnAwake = false;
                    }
                }

                 sources = array;
            }

            trackSources = new AudioSource[] { sources[0], sources[1] };
            TrackSource.loop = true;
            SupportTrackSource.loop = true;
            sfxSource = sources[2];

        }

        // DontDestroyOnLoad(gameObject);
        CheckExisting();

    }

    public void CheckExisting()
    {
        if (Instance == null)
        {
            Instance = FindObjectOfType<SoundManager>();

        }
        else
        {
            gameObject.SetActive(false);
            gameObject.name = "Disabled Sound Manager";
            Debug.Log("Sound Manager already present. Shutting down ...");
        }

    }

}
