using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Behaviour that can be applied to a game object with a raw image component
/// that allows for it to be faded in and out as needed.
/// 
/// This is used to fade out the scene.
/// </summary>
[RequireComponent(typeof(RawImage))]
public class FadeInOut : MonoBehaviour
{
    /// <summary>
    /// How long in seconds it should take for the fade transition to occur.
    /// This time is for each way, i.e. it will take this long to fade in, and this long again to fade out.
    /// </summary>
    public float _fadeDurationSeconds;

    /// <summary>
    /// Records if we are currently faded in or out.
    /// </summary>
    private bool _fadedOut = true;

    public void Start()
    {
        FadeInFromBlack();
    }

    /// <summary>
    /// Fade the scene out to black.
    /// </summary>
    public void FadeOutToBlack()
    {
        if(_fadedOut)
        {
            return;
        }

        _fadedOut = true;

        var image = GetComponent<RawImage>();
        image.CrossFadeAlpha(alpha: 1f, duration: _fadeDurationSeconds * .75f, ignoreTimeScale: true);
    }

    /// <summary>
    /// Fade the scene back in from black.
    /// </summary>
    public void FadeInFromBlack()
    {
        if(!_fadedOut)
        {
            return;
        }

        _fadedOut = false;

        var image = GetComponent<RawImage>();
        image.CrossFadeAlpha(alpha: 0f, duration: _fadeDurationSeconds * .75f, ignoreTimeScale: true);
    }


    public void ReloadCurrentScene()
    {
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().name));
    }

    public void LoadSceneByName(string sceneName)
    {
        StartCoroutine(LoadScene(sceneName));
    }

    private IEnumerator LoadScene(string sceneName)
    {
        FadeOutToBlack();
        yield return new WaitForSeconds(_fadeDurationSeconds);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
