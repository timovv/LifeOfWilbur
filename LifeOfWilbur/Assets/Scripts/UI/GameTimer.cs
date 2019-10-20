using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Component that keep strack of game time used for scoring and upates it.
/// This component is part of the game's HUD.
/// </summary>
[RequireComponent(typeof(Text))]
public class GameTimer : MonoBehaviour
{
    /// <summary>
    /// Total elapsed game time in seconds.
    /// </summary>
    public static float ElapsedTimeSeconds { get; set; } = 0;

    /// <summary>
    /// Whether the game timer is currently paused.
    /// </summary>
    public static bool Paused { get; set; } = false;

    private Text _text;

    /// <summary>
    /// Elapsed game time in seconds, formatted in the form mm:ss:ii
    /// </summary>
    public static string FormattedElapsedTime
    {
        get
        {
            return FormatTime(ElapsedTimeSeconds);
        }
    }

    /// <summary>
    /// Formats the given time to mm:ss:ii
    /// </summary>
    public static string FormatTime(float seconds)
    {
        return string.Format("{0:00}:{1:00}:{2:00}",
                (int)(seconds / 60), (int)(seconds % 60), (int)(seconds % 1 * 100));
    }

    private void Awake()
    {
        _text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (LifeOfWilbur.GameController != null)
        {
            _text.enabled = LifeOfWilbur.GameController.CurrentGameMode == GameMode.SpeedRun;
        }

        if (!Paused)
        {
            ElapsedTimeSeconds += Time.unscaledDeltaTime;
        }
        
        // update text on HUD.
        GetComponent<Text>().text = FormattedElapsedTime;
    }
}
