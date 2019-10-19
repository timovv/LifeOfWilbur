﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject _optionsMenu;
    public GameObject _creditsMenu;
    public GameObject _mainMenu;
    

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X)) 
        {
            LifeOfWilbur.GameController.StartGame(GameMode.Story);
        }

        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            //Disable the Credits menu and go back to the main menu
            _creditsMenu.SetActive(false);
            _mainMenu.SetActive(true);

            //Additionally, disable the options menu
            _optionsMenu.SetActive(false);
        }

        //In the case that the game was paused and the we restart, unpause the game
        if (PauseScript.IsPaused)
        {
            PauseScript.IsPaused = false;
        }
    }

    /// <summary>
    /// Method call for the 'Play Button', which queues the next scene.
    /// </summary>
    private void PlayGame()
    {
        //Queue the next scene in the build order
        _mainMenu.SetActive(false);
        SceneManager.LoadScene(1);

        //Setting the GamerTime back to 0 in the case of restarting the game
        GameTimer.ElapsedTimeSeconds = 0;
    }

    /// <summary>
    /// Method that disables the MainMenu panel and enables the options pane. 
    /// Essentially, method call for the "Options" button
    /// </summary>
    public void LoadOptionsScene()
    {
        _mainMenu.SetActive(false);
        _optionsMenu.SetActive(true);
    }



    /// <summary>
    /// Method call of the 'Quit' button which closes the application given that it is running in a window mode
    /// </summary>
   public void QuitGame()
    {
        Application.Quit();
    }


    /// <summary>
    /// Method to set the visibility of the MenuUI externally. Done in order to not have to make 
    /// the MainMenu panel static. This ensures that the panel has been instantiated first. 
    /// </summary>
    /// <param name="visibility"></param>
    public void SetVisibilityOfMenuUI(bool visibility)
    {
        if (visibility)
        {
            _mainMenu.SetActive(true);
        }
        else
        {
            _mainMenu.SetActive(false);
        }
    }



}
