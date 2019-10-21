using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowStormAudio : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartSnowStormMusic();
    }

    /// <summary>
    /// Method that handles starting the snow storm music when the level gets started. Stops playing the other sound
    /// tracks and sets the volume, and starts to play the snowstorm audio.
    /// </summary>
    public void StartSnowStormMusic()
    {
        AudioManager audioManager = AudioManager._instance;

        //Stop playback of Young and Old Wilbur Soundtracks
        Sound OldWilbur = Array.Find(audioManager._sounds, sound => sound._name == "OldWilbur");
        Sound YoungWilbur = Array.Find(audioManager._sounds, sound => sound._name == "YoungWilbur");

        OldWilbur.source.Stop();
        YoungWilbur.source.Stop();

        //Set SnowStormLevel Audio Source properties
        Sound SnowStormLevel = Array.Find(audioManager._sounds, sound => sound._name == "SnowStormLevel");
        SnowStormLevel._volume = FindObjectOfType<OptionsMenu>().GetBackgroundSliderValue();
        SnowStormLevel.source.volume = SnowStormLevel._volume;

        //Start Playback of SnowStorm level
        audioManager.Play("SnowStormLevel");

    }

}
