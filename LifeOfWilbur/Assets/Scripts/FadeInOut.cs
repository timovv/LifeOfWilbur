using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
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
    private bool _fadedOut;

    public void Start()
    {
        var image = GetComponent<RawImage>();
        image.CrossFadeAlpha(0f, 0, true);
    }

    /// <summary>
    /// Coroutine fade to black.
    /// </summary>
    /// <returns>Enumerator to be passed to StartCoroutine</returns>
    public IEnumerator FadeOutToBlack()
    {
        if(_fadedOut)
        {
            yield break;
        }

        _fadedOut = true;

        var image = GetComponent<RawImage>();
        image.CrossFadeAlpha(alpha: 1f, duration: _fadeDurationSeconds * .75f, ignoreTimeScale: true);
        yield return new WaitForSecondsRealtime(_fadeDurationSeconds);
    }

    /// <summary>
    /// Coroutine to fade the scene back in from black.
    /// </summary>
    /// <returns>Enumerator to be passed to StartCoroutine</returns>
    public IEnumerator FadeInFromBlack()
    {
        if(!_fadedOut)
        {
            yield break;
        }

        _fadedOut = false;

        var image = GetComponent<RawImage>();
        image.CrossFadeAlpha(alpha: 0f, duration: _fadeDurationSeconds * .75f, ignoreTimeScale: true);
        yield return new WaitForSecondsRealtime(_fadeDurationSeconds);
    }
}
