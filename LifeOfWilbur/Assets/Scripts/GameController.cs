using System;
using System.Linq;
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
    private const string END_SCENE_NAME = "LeaderboardExit";

    /// <summary>
    /// Value indicating number of resets/deaths/attempts.
    /// </summary>
    public static int Resets { get; private set; }

    // TODO(timo): I have defined Level and Room structs for this stuff, so can use that stuff in here.
    // Those structs can also be used for level information prompts etc
    public Level[] _levels;

    private IEnumerator<Level> _levelEnumerator;
    private IEnumerator<Room> _roomEnumerator;

    private bool _movingToNextLevel = false;
    private bool _resettingLevel;

    private int _levelCount;

    /// <summary>
    /// The current game mode.
    /// </summary>
    public GameMode CurrentGameMode { get; private set; } = GameMode.NotInGame;

    void Awake()
    {
        if(LifeOfWilbur.GameController != null && LifeOfWilbur.GameController != this)
        {
            Destroy(gameObject);
        }

        // Don't destroy this object or its immediate children.
        DontDestroyOnLoad(gameObject);

        GetComponent<TimeTravelController>().enabled = false;
        GetComponent<TransitionController>().enabled = true;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
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
            StartCoroutine(FindObjectOfType<LevelIndicator>().SetUpPanel(_roomEnumerator.Current._readableName));
        }
        else
        {
            GetComponent<TimeTravelController>().enabled = false;
            GetComponent<TransitionController>().enabled = false;
            if (scene.name == END_SCENE_NAME)
            {
                StartCoroutine(GetComponent<TransitionController>().FadeInFromBlack());
            }
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
        ResetRoomEnumerators();
        NextRoom();
    }

    public void StartGameAt(GameMode gameMode, string sceneName)
    {
        if(!gameMode.IsInGame())
        {
            throw new ArgumentException(nameof(gameMode));
        }

        CurrentGameMode = gameMode;

        ResetRoomEnumerators();

        while(AdvanceRoomEnumerators())
        {
            if(_roomEnumerator.Current._sceneName == sceneName)
            {
                SceneManager.LoadScene(sceneName);
                return;
            }
        }

        // Invalid scene.
        throw new ArgumentException(nameof(sceneName));
    }

    private void ResetRoomEnumerators()
    {
        _levelCount = 0;
        _levelEnumerator = ((IEnumerable<Level>)_levels).GetEnumerator();
        _levelEnumerator.MoveNext();
        _roomEnumerator = ((IEnumerable<Room>)_levelEnumerator.Current._rooms)
            .Where(x => x._playInSpeedRunMode || CurrentGameMode == GameMode.Story)
            .GetEnumerator();
    }

    private bool AdvanceRoomEnumerators()
    {
        if(_roomEnumerator.MoveNext())
        {
            return true;
        }
        else if(_levelEnumerator.MoveNext())
        {
            SaveData.Instance.HighestUnlockedLevel = Mathf.Max(SaveData.Instance.HighestUnlockedLevel, ++_levelCount + 1);
            _roomEnumerator = ((IEnumerable<Room>)_levelEnumerator.Current._rooms)
                .Where(x => x._playInSpeedRunMode || CurrentGameMode == GameMode.Story)
                .GetEnumerator();
            _roomEnumerator.MoveNext();
            return true;
        }

        SaveData.Instance.UnlockedSpeedRunMode = true;

        return false;
    }

    public void NextRoom()
    {
        StartCoroutine(NextRoomCoroutine());
    }

    private IEnumerator NextRoomCoroutine()
    {
        if (_movingToNextLevel || _resettingLevel)
        {
            // don't let this happen multiple times
            // this can happen and would lead to a scene being skipped.
            yield break;
        }

        _movingToNextLevel = true;

        yield return StartCoroutine(GetComponent<TransitionController>().FadeOutToBlack());

        if(AdvanceRoomEnumerators())
        {
            Debug.Log($"Loading room: ${_roomEnumerator.Current}");
            SceneManager.LoadScene(_roomEnumerator.Current._sceneName);
        }
        else
        {
            // Going to menu.
            GetComponent<TimeTravelController>().enabled = false;
            GetComponent<TransitionController>().enabled = false;
            CurrentGameMode = GameMode.NotInGame;

            GameTimer.Paused = true;
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
        SceneManager.LoadScene(_roomEnumerator.Current._sceneName);

    }

    /// <summary>
    /// Returns the game to the main menu.
    /// </summary>
    public void ReturnToMenu()
    {
        CurrentGameMode = GameMode.NotInGame;
        SceneManager.LoadScene(MENU_SCENE_NAME);
    }

    public void StartStoryMode()
    {
        StartGame(GameMode.Story);
    }

    public void StartSpeedrunMode()
    {
        GameTimer.ElapsedTimeSeconds = 0;
        StartGame(GameMode.SpeedRun);
    }
}
