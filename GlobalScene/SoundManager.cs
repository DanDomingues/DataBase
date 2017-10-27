using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class SoundManager : MonoBehaviour {

    public static SoundManager instance;

    public AudioSource sfxSource;
    public AudioSource trackSource;

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
            PauseTrack(value);
        }
    }

    [Range(0,3)]
    public float pitchShiftCap = 0.5f;

    public AudioClip[] tracks;
    public CommonSFX sfxBank;

    /// <summary>
    /// Toggles TrackOn to false if it is true and vice-versa.
    /// </summary>
    public void ToggleTrack()
    {
        m_trackOn =  !m_trackOn;
        if(!m_trackOn) PauseTrack();
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

    public void PlayTrack(AudioClip[] trackArray)
    {
        if (trackSource == null) return;

        StartCoroutine(TrackLoopRoutine( trackArray));
    }

    IEnumerator TrackLoopRoutine(AudioClip[] trackArray)
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
            PlayTrack(selectedClip);

            while(trackSource.time < selectedClip.length)
            {

                yield return new WaitForEndOfFrame();

            }

        }
    }

    /// <summary>
    /// Plays a index-specific AudioClip from the 'tracks' AudioClip array.
    /// </summary>
    /// <param name="i"></param>
    public void PlayTrack(int i)
    {
        if (tracks == null) return;
        if (tracks.Length == 0 || i >= tracks.Length || trackSource.isPlaying)
        {
            Debug.Log("Track cancelled");
            return;
        }

        trackSource.loop = true;

        PlayTrack(tracks[i]);
    }

    public void PlayTrack(AudioClip track)
    {
        if (trackSource == null) return;
        trackSource.clip = track;

        if (!TrackOn || trackSource.isPlaying) return; 

        trackSource.Play();

    }

    /// <summary>
    /// Plays a random AudioClip from the 'tracks' AudioClip array.
    /// </summary>
    public void PlayTrack()
    {
        PlayTrack(Random.Range(0, tracks.Length));

    }

    /// <summary>
    /// Pauses and/or Unpauses the trackSource depending if it's currently playing
    /// </summary>
    public void PauseTrack()
    {
        PauseTrack(trackSource.isPlaying);
    }

    /// <summary>
    /// Pauses (true) or Unpauses (false) the 'trackSource' depending on the nature parameter.
    /// </summary>
    /// <param name="nature">Parameter that determines if this function pauses or unpauses the 'trackSource'</param>
    public void PauseTrack(bool nature)
    {

        if (trackSource == null) return;
        if (trackSource.clip == null || trackSource.time == 0) return;

        if (nature) trackSource.Pause();
        else trackSource.UnPause();
    }


    // Use this for initialization
     void Awake  ()
    {
        instance = GetComponent<SoundManager>();
        sfxBank = Resources.Load<CommonSFX>("DataObjects/SFXBank");

        if (sfxSource == null || trackSource == null)
        {
            AudioSource[] sources = GetComponents<AudioSource>();

            if(sources == null)
            {
                print("Error");
                return;

            }
            if(sources.Length > 2)
            {
                for (int i = 2; i < sources.Length; i++)
                {
                    Destroy(sources[i]);

                }

            }
             else if (sources.Length < 2)
            {

                AudioSource[] array = new AudioSource[2];

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

            trackSource = sources[0];
            sfxSource = sources[1];

        }

        // DontDestroyOnLoad(gameObject);
        //  CheckExisting();


    }

    private void Start()
    {
        CheckExisting();
    }

    public void CheckExisting()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<SoundManager>();

        }
        else
        {
            gameObject.SetActive(false);
            gameObject.name = "Disabled Sound Manager";
            Debug.Log("Sound Manager already present. Shutting down ...");
        }

    }

}
