using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Singleton that manages game state.
/// </summary>
[RequireComponent(typeof(TransitionController))]
[RequireComponent(typeof(TimeTravelController))]
[RequireComponent(typeof(LevelReset))]
public class GameController : MonoBehaviour, ILevelController
{
    private const string MENU_SCENE_NAME = "MenuScene";
    private const string END_SCENE_NAME = "ExitScene";

    /// <summary>
    /// Value indicating number of resets/deaths/attempts.
    /// </summary>
    public static int Resets { get; private set; }

    // TODO(timo): I have defined Level and Room structs for this stuff, so can use that stuff in here.
    // Those structs can also be used for level information prompts etc
    private readonly IReadOnlyDictionary<GameMode, List<string>> GAME_MODE_LEVELS = new Dictionary<GameMode, List<string>>
    {
        [GameMode.Story] = new List<string> 
        {
            "Level1_1",
            "Level1_2",
            "Level1_3",
            "Level1_4",
            "Level1_5",
        },
        [GameMode.SpeedRun] = new List<string>
        {
            "Level1_1",
            "Level1_2",
        },
    };

    private IEnumerator<string> _levelIterator;

    private bool _movingToNextLevel = false;
    private bool _resettingLevel;

    /// <summary>
    /// The current game mode.
    /// </summary>
    public GameMode CurrentGameMode { get; private set; } = GameMode.NotInGame;
    public string CurrentSceneName { get { return _levelIterator.Current; } }

    void Awake()
    {
        // Don't destroy this object or its immediate children.
        DontDestroyOnLoad(gameObject);

        GetComponent<TimeTravelController>().enabled = false;
        GetComponent<TransitionController>().enabled = true;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(CurrentGameMode.IsInGame())
        {
            _movingToNextLevel = false;
            _resettingLevel = false;
            StartCoroutine(GetComponent<TransitionController>().FadeInFromBlack());
            if(GetComponent<TimeTravelController>().enabled)
            {
                GetComponent<TimeTravelController>().RegisterGameObjects();
            }
            GetComponent<TimeTravelController>().enabled = true;
            GetComponent<TransitionController>().enabled = true;
            StartCoroutine(FindObjectOfType<LevelIndicator>().SetUpPanel("Level X-X"));
        }
        else
        {
            GetComponent<TimeTravelController>().enabled = false;
            GetComponent<TransitionController>().enabled = false;
        }
    }


    /// <summary>
    /// Starts a new game. If a game is currently in progress, the game will not be 
    /// </summary>
    /// <param name="gameMode"></param>
    public void StartGame(GameMode gameMode)
    {
        if(!gameMode.IsInGame())
        {
            throw new ArgumentException(nameof(gameMode));
        }

        CurrentGameMode = gameMode;
        _levelIterator = GAME_MODE_LEVELS[gameMode].GetEnumerator();
        NextLevel();
    }

    public void StartGameAt(GameMode gameMode, string sceneName)
    {
        if(!gameMode.IsInGame())
        {
            throw new ArgumentException(nameof(gameMode));
        }

        CurrentGameMode = gameMode;

        _levelIterator = GAME_MODE_LEVELS[gameMode].GetEnumerator();

        while(_levelIterator.MoveNext())
        {
            if(_levelIterator.Current == sceneName)
            {
                SceneManager.LoadScene(sceneName);
                return;
            }
        }

        // Invalid scene.
        throw new ArgumentException(nameof(sceneName));
    }

    public void NextLevel()
    {
        StartCoroutine(NextLevelCoroutine());
    }

    private IEnumerator NextLevelCoroutine()
    {
        if (_movingToNextLevel || _resettingLevel)
        {
            // don't let this happen multiple times
            // this can happen and would lead to a scene being skipped.
            yield break;
        }

        _movingToNextLevel = true;

        yield return StartCoroutine(GetComponent<TransitionController>().FadeOutToBlack());

        if (_levelIterator.MoveNext())
        {
            Debug.Log($"Loading level: ${_levelIterator.Current}");
            SceneManager.LoadScene(_levelIterator.Current);
        }
        else
        {
            // Going to menu.
            GetComponent<TimeTravelController>().enabled = false;
            GetComponent<TransitionController>().enabled = false;
            CurrentGameMode = GameMode.NotInGame;

            SceneManager.LoadScene(END_SCENE_NAME);
        }
    }

    public void ResetLevel()
    {
        StartCoroutine(ResetLevelCoroutine());
    }

    private IEnumerator ResetLevelCoroutine()
    {
        if(_movingToNextLevel || _resettingLevel)
        {
            yield break;
        }

        _resettingLevel = true;
        Resets++;

        yield return StartCoroutine(GetComponent<TransitionController>().FadeOutToBlack());
        SceneManager.LoadScene(_levelIterator.Current);

    }

    /// <summary>
    /// Returns the game to the main menu.
    /// </summary>
    public void ReturnToMenu()
    {
        CurrentGameMode = GameMode.NotInGame;
        SceneManager.LoadScene(MENU_SCENE_NAME);
    }
}
