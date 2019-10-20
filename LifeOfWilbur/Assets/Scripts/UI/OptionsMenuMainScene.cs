using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuMainScene : MonoBehaviour
{
    public GameObject _optionMenuUI;
    public GameObject _mainMenuUI;

    // Gets called every frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EscapeMenu();
        }
    }


    /// <summary>
    /// Method call that sets the background music volume, when the slider is moved.
    /// </summary>
    public void SetBackgroundVolume()
    {
        float VolumeSliderGet = GameObject.Find("BackgroundVolumeSlider").GetComponent<Slider>().value;
        try
        {
            //Find the slider in the object hierarchy
            Sound s = Array.Find(FindObjectOfType<AudioManager>()._sounds, sound => sound._name == "BackgroundMusic");

            //Update the field in the sound object
            s._volume = VolumeSliderGet;

            //Update the source of the audio
            s.source.volume = s._volume;
        }
        catch (NullReferenceException e)
        {
            Debug.LogWarning("Sound Source not loaded correctly!: " + e);
        }


    }


    /// <summary>
    /// Method call that sets the SFX volume, when its respective slider is moved
    /// </summary>
    public void SetSFXVolume()
    {
        float volume = GameObject.Find("SFXVolumeSlider").GetComponent<Slider>().value;

        try
        {
            //Find the slider in the object hierarchy
            Sound s = Array.Find(FindObjectOfType<AudioManager>()._sounds, sound => sound._name == "SnowWalk");

            //Update the field in the sound object
            s._volume = volume;

            //Update the source of the audio
            s.source.volume = s._volume;
        }
        catch (NullReferenceException e)
        {
            Debug.LogWarning("Sound Source not loaded correctly!: " + e);
        }


    }


    private void EscapeMenu()
    {
        //Enable main menu panel and disable the option menu panel
        _optionMenuUI.SetActive(false);
        _mainMenuUI.SetActive(true);
    }

    public void EscapeMenuButtonClick()
    {
        EscapeMenu();
    }

    public void OnButtonHover()
    {
        FindObjectOfType<AudioManager>().ForcePlay("ButtonHover");
    }

}
