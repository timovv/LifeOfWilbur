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
            pressed = true;
            StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex));
        }
    }

    IEnumerator LoadScene(int sceneIndex)
    {
        // Need to persist the black canvas too
        FadeInOut script = GetComponent<FadeInOut>();

        yield return StartCoroutine(script.FadeOutToBlack());
        SceneManager.LoadScene(sceneIndex);
    }
}
