using UnityEngine.Audio;
using UnityEngine;
using System;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public Sound[] _sounds;

    public static AudioManager _instance;

    //Use this for initialisation
    void Awake()
    {
        //Adopting the singleton pattern for the AudioManger, since you don't want 
        //the sound cutting of and new instances of the same sound being created, on scene change.
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        //Iterating through all the sounds and assigning their attributes to the source values.
        foreach (Sound s in _sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s._clip;

            s.source.volume = s._volume;
            s.source.pitch = s._volume;
            s.source.loop = s._loop;
        }
    }

    void Start()
    {
        //Find the intiail sound tracks and set the Volumes Corresponding
        Sound OldWilbur = Array.Find(_sounds, sound => sound._name == "OldWilbur");
        Sound YoungWilbur = Array.Find(_sounds, sound => sound._name == "YoungWilbur");

        //Start the game with both sound tracks playing
        YoungWilbur._volume = 0.8f;
        OldWilbur._volume = 0.8f;

        //Set the volume to the sound object
        OldWilbur.source.volume = OldWilbur._volume;
        YoungWilbur.source.volume = YoungWilbur._volume;


        //Play the background music on scene start up
        Play("YoungWilbur");
        Play("OldWilbur");

        //Subscribe to the event handler for fading
        FindObjectOfType<TransitionController>().OnFadingIn += ChangeVolumeTense;
    }


    /// <summary>
    /// Restart the background music in the case that the Snowstorm or any specific background music is played. The method
    /// calls on the start method, after pausing the SnowStorm levels background music. 
    /// </summary>
    public void Restart()
    {
        Sound SnowStorm = Array.Find(_sounds, sound => sound._name == "SnowStormLevel");
        SnowStorm.source?.Stop(); //Only stop the audio source if it is not null
        Start();
    }

    /// <summary>
    /// This method abstracts and allows the sounds to be played from any other script in the game, by finding
    /// its object and specifying the name of the sound file.
    /// </summary>
    /// <param name="name"></param>
    public void Play(string name)
    {
        try
        {
            //Try to find the sound from its given name, and play it. 
            Sound s = Array.Find(_sounds, sound => sound._name == name);
            if (!s.source.isPlaying)
            {
                s.source.Play();
            }
        }
        catch (NullReferenceException e)
        {
            Debug.LogWarning("Sound " + name + " not found!");
            return;
        }
    }

    /// <summary>
    /// Similar to play, but in contrast to only playing the source in the case that it is not currently playing, it will
    /// override and stop the instance of the source and start the playback of the audio source again.
    /// </summary>
    /// <param name="name"></param>
    public void ForcePlay(string name)
    {
        try
        {
            //Try to find the sound from its given name, and play it. 
            Sound s = Array.Find(_sounds, sound => sound._name == name);
            s.source.Play();
        }
        catch (NullReferenceException e)
        {
            Debug.LogWarning("Sound " + name + " not found!");
            return;
        }
    }


    /// <summary>
    /// Method to pause the Audio Source in question given its name
    /// </summary>
    /// <param name="name"></param>
    public void Pause(string name)
    {
        try
        {
            //Try to find the sound from its given name, and play it. 
            Sound s = Array.Find(_sounds, sound => sound._name == name);
            if (s.source.isPlaying)
            {
                s.source.Pause();
            }

        }
        catch (NullReferenceException e)
        {
            Debug.LogWarning("Sound " + name + " not found!");
            return;
        }
    }

    /// <summary>
    /// Method that randomises the pitch of the audio and then plays the audio source in question.
    /// Method is designed to be used with non-looping audio sources and audio sources that are 
    /// meant to be SFX files. 
    /// </summary>
    /// <param name="name"></param>
    public void RandomisePitchAndPlay(string name)
    {
        RandomisePitch(name);
        Play(name);
    }

    /// <summary>
    /// Randomise the pitch of the given audio file, from its original file
    /// </summary>
    /// <param name="name"></param>
    private void RandomisePitch(string name)
    {
        Sound s = Array.Find(_sounds, sound => sound._name == name); //Find the sound in questions
        float newPitch = UnityEngine.Random.Range(1f, 3f); //Random Number between 1-3
        s._pitch = newPitch; //Set the new pitch (Done to keep track of the source vs sound object pitch)
        s.source.pitch = s._pitch; //Set the source to have the new pitch

    }

    /// <summary>
    /// Method used to change the sound tracks between when adult Wilbur is being played, vs when Young Wilbur is being played.
    /// To keep the sound tracks synchronised, they are always looping in the background but only the volume of them is changed.
    /// To ensure that the change is seemless, and not that abrupt for the players, a fade is done that linearly reduces the 
    /// volume and linearly increases the other sound track.
    /// </summary>
    public void ChangeVolumeTense()
    {
        Sound youngWilburSound = Array.Find(_sounds, sound => sound._name == "YoungWilbur");
        Sound oldWilburSound = Array.Find(_sounds, sound => sound._name == "OldWilbur");

        if (TimeTravelController.IsInPast) //Set YoungWilbur soundtrack to slider volume
        {
            StartCoroutine(FadeAudio(youngWilburSound, 0.5f, youngWilburSound._volume));
            StartCoroutine(FadeAudio(oldWilburSound, 0.5f, 0));
        }
        else //Set AdultWilbur sound track to slider volume
        {
            StartCoroutine(FadeAudio(youngWilburSound, 0.5f, 0));
            StartCoroutine(FadeAudio(oldWilburSound, 0.5f, oldWilburSound._volume));
        }
    }

    /// <summary>
    /// Couroutine to Fade the audio in and out. The fade linearly changes the volume of the sound tracks, given the duration, and
    /// the target volume.
    /// </summary>
    /// <param name="sound"></param>
    /// <param name="duration"></param>
    /// <param name="targetVol"></param>
    /// <returns></returns>
    public IEnumerator FadeAudio(Sound sound, float duration, float targetVol)
    {
        float currentTime = 0;
        float start = sound.source.volume;

        while (currentTime < duration)
        {
            //Ensure that the change is based on real time and not game time, and that it does not get affected
            //by the speed up in physics during "Time" change. 
            currentTime += Time.unscaledDeltaTime; 
            sound.source.volume = Mathf.Lerp(start, targetVol, currentTime / duration);
            //Forcefully wait for the next frame.
            yield return null;
        }
    }
}
