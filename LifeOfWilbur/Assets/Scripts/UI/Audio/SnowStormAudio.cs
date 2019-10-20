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

    public void StartSnowStormMusic()
    {
        AudioManager audioManager = AudioManager._instance;

        //Stop playback of Young and Old Wilbur Soundtracks
        Sound OldWilbur = Array.Find(audioManager._sounds, sound => sound._name == "OldWilbur");
        Sound YoungWilbur = Array.Find(audioManager._sounds, sound => sound._name == "YoungWilbur");

        OldWilbur.source.Stop();
        YoungWilbur.source.Stop();

        Sound SnowStormLevel = Array.Find(audioManager._sounds, sound => sound._name == "SnowStormLevel");
        SnowStormLevel._volume = FindObjectOfType<OptionsMenu>().GetBackgroundSliderValue();
        SnowStormLevel.source.volume = SnowStormLevel._volume;

        audioManager.Play("SnowStormLevel");

    }

}
