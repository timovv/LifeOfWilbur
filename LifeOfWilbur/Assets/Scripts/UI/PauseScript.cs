using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    public static bool IsPaused {get; set;} = false;
    public GameObject _pauseMenuUI;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(IsPaused)
            {
                Resume(); //Resume the state of the game
            }
            else
            {
                Pause(); //Pause the state of the game
            }
        }
    }

    /// <summary>
    /// Pauses the game, and enables the overlay and the animations
    /// </summary>
    void Pause()
    {
        _pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; //Pause the time in the game, in order to not effect the highscore or Wilbur's position
        IsPaused = true;
    }

    /// <summary>
    /// Resumes the game state, and disables the PauseMenu overlay
    /// </summary>
    public void Resume()
    {
        _pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; //Re-enable the time to enable Wilbur's movement and the time counter
        IsPaused = false;
    }

    /// <summary>
    /// Method call for the 'Main Menu' button which re-enables the time and loads the MenuScene
    /// </summary>
    public void BackToMainMenu()
    {
        Time.timeScale = 1f; //Re-enable the time, in the case of restarting the game.
        SceneManager.LoadScene("MenuScene");
    }
}
