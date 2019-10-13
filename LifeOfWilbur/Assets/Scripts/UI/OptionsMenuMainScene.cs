using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuMainScene : MonoBehaviour
{
    public GameObject _optionMenuUI;
    public GameObject _mainMenuUI;

    void Awake()
    {
        try
        {
            Sound s = Array.Find(FindObjectOfType<AudioManager>()._sounds, sound => sound._name == "BackgroundMusic");
            GameObject.Find("BackgroundVolumeSlider").GetComponent<Slider>().value = s._volume;

            Sound s1 = Array.Find(FindObjectOfType<AudioManager>()._sounds, sound => sound._name == "SnowWalk");
            GameObject.Find("SFXVolumeSlider").GetComponent<Slider>().value = s1._volume;
        }
        catch (NullReferenceException e)
        {
            Debug.LogWarning("Sound not loaded correctly! " + e);
        }

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _optionMenuUI.SetActive(false);
            _mainMenuUI.SetActive(true);
        }
    }


    public void SetBackgroundVolume()
    {
        float VolumeSliderGet = GameObject.Find("BackgroundVolumeSlider").GetComponent<Slider>().value;
        try
        {
            Sound s = Array.Find(FindObjectOfType<AudioManager>()._sounds, sound => sound._name == "BackgroundMusic");
            s._volume = VolumeSliderGet;
            s.source.volume = s._volume;
        }
        catch (NullReferenceException e)
        {
            Debug.LogWarning("Sound Source not loaded correctly!: " + e);
        }


    }


    public void SetSFXVolume()
    {
        float volume = GameObject.Find("SFXVolumeSlider").GetComponent<Slider>().value;

        try
        {
            Sound s = Array.Find(FindObjectOfType<AudioManager>()._sounds, sound => sound._name == "SnowWalk");
            s._volume = volume;
            s.source.volume = s._volume;
        }
        catch (NullReferenceException e)
        {
            Debug.LogWarning("Sound Source not loaded correctly!: " + e);
        }


    }


}
