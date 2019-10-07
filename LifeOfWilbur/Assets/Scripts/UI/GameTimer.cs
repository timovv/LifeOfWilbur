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

    /// <summary>
    /// Elapsed game time in seconds, formatted in the form mm:ss:ii
    /// </summary>
    public static string FormattedElapsedTime
    {
        get
        {
            return string.Format("{0:00}:{1:00}:{2:00}",
                (int)(ElapsedTimeSeconds / 60), (int)(ElapsedTimeSeconds % 60), (int)(ElapsedTimeSeconds % 1 * 100));
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (!Paused)
        {
            ElapsedTimeSeconds += Time.unscaledDeltaTime;
        }
        
        // update text on HUD.
        GetComponent<Text>().text = FormattedElapsedTime;
    }
}
