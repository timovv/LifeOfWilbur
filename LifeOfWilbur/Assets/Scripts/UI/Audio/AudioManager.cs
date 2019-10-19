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
        if(_instance == null)
        {
            _instance = this;
        }else {
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
            Debug.Log(s._name);
        }




    }


    void Start()
    {
        //Play the background music on scene start up
        Play("BackgroundMusic");
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
            s.source.Play();
        }catch(NullReferenceException e){
            Debug.LogWarning("Sound " + name + " not found!");
            return;
        }
        
    }
}
