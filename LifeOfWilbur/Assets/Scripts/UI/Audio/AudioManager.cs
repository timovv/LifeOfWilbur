using UnityEngine.Audio;
using UnityEngine;
using System;

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
        //Set the Volumes Corresponding
        Sound OldWilbur = Array.Find(_sounds, sound => sound._name == "OldWilbur");
        Sound YoungWilbur = Array.Find(_sounds, sound => sound._name == "YoungWilbur");

        YoungWilbur._volume = 0.8f;
        OldWilbur._volume = 0.8f;

        OldWilbur.source.volume = OldWilbur._volume;
        YoungWilbur.source.volume = YoungWilbur._volume;


        //Play the background music on scene start up
        Play("YoungWilbur");
        Play("OldWilbur");
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




    public void Pause(string name)
    {
        try
        {
            //Try to find the sound from its given name, and play it. 
            Sound s = Array.Find(_sounds, sound => sound._name == name);
            if (s.source.isPlaying)
            {
                s.source.Stop();
            }

        }
        catch (NullReferenceException e)
        {
            Debug.LogWarning("Sound " + name + " not found!");
            return;
        }
    }

    public void PlayWalking(string name)
    {
        RandomisePitch(name);
        Play(name);
    }

    private void RandomisePitch(string name)
    {
        Sound s = Array.Find(_sounds, sound => sound._name == name);
        float newPitch = UnityEngine.Random.Range(1f, 3f);
        s._pitch = newPitch;
        s.source.pitch = s._pitch;

    }

    public void ChangeVolumeTense(bool IsInPast)
    {
        Sound youngWilburSound = Array.Find(_sounds, sound => sound._name == "YoungWilbur");
        Sound oldWilburSound = Array.Find(_sounds, sound => sound._name == "OldWilbur");

        if (IsInPast) //Set YoungWilbur soundtrack to slider volume
        {
            Debug.Log("YoungWilbur");
            youngWilburSound.source.volume = youngWilburSound._volume;
            oldWilburSound.source.volume = 0;

        }
        else //Set AdultWilbur sound track to slider volume
        {
            Debug.Log("Old Wilbur");
            youngWilburSound.source.volume = 0;
            oldWilburSound.source.volume = oldWilburSound._volume;

        }

        Debug.Log("OldWilbur Volume: " + oldWilburSound.source.volume);
        Debug.Log("YoungWilbur Volume: " + youngWilburSound.source.volume);
    }



}
