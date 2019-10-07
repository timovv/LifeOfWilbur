using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class GameTimer : MonoBehaviour
{
    public static float ElapsedTimeSeconds { get; private set; }

    public static bool Paused { get; set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        ElapsedTimeSeconds = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Paused)
        {
            ElapsedTimeSeconds += Time.unscaledDeltaTime;
        }

        var ts = TimeSpan.FromSeconds(ElapsedTimeSeconds);
        GetComponent<Text>().text = string.Format("{0:00}:{1:00}:{2:00}", ts.TotalMinutes, ts.Seconds, ts.Milliseconds / 10f);
    }
}
