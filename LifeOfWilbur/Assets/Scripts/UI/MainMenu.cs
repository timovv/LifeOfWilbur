using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Script containing the various options from the main menu.
///     Controls the starting of the game
///     Controls the going into the options from the main menu
///     Controls quiting
///     Controls the displaying of credits
/// </summary>
public class MainMenu : MonoBehaviour
{
    public GameObject _optionsMenu;
    public GameObject _creditsMenu;
    public GameObject _mainMenu;
    public static string PlayerName { get; private set; } = "Wilbur";
    

    void Start()
    {
        AudioManager audioManager = AudioManager._instance;
        audioManager?.Restart();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EscButtonOnClick();
        }
        else if(Input.GetKeyDown(KeyCode.F1))
        {
            // F1: unlock all
            SaveData.Instance.HighestUnlockedLevel = 3;
            SaveData.Instance.UnlockedSpeedRunMode = true;
        }
        else if(Input.GetKeyDown(KeyCode.F2))
        {
            SaveData.Instance.HighestUnlockedLevel = 0;
            SaveData.Instance.UnlockedSpeedRunMode = false;
        }

        var speedRunButton = GameObject.Find("SpeedrunButton");

        if (speedRunButton != null)
        {
            if (SaveData.Instance.UnlockedSpeedRunMode)
            {
                speedRunButton.GetComponent<Button>().interactable = true;
                speedRunButton.GetComponent<SpriteRenderer>().color = Color.white;
                speedRunButton.GetComponent<Animator>().enabled = true;
            }
            else
            {
                speedRunButton.GetComponent<Button>().interactable = false;
                speedRunButton.GetComponent<SpriteRenderer>().color = new Color(.5f, .5f, .5f);
                speedRunButton.GetComponent<Animator>().enabled = false;
            }
        }

        //In the case that the game was paused and the we restart, unpause the game
        if (PauseScript.IsPaused)
        {
            PauseScript.IsPaused = false;
        }
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

    public void StoryButtonClick()
    {
        if (SaveData.Instance.HighestUnlockedLevel == 0)
        {
            SaveData.Instance.HighestUnlockedLevel = 1;
            LifeOfWilbur.GameController.StartGame(GameMode.Story);
        }
        else
        {
            SceneManager.LoadScene("LevelSelectScene");
        }
    }

    public void SpeedrunPopupShow()
    {
        transform.Find("SpeedrunPopup").gameObject.SetActive(true);
        GameObject.Find("PlayerName").GetComponent<InputField>().text = PlayerName;
    }

    public void SpeedrunButtonClick()
    {
        PlayerName = GameObject.Find("PlayerName").GetComponent<InputField>().text;
        LifeOfWilbur.GameController.StartGame(GameMode.SpeedRun);
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


    public void EscButtonOnClick()
    {
        //Disable the Credits menu and go back to the main menu
        _creditsMenu.SetActive(false);
        _mainMenu.SetActive(true);

        //Additionally, disable the options menu
        _optionsMenu.SetActive(false);
    }

    public void ButtonOnHoverSound()
    {
        FindObjectOfType<AudioManager>().ForcePlay("ButtonHover");
    }

}
