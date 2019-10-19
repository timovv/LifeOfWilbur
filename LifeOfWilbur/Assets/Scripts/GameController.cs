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
public class GameController : MonoBehaviour
{
    private const string MENU_SCENE_NAME = "MainMenu";
    private const string END_SCENE_NAME = "ExitScene";

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

    private bool _isTimeTravelling = false;
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

        SceneManager.activeSceneChanged += OnSceneLoad;
    }

    void OnSceneLoad(Scene previous, Scene next)
    {
        if(CurrentGameMode.IsInGame())
        {
            _movingToNextLevel = false;
            _resettingLevel = false;
            StartCoroutine(GetComponent<TransitionController>().FadeInFromBlack());
            GetComponent<TimeTravelController>().enabled = true;
            GetComponent<TimeTravelController>().RegisterGameObjects();
            GetComponent<TimeTravelController>().UpdateTimeTravelState(true);
            GetComponent<TransitionController>().enabled = true;
        }
        else
        {
            GetComponent<TimeTravelController>().enabled = false;
            GetComponent<TransitionController>().enabled = false;
        }
    }

    void Update()
    {
        // TODO: we should move this to a dedicated "InputController" component
        // along with other input events
        // this is better design(TM)

        if(!CurrentGameMode.IsInGame())
        {
            return;
        }

        if(Input.GetKey(KeyCode.X) && !_isTimeTravelling)
        {
            // User requests TIME TRAVEL.
            // change their time as applicable. The action should not be able to be performed while another time travel event is happening.
            StartCoroutine(GetComponent<TimeTravelController>().TimeTravelWithFade(GetComponent<TransitionController>()));
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

        yield return StartCoroutine(GetComponent<TransitionController>().FadeOutToBlack());
        SceneManager.LoadScene(_levelIterator.Current);

    }

    /// <summary>
    /// Returns the game to the main menu.
    /// </summary>
    public void ReturnToMenu()
    {
        SceneManager.LoadScene(MENU_SCENE_NAME);
    }
}
