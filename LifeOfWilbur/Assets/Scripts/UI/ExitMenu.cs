using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Script controls the game finished scene
///     Allows player to quit or play again
/// </summary>
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

    public void OnButtonHover()
    {
        FindObjectOfType<AudioManager>().ForcePlay("ButtonHover");
    }

}
