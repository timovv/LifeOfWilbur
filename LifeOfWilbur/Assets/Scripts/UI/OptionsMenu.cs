using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{

    public GameObject _optionMenuUI;
    private static float _backgroundVolume = 0.8f;
    private static float _sfxVolume = 0.8f;

    // Gets called every frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Resume real time and Gamer time
            Time.timeScale = 1f; 
            GameTimer.Paused = false;

            //Hide the opttions UI and show the Pause Panel
            _optionMenuUI.SetActive(false);
            FindObjectOfType<PauseScript>()?.SetVisibilityOfPauseUI(true);
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
            _backgroundVolume = VolumeSliderGet;

            //Update the source of the audio
            s.source.volume = s._volume;
        }
        catch(NullReferenceException e)
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
            Sound s1 = Array.Find(FindObjectOfType<AudioManager>()._sounds, sound => sound._name == "DeathSFX");

            //Update the field in the sound object
            s._volume = volume;
            s1._volume = volume;

            //Update the source of the audio
            s.source.volume = s._volume;
            s1.source.volume = s1._volume;

            //Set the field to the slider volume
            _sfxVolume = volume;
        }
        catch (NullReferenceException e)
        {
            Debug.LogWarning("Sound Source not loaded correctly!: " + e);
        }
        

    }


    /// <summary>
    /// Getter to get the instance of the OptionsUI
    /// </summary>
    /// <returns></returns>
    public GameObject getOptionsUI()
    {
        //TODO: Can potentially delete, as it might not be being used.
        return _optionMenuUI;
    }


    /// <summary>
    /// Sets the visibiility of the Options Menu, this is done as a method to ensure that the 
    /// panel has been instantiated and allows for the panel to not have to be static. 
    /// </summary>
    /// <param name="visibility"></param>
    public void SetVisibilityOfUI(bool visibility)
    {
        if (visibility)
        {
            _optionMenuUI.SetActive(visibility);
            GameObject.Find("BackgroundVolumeSlider").GetComponent<Slider>().value = _backgroundVolume;
            GameObject.Find("SFXVolumeSlider").GetComponent<Slider>().value = _sfxVolume;

        }
        else
        {
            _optionMenuUI.SetActive(visibility);
        }
    }



}
