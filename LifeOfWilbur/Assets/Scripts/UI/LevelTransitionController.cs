using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Behaviour that can be applied to a game object with a raw image component
/// that allows for it to be faded in and out as needed to support level transitions.
/// </summary>
[RequireComponent(typeof(RawImage))]
public class LevelTransitionController : MonoBehaviour
{
    /// <summary>
    /// How long in seconds it should take for the fade transition to occur.
    /// This time is for each way, i.e. it will take this long to fade in, and this long again to fade out.
    /// </summary>
    public float _fadeDurationSeconds;

    /// <summary>
    /// Records if we are currently faded in or out.
    /// </summary>
    public bool _fadedOut = true;

    /// <summary>
    /// Flag to determine whether a transition is currently occuring.
    /// </summary>
    public bool IsTransitioning { get; private set; }

    public event EventHandler OnFadingOut;
    public event EventHandler OnFadedOut;

    public event EventHandler OnFadingIn;
    public event EventHandler OnFadedIn;

    public void Awake()
    {
        GetComponent<RawImage>().color = Color.black;
        StartCoroutine(FadeInFromBlackCoroutine());
    }

    public IEnumerator FadeOutToBlack()
    {
        if (_fadedOut)
        {
            yield break;
        }

        OnFadingOut?.Invoke(this, EventArgs.Empty);
        _fadedOut = true;

        var image = GetComponent<RawImage>();
        image.CrossFadeAlpha(alpha: 1f, duration: _fadeDurationSeconds * .75f, ignoreTimeScale: true);
        yield return new WaitForSeconds(_fadeDurationSeconds);
    }

    public IEnumerator FadeInFromBlackCoroutine()
    {
        if(!_fadedOut)
        {
            yield break;
        }

        _fadedOut = false;
        var image = GetComponent<RawImage>();
        image.CrossFadeAlpha(alpha: 0f, duration: _fadeDurationSeconds * .75f, ignoreTimeScale: true);
        yield return new WaitForSeconds(_fadeDurationSeconds);  
    }

    /// <summary>
    /// Reloads the current scene with a fade transition
    /// </summary>
    public void ReloadCurrentScene()
    {
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().name));
    }

    /// <summary>
    /// Loads the given scene with a fade transition
    /// </summary>
    /// <param name="sceneName">The scene's name</param>
    public void LoadSceneByName(string sceneName)
    {
        StartCoroutine(LoadScene(sceneName));
    }


    /// <summary>
    /// Implements ReloadCurrentScene() and LoadSceneByName()
    /// </summary>
    /// <param name="sceneName"></param>
    private IEnumerator LoadScene(string sceneName)
    {
        yield return StartCoroutine(FadeOutToBlack());
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
