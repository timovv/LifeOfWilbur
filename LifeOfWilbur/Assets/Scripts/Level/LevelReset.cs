using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Script to trigger a scene reset when R is pressed
/// </summary>
public class LevelReset : MonoBehaviour
{
    /// <summary>
    /// Allows other scripts to temporarily disable the reset functionality
    /// </summary>
    public static bool ResetDisabled { get; set; } = false;

    /// <summary>
    /// Stores if a reset has previously started executing
    /// </summary>
    private bool _pressed;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !_pressed && !ResetDisabled) //TODO: change to button, not keycode
        {
            _pressed = true; // Disable further requests in case the button was spammed
            LevelTransitionController script = GameObject.FindWithTag("GameController") .GetComponent<LevelTransitionController>();
            script.ReloadCurrentScene();
        }
    }
}
