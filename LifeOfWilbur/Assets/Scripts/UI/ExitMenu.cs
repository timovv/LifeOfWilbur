using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitMenu : MonoBehaviour
{
    /// <summary>
    /// Method call for the 'Play Again' button. Reloads the first screen in the build queue. It loads the Menu Screen.
    /// </summary>
    public void PlayAgain()
    {
        //Loading the Game again, and not the menu screen
        SceneManager.LoadScene(0);
    }


    /// <summary>
    /// Method call for the 'Quit' button. Closes the application given that you are running on Desktop.
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

}
