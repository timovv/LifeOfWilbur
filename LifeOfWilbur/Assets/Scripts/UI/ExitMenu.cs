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
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetTime(float f)
    {
        TimeText.text = f.ToString();
    }

}
