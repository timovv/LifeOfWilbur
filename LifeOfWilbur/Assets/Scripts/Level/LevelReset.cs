using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelReset : MonoBehaviour
{
    private bool pressed;

    public static bool ResetDisabled { get; set; } = false;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !pressed && !ResetDisabled) //TODO: change to button, not keycode
        {
            pressed = true; // Disable further requests in case the button was spammed
            FadeInOut script = GameObject.Find("LevelController").GetComponent<FadeInOut>();
            script.ReloadCurrentScene();
        }
    }
}
