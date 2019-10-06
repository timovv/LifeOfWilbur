using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitMenu : MonoBehaviour
{
    public TextMeshProUGUI TimeText;
    public void PlayAgain()
    {
        //Loading the Game again, and not the menu screen
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting the Game");
        Application.Quit();
    }

    public void SetTime(float f)
    {
        TimeText.text = f.ToString();
    }

}
