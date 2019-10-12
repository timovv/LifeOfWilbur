using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string _name;
    public AudioClip _clip;

    [Range(0f, 1f)]
    public float _volume;

    [Range(0.1f, 3f)]
    public float _pitch;

    [HideInInspector]
    public AudioSource source;

    public bool _loop;
}
