using System;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{

    public GameObject _optionMenuUI;

    void Awake()
    {
        try
        {
            Sound s = Array.Find(FindObjectOfType<AudioManager>()._sounds, sound => sound._name == "BackgroundMusic");
            GameObject.Find("BackgroundVolumeSlider").GetComponent<Slider>().value = s._volume;
        }
        catch(NullReferenceException e)
        {
            Debug.LogWarning("BackgroundMusic not loaded correctly!");
        }

        try
        {
            Sound s = Array.Find(FindObjectOfType<AudioManager>()._sounds, sound => sound._name == "SnowWalk");
            GameObject.Find("SFXVolumeSlider").GetComponent<Slider>().value = s._volume;
        }
        catch(NullReferenceException e)
        {
            Debug.LogWarning("SnowWalking not loaded correctly!");
        }
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 1f;
            GameTimer.Paused = false;
            _optionMenuUI.SetActive(false);
            FindObjectOfType<PauseScript>()?.SetVisibilityOfPauseUI(true);
        }
    }



    void Start()
    {
        Debug.Log("Options Menu Loaded!");
    }

    public void SetBackgroundVolume()
    {
        float VolumeSliderGet = GameObject.Find("BackgroundVolumeSlider").GetComponent<Slider>().value;

        Sound s = Array.Find(FindObjectOfType<AudioManager>()._sounds, sound => sound._name == "BackgroundMusic");
        s._volume = VolumeSliderGet;
        s.source.volume = s._volume;
       

        Debug.Log("Background Volume is: " + VolumeSliderGet);
    }


    public void SetSFXVolume()
    {
        float volume = GameObject.Find("SFXVolumeSlider").GetComponent<Slider>().value;

        Sound s = Array.Find(FindObjectOfType<AudioManager>()._sounds, sound => sound._name == "SnowWalk");
        s._volume = volume;
        s.source.volume = s._volume;

        Debug.Log("SFX Volume is: " + volume);
    }


    public GameObject getOptionsUI()
    {
        return _optionMenuUI;
    }


    public void SetVisibilityOfUI(bool visibility)
    {
        if (visibility)
        {
            _optionMenuUI.SetActive(visibility);
        }
        else
        {
            _optionMenuUI.SetActive(visibility);
        }
    }



}
