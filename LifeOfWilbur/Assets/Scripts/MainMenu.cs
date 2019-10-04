using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        //Queue the next scene in the build order
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

   public void QuitGame()
    {
        //TODO: Remove Debug Statement
        //Testing/Making sure that the quit command is thrown
        Debug.Log("Quitting Game");
        Application.Quit();
    }
}
