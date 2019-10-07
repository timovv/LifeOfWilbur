using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class GameTimer : MonoBehaviour
{
    public static float ElapsedTimeSeconds { get; private set; } = 0;
    public static string FormattedElapsedTime
    {
        get
        {
            return string.Format("{0:00}:{1:00}:{2:00}",
                (int)(ElapsedTimeSeconds / 60), (int)(ElapsedTimeSeconds % 60), (int)(ElapsedTimeSeconds % 1 * 100));
        }
    }


    public static bool Paused { get; set; } = false;

    // Update is called once per frame
    void Update()
    {
        if (!Paused)
        {
            ElapsedTimeSeconds += Time.unscaledDeltaTime;
        }

        GetComponent<Text>().text = FormattedElapsedTime;
    }
}
