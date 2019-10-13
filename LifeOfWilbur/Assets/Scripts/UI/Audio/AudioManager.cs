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
        if(_instance == null)
        {
            _instance = this;
        }else {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject);

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
        Play("BackgroundMusic");
    }


    public void Play(string name)
    {
        Sound s = Array.Find(_sounds, sound => sound._name == name);
        try
        {
            s.source.Play();
        }catch(NullReferenceException e){
            Debug.LogWarning("Sound " + name + " not found!");
            return;
        }
        
    }
}
