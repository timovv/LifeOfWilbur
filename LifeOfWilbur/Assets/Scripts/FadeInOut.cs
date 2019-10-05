using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class FadeInOut : MonoBehaviour
{
    public float _fadeDurationSeconds;
    private bool _fadedOut;

    public void Start()
    {
        var image = GetComponent<RawImage>();
        image.CrossFadeAlpha(0f, 0, true);
    }

    public IEnumerator FadeOutToBlack()
    {
        if(_fadedOut)
        {
            yield break;
        }

        _fadedOut = true;
        //StopCoroutine("FadeInFromBlack");

        _fadedOut = true;

        var image = GetComponent<RawImage>();
        image.CrossFadeAlpha(alpha: 1f, duration: _fadeDurationSeconds * .75f, ignoreTimeScale: true);
        yield return new WaitForSecondsRealtime(_fadeDurationSeconds);
    }

    public IEnumerator FadeInFromBlack()
    {
        if(!_fadedOut)
        {
            yield break;
        }

        _fadedOut = false;
        //StopCoroutine("FadeOutToBlack");

        var image = GetComponent<RawImage>();
        image.CrossFadeAlpha(alpha: 0f, duration: _fadeDurationSeconds * .75f, ignoreTimeScale: true);
        yield return new WaitForSecondsRealtime(_fadeDurationSeconds);
    }
}
