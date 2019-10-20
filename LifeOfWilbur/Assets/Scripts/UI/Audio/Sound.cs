using UnityEngine.Audio;
using UnityEngine;


/// <summary>
/// Essentially a container class, that keeps track of the attributes of the sound file that is required by
/// the audio manager.
/// </summary>
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

    public bool _isSFX;
}
